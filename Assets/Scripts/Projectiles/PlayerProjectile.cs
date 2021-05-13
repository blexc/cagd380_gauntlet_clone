using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    [SerializeField] protected int movementSpeed;
    [SerializeField] protected int damage;

    void Start()
    {
        // destory object if it's been alive for more than 1 second
        Destroy(gameObject, 1f);
    }

    // sets the velocity of the projectile based on player direction
    public void ShootProjectile(Vector3 direction)
    {
        var rb = GetComponent<Rigidbody>();
        rb.velocity = direction * movementSpeed * Time.fixedDeltaTime;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        var eb = other.gameObject.GetComponent<enemies>();
        if (eb)
        {
            // TODO 
            //eb.TakeDamage(damage);
        }

        // destroy projectile if you hit a wall or an enemy
        if (other.CompareTag("Wall") || eb)
            Destroy(gameObject);
    }
}
