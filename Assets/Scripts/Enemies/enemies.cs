using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemies : MonoBehaviour
{
    //variables
    public float health;
    public float damage;
    public NavMeshAgent agent;
    public GameObject player;
    private Vector3 playerPos;

    private void Update()
    {
        playerPos = player.transform.position;

        agent.SetDestination(playerPos);
    }

    //functions

    void Attack()
    {
        Debug.Log("attack");
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Attack();
        }
    }
}
