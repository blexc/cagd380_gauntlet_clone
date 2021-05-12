using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : Item
{
    [SerializeField] private int healAmount = 100;

    protected override void OnTriggerEnter(Collider other)
    {
        var player = other.gameObject.GetComponent<Player>();
        var projectile = other.GetComponent<PlayerProjectile>();
        if (player)
        {
            player.Heal(healAmount);

            Narrator.Instance.SayLine(NarratorLine.foodHeals);
            Destroy(gameObject);
        }
        else if (projectile)
        {
            Narrator.Instance.SayLine(NarratorLine.playerShotFood);
            Destroy(gameObject);
        }
    }
}
