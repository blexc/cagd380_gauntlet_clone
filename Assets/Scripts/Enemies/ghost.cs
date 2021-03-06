using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ghost : enemies
{
    private void Update()
    {
        Movement();

        //check for death
        if (health <= 0)
        {
            Death();
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player player = other.gameObject.GetComponent<Player>();
            player.TakeDamage(damage);
            Destroy(this.gameObject);
        }
    }
}
