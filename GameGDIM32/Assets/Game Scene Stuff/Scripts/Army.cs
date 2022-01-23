using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Army : MonoBehaviour, ICharacter
{
    [SerializeField]
    private bool IsPirate;

    private List<ICharacter> KnightArmyList = new List<ICharacter>();

    public void Add(ICharacter character)
    {
        KnightArmyList.Add(character);
    }

    public void TakeDamage(int damage)
    {
        foreach (ICharacter character in KnightArmyList)
        {
            character.TakeDamage(damage);
        }
    }

    public void Attack(GameObject target)
    {
        foreach (ICharacter character in KnightArmyList)
        {
            character.Attack(target);
        }
    }

    public void Die()
    {
        foreach (ICharacter character in KnightArmyList)
        {
            character.Die();
        }
    }
}
