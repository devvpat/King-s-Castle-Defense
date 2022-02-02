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
            GameStateManager.state = GameState.Menu;
        }
        else
        {
            Destroy(this);
        }
    }

    public static GameState state { get; private set; }

    public enum GameState //list gamestates
    {
        Menu, Playing
    }

    public static void NewGame() //starting a new game
    {
        state = GameState.Playing;
        SceneManager.LoadScene(GameStateManager.instance.GameSceneName);
    }

    public static void QuitToTitle() //moving back to the MENU
    {
        state = GameState.Menu;
        SceneManager.LoadScene(GameStateManager.instance.MenuSceneName);
    }

    

        
}