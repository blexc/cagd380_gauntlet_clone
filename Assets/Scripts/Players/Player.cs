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
    [SerializeField] protected GameObject projectilePrefab;
    protected List<Item> inventory = new List<Item>();

    Rigidbody rb;

    // input vars
    float horizontal;
    float vertical;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical"); 
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector3 dir = new Vector3(horizontal, 0f, vertical);
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
