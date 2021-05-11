using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : Item
{
    [SerializeField] private int healAmount = 100;

    protected override void OnTriggerEnter(Collider other)
    {
        var player = other.gameObject.GetComponent<Player>();
        if (player)
        {
            player.Heal(healAmount);
            Destroy(gameObject);
        }

        // TODO destroy upon hitting it with proj
    }
}
