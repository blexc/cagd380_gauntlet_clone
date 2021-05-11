using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Item
{
    public void UnlockDoor(Door door)
    {
        door.UnlockDoor();
    }
}
