using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager _instance;

    [SerializeField]
    private GameObject SingleplayerCanvas;
    [SerializeField]
    private TMPro.TextMeshProUGUI SP_ScoreText;
    [SerializeField]
    private TMPro.TextMeshProUGUI SP_CoinText;

    //For future use
    [SerializeField]
    private GameObject MultiplayerCanvas;
    [SerializeField]
    private TMPro.TextMeshProUGUI[] MP_ScoreText;
    [SerializeField]
    private TMPro.TextMeshProUGUI[] MP_CoinText;

    [SerializeField]
    private GameData GD;

    public delegate void Data();
    public Data UpdateData;

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
    void Start()
    {
        //Turns on Singleplayer version of the canvas UI if the game is in singleplayer mode, else turn on multiplayer canvas UI
        if (GameplayManager._instance.GetSoloMode())
        {
            SingleplayerCanvas.SetActive(true);
            MultiplayerCanvas.SetActive(false);
            UpdateData = CanvasManager._instance.SP_UpdateScore;
            UpdateData += CanvasManager._instance.SP_UpdateCoins;
        }
        else
        {
            SingleplayerCanvas.SetActive(false);
            MultiplayerCanvas.SetActive(true);
            UpdateData = CanvasManager._instance.MP_UpdateScore;
            UpdateData += CanvasManager._instance.MP_UpdateCoins;
        }
        
    }

    public void SP_UpdateScore()
    {
        SP_ScoreText.text = "Score: " + GD.SP_Score;
    }

    public void SP_UpdateCoins()
    {
        SP_CoinText.text = "Coins: " + GD.SP_Coins;
    }

    public void MP_UpdateScore()
    {
        //to be implemented
    }

    public void MP_UpdateCoins()
    {
        //to be implemented
    }
}
