using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
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
    public bool isDebugging = true;

    public GameState gameState;
    public int levelIndex;
    public List<GameObject> levels;
    
    // UI
    public GameObject pausePanel;
    public GameObject inGamePanel;
    public GameObject infoPanel; 
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
        
        // for these to default values
        inGamePanel.SetActive(true);
        infoPanel.SetActive(false);
        pausePanel.SetActive(false);
        
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

        FollowCam.Instance.ForcePosToPOI();

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
}
