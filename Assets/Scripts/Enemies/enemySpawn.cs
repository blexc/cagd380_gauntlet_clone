using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawn : MonoBehaviour
{
    public GameObject grunt;
    public GameObject ghost;
    public GameObject demon;
    public GameObject thief;
    public GameObject lobber;
    public GameObject sorcerer;
    
    public bool gruntSpawner;
    public bool ghostSpawner;
    public bool demonSpawner;
    public bool thiefSpawner;
    public bool lobberSpawner;
    public bool sorcererSpawner;

    private void Start()
    {
        if (gruntSpawner)
        {
            StartCoroutine(SpawnEnemy(grunt));
        }

        if (ghostSpawner)
        {
            StartCoroutine(SpawnEnemy(ghost));
        }

        if (demonSpawner)
        {
            StartCoroutine(SpawnEnemy(demon));
        }

        if (thiefSpawner)
        {
            StartCoroutine(SpawnEnemy(thief));
        }

        if (lobberSpawner)
        {
            StartCoroutine(SpawnEnemy(lobber));
        }

        if (sorcererSpawner)
        {
            StartCoroutine(SpawnEnemy(sorcerer));
        }
    }


    IEnumerator SpawnEnemy(GameObject enemyType)
    {
        while (true)
        {
            Instantiate(enemyType);
            enemyType.transform.position = this.gameObject.transform.position + new Vector3(2,0,2);
            yield return new WaitForSeconds(3f);
        }
    }
}
