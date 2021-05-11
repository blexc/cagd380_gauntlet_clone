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
    protected int numKeys;
    protected int numPotions;

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
        if (ctx.performed) TogglePauseGame();
    }

    public void OnSpeedUp(InputAction.CallbackContext ctx)
    {
        if (ctx.performed) DebugSpeedUp();
    }
    #endregion

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        PlaceAtSpawn();

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
        // prolly a bad idea to leave this in FixedUpdate 
        if (myUI) myUI.UpdateText(score, health, numKeys, numPotions);

        // force 8 directions and a resultant radius of 1f
        movementInput.x = Mathf.Round(movementInput.x);
        movementInput.y = Mathf.Round(movementInput.y);

        // only consider a new direction if it's non-zero
        // and the game isn't paused
        if (movementInput != Vector2.zero && GameManager.Instance.gameState != GameState.paused)
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

    public void TakeDamage(int value)
    {
        // no negative values allowed
        if (value <= 0) return;

        // deal damage-minus-armor damage
        health -= Mathf.Max(value - armor, 0);
        if (health <= 0)
        {
            // die
            Destroy(gameObject);
        }
    }
    
    #region item related funcs

    public void Heal(int value)
    {
        health += value;
    }

    public void AddPoints(int value)
    {
        score += value;
    }

    public void AddPotion()
    {
        numPotions++;
    }

    public void AddKey()
    {
        numKeys++;
    }

    protected void UsePotion()
    {
        if (numPotions > 0)
        {
            // TODO kill all enemies
            numPotions--;
        }
    }

    #endregion

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
        UsePotion();

        fireBuffer = fireBufferStart;
        GameObject projectileObject = Instantiate(magicProjectilePrefab);
            projectileObject.transform.position = transform.position;
        projectileObject.GetComponent<PlayerProjectile>().ShootProjectile(directionFacing);
    }

    // toggle between pausing and unpausing the game
    // any player can do this
    private void TogglePauseGame()
    {
        var GM = GameManager.Instance;
        if (GM.gameState == GameState.paused)
            GM.Resume();
        else if (GM.gameState == GameState.playing)
            GM.Pause();
        else{}// if the game state is anything else, don't do anything!
    }

    private void DebugSpeedUp()
    {
        // don't do anything if you're not debugging
        if (!GameManager.Instance.isDebugging) return;

        if (GameManager.Instance.gameState == GameState.playing)
            Time.timeScale = 4f;
        else
            Time.timeScale = 1f;
    }

    // called by door
    public void TryToUnlock(Door door)
    {
        if (numKeys > 0)
        {
            door.UnlockDoor();
            numKeys--;
        }
    }

    // if there is a spawner in the scene, set the player pos there
    public void PlaceAtSpawn()
    {
        var playerSpawn = FindObjectOfType<PlayerSpawn>();
        if (playerSpawn)
        {
            Vector3 offset = Vector3.right * (int)playerType * 2;
            transform.position = playerSpawn.transform.position + offset;
        }
    }


}
