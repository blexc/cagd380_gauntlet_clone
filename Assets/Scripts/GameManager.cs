using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    public int level;
    public GameObject pauseMenu;
    public GameObject nextLevelMenu;
    public GameObject infoBox;
    public GameState gameState;

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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
