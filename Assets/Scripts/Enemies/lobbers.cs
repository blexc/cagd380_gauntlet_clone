using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lobbers : enemies
{
    public GameObject bomb;
    public GameObject rock;
    private Vector3 direction;

    private void Start()
    {
        StartCoroutine(spawnBomb());
        StartCoroutine(spawnRock());
    }

    private void Update()
    {
        Movement();
    }

    IEnumerator spawnBomb()
    {
        while (true)
        {
            GameObject projectile = Instantiate(bomb);
            projectile.transform.position = transform.position;

            if (GetComponent<UnityEngine.AI.NavMeshAgent>().velocity.normalized != Vector3.zero)
            {
                direction = GetComponent<UnityEngine.AI.NavMeshAgent>().velocity.normalized;
            }

            projectile.GetComponent<enemyProjectile>().ShootProjectile(direction);

            yield return new WaitForSeconds(5f);
        }
    }
    
    IEnumerator spawnRock()
    {
        while (true)
        {
            GameObject projectile = Instantiate(rock);
            projectile.transform.position = transform.position;

            if (GetComponent<UnityEngine.AI.NavMeshAgent>().velocity.normalized != Vector3.zero)
            {
                direction = GetComponent<UnityEngine.AI.NavMeshAgent>().velocity.normalized;
            }

            projectile.GetComponent<enemyProjectile>().ShootProjectile(direction);

            yield return new WaitForSeconds(3f);
        }
    }
}
