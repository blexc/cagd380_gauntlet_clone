using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    // singleton
    static private FollowCam _instance;
    static public FollowCam Instance { get { return _instance; } }

    public float camY = 20f;
    public float easing = 0.05f;

    public GameObject poi;

    private void Awake()
    {
        // ensure only one instance
        if (_instance != null && _instance != this)
            Destroy(_instance);
        else
            _instance = this;
    }

    private void FixedUpdate()
    {
        Vector3 destination;

        if (!poi)
        {
            destination = Vector3.zero;
        }
        else
        {
            destination = poi.transform.position;
            destination = Vector3.Lerp(transform.position, destination, easing);
        }

        destination.y = camY;
        transform.position = destination;
        Camera.main.orthographicSize = camY;
    }
}




