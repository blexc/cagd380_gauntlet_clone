using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// you cannot move while shooting projectiles
// health is slowly depleated over time
// you can only move in 8 directions
public class Player : MonoBehaviour
{
    [SerializeField] protected int score;
    [SerializeField] protected int health;
    [SerializeField] protected int damage;
    [SerializeField] protected int armor;
    [SerializeField] protected int movementSpeed;
    [SerializeField] protected int attackSpeed;
    [SerializeField] protected GameObject physicalProjectilePrefab;
    [SerializeField] protected GameObject magicProjectilePrefab;
    protected List<Item> inventory = new List<Item>();

    // timer to stop the player from moving while firing projectiles
    float fireBuffer;
    float fireBufferStart = 0.25f;

    Rigidbody rb;
    Vector2 movementInput;
    Vector3 directionFacing;

    #region recieve input
    public void OnMove(InputAction.CallbackContext ctx) =>
        movementInput = ctx.ReadValue<Vector2>();

    public void OnPhysical(InputAction.CallbackContext ctx)
    {
        if (ctx.performed) PhysicalAttack();
    }

    public void OnMagic(InputAction.CallbackContext ctx)
    {
        if (ctx.performed) MagicAttack(); 
    }

    public void OnStart(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            // TODO pause game?
        }
    }
    #endregion

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (FollowCam.Instance)
            FollowCam.Instance.AddPlayerTransform(transform);
    }

    private void Destroy()
    {
        if (FollowCam.Instance)
            FollowCam.Instance.RemovePlayerTransform(transform);
    }

    private void FixedUpdate()
    {
        // force 8 directions and a resultant radius of 1f
        movementInput.x = Mathf.Round(movementInput.x);
        movementInput.y = Mathf.Round(movementInput.y);

        // only consider a new direction if it's non-zero
        if (movementInput != Vector2.zero)
        {
            directionFacing = new Vector3(movementInput.x, 0f, movementInput.y);
            directionFacing = Vector3.ClampMagnitude(directionFacing, 1f);

            // apply velocity
            rb.velocity = movementSpeed * Time.fixedDeltaTime * directionFacing;
        }
        else
        {
            rb.velocity = Vector3.zero;
        }

        // don't move while player is firing projectiles
        // or if the player isn't trying to move
        fireBuffer = Mathf.Max(fireBuffer - Time.fixedDeltaTime, 0f);
        if (fireBuffer > 0f) rb.velocity = Vector3.zero;
    }

    public void AddItem(Item item)
    {
        inventory.Add(item); 
    }

    public void TakeDamage(int value)
    {
        health -= value;
        if (health <= 0)
        {
            // die
            Destroy(gameObject);
        }
    }

    public void Heal(int value)
    {
        health += value;
    }

    public void AddPoints(int value)
    {
        score += value;
    }

    // called when the attack button is pressed 
    protected virtual void PhysicalAttack()
    {
        fireBuffer = fireBufferStart;
        GameObject projectileObject = Instantiate(physicalProjectilePrefab);
        projectileObject.transform.position = transform.position;
        projectileObject.GetComponent<PlayerProjectile>().ShootProjectile(directionFacing);
    }

    // called when the magic button is pressed 
    protected virtual void MagicAttack()
    {
        // if the player has a potion, use it
        foreach (Item i in inventory)
        {
            Potion potion = i.GetComponent<Potion>();
            if (potion)
            {
                potion.UsePotion();
            }
        }

        fireBuffer = fireBufferStart;
        GameObject projectileObject = Instantiate(magicProjectilePrefab);
            projectileObject.transform.position = transform.position;
        projectileObject.GetComponent<PlayerProjectile>().ShootProjectile(directionFacing);
    }

    public void TryToUnlock(Door door)
    {
        foreach(Item i in inventory)
        {
            var key = i.GetComponent<Key>();
            if (key)
            {
                key.UnlockDoor(door);
            }
        }
    }
}
