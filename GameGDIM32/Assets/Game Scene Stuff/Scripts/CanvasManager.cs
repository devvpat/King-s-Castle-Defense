//Class written by: Dev Patel

using System.Collections;
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
    private TMPro.TextMeshProUGUI MP_PirateKingInfo;
    [SerializeField]
    private float TimeToDisablePirateKingInfo;

    [SerializeField]
    private GameObject PirateWinCanvas;
    [SerializeField]
    private GameObject CastleWinCanvas;

    [SerializeField]
    private GameObject PauseMenuCanvas;

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
            StartCoroutine(MP_PirateKingInfoDisplay());
        }
        GameplayManager._instance.OnPirateWin += EnablePirateWinCanvas;
        GameplayManager._instance.OnCastleWin += EnableCastleWinCanvas;        
    }

    private IEnumerator MP_PirateKingInfoDisplay()
    {
        MP_PirateKingInfo.gameObject.SetActive(true);
        MP_PirateKingInfo.text = $"Pirate King will spawn in {CharacterManager._instance.MP_PirateKingSpawnTime} seconds";
        yield return new WaitForSeconds(TimeToDisablePirateKingInfo);
        MP_PirateKingInfo.text = "";
        yield return new WaitForSeconds(CharacterManager._instance.MP_PirateKingSpawnTime - TimeToDisablePirateKingInfo);
        MP_PirateKingInfo.text = $"Pirate King spawning!";
        yield return new WaitForSeconds(1);
        MP_PirateKingInfo.text = "";
        CharacterManager._instance.MP_SpawnPirateKing();
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

    public void TogglePauseMenuCanvas()
    {
        PauseMenuCanvas.SetActive(!PauseMenuCanvas.activeSelf);
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
        MP_ScoreText[0].text = "Score: " + ScoreManager._instance.Scores[0];
        MP_ScoreText[1].text = "Score: " + ScoreManager._instance.Scores[1];
    }

    public void MP_UpdateCoins()
    {
        MP_CoinText[0].text = "Coins: " + CoinManager._instance.Coins[0];
        MP_CoinText[1].text = "Coins: " + CoinManager._instance.Coins[1];
    }
}
