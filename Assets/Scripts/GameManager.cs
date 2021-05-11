using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public enum GameState 
{
    playing,
    paused,
    gameOver,
    narrating
}

public class GameManager : MonoBehaviour
{
    // singleton
    static private GameManager _instance;
    static public GameManager Instance { get { return _instance; } }

    public GameState gameState;
    public int levelIndex;
    public List<GameObject> levels;
    
    // UI
    public GameObject pauseMenu;
    public GameObject nextLevelMenu;
    public GameObject infoBox;
    public List<PlayerUI> playerUIList;

    // private vars
    private GameObject currentLevel;

    private void Awake()
    {
        // ensure only one instance
        if (_instance != null && _instance != this)
            Destroy(_instance);
        else
            _instance = this;
    }

    public void Pause()
    {
        // TODO 
    }

    public void Resume()
    {
        // TODO 
    }

    public void GoToNextLevel()
    {
        // increment the index
        levelIndex++;
        levelIndex %= levels.Count;

        // if a level exists in the scene, destroy it
        if (currentLevel) Destroy(currentLevel);

        // spawn the new scene, and update the currentLevel var
        currentLevel = Instantiate(levels[levelIndex]);
    }
}
