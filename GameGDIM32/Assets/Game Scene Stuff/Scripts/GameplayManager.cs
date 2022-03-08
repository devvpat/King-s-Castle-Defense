//Class written by: Dev Patel

using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager _instance;

    public bool SoloMode { get; private set; }
    public bool OnlineMode { get; private set; }

    //Delegates for notifying when either the pirates win or the castle wins
    public delegate void OnWin();
    public OnWin OnCastleWin;
    public OnWin OnPirateWin;

    [SerializeField]
    private GameObject PlayerPrefab;

    private List<GameObject> PlayerList = new List<GameObject>();

    private void Awake()
    {

        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    // Start is called before the first frame update
    //Sets itself up first because other managers rely on the state of solomode
    void Start()
    {

        Time.timeScale = 1; //Tien-Yi added this, so game will run when play again
        SelfSetup();
        SetupOthersManagers();
        OnCastleWin += CastleWin;
        OnPirateWin += PirateWin;
        CanvasManager._instance.UpdateDisplayedData();
        CoinManager._instance.BeginGeneration();
        ScoreManager._instance.StartScoreCount();
        SpawnHandler.EnableSpawn(); //Tien-Yi added this, for turning on the spawn buttons when game starts
    }

    //creates player characters (which are used for input)
    private void SelfSetup()
    {
        PlayerList.Add(Instantiate(PlayerPrefab));
        PlayerList[0].GetComponent<PlayerChar>().SetPlayerNum(1);
        PlayerList[0].name = "Player 1";
        if (PlayerPrefs.GetInt("SoloMode") == 1)
        {
            SoloMode = true;           
        }
        else
        {
            SoloMode = false;
            PlayerList.Add(Instantiate(PlayerPrefab));
            PlayerList[1].GetComponent<PlayerChar>().SetPlayerNum(2);
            PlayerList[1].name = "Player 2";
            if (PlayerPrefs.GetInt("Online") == 1) OnlineMode = true;
            else OnlineMode = false;
        }
    }

    private void SetupOthersManagers()
    {
        ScoreManager._instance.Setup();
        CoinManager._instance.Setup();
        CameraManager._instance.Setup();
        CanvasManager._instance.Setup();
        CharacterManager._instance.Setup();
        SpecialEffectManager._instance.Setup();
        if (SoloMode)
        {
            //On solomode, setup singleplayer ai
            //SP_PirateSpawner._instance.Setup(); //random spawner
            AIBehaviour._instance.Setup();
        }
        OnCastleWin += SpawnHandler.DisableSpawn;
        OnPirateWin += SpawnHandler.DisableSpawn;
    }

    //Looking into if there is a cleaner way to do this...
    private void Update()
    {
        //Using Unity's InputManager to check if a certain button is pressed
        if (Input.GetButtonDown("PauseToggle"))
        {
            GameStateManager.instance.TogglePause();   
        }
    }

    //Two methods for winning that have same body because in the future, each one will display a unique message/picture
    private void PirateWin()
    {
        Time.timeScale = 0;
    }

    private void CastleWin()
    {
        Time.timeScale = 0;
        if (SoloMode)
        {
            int score = ScoreManager._instance.Scores[0];
            int highScore = PlayerPrefs.GetInt("SoloHighScore");
            //faster win (lower score) is "better"
            PlayerPrefs.SetInt("SoloHighScore", Mathf.Min(score, highScore));
            PlayerPrefs.SetInt("SoloWins", PlayerPrefs.GetInt("SoloWins") + 1);
        }
    }
}
