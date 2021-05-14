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

public enum ItemStolen
{
    potion,
    key,
    score
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
    [SerializeField] protected GameObject projectilePrefab;
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

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();

        PlaceAtSpawn();

        if (FollowCam.Instance)
            FollowCam.Instance.AddPlayerTransform(transform);

        if (myUI) myUI.ShowText();

        GameManager.Instance.spareCamera.SetActive(false);
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

        Narrator.Instance.SayLine(NarratorLine.playerTookDamage);

        // deal damage-minus-armor damage
        health -= Mathf.Max(value - armor, 0);
        if (health <= 0) KillPlayer();
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
            // destroy all enemies (TODO test this!)
            enemies[] enemies = FindObjectsOfType<enemies>();
            foreach (enemies e in enemies)
                Destroy(e.gameObject);

            numPotions--;
        }
        else
        {
            Narrator.Instance.SayLine(NarratorLine.cantUseMagicWithout);
        }
    }

    #endregion

    // called when the attack button is pressed 
    protected virtual void PhysicalAttack()
    {
        fireBuffer = fireBufferStart;
        Vector3 offset = directionFacing * 1.25f;
        Vector3 spawnPos = transform.position + offset;
        GameObject projectileObject = Instantiate(projectilePrefab, spawnPos, Quaternion.Euler(Vector3.zero));
        projectileObject.GetComponent<PlayerProjectile>().ShootProjectile(directionFacing);
    }

    // called when the magic button is pressed 
    protected virtual void MagicAttack()
    {
        // if the player has a potion, use it
        UsePotion();
        fireBuffer = fireBufferStart;
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

        if (Time.timeScale == 1f &&
            GameManager.Instance.gameState == GameState.playing)
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
            Vector3 xoffset = Vector3.right * (int)playerType * 2;// for slight randomness
            transform.position = playerSpawn.transform.position + xoffset;

            // spawn in a y position of 2 
            Vector3 pos = transform.position;
            pos.y = 2f;
            transform.position = pos;
        }
    }

    private void KillPlayer()
    {
        if (FollowCam.Instance) FollowCam.Instance.RemovePlayerTransform(transform);
        if (myUI) myUI.HideText();

        Destroy(gameObject);
    }

    // removes either a potion, key or score
    // depending on what the player has
    public ItemStolen StealFromMe()
    {
        ItemStolen itemStolen;
        if (numPotions > 0)
        {
            numPotions--;
            itemStolen = ItemStolen.potion;
        }
        else if (numKeys > 0)
        {
            numKeys--;
            itemStolen = ItemStolen.key;
        }
        else
        {
            score -= 100;
            itemStolen = ItemStolen.score;
        }

        return itemStolen;
    }
}
