using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Item
{
    protected override void OnTriggerEnter(Collider other)
    {
        var player = other.gameObject.GetComponent<Player>();
        if (player)
        {
            player.AddKey(this);

            // don't remove the gameobject, but make it not interactible
            GetComponent<SphereCollider>().enabled = false;
            GetComponent<MeshRenderer>().enabled = false;
        }
    } 

    public void UnlockDoor(Door door)
    {
        door.UnlockDoor();
    }
}
