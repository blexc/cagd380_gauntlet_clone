using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] GameObject[] players = null;
    int numPlayers = 0;

    private void Start()
    {
        PlayerInputManager.instance.playerPrefab = players[numPlayers];
    }

    public void OnPlayerJoined()
    {
        numPlayers++;
        PlayerInputManager.instance.playerPrefab = players[numPlayers];
        print("Player joined");
    }

    public void OnPlayerLeft()
    {
        numPlayers--;
        print("Player left");
    }
}