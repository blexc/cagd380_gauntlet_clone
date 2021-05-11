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
            player.AddPotion(this);

            // don't remove the gameobject, but make it not interactible
            GetComponent<SphereCollider>().enabled = false;
            GetComponent<MeshRenderer>().enabled = false;
        }
    } 

    public void UsePotion()
    {
        // TODO kill all enemies
        Destroy(gameObject);
    }
}
