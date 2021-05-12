using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : Item
{
    protected override void OnTriggerEnter(Collider other)
    {
        var player = other.gameObject.GetComponent<Player>();
        if (player)
        {
            player.AddPotion();
            Narrator.Instance.SayLine(NarratorLine.savePotion);
            Destroy(gameObject);
        }
    } 
}
