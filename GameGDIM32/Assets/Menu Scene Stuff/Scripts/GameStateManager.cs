//Tien-Yi Lee
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    [SerializeField]
    private string MenuSceneName;
    [SerializeField]
    private string GameSceneName;

    public static GameStateManager instance; //singleton of game state manager

    private void Awake() //Dont destroy on load
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
            state = GameState.Menu;
        }
        else
        {
            Destroy(this);
        }
    }

    public static GameState state { get; private set; }

    public enum GameState //list gamestates
    {
        Menu, Playing, Paused
    }

    //Method added by Dev Patel
    public void TogglePause()
    {
        //for unpausing
        if (state == GameState.Paused)
        {
            Time.timeScale = 1;
            state = GameState.Playing;
            CanvasManager._instance.TogglePauseMenuCanvas();
        }
        //for pausing
        else
        {
            state = GameState.Paused;
            Time.timeScale = 0;
            CanvasManager._instance.TogglePauseMenuCanvas();
        }
    }

    //Following 4 methods created by both Dev Patel and Tien-Yi Lee
    public void NewGame() //starting a new game
    {
        state = GameState.Playing;
        SceneManager.LoadScene(GameStateManager.instance.GameSceneName);
    }

    public void QuitToTitle() //moving back to the MENU
    {
        if (state == GameState.Paused)
        {
            GameStateManager.instance.TogglePause(); //added by Dev Patel - turns off pause then quits to title
        }
        state = GameState.Menu;
        SceneManager.LoadScene(GameStateManager.instance.MenuSceneName);
    }

    public void QuitGame() //quit button on MENU
    {
        Application.Quit(); //Quit the game
    }
}