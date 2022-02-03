//Class written by: Dev Patel

using System.Collections;
using UnityEngine;

//Leaf part of the ICharacter composite pattern
public class Character : MonoBehaviour, ICharacter
{
    public int Health { get; set; }
    public int Damage { get; set; }
    //damage the character negates upon being attacked
    public int Resistance { get; set; }
    //attack range
    public float Range { get; set; }
    //true = pirate, false = on the castle's team
    public bool IsPirate { get; private set; }
    public int Cost { get; private set; }
    public float SpeedMod { get; set; }
    public bool IsKing { get; private set; }

    private float TimeBetweenAttacks;

    public CharacterData CharacterStats;

    private void Start()
    {
        ResetStats();
        TimeBetweenAttacks = 0.5f;
        //If the character is not the castle king, use the normal attack function; the king is not meant to attack
        if (!IsKing || IsPirate) Invoke("Attack", 1);
    }

    private void ResetStats()
    {
        Health = CharacterStats.StartingHealth;
        Damage = CharacterStats.AttackDamage;
        Resistance = CharacterStats.Armor;
        Range = CharacterStats.AttackRange;
        IsPirate = CharacterStats.IsPirate;
        Cost = CharacterStats.Cost;
        SpeedMod = CharacterStats.SpeedMod;
        IsKing = CharacterStats.IsKing;
    }

    //if the character hasn't been instantiated yet, get cost from character data
    public int GetCost()
    {
        return CharacterStats.Cost;
    }

    //if the character hasn't been instantiated yet, get ispirate from character data
    public bool GetIsPirate()
    {
        return CharacterStats.IsPirate;
    }

    public void TakeDamage(int damage)
    {
        //if the damage is 0 or positive, apply resistance then take damage
        if (damage >= 0)
        {
            damage -= Resistance;
            if (damage <= 0) damage = 1;
            Health -= damage;
            if (Health <= 0)
            {
                //Both kings (pirate and castle) have a decorator, so call their decorator's death method instead of the base character death method
                if (IsKing && !IsPirate) CharacterManager._instance.CKD.Die();
                if (IsKing && IsPirate) CharacterManager._instance.PKD.Die();
                else Die();
            }
        }
        //if the damage is a negative number, treat it as a heal
        else
        {
            Health -= damage;
        }
    }

    //remove from respective armylist and delete object on death, and grant killing player coins equals to half the cost of the unit (floored)
    public void Die()
    {
        CharacterManager._instance.RemoveFromArmyList(this);
        if (IsPirate)
        {
            CoinManager._instance.Coins[0] += Mathf.FloorToInt(Cost / 2);
        }
        else
        {
            CoinManager._instance.Coins[1] += Mathf.FloorToInt(Cost / 2);
        }
        if (this.GetType() == typeof(Character)) Destroy(this.gameObject);
    }

    public Character GetCharacter()
    {
        return this;
    }

    public void Attack()
    {
        //first finds an enemy to attack (if there is one)
        GameObject enemy = Attack_FindEnemy();        
        //if an enemy is found, the character moves to the enemy then attacks it
        if (enemy.tag == "Player")
        {
            StartCoroutine(MoveToObject(enemy));
        } 
        //try again to find an enemy if an appropriate enemy isn't found
        else
        {
            Invoke("Attack", 1);
        }        
    }

    //returns an enemy from either the pirate team or castle team
    private GameObject Attack_FindEnemy()
    {
        GameObject enemy;
        if (IsPirate)
        {
            enemy = CharacterManager._instance.GetRandomEnemy(false);
        }
        else
        {
            enemy = CharacterManager._instance.GetRandomEnemy(true);
        }
        return enemy;
    }

    //moves the character toward the enemy until they are within their attack range
    private IEnumerator MoveToObject(GameObject enemy)
    {
        while (enemy != null && Vector2.Distance(transform.position, enemy.transform.position) > Range)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemy.transform.position, SpeedMod * Time.deltaTime);
            yield return null;
        }
        StartCoroutine(Attack_DealDamageToEnemy(enemy));    
    }

    //
    private IEnumerator Attack_DealDamageToEnemy(GameObject enemy)
    {
        Character enemyCharComp = null;
        if (enemy != null)
        {
            enemyCharComp = enemy.GetComponent<Character>();
        }        
        if (enemyCharComp != null)
        {
            //first hit the enemy (because the enemy is in the character's attack range due to the MoveToObject coroutine)
            enemyCharComp.TakeDamage(Damage);
            yield return new WaitForSeconds(TimeBetweenAttacks);
            //continously hit the enemy while they remain in range every x seconds and aren't dead
            while (enemy != null && Vector2.Distance(transform.position, enemy.transform.position) <= Range)
            {
                if (enemyCharComp != null)
                {
                    enemyCharComp.TakeDamage(Damage);
                }
                yield return new WaitForSeconds(TimeBetweenAttacks);
            }
        }        
        //if the enemy isn't dead/not found but out of range, go to it again
        if (enemyCharComp != null && Vector2.Distance(transform.position, enemy.transform.position) > Range)
        {
            StartCoroutine(MoveToObject(enemy));
            yield break;
        }
        //in the case the enemy dies, stop trying to attack it and search for a new enemy to attack
        Attack();
    }
}
