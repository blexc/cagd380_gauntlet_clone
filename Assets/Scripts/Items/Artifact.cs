using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artifact : Item
{
    [SerializeField] int pointAmount = 100;

    protected override void OnTriggerEnter(Collider other)
    {
        var player = other.gameObject.GetComponent<Player>();
        if (player)
        {
            player.AddPoints(pointAmount);
            Narrator.Instance.SayLine(NarratorLine.artifactPoints);
            Destroy(gameObject);
        }
    }
}
