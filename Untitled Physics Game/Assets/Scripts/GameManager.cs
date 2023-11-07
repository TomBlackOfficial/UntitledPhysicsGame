using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : SingletonTemplate<GameManager>
{
    //start and lvl over : create random lvl. remove said lvl from list.
    //round start : vehicle selection : countdown : hide ui
    //round end : update score : show lvl end ui : instant replay?

    #region Variables
    public List<GameObject> levelPrefabs = new List<GameObject>();
    public GameObject roundStartUI, roundEndUI, roundUI;
    public GameObject p1Vehicle, p2Vehicle;
    public int numberOfLevels = 9;

    public enum LEVEL_STATE
    {
        ROUND_START,
        ROUND_PLAYING,
        ROUND_END
    }
    public LEVEL_STATE currentState {  get; private set; }
    #endregion

    private void Start()
    {
        ClearScreen();
       // InitializePlayers();
       // InitializeLevels();
       // InitializeUI();

        //RoundStart
        //ChangeLevelState(LEVEL_STATE.ROUND_START);

        //IncreaseScore();
    }

    private void InitializePlayers()
    {
        p1Vehicle = Resources.Load("Players/Player_1") as GameObject;
        p2Vehicle = Resources.Load("Players/Player_2") as GameObject;
    }
    
    private void InitializeUI()
    {
        roundStartUI = Resources.Load("UI/roundStartUI") as GameObject;
        roundUI = Resources.Load("UI/roundUI") as GameObject;
        roundEndUI = Resources.Load("UI/roundEndUI") as GameObject;
    }

    private void InitializeLevels()
    {
        for(int i = 0; i < numberOfLevels; i++)
        {
            levelPrefabs.Add(Resources.Load($"Levels/level{i}") as GameObject);
        }        
    }

    public void LoadNewLevel()
    {
        int levelToLoad = Random.Range(0, levelPrefabs.Count);
        Instantiate(levelPrefabs[levelToLoad]);

        Instantiate(p1Vehicle, levelPrefabs[levelToLoad].GetComponent<LevelScript>().p1SpawnPoint, Quaternion.identity);
        Instantiate(p2Vehicle, levelPrefabs[levelToLoad].GetComponent<LevelScript>().p2SpawnPoint, Quaternion.identity);

        levelPrefabs.RemoveAt(levelToLoad);
    }

    private void ClearScreen()
    {
        PlayerController[] players = FindObjectsByType<PlayerController>(FindObjectsSortMode.None);
        print(players[0].gameObject.name);
        print(players[1].gameObject.name);
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
                ActivateUI(roundUI); 
            break;

            case LEVEL_STATE.ROUND_END:
                ActivateUI(roundEndUI); 
            break;
        }
    }

    private void ActivateUI(GameObject UIToActivate)
    {
        roundStartUI.SetActive(false);
        roundUI.SetActive(false);
        roundEndUI.SetActive(false);

        UIToActivate.SetActive(true);
    }

    private void StartCountdown()
    {
        //TODO
        ChangeLevelState(LEVEL_STATE.ROUND_PLAYING);
    }

    private void PlayerScored(GameObject player)
    {
        //TODO
        ChangeLevelState(LEVEL_STATE.ROUND_END);
    }
}
