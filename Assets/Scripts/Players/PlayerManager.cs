using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] GameObject[] players = null;
    [SerializeField] int curPlayerIndex = 0; // the player to spawn next

    private void Start()
    {
        PlayerInputManager.instance.playerPrefab = players[curPlayerIndex];
    }

    // sets up the next player prefab to be spawned 
    public void OnPlayerJoined()
    {
        int loopCount = 0;

        // iterate through player types until there is one that is 
        // not currently in the game.
        // loopCount is to prevent infinite loops
        while (GameManager.Instance.IsPlayerNumInGame(curPlayerIndex))
        {
            curPlayerIndex = (curPlayerIndex + 1) % players.Length;
            print(curPlayerIndex);
            loopCount++;

            // do update the player prefab (this probably wont happen)
            if (loopCount >= players.Length)
            {
                return;
            }
        }

        PlayerInputManager.instance.playerPrefab = players[curPlayerIndex];

        if (Warrior.instance && Valkyrie.instance && Wizard.instance && Elf.instance)
        {
            print("disabling joining.");
            PlayerInputManager.instance.DisableJoining();
        }
    }

    public void OnPlayerLeft()
    {
        // idk if we need this ??
    }
}