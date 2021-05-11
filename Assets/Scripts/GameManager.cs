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
    public GameObject pausePanel;
    public GameObject inGamePanel;
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
        gameState = GameState.paused;

        inGamePanel.SetActive(false);
        pausePanel.SetActive(true);

        Time.timeScale = 0f;
        
        // TODO make enemies stop moving??
    }

    public void Resume()
    {
        gameState = GameState.playing;

        inGamePanel.SetActive(true);
        pausePanel.SetActive(false);

        Time.timeScale = 1f;
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
