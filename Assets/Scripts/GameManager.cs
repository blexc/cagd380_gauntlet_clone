using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;

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

    // TODO change to false on turnin
    public bool isDebugging;

    public GameState gameState;
    public int levelIndex;
    public List<GameObject> levels;
    
    // UI
    public GameObject pausePanel;
    public GameObject inGamePanel;
    public GameObject gameOverPanel;
    public GameObject infoPanel; 
    public List<PlayerUI> playerUIList;

    // used when no player is alive
    public GameObject spareCamera;

    // private vars
    private GameObject currentLevel;

    private void Awake()
    {
        // ensure only one instance
        if (_instance != null && _instance != this)
            Destroy(_instance);
        else
            _instance = this;
        
        // for these to default values
        inGamePanel.SetActive(true);
        spareCamera.SetActive(true);

        infoPanel.SetActive(false);
        pausePanel.SetActive(false);
        gameOverPanel.SetActive(false);
        
        BuildLevel();
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

    void BuildLevel()
    {
        // if a level exists in the scene, destroy it
        if (currentLevel) Destroy(currentLevel);

        // spawn the new scene, and update the currentLevel var
        currentLevel = Instantiate(levels[levelIndex]);

        // put the players in the new spawn point
        Player[] players = FindObjectsOfType<Player>();
        foreach (Player p in players)
        {
            p.PlaceAtSpawn();
        }

        // bake navmesh at runtime
        var nm = GameObject.FindObjectOfType<NavMeshSurface>();
        if (nm) nm.BuildNavMesh();
        else Debug.LogError("Nav mesh not in scene!");
    }

    public void GoToNextLevel()
    {
        // increment the index
        levelIndex++;
        levelIndex %= levels.Count;
        BuildLevel();
    }
    
    // called by narrator
    // display info for the duration of the audio clip
    public void DisplayInfo(string text, float pauseTime)
    {
        infoPanel.SetActive(true);
        Time.timeScale = 0f; // pause the game!
        StartCoroutine(HideInfoAndResume(pauseTime));

        var textBox = infoPanel.GetComponentInChildren<TextMeshProUGUI>();
        if (textBox) textBox.text = text;
    }

    // resume the game over time
    IEnumerator HideInfoAndResume(float pauseTime)
    {
        yield return new WaitForSecondsRealtime(pauseTime);
        infoPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    // if player 1 in game? 2?, etc.
    public bool IsPlayerNumInGame(int num)
    {
        PlayerType playerType = (PlayerType)num;
        switch (playerType)
        {
            case PlayerType.warrior:
                if (Warrior.instance != null) return true;
                break;
            case PlayerType.valkyrie:
                if (Valkyrie.instance != null) return true;
                break;
            case PlayerType.wizard:
                if (Wizard.instance != null) return true;
                break;
            case PlayerType.elf:
                if (Elf.instance != null) return true;
                break;
        }

        return false;
    }

    public void OnGameOver()
    {
        gameState = GameState.gameOver;
        gameOverPanel.SetActive(true);
        spareCamera.SetActive(true);
        Destroy(PlayerInputManager.instance);
        StartCoroutine(RestartGame());
    }

    // reloads the scene
    IEnumerator RestartGame()
    {
        yield return new WaitForSecondsRealtime(2f);
        Scene s = SceneManager.GetSceneByBuildIndex(0);
        SceneManager.LoadScene(s.name);
    }
}

















