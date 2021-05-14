using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class demon : enemies
{
    public GameObject fireBall;

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
        if (distance <= 3)
        {
            Instantiate(fireBall);
            fireBall.transform.position = this.gameObject.transform.position;
        }

        yield return new WaitForSeconds(2f);
    }
}
