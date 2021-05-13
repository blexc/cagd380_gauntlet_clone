using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private void Start()
    {
        // force y position
        Vector3 pos = transform.position;
        pos.y = 0.5f;
        transform.position = pos;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        // overriden by children
    } 
}
