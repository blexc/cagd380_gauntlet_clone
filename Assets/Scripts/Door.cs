using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        var player = other.gameObject.GetComponent<Player>();
        if (player)
        {
            player.TryToUnlock(this);
        }
    }

    public void UnlockDoor()
    {
        Destroy(gameObject);
    }
}
