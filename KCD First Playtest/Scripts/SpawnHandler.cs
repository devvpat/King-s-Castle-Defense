//Class written by: Dev Patel

using UnityEngine;

//A collection of spawn methods that are intended to be attached to spawn buttons on the UI
public class SpawnHandler : MonoBehaviour
{
    public static bool AllowSpawn = true;

    public static void DisableSpawn()
    {
        AllowSpawn = false;
    }

    public static void EnableSpawn() //Tien-Yi added this, Enable spawn, made for turning spawn on when game starts
    {
        AllowSpawn = true;
    }

    //Spawn castle characters
    public static void Spawn_C1() 
    {
        if (AllowSpawn)
        {
            string characterType = CharacterManager._instance.CharacterName[0];
            CharacterManager._instance.SpawnCharacter(characterType, 1);
        }
    }

    public static void Spawn_C2()
    {
        if (AllowSpawn)
        {
            string characterType = CharacterManager._instance.CharacterName[1];
            CharacterManager._instance.SpawnCharacter(characterType, 1);
        }
    }

    public static void Spawn_C3()
    {
        if (AllowSpawn)
        {
            string characterType = CharacterManager._instance.CharacterName[2];
            CharacterManager._instance.SpawnCharacter(characterType, 1);
        }
    }

    public static void Spawn_C4()
    {
        if (AllowSpawn)
        {
            string characterType = CharacterManager._instance.CharacterName[3];
            CharacterManager._instance.SpawnCharacter(characterType, 1);
        }
    }

    public static void Spawn_C5()
    {
        if (AllowSpawn)
        {
            string characterType = CharacterManager._instance.CharacterName[4];
            CharacterManager._instance.SpawnCharacter(characterType, 1);
        }
    }

    //Spawn pirate characters
    public static void Spawn_P1()
    {
        if (AllowSpawn)
        {
            string characterType = CharacterManager._instance.CharacterName[6];
            CharacterManager._instance.SpawnCharacter(characterType, 2);
        }
    }

    public static void Spawn_P2()
    {
        if (AllowSpawn)
        {
            string characterType = CharacterManager._instance.CharacterName[7];
            CharacterManager._instance.SpawnCharacter(characterType, 2);
        }
    }

    public static void Spawn_P3()
    {
        if (AllowSpawn)
        {
            string characterType = CharacterManager._instance.CharacterName[8];
            CharacterManager._instance.SpawnCharacter(characterType, 2);
        }
    }

    public static void Spawn_P4()
    {
        if (AllowSpawn)
        {
            string characterType = CharacterManager._instance.CharacterName[9];
            CharacterManager._instance.SpawnCharacter(characterType, 2);
        }
    }

    public static void Spawn_P5()
    {
        if (AllowSpawn)
        {
            string characterType = CharacterManager._instance.CharacterName[10];
            CharacterManager._instance.SpawnCharacter(characterType, 2);
        }
    }

}
