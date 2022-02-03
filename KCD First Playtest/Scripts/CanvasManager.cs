//Class written by: Dev Patel

using UnityEngine;

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
    private GameObject PirateWinCanvas;
    [SerializeField]
    private GameObject CastleWinCanvas;

    public delegate void Data();
    public Data UpdateDisplayedData;

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
    public void Setup()
    {
        //Turns on Singleplayer version of the canvas UI if the game is in singleplayer mode, else turns on the multiplayer canvas UI (not configured yet)
        //Also creates a delegate for updating the canvas display (update coins and score count), and adds methods to other delegates that get called when the game ends
        if (GameplayManager._instance.SoloMode)
        {
            SingleplayerCanvas.SetActive(true);
            MultiplayerCanvas.SetActive(false);
            UpdateDisplayedData = CanvasManager._instance.SP_UpdateScore;
            UpdateDisplayedData += CanvasManager._instance.SP_UpdateCoins;
        }
        else
        {
            SingleplayerCanvas.SetActive(false);
            MultiplayerCanvas.SetActive(true);
            UpdateDisplayedData = CanvasManager._instance.MP_UpdateScore;
            UpdateDisplayedData += CanvasManager._instance.MP_UpdateCoins;
        }
        GameplayManager._instance.OnPirateWin += EnablePirateWinCanvas;
        GameplayManager._instance.OnCastleWin += EnableCastleWinCanvas;        
    }

    private void EnableCastleWinCanvas()
    {
        CastleWinCanvas.SetActive(true);
        PirateWinCanvas.SetActive(false);
    }

    private void EnablePirateWinCanvas()
    {
        PirateWinCanvas.SetActive(true);
        CastleWinCanvas.SetActive(false);
    }

    public void SP_UpdateScore()
    {
        SP_ScoreText.text = "Score: " + ScoreManager._instance.Scores[0];
    }

    public void SP_UpdateCoins()
    {
        SP_CoinText.text = "Coins: " + CoinManager._instance.Coins[0];
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
