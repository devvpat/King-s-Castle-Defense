//Class written by: Dev Patel

using System;
using System.Collections.Generic;
using UnityEngine;

//composite part of the ICharacter composite pattern that also is used by the game as a reference for all spawned characters
public class Army : MonoBehaviour, ICharacter
{
    [SerializeField]
    private bool IsPirate;

    [SerializeField]
    private GameObject Empty;

    public List<ICharacter> ArmyList { get; private set; }

    private void Start()
    {
        ArmyList = new List<ICharacter>();
        ArmyList.Clear();
    }

    public void Add(ICharacter character)
    {
        ArmyList.Add(character);
    }

    public void TakeDamage(int damage)
    {
        //because the character's in armylist can change while this is running, try this while catching InvalidOperationExceptions
        try
        {
            foreach (ICharacter character in ArmyList)
            {
                //deal damage to each character except the kings
                if (!character.GetCharacter().CharacterStats.IsKing) character.TakeDamage(damage);
            }
        }
        catch (Exception e)
        {
            if (e.GetType() == typeof(InvalidOperationException)) Debug.Log("Armylist was changed:\n" + e.Message);
            else Debug.Log(e.Message);
        }
    }

    public void Attack()
    {
        foreach (ICharacter character in ArmyList)
        {
            character.Attack();
        }
    }

    public void Die()
    {
        foreach (ICharacter character in ArmyList)
        {
            character.Die();
        }
    }

    public Character GetCharacter()
    {
        foreach (ICharacter character in ArmyList)
        {
            return character.GetCharacter();
        }
        return new Character();
    }

    public GameObject GetRandomUnit()
    {
        try
        {
            if (ArmyList.Count > 0)
            {
                if (!IsPirate)
                {
                    //when getting a random castle character, if there are less than or equal to 3 characters in the castle armylist (including king),
                    //there's a chance the method will return the king. otherwise (when there are 4 or more castle characters),
                    //never return the king
                    GameObject obj = (ArmyList[UnityEngine.Random.Range(0, ArmyList.Count)]).GetCharacter().gameObject;
                    if (ArmyList.Count <= 3)
                    {
                        if (CharacterManager._instance.CKD.GetCharacter() != null && UnityEngine.Random.Range(0, 99) < (1 / (ArmyList.Count)) * 100)
                        {
                            obj = CharacterManager._instance.CKD.GetCharacter().gameObject;
                        }
                        else
                        {
                            Character objChar = obj.GetComponent<Character>();
                            while (objChar.CharacterStats.IsKing)
                            {
                                obj = (ArmyList[UnityEngine.Random.Range(0, ArmyList.Count)]).GetCharacter().gameObject;
                                objChar = obj.GetComponent<Character>();
                            }
                        }
                    }
                    //in the case there is no valid target, return an empty object (as in one that is from CreateAssestMenu > Create Empty)
                    if (obj != null) return obj;
                    else return Empty;
                }
                //always return a random pirate if the pirate armylist isn't empty
                return (ArmyList[UnityEngine.Random.Range(0, ArmyList.Count)]).GetCharacter().gameObject;
            }
            else
            {
                return Empty;
            }
        }
        catch (Exception e)
        {
            if (e.GetType() == typeof(MissingReferenceException)) Debug.Log("Character was destroyed:\n" + e.Message);
            else Debug.Log(e.Message);
        }
        return Empty;
    }

    public void RemoveUnit(Character character)
    {
        if (ArmyList.Count > 0)
        {
            int index = ArmyList.IndexOf(character);
            if (index > -1 && index < ArmyList.Count) ArmyList.RemoveAt(index);
        }
    }
}
