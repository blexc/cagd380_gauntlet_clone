using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    // singleton
    static private FollowCam _instance;
    static public FollowCam Instance { get { return _instance; } }

    [SerializeField] private float camY = 20f;
    [SerializeField] private float easing = 0.05f;

    private List<Transform> playerTransforms = new List<Transform>();
    Camera cam;

    private void Awake()
    {
        // ensure only one instance
        if (_instance != null && _instance != this)
            Destroy(_instance);
        else
            _instance = this;

        cam = GetComponent<Camera>();
    }

    private void FixedUpdate()
    {
        // if there are players in the scene...
        if (playerTransforms.Count > 0)
        {
            // take the average of all the positions of each active player
            Vector3 target = Vector3.zero;
            foreach (Transform t in playerTransforms)
                target += t.position;
            target /= playerTransforms.Count;

            // lerp the cur pos to the target pos
            target = Vector3.Lerp(transform.position, target, easing);
            target.y = camY;

            // update pos
            transform.position = target;
            cam.orthographicSize = camY;
        }
    }

    // adds the player to the list
    public void AddPlayerTransform(Transform t)
    {
        playerTransforms.Add(t);
    }

    // removes player from the list
    public void RemovePlayerTransform(Transform t)
    {
        playerTransforms.Remove(t);
    }
}




