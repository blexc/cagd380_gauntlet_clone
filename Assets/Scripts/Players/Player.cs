using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] protected int playerNumber;
    [SerializeField] protected int score;
    [SerializeField] protected int health;
    [SerializeField] protected int damage;
    [SerializeField] protected int armor;
    [SerializeField] protected int movementSpeed;
    [SerializeField] protected int attackSpeed;
    [SerializeField] protected GameObject projectilePrefab;
    protected List<Item> inventory = new List<Item>();

    Rigidbody rb;
    Vector2 movementInput;

    #region recieve input
    public void OnMove(InputAction.CallbackContext ctx) =>
        movementInput = ctx.ReadValue<Vector2>();

    public void OnFire(InputAction.CallbackContext ctx)
    {
        if (ctx.performed) MeleeAttack();
    }

    public void OnMagic(InputAction.CallbackContext ctx)
    {
        if (ctx.performed) ProjectileAttack(); 
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
        Vector3 dir = new Vector3(movementInput.x, 0f, movementInput.y);
        rb.velocity = movementSpeed * Time.fixedDeltaTime * dir;
    }

    public void AddItem(GameObject item)
    {
        // TODO 
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

    protected virtual void MeleeAttack()
    {
        // TODO 
    }

    protected virtual void ProjectileAttack()
    {
        // TODO 
    }
}
