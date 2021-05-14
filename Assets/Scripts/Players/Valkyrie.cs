using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Valkyrie : Player
{
    public static Valkyrie instance;

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
