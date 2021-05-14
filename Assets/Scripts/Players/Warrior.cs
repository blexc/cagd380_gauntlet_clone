using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : Player
{
    public static Warrior instance;

    protected override void Start()
    {
        // ensure only one instance
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
        }

        base.Start();
    }
}
