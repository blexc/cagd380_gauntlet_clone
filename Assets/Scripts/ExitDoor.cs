using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    /// <summary>
    /// if a player hits the exit, go to the next level
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        var player = other.gameObject.GetComponent<Player>();
        if (player) GameManager.Instance.GoToNextLevel();
    }
}
