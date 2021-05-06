using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemies : MonoBehaviour
{
    //variables
    public float health;
    public float damage;
    public float moveSpeed;

    public GameObject player;
    public float u;
    public Vector3 p0, p1, p01;

    bool moving = true;

    private void Start()
    {
        u = moveSpeed / 100;
    }

    private void FixedUpdate()
    {
        if (moving)
        {
            Movement();
        }
    }

    //functions
    void Movement()
    {
        //get the position of this (what we are attached to) and poi
        p0 = this.transform.position;
        p1 = player.transform.position;

        //interpolate between them
        p01 = (1 - u) * p0 + u * p1;

        //move to new position
        this.transform.position = p01;
    }

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
