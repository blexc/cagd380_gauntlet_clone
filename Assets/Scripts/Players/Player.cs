using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerType
{
    warrior,
    valkyrie,
    wizard,
    elf
}

// you cannot move while shooting projectiles
// health is slowly depleated over time
// you can only move in 8 directions
public class Player : MonoBehaviour
{
    // gets the UI associated to this player
    private PlayerUI myUI {
        get { return GameManager.Instance.playerUIList[(int)playerType]; }
    }

    [SerializeField] protected PlayerType playerType;
    [SerializeField] protected int score;
    [SerializeField] protected int health;
    [SerializeField] protected int damage;
    [SerializeField] protected int armor;
    [SerializeField] protected int movementSpeed;
    [SerializeField] protected int attackSpeed;
    [SerializeField] protected GameObject physicalProjectilePrefab;
    [SerializeField] protected GameObject magicProjectilePrefab;
    protected List<Potion> potions = new List<Potion>();
    protected List<Key> keys = new List<Key>();

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

        if (myUI) myUI.ShowText();
    }

    private void Destroy()
    {
        if (FollowCam.Instance)
            FollowCam.Instance.RemovePlayerTransform(transform);

        if (myUI) myUI.HideText();
    }

    private void FixedUpdate()
    {
        if (myUI) myUI.UpdateText(health, score, keys.Count, potions.Count);

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

    public void AddPotion(Potion p)
    {
        potions.Add(p); 
    }

    public void AddKey(Key k)
    {
        keys.Add(k); 
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
        foreach (Potion p in potions)
        {
            p.UsePotion();
        }

        fireBuffer = fireBufferStart;
        GameObject projectileObject = Instantiate(magicProjectilePrefab);
            projectileObject.transform.position = transform.position;
        projectileObject.GetComponent<PlayerProjectile>().ShootProjectile(directionFacing);
    }

    public void TryToUnlock(Door door)
    {
        foreach(Key i in keys)
        {
            i.UnlockDoor(door);
        }
    }
}
