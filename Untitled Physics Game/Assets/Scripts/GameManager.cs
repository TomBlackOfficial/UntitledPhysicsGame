using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonTemplate<GameManager>
{
    #region Variables
    [Header("Level")]
    public List<GameObject> levelPrefabs = new List<GameObject>();
    public GameObject currentLevel;
    private Vector2 originalGravity;
    private int numberOfLevels;

    [Header("UI")]
    public GameObject credits;
    public GameObject rules;
    public GameObject mainMenu;
    public GameObject gameOver;
    public GameObject scorePanel;

    [Header("Players")]
    public GameObject p1, p2;
    private PlayerController pc1, pc2;
    public ScorePanel p1ScorePanel, p2ScorePanel;
    private List<GameObject> p1List = new List<GameObject>();
    private List<GameObject> p2List = new List<GameObject>();

    private bool canChangeColor = true;
    private float colorChangeTime = 1f;
    public static event Action<bool> onGameOver;
    private bool isGameOver, gameStarted;

    private bool isPlayerDead;
    
    #endregion

    private void OnEnable()
    {
        TextMeshSharpener.buttonPressed += OnButtonPressed;
        TextMeshSharpener.buttonHover += MouseOver;
        PlayerController.playerDead += RoundOver;
    }

    private void OnDisable()
    {
        TextMeshSharpener.buttonPressed -= OnButtonPressed;
        TextMeshSharpener.buttonHover -= MouseOver;
        PlayerController.playerDead -= RoundOver;
    }

    private void OnButtonPressed(string buttonName)
    {
        switch(buttonName)
        {
            default:
                Vector2 tempGrav = Physics2D.gravity;
                Physics2D.gravity = -tempGrav;
                break;
            case ("play"):
                Play();
                break;
            case ("credits"):
                ToggleCredits();
                break;
            case ("rules"):
                ToggleRules();
                break;
            case ("quit"):
                Quit();
                break;
        }
    }
    
    private void MouseOver(string buttonName, TextMesh textMesh)
    {
        switch(buttonName)
        {
            default:
                if (canChangeColor)
                {
                    StartCoroutine(ColorChange());
                    textMesh.color = UnityEngine.Random.ColorHSV(0, 1, 1, 1, 1, 1);
                }
                break;
            case ("play"):
                if (canChangeColor)
                {
                    StartCoroutine(ColorChange());
                    textMesh.color = UnityEngine.Random.ColorHSV(0, .25f, 1, 1, 1, 1);
                }
                break;
            case ("credits"):
                if (canChangeColor)
                {
                    StartCoroutine(ColorChange());
                    textMesh.color = UnityEngine.Random.ColorHSV(.25f, .5f, 1, 1, 1, 1);
                }
                break;
            case ("rules"):
                if (canChangeColor)
                {
                    StartCoroutine(ColorChange());
                    textMesh.color = UnityEngine.Random.ColorHSV(.5f, .75f, 1, 1, 1, 1);
                }
                break;
            case ("quit"):
                if (canChangeColor)
                {
                    StartCoroutine(ColorChange());
                    textMesh.color = UnityEngine.Random.ColorHSV(.75f, 1, 1, 1, 1, 1);
                }
                break;
        }
    }

    private IEnumerator ColorChange()
    {
        yield return null;
        canChangeColor = false;
        yield return new WaitForSeconds(colorChangeTime);
        canChangeColor = true;
    }

    protected override void Awake()
    {
        base.Awake();
        InitializeLevels();
        InitializePlayers();

        AudioListener.volume = 0.4f;
    }

    private void Start()
    {
        originalGravity = Physics2D.gravity;
    }

    public void Play()
    {
        isGameOver = false;
        gameStarted = true;
        AudioManager.instance.UpdateBGM(AudioManager.GAME_STATES.PLAYING);

        Physics2D.gravity = originalGravity; 

        mainMenu.SetActive(false);
        scorePanel.SetActive(true);

        LoadNewLevel();
    }

    public void ToggleCredits()
    {
        credits.SetActive(!credits.activeInHierarchy);
    }
    
    public void ToggleRules()
    {
        rules.SetActive(!rules.activeInHierarchy);
    }

    public void Quit()
    {
        Application.Quit();
    }

    private void InitializePlayers()
    {
        GameObject[] players1 = Resources.LoadAll<GameObject>("Vehicles/p1");
        foreach (GameObject p in players1)
        {
                p1List.Add(p);            
        }
        GameObject[] players2 = Resources.LoadAll<GameObject>("Vehicles/p2");
        foreach (GameObject p in players2)
        {
                p2List.Add(p);            
        }
    }

    private void InitializeLevels()
    {
        GameObject[] levels = Resources.LoadAll<GameObject>("Levels");
        numberOfLevels = levels.Length;

        for(int i = 0; i < numberOfLevels; i++)
        {
            levelPrefabs.Add(levels[i]);            
        }        
    }

    private void LoadNewLevel()
    {
        isPlayerDead = false;
        Time.timeScale = 1f;

        //Destroy old players and level
        if (pc1)
            Destroy(pc1.gameObject);        
        if(pc2)
            Destroy(pc2.gameObject);        
        if(currentLevel)
            Destroy(currentLevel);        

        //Create level. remove level from list.
        int levelToLoad = UnityEngine.Random.Range(0, levelPrefabs.Count);
        currentLevel = Instantiate(levelPrefabs[levelToLoad]);

        //Create players. remove players from list.
        int p1Index = UnityEngine.Random.Range(0, p1List.Count);
        p1 = Instantiate(p1List[p1Index], levelPrefabs[levelToLoad].GetComponent<LevelScript>().p1SpawnPoint.position, Quaternion.identity);
        p1.TryGetComponent(out pc1);
        
        int p2Index = UnityEngine.Random.Range(0, p2List.Count);
        p2 = Instantiate(p2List[p2Index], levelPrefabs[levelToLoad].GetComponent<LevelScript>().p2SpawnPoint.position, Quaternion.identity);
        p2.TryGetComponent(out pc2);

        levelPrefabs.RemoveAt(levelToLoad);
        p1List.RemoveAt(p1Index);
        p2List.RemoveAt(p2Index);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }


    private void ActivateUI(GameObject UIToActivate)
    {
        UIToActivate.SetActive(true);
    }

    private IEnumerator Countown()
    {
        TextMeshProUGUI countdown = GameObject.Find("Countdown").GetComponent<TextMeshProUGUI>();
        int time = 3, timeToWait = 1;
        while(time > 0)
        {
            countdown.text = time.ToString();
            time--;
            yield return new WaitForSeconds(timeToWait);
        }
    }

    public void PlayerDied(bool isP1)
    {
        if (isPlayerDead)
            return;

        if(isP1)        
            p2ScorePanel.AddScore();        
        else
            p1ScorePanel.AddScore();

        isPlayerDead = true;
    }

    public void RoundOver()
    {
        CheckWinCondition();
        if (!isGameOver)
        {
            LoadNewLevel();
        }
    }

    private void CheckWinCondition()
    {
        if(p1ScorePanel.score >= 3)
        {
            GameOver(true);
        }
        else if(p2ScorePanel.score >= 3)
        {
            GameOver(false);
        }
    }

    private void GameOver(bool isP1)
    {
        isGameOver = true;
        gameStarted = false;
        gameOver.SetActive(true);
        AudioManager.instance.UpdateBGM(AudioManager.GAME_STATES.MAINMENU);

        onGameOver?.Invoke(isP1);
    }
}
