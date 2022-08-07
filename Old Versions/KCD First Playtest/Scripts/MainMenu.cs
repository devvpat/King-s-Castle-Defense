//Tien-Yi Lee
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame() //play button on MENU
    {
        GameStateManager.NewGame();  //moving to game scene
    }

    public void QuitGame() //quit button on MENU
    {
        Application.Quit(); //Quit the game
    }

    public void QuitToTitle() //quit button on GAME scene
    {
        GameStateManager.QuitToTitle(); //moving to menu scene
    }

    public void RestartGame() //restart button on GAME scene
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); //load the game scene again
    }
}
