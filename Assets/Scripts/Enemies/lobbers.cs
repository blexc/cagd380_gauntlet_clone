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
        StartCoroutine(spawnFireball());
    }

    private void Update()
    {
        Movement();
    }

    IEnumerator spawnFireball()
    {
        while (true)
        {
            GameObject projectile = Instantiate(bomb);
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
