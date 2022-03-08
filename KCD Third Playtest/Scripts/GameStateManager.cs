//Tien-Yi Lee and Dev Patel
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class GameStateManager : MonoBehaviourPunCallbacks
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
            PlayerPrefs.DeleteAll();
        }
        else
        {
            Destroy(this);
        }
    }

    //Dev Patel
    public String GetMenuSceneName() { return MenuSceneName; }
    public String GetGameSceneName() { return GameSceneName; }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public static GameState state;

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

    //Dev Patel
    public void NewGame(bool singlePlayer) //starting a new game
    {
        state = GameState.Playing;
        if (singlePlayer)
        {
            PlayerPrefs.SetInt("SoloMode", 1);
            PlayerPrefs.SetInt("Online", 0);
        }
        else
        {
            PlayerPrefs.SetInt("SoloMode", 0);
            PlayerPrefs.SetInt("Online", 0);
        }
        SceneManager.LoadScene(GameStateManager.instance.GameSceneName); //use scene manager for singleplayer/local multiplayer
    }

    //Dev Patel
    public void RestartGameScene()
    {
        if (state == GameState.Paused)
        {
            GameStateManager.instance.TogglePause();
        }
        state = GameState.Playing;
        if (PlayerPrefs.GetInt("Online") == 0) SceneManager.LoadScene(GameStateManager.instance.GameSceneName);
        else if (PhotonNetwork.IsMasterClient) PhotonManager._instance.NewOnlineGame();
    }

    //Dev Patel
    public void QuitToTitle()
    {
        if (state == GameState.Paused)
        {
            GameStateManager.instance.TogglePause();
        }
        state = GameState.Menu;
        if (PlayerPrefs.GetInt("Online") == 1)
        {
            PhotonNetwork.LeaveRoom();
        }
        else
        {
            SceneManager.LoadScene(GameStateManager.instance.MenuSceneName);
        }
    }

    //Dev Patel
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(GameStateManager.instance.MenuSceneName);
        base.OnLeftRoom();
    }

    //Tien-Yi
    public void QuitGame() //quit button on MENU
    {
        Application.Quit(); //Quit the game
    }
}