using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thief : enemies
{
    private ItemStolen itemStolen;

    public GameObject potion;
    public GameObject key;
    public GameObject artifact;

    private Vector3 spawnPos;
    private Vector3 currentPos;

    private bool stole;

    private void Start()
    {
        stole = false;
        spawnPos = this.gameObject.transform.position - new Vector3(2, 0, 2);
    }

    private void Update()
    {
        currentPos = this.gameObject.transform.position;

        if (!stole)
        {
            Movement();
        }

        if (health <= 0)
        {
            if (itemStolen == ItemStolen.potion)
            {
                Instantiate(potion);
            }

            if (itemStolen == ItemStolen.key)
            {
                Instantiate(key);
            }

            if (itemStolen == ItemStolen.score)
            {
                Instantiate(artifact);
            }

            Death();
        }

        if (stole)
        {
            agent.SetDestination(spawnPos);
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player player = other.gameObject.GetComponent<Player>();
            itemStolen = player.StealFromMe();
            stole = true;
        }

        if (other.gameObject.CompareTag("espawn"))
        {
            Destroy(this.gameObject);
        }
    }
}
