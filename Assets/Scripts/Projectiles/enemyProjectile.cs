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
    }

    protected void Update()
    {
        speed *= Time.deltaTime;

        //generate the list of players, then run a function to determine the closest player
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        Transform t = GetClosestPlayer(players);

        if (t != null)
        {
            this.gameObject.transform.position = Vector3.Lerp(startPos, t.position, speed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player player;
            player = other.gameObject.GetComponent<Player>();
            player.TakeDamage(damage);
        }
        Destroy(this.gameObject);
    }

    Transform GetClosestPlayer(GameObject[] players)
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (GameObject potentialTarget in players)
        {
            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget.transform;
            }
        }

        return bestTarget;
    }
}
