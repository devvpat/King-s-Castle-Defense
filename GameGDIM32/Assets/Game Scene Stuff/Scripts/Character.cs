//Class written by: Dev Patel

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

//Leaf part of the ICharacter composite pattern
public class Character : MonoBehaviour, ICharacter
{
    public int Health { get; set; }
    public int Damage { get; set; }
    //damage the character negates upon being attacked, can be negative = take more damage (vulnerable)
    public int Resistance { get; set; }
    //attack range
    public float Range { get; set; }
    //true = pirate, false = on the castle's team
    public bool IsPirate { get; private set; }
    public int Cost { get; private set; }
    public float SpeedMod { get; set; }
    public bool IsKing { get; private set; }

    private float TimeBetweenAttacks;

    [SerializeField]
    private Image HealthBar;

    public CharacterData CharacterStats;

    //Using NavMesh 2D Package by h8man for agent stuff
    private NavMeshAgent Agent;

    private float DelayBetweenAttack = 1;

    private float Agent_StoppingDistance = 0;
    private float Agent_Radius = 0.1f;
    private float Agent_Acceleration = 3.5f;

    private float MinRange = 0.1f;

    private void Start()
    {
        ResetStats();
        SetupAgent();
        transform.rotation = Quaternion.identity;
        TimeBetweenAttacks = 0.5f;
        HealthBar.fillAmount = (float)Health / CharacterStats.StartingHealth;
        //If the character is not the castle king, use the normal attack function; the king is not meant to attack
        if (!IsKing || IsPirate) Invoke("Attack", DelayBetweenAttack);
    }

    private void SetupAgent()
    {
        Agent = this.GetComponent<NavMeshAgent>();
        if (Agent != null)
        {
            Agent.updateRotation = false;
            Agent.updateUpAxis = false;
            Agent.speed = SpeedMod;
            Agent.stoppingDistance = Agent_StoppingDistance;
            Agent.radius = Agent_Radius;
            Agent.acceleration = Agent_Acceleration;
        }
    }

    //changes speedmod and ensures it is not below 0
    public void ChangeSpeedMod(float speed)
    {
        SpeedMod += speed;
        SpeedMod = Mathf.Max(SpeedMod, 0);
        Agent.speed = SpeedMod;
    }

    //changes attack damage and ensures it is not below 0
    public void ChangeAttackDamage(int damage)
    {
        Damage += damage;
        Damage = Mathf.Max(Damage, 0);
    }

    //changes range and ensures it is not below min attack range
    public void ChangeRange(float range)
    {
        Range += range;
        Range = Mathf.Max(Range, MinRange);
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
                else if (IsKing && IsPirate) CharacterManager._instance.PKD.Die();
                else Die();
            }
        }
        //if the damage is a negative number, treat it as a heal and make sure current health doesnt go above starting health
        else
        {
            Health -= damage;
            if (Health > CharacterStats.StartingHealth) Health = CharacterStats.StartingHealth;
        }
        //update healthbar
        HealthBar.fillAmount = (float)Health / CharacterStats.StartingHealth;
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
            StartCoroutine(MoveToTarget(enemy));
        } 
        //try again to find an enemy if an appropriate enemy isn't found
        else
        {
            Invoke("Attack", DelayBetweenAttack);
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

    //because a lot of agents are in the game at once and because they can die mid-activity, there are a lot of checks
    //to see if they are alive to not break anything. I'm unsure of a better solution than to constantly check so until I find one
    //(if i can), i simply implemented many checks
    private IEnumerator MoveToTarget(GameObject enemy)
    {
        if (Agent != null && Agent.isActiveAndEnabled)
        {
            //make sure the agent can move (is stopped = false)
            Agent.isStopped = false;
        }
        while (enemy != null && Vector2.Distance(transform.position, enemy.transform.position) > Range)
        {
            //Every now and then this error shows up:
            //"Set Destination" can only be called on an active agent that has been placed on a navmesh
            //Which is why I added many checks to make sure this doesnt happen
            if (Agent != null && Agent.isActiveAndEnabled && 
                enemy != null && Agent.SetDestination(enemy.transform.position)) yield return null;
            //transform.position = Vector2.MoveTowards(transform.position, enemy.transform.position, SpeedMod * Time.deltaTime);
            else
            {
                Invoke("Attack", DelayBetweenAttack);                
                yield break;
            }
        }
        //make sure the agent cant move once in desired range of enemy
        if (Agent != null && Agent.isActiveAndEnabled)
        {
            Agent.velocity = Vector3.zero;
            Agent.isStopped = true;
            StartCoroutine(Attack_DealDamageToEnemy(enemy));
        }
    }

    private IEnumerator Attack_DealDamageToEnemy(GameObject enemy)
    {
        Character enemyCharComp = null;
        if (enemy != null)
        {
            enemyCharComp = enemy.GetComponent<Character>();
        }        
        if (enemyCharComp != null)
        {
            //first hit the enemy (because the enemy is in the character's attack range due to the MoveToTarget coroutine)
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
            StartCoroutine(MoveToTarget(enemy));
            yield break;
        }
        //in the case the enemy dies, stop trying to attack it and search for a new enemy to attack
        Attack();
    }
}
