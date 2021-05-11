﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Item
{
    protected override void OnTriggerEnter(Collider other)
    {
        var player = other.gameObject.GetComponent<Player>();
        if (player)
        {
            player.AddKey();
            Destroy(gameObject);
        }
    } 
}
