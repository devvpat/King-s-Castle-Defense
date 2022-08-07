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

    //Thanks to edwardrowe (post #4) in https://forum.unity.com/threads/clean-est-way-to-find-nearest-object-of-many-c.44315/
    //for help with the code to find the closest object based on a point. I had the general idea of starting with min dist = infinity and closestobj = null,
    //then updating those with better objects as i traversed through ArmyList, but i didn't know exactly how that code would look (and that .sqrMagnitude is faster than just .distance),
    //so i used the code posted by edwardrowe in the linked thread
    public GameObject GetClosetUnit(Vector2 originPos)
    {
        try
        {
            if (ArmyList.Count > 0)
            {
                GameObject closetObj = null;
                float closetDist = Mathf.Infinity;
                foreach (ICharacter character in ArmyList)
                {
                    Vector2 dir = (Vector2)character.GetCharacter().gameObject.transform.position - originPos;
                    float dSqr = dir.sqrMagnitude;
                    if (dSqr < closetDist)
                    {
                        closetDist = dSqr;
                        closetObj = character.GetCharacter().gameObject;
                    }
                }
                if (closetObj != null) return closetObj;
                else return Empty;
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
