using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] GameObject[] players = null;
    [SerializeField] int numPlayers = 1;

    private void Start()
    {
        PlayerInputManager.instance.playerPrefab = players[numPlayers];
    }

    public void OnPlayerJoined()
    {
        numPlayers++;
        PlayerInputManager.instance.playerPrefab = players[numPlayers];
    }

    public void OnPlayerLeft()
    {
        numPlayers--;
    }
}