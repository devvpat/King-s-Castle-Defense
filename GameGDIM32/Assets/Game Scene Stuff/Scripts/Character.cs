using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour, ICharacter
{
    private int Health;
    private int Damage;
    //damage the character negates upon being attacked
    private int Resistance;
    //attack range
    private float Range;
    //true = pirate (enemy in solo), false = on the king's side/knight
    private bool IsPirate;

    [SerializeField]
    private CharacterData CharacterStats;

    private void Start()
    {
        ResetStats();
    }

    private void ResetStats()
    {
        Health = CharacterStats.StartingHealth;
        Damage = CharacterStats.AttackDamage;
        Resistance = CharacterStats.Armor;
        Range = CharacterStats.AttackRange;
        IsPirate = CharacterStats.IsPirate;
    }

    public void TakeDamage(int damage)
    {
        int damageToDeal = damage - Resistance;
        if (damageToDeal >= 0)
        {
            Health -= damageToDeal;
            if (Health <= 0)
            {
                Die();
            }
        }
    }

    public void Die()
    {
        this.gameObject.SetActive(false);
        //add to object pool
        Debug.Log("death from character class");
    }

    public void Attack(GameObject target)
    {
        //checks if target is a character and if so, if the target is within attack range before attacking
        Character targetChar = target.GetComponent<Character>();
        if (targetChar != null && Vector3.Distance(this.gameObject.transform.position, target.transform.position) <= Range)
        {
            targetChar.TakeDamage(Damage);
        }
    }
}
