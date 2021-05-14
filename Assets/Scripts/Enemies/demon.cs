using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class demon : enemies
{
    public GameObject fireBall;
    private Vector3 direction;

    private void Awake()
    {
        StartCoroutine(spawnFireball());
    }

    private void Update()
    {
        Movement();

        //check for death
        if (health <= 0)
        {
            Death();
        }
    }

    IEnumerator spawnFireball()
    {
        while (true)
        {
            GameObject projectile = Instantiate(fireBall);
            projectile.transform.position = transform.position;

            if (GetComponent<NavMeshAgent>().velocity.normalized != Vector3.zero)
            {
                direction = GetComponent<NavMeshAgent>().velocity.normalized;
            }

            projectile.GetComponent<enemyProjectile>().ShootProjectile(direction);

            yield return new WaitForSeconds(2f);
        }
    }
}
