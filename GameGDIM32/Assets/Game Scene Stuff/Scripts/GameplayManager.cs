//Class written by: Dev Patel

using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager _instance;

    public bool SoloMode { get; private set; }

    //Delegates for notifying when either the pirates win or the castle wins
    public delegate void OnWin();
    public OnWin OnCastleWin;
    public OnWin OnPirateWin;

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

    private void SelfSetup()
    {
        //Game only has singleplayer functionality right now, so solomode should always be true
        //if (PlayerPrefs.GetInt("SoloMode") > 0) SoloMode = true;
        //else SoloMode = false;
        SoloMode = true;
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
            //On solomode, turn off button spawner on loss and setup singleplayer pirate spawner
            OnPirateWin += SpawnHandler.DisableSpawn;
            SP_PirateSpawner._instance.Setup();
        }
    }

    //Looking into if there is a cleaner way to do this...
    private void Update()
    {
        //Using Unity's InputManager to check if a certain button is pressed
        if (Input.GetButtonDown("PauseToggle"))
        {
            GameStateManager.instance.TogglePause();   
        }
        if (GameStateManager.state == GameStateManager.GameState.Playing)
        {
            if (Input.GetButtonDown("SpawnCastle1"))
            {
                CharacterManager._instance.SpawnCharacter("C1", 1);
            }
            if (Input.GetButtonDown("SpawnCastle2"))
            {
                CharacterManager._instance.SpawnCharacter("C2", 1);
            }
            if (Input.GetButtonDown("SpawnCastle3"))
            {
                CharacterManager._instance.SpawnCharacter("C3", 1);
            }
            if (Input.GetButtonDown("SpawnCastle4"))
            {
                CharacterManager._instance.SpawnCharacter("C4", 1);
            }
            if (Input.GetButtonDown("SpawnCastle5"))
            {
                CharacterManager._instance.SpawnCharacter("C5", 1);
            }
            if (Input.GetButtonDown("SpawnPirate1"))
            {
                CharacterManager._instance.SpawnCharacter("P1", 2);
            }
            if (Input.GetButtonDown("SpawnPirate2"))
            {
                CharacterManager._instance.SpawnCharacter("P2", 2);
            }
            if (Input.GetButtonDown("SpawnPirate3"))
            {
                CharacterManager._instance.SpawnCharacter("P3", 2);
            }
            if (Input.GetButtonDown("SpawnPirate4"))
            {
                CharacterManager._instance.SpawnCharacter("P4", 2);
            }
            if (Input.GetButtonDown("SpawnPirate5"))
            {
                CharacterManager._instance.SpawnCharacter("P5", 2);
            }
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
    }
}
