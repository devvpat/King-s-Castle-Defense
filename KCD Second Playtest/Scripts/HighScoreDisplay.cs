//Class written by: Dev Patel

using UnityEngine;

public class HighScoreDisplay : MonoBehaviour
{
    [SerializeField]
    private TMPro.TextMeshProUGUI Text;

    void Start()
    {
        int soloWins = PlayerPrefs.GetInt("SoloWins");
        string victoryText = "" + soloWins;
        if (soloWins == 1) victoryText += " win";
        else victoryText += " wins";
        Text.text = "Singleplayer High Score: " + PlayerPrefs.GetInt("SoloHighScore") + ", " + victoryText;
    }

}
