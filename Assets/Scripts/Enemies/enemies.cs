using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemies : MonoBehaviour
{
    //variables
    Vector3 currentPos;
    public float distance;
    public float health;
    public int damage;
    public NavMeshAgent agent;
    private int currentPlayers; //current number of players in game
    private List<Vector3> playersTransforms; //a list of all the active players
    Player player; //an instance of the player that was just collided with

    //timer stuff
    float timer;
    float timerStart;

    private void Start()
    {
        timerStart = 0.5f;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player = other.gameObject.GetComponent<Player>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player = null;
        }
    }

    //functions
    public void Death()
    {
        Destroy(this.gameObject);
    }

    public void takeDamage(int damage)
    {
        health = health - damage;
    }

    public void Movement()
    {
        //generate the list of players, then run a function to determine the closest player
        //if the closest player is not null, move towards it
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        Transform t = GetClosestPlayer(players);
        currentPos = this.gameObject.transform.position;

        if (t != null)
        {
            distance = Vector3.Distance(currentPos, t.position);
            agent.SetDestination(t.position);
        }

        //deal damage over time on contact
        timer -= Time.deltaTime;
        if (timer <= 0 && player != null)
        {
            timer = timerStart;
            player.TakeDamage(damage);
        }
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
