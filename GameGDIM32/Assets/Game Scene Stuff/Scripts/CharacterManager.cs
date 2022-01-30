//Class written by: Dev Patel

using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager _instance;

    //Since Unity doesn't serialize dictionaries from what I can see, this is a sort of work around though a bit rough
    //the first list is public because SpawnHandler needs and also because Unity doesn't seralize properties (from what I can see)
    public List<string> CharacterName;
    [SerializeField]
    private List<GameObject> CharacterPrefab;

    [SerializeField]
    private GameObject CastleArmy;
    public Army CastleArmyComp { get; private set; }
    [SerializeField]
    private GameObject PirateArmy;
    public Army PirateArmyComp { get; private set; }

    [SerializeField]
    private float MaxSpawnY;
    [SerializeField]
    private float MinSpawnY;

    [SerializeField]
    private Vector3 CastleKingSpawn;
    [SerializeField]
    private Vector3 PirateKingSpawn;

    public KingCharacterDecorator CKD { get; private set; }
    public KingCharacterDecorator PKD { get; private set; }

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

    public void Setup()
    {
        CastleArmyComp = CastleArmy.GetComponent<Army>();
        PirateArmyComp = PirateArmy.GetComponent<Army>();
        InitialSpawn();
    }

    //spawns the king as well as 2 castle troops before the first enemies spawn
    private void InitialSpawn()
    {
        SpawnCharacter("CKing", 1);
        Spawn(1);
        Spawn(2);
    }

    //takes the characterName and references the CharacterName list to find the corresponding index in the CharacterPrefab list
    //used to check if the player is able to spawn the character (if they try to buy one using coins), sends spawn data to Spawn() if spawning is allowed
    public void SpawnCharacter(string characterName, int player)
    {
        
        int index = CharacterName.IndexOf(characterName);
        if (index != -1)
        {
            Character characterCharComp = CharacterPrefab[index].GetComponent<Character>();
            if (characterCharComp != null)
            {
                //if in singleplayer and the player has enough coins, spawn character (always spawn pirate).
                //if in multiplayer, check the specific player's coin bank and spawn if they have enough
                if (GameplayManager._instance.SoloMode)
                {
                    if (characterCharComp.GetIsPirate())
                    {
                        Spawn(index);
                    }
                    else if (CoinManager._instance.Coins[0] >= characterCharComp.CharacterStats.Cost)
                    {
                        CoinManager._instance.Coins[0] -= characterCharComp.CharacterStats.Cost;
                        CanvasManager._instance.UpdateDisplayedData();
                        Spawn(index);
                    }
                }
                else if (CoinManager._instance.Coins[player - 1] >= characterCharComp.CharacterStats.Cost)
                {
                    CoinManager._instance.Coins[player - 1] -= characterCharComp.CharacterStats.Cost;
                    CanvasManager._instance.UpdateDisplayedData();
                    Spawn(index);
                }
            }            
        }
    }

    //Uses index (in terms of CharacterPrefab list) to spawn the appropriate character
    private void Spawn(int index)
    {
        //creates variation in spawn location
        float randomY = Random.Range(MinSpawnY, MaxSpawnY);
        //for spawning a castle character, 5 represents king character/final castle character as seen in unity editor inspector list
        if (index < 6)
        {
            Vector3 spawnPos = new Vector3(CastleArmy.transform.position.x, randomY, CastleArmy.transform.position.z);
            GameObject newCharacter = Instantiate(CharacterPrefab[index], CastleArmy.transform);
            newCharacter.transform.position = spawnPos;
            Character characterCharComp = newCharacter.GetComponent<Character>();
            //if the spawned character is a castle king, use the king decorator and store that reference for future use
            //also, add the character (decorator for king) component to the castle armylist
            if (index == 5)
            {
                CKD = new KingCharacterDecorator(characterCharComp);
                newCharacter.transform.position = CastleKingSpawn;
                CastleArmyComp.Add(CKD);
            }
            else
            {
                CastleArmyComp.Add(characterCharComp);
            }
        }
        //for spawning a pirate character, 11 represents king character/final pirate character as seen in unity editor inspector list
        else
        {
            Vector3 spawnPos = new Vector3(PirateArmy.transform.position.x, randomY, PirateArmy.transform.position.z);
            GameObject newCharacter = Instantiate(CharacterPrefab[index], PirateArmy.transform);
            newCharacter.transform.position = spawnPos;
            Character characterCharComp = newCharacter.GetComponent<Character>();
            //if the spawned character is a pirate king, use the king decorator and store that reference for future use
            //also, add the character (decorator for king) component to the pirate armylist
            if (index == 11)
            {
                PKD = new KingCharacterDecorator(characterCharComp);
                newCharacter.transform.position = PirateKingSpawn;
                PirateArmyComp.Add(PKD);
            }
            else
            {
                PirateArmyComp.Add(characterCharComp);
            }
        }
    }

    //returns a random pirate or castle character
    public GameObject GetRandomEnemy(bool getPirate)
    {
        //returns random pirate
        if (getPirate)
        {
            return PirateArmyComp.GetRandomUnit();
        }
        //returns random castle
        else
        {
            return CastleArmyComp.GetRandomUnit();
        }
    }

    //removes a specific character from its respective armylist
    public void RemoveFromArmyList(Character character)
    {
        if (character.GetIsPirate()) 
        {
            PirateArmyComp.RemoveUnit(character);
        }
        else
        {
            CastleArmyComp.RemoveUnit(character);
        }
    }
}
