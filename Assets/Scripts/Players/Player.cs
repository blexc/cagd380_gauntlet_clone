using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] protected int score;
    [SerializeField] protected int health;
    [SerializeField] protected int damage;
    [SerializeField] protected int armor;
    [SerializeField] protected int movementSpeed;
    [SerializeField] protected int attackSpeed;
    protected List<Item> inventory = new List<Item>();

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical"); 
        Vector3 dir = new Vector3(horizontal, 0f, vertical);

        rb.velocity = movementSpeed * Time.fixedDeltaTime * dir;
    }

    public void AddItem(GameObject item)
    {
        // TODO 
    }

    public void TakeDamage(int value)
    {
        // TODO 
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
