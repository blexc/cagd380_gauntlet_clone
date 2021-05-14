using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyProjectile : MonoBehaviour
{ 
    public int damage;
    public float speed;
    float distance;
    Vector3 startPos;

    private void Start()
    {
        startPos = this.gameObject.transform.position;
        Destroy(gameObject, 5.0f);
    }

    public void ShootProjectile(Vector3 direction)
    {
        var rb = GetComponent<Rigidbody>();
        rb.velocity = direction * speed * Time.fixedDeltaTime;

        if (rb.velocity == Vector3.zero)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player player;
            player = other.gameObject.GetComponent<Player>();
            player.TakeDamage(damage);
            Destroy(this.gameObject);
        }
    }
}
