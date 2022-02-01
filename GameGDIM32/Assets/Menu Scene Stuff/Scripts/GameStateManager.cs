//Tien-Yi Lee
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    [SerializeField]
    private List<string> scenes = new List<string>(); //make the list of all scenes

    private int sceneNumber = 0;


    private static GameStateManager instance; //singleton of game state manager

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
        instance.sceneNumber++;
        SceneManager.LoadScene(instance.scenes[instance.sceneNumber]);
    }

   

    public static void QuitToTitle() //moving back to the MENU
    {
        state = GameState.Menu;
        instance.sceneNumber = 0;
        SceneManager.LoadScene(instance.scenes[instance.sceneNumber]);
    }

    

        
}