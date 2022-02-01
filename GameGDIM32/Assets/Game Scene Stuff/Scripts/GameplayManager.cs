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

    private void PirateWin()
    {
        Time.timeScale = 0;
    }

    private void CastleWin()
    {
        Time.timeScale = 0;
    }
}
