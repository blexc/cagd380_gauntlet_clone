using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grunt : enemies
{
    private void Update()
    {
        Movement();

        //check for death
        if (health <= 0)
        {
            Death();
        }
    }
}
