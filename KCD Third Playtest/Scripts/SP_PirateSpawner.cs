//Class written by: Dev Patel

//No longer used - replaced by singleplayer AI

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SP_PirateSpawner : MonoBehaviour
{
    public static SP_PirateSpawner _instance;

    private float Timer;

    [SerializeField]
    private float WaitTime;

    private List<string> SpawnList;

    //Since Unity doesn't serialize dictionaries from what I can see, this is a sort of work around though a bit rough
    //The indicies of both lists are intended to match so that for example, element 1 (index 0) of both lists are meant to go together
    [SerializeField]
    private List<float> SpawnTimers;
    [SerializeField]
    private List<string> CharacterToSpawn;

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
        Timer = 0;
        SpawnList = new List<string>();
        StartCoroutine(TimerCounter());
    }

    //Sets the timer to Time.time which is how long since the scene started
    //Then it checks and spawns any enemies that need to be spawned
    private IEnumerator TimerCounter()
    {
        CheckEnemySpawn();
        yield return new WaitForSeconds(WaitTime);
        Timer += WaitTime;
        StartCoroutine(TimerCounter());
    }

    private void CheckEnemySpawn()
    {
        //If there are still enemies that haven't been spawned, if those character's spawntime (how long after starting the scene to spawn them)
        //is less than or equal to the current amount of time since the scene started (Timer variable), spawn them
        if (CharacterToSpawn.Count > 0 && SpawnTimers.Count > 0 && CharacterToSpawn.Count == SpawnTimers.Count)
        {
            //Because the list of spawntimes is already ordered from least to greatest, the 1st element (0 index) will always be the next one to spawn
            float curSpawnTime = SpawnTimers[0];
            //Get a list of all objects that haven't been spawned with a spawn timer <= to current Time
            while (curSpawnTime <= Timer)
            {
                curSpawnTime = AddToSpawnList();
            }
            //Spawn each character that needs to be spawned
            foreach (string name in SpawnList)
            {
                CharacterManager._instance.SpawnCharacter(name, 2);
            }
            SpawnList.Clear();
        }        
    }

    private float AddToSpawnList()
    {
        //Adds the 1st element (0 index) of CharacterToSpawn (name of character to spawn) to a list of things that need to be spawned
        //Then removes the 1st element (0 index) from both SpawnTimers and CharacterToSpawn lists (becasue they correspond and their appropriate data has been processed)
        //If there is a valid next spawn time (whether spawnable in the moment or the future), return that value
        //Else (meaning no more valid spawns), return Int32.MaxValue to make sure nothing else tries to get spawned
        //Edge case when a player plays the game for Int32.MaxValue seconds, but that's not feasible (hopefully no one attempts this)
        SpawnList.Add(CharacterToSpawn[0]);
        SpawnTimers.RemoveAt(0);
        CharacterToSpawn.RemoveAt(0);
        if ((SpawnTimers.Count <= 0 || CharacterToSpawn.Count <= 0) || CharacterToSpawn.Count != SpawnTimers.Count) return Int32.MaxValue;
        else return SpawnTimers[0];
    }

    
}
