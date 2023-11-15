using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : SingletonTemplate<GameManager>
{
    //start and lvl over : create random lvl. remove said lvl from list.
    //round start : vehicle selection : countdown : hide ui
    //round end : update score : show lvl end ui : instant replay?

    #region Variables
    [Header("Level")]
    public List<GameObject> levelPrefabs = new List<GameObject>();
    public GameObject currentLevel;
    public int numberOfLevels = 9;

    [Header("UI")]
    public GameObject roundStartUI, roundEndUI, roundPlayingUI, credits, mainMenu;
    public TextMeshProUGUI p1Score, p2Score;

    [Header("Players")]
    public GameObject p1Vehicle, p2Vehicle;
    private PlayerController pc1, pc2;

    public enum LEVEL_STATE
    {
        START_GAME,
        ROUND_START,
        ROUND_PLAYING,
        ROUND_END,
        GAME_OVER
    }
    public LEVEL_STATE currentState {  get; private set; }
    private Coroutine state;
    #endregion

    private void OnEnable()
    {
        TextMeshSharpener.buttonPressed += OnButtonPressed;
        PlayerController.playerDead += PlayerDied;
    }

    private void OnDisable()
    {
        TextMeshSharpener.buttonPressed -= OnButtonPressed;
        PlayerController.playerDead -= PlayerDied;
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
            case ("quit"):
                Quit();
                break;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        InitializeLevels();
        InitializePlayers();
        InitializeUI();
    }

    private void Start()
    {
        
    }

    private void Update()
    {

    }

    public void Play()
    {
        mainMenu.SetActive(false);
        LoadNewLevel();

        //Instantiate(roundStartUI);
        Instantiate(roundPlayingUI);
        Instantiate(roundEndUI);

        ActivateUI(roundStartUI);
    }

    public void ToggleCredits()
    {
        credits.SetActive(!credits.activeInHierarchy);
    }

    public void Quit()
    {
        Application.Quit();
    }

    private void InitializePlayers()
    {
        p1Vehicle = Resources.Load("Players/Player_1") as GameObject;
        p2Vehicle = Resources.Load("Players/Player_2") as GameObject;
    }
    
    private void InitializeUI()
    {
        roundStartUI = Resources.Load<GameObject>("UI/RoundStartUI");
        roundPlayingUI = Resources.Load<GameObject>("UI/RoundPlayingUI");
        roundEndUI = Resources.Load<GameObject>("UI/RoundEndUI");
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
        if(pc2)
        {
            Destroy(pc1.gameObject);
        }
        if(pc1)
        {
            Destroy(pc2.gameObject);
        }
        if(currentLevel)
        {
            Destroy(currentLevel);
        }

        //PlayerController[] players = FindObjectsByType<PlayerController>(FindObjectsSortMode.None);
        int levelToLoad = Random.Range(0, levelPrefabs.Count);
        currentLevel = Instantiate(levelPrefabs[levelToLoad]);
        levelPrefabs.RemoveAt(levelToLoad);

        Instantiate(p1Vehicle, levelPrefabs[levelToLoad].GetComponent<LevelScript>().p1SpawnPoint.position, Quaternion.identity);
        p1Vehicle.TryGetComponent(out pc1);
        Instantiate(p2Vehicle, levelPrefabs[levelToLoad].GetComponent<LevelScript>().p2SpawnPoint.position, Quaternion.identity);
        p2Vehicle.TryGetComponent(out pc2);
    }

    public void ChangeLevelState(LEVEL_STATE levelState)
    {
        currentState = levelState;

        switch (levelState)
        {
            case LEVEL_STATE.ROUND_START:
                ActivateUI(roundStartUI);                
            break;

            case LEVEL_STATE.ROUND_PLAYING:
                ActivateUI(roundPlayingUI);
                StopCoroutine(state);
            break;

            case LEVEL_STATE.ROUND_END:
                ActivateUI(roundEndUI); 
            break;
        }
    }

    private void ActivateUI(GameObject UIToActivate)
    {
        roundStartUI.SetActive(false);
        roundPlayingUI.SetActive(false);
        roundEndUI.SetActive(false);

        UIToActivate.SetActive(true);
    }

    private void StartCountdown()
    {
        TextMeshProUGUI countdown = GameObject.Find("Countdown").GetComponent<TextMeshProUGUI>();
        state = StartCoroutine(Countown(countdown));
    }

    private IEnumerator Countown(TextMeshProUGUI countdown)
    {
        int time = 3, timeToWait = 1;
        while(time > 0)
        {
            countdown.text = time.ToString();
            time--;
            yield return new WaitForSeconds(timeToWait);
        }
    }

    private void PlayerDied(PlayerController pc)
    {
        if(pc.isPlayer1)
        {
            pc1.score++;
            p1Score.text = pc1.score.ToString();
            CheckWinCondition();
        }
        else
        {
            pc2.score++;
            p2Score.text = pc2.score.ToString();
            CheckWinCondition();
        }        
    }

    private void CheckWinCondition()
    {
        if(pc1.score >= 5)
        {
            GameOver(pc1);
        }
        else if(pc2.score >= 5)
        {
            GameOver(pc2);
        }
    }

    private void GameOver(PlayerController winner)
    {
        if(winner.isPlayer1)
        {
            print("Player 1 Wins!");
        }
        else
        {
            print("Player 2 Wins!");
        }
    }
}
