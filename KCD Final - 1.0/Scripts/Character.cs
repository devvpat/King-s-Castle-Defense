//Class written by: Dev Patel

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using Photon.Pun;
using Photon.Realtime;

//Leaf part of the ICharacter composite pattern
public class Character : MonoBehaviourPunCallbacks, ICharacter, IPunObservable
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

    private float TimeBetweenAttacks = 0.5f;
    private float TimeToPrepare = 1;
    private float TimeBetweenRecheckTarget = 2.5f;

    [SerializeField]
    private Image HealthBar;

    public CharacterData CharacterStats;

    //Using NavMesh 2D Package by h8man for agent stuff
    public NavMeshAgent Agent { get; private set; }

    private float DelayBetweenAttack = 1;

    private float Agent_StoppingDistance = 0;
    private float Agent_Radius = 0.1f;
    private float Agent_Acceleration = 3.5f;

    private float MinRange = 0.1f;

    //public so king character decorator can access the AIStates
    public enum AIState
    {
        Prepare, LookForEnemy, GoToEnemy, AttackEnemy, King_StayPut, King_LastDefense, King_Flee
    }

    //public so king character decorator can access it, the king character doesnt use any of AI states defined in this
    //class which is why the methods for states in this class are private
    public AIState State { get; set; }

    //used to keep track of current target
    private GameObject Enemy;

    //different animation states in the animator depends on the variable "AnimationState" where:
    //"AnimationState" = 0 -> idle
    //"AnimationState" = 1 -> walking
    //"AnimationState" = 2 -> attack
    public Animator CharAnimator { get; private set; }

    private void Start()
    {
        ResetStats();
        SetupAgent();
        transform.rotation = Quaternion.identity;
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        HealthBar.fillAmount = (float)Health / CharacterStats.StartingHealth;
        CharAnimator = this.GetComponent<Animator>();
        if (CharAnimator != null) CharAnimator.SetInteger("AnimationState", 0);
        //if the character is not the caslte king, prepare to attack, otherwise (character is castle king) stay put
        if (!IsKing || IsPirate) State = AIState.Prepare;
        else State = AIState.King_StayPut;
        DoCurrentStateAction();
    }

    //sets up NavMeshAgent's options
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

    public void ChangeResistance(int res)
    {
        Resistance += res;
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

    //if the character hasn't been instantiated yet, get is pirate from character data
    public bool GetIsPirate()
    {
        return CharacterStats.IsPirate;
    }

    public Character GetCharacter()
    {
        return this;
    }

    //Networked multiplayer code - not used
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(Health);
            stream.SendNext(Damage);
            stream.SendNext(Resistance);
            stream.SendNext(Range);
            stream.SendNext(IsPirate);
            stream.SendNext(Cost);
            stream.SendNext(SpeedMod);
            stream.SendNext(IsKing);
        }
        else
        {
            Health = (int)stream.ReceiveNext();
            Damage = (int)stream.ReceiveNext();
            Resistance = (int)stream.ReceiveNext();
            Range = (float)stream.ReceiveNext();
            IsPirate = (bool)stream.ReceiveNext();
            Cost = (int)stream.ReceiveNext();
            SpeedMod = (float)stream.ReceiveNext();
            IsKing = (bool)stream.ReceiveNext();
        }
    }

    //A simple finite state machine using an enum (AIState) and switch statement. Due to the relatively limited and simplistic
    //states (and the fact that there is no plan to add additional states), I felt like it was more appropriate to do implement
    //the finite state machine like this rather than implementing the entire state pattern using inheritance and multiple classes.
    //Professor Salmon also agreed that this would be an alright way to implement the finite state machine pattern for this game.
    private void DoCurrentStateAction()
    {
        switch (State)
        {
            case AIState.Prepare:
                StartCoroutine(Prepare());
                break;
            case AIState.LookForEnemy:
                StartCoroutine(FindEnemy());
                break;
            case AIState.GoToEnemy:
                StartCoroutine(MoveToEnemy());
                break;
            case AIState.AttackEnemy:
                StartCoroutine(AttackEnemy());
                break;
            case AIState.King_StayPut:
                //do nothing because the king is to not move (or do anything)
                break;
            case AIState.King_LastDefense:
                //the working method for this is in the king character decorator because its only for the king
                break;
            case AIState.King_Flee:
                //the working method for this is in the king character decorator because its only for the king
                break;
        }
    }

    //makes the character pause before starting the attack sequence, included to reduce the effectiveness of
    //spam spawning characters
    private IEnumerator Prepare()
    {
        yield return new WaitForSeconds(TimeToPrepare);
        State = AIState.LookForEnemy;
        DoCurrentStateAction();
    }

    public void TakeDamage(int damage)
    {
        //if the damage is 0 or positive, apply resistance then take damage
        if (damage >= 0)
        {
            damage -= Resistance;
            //every attack should at least one damage regardless of resistance/original damage
            damage = Mathf.Max(damage, 1);
            Health -= damage;
            //in the case a character dies...
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
            Health = Mathf.Min(Health, CharacterStats.StartingHealth);
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
            CoinManager._instance.Coins[0] += Mathf.FloorToInt(Cost / 2.0f);
        }
        else
        {
            CoinManager._instance.Coins[1] += Mathf.FloorToInt(Cost / 2.0f);
        }
        //make sure it is character before destroying
        if (this.GetType() == typeof(Character)) Destroy(this.gameObject);
    }

    public void Attack()
    {
        //The Attack is split into multiple methods within the finite state machine, so this method is a direct way of starting the Attack
        //sequence rather than allowing for the finite state machine to continue making choices
        StopAllCoroutines();
        State = AIState.LookForEnemy;
        DoCurrentStateAction();     
    }

    //gets an enemy from either the pirate team or castle team and sets this character's target to that enemy
    private IEnumerator FindEnemy()
    {
        if (CharAnimator != null) CharAnimator.SetInteger("AnimationState", 0);
        //first set/get an enemy to attack
        if (IsPirate)
        {
            //pirates attack the closest enemy to them
            Enemy = CharacterManager._instance.GetClosestEnemy(false, this.transform.position);
        }
        else
        {
            //knights attack the closest enemy to the king
            Enemy = CharacterManager._instance.GetClosestEnemy(true, CharacterManager._instance.GetCastleKingPos());
        }
        //if target/enemy acquired successfully (tag is player), transition the finite state machine to go to enemy
        if (Enemy.tag == "Player")
        {
            State = AIState.GoToEnemy;
            DoCurrentStateAction();
        }
        //else (no enemy found) use finite state machine to repeat this state's action of finding an enemy
        else
        {
            State = AIState.LookForEnemy;
            yield return new WaitForSeconds(DelayBetweenAttack);
            DoCurrentStateAction();
        }
    }

    //because a lot of agents are in the game at once and because they can die mid-activity, there are a lot of checks
    //to see if they are alive to not break anything. I'm unsure of a better solution than to constantly check so until I find one
    //(if i can), i simply implemented many checks
    private IEnumerator MoveToEnemy()
    {
        if (Agent != null && Agent.isActiveAndEnabled)
        {
            //make sure the agent can move (is stopped = false)
            Agent.isStopped = false;
        }
        float count = 0;
        while (Enemy != null && Vector2.Distance(transform.position, Enemy.transform.position) > Range)
        {
            //to keep AI more consistent, every now and then (TimeBetweenRecheckTarget) get a better target
            //if possible (by going back to find enemy state)
            if (count >= TimeBetweenRecheckTarget)
            {
                State = AIState.LookForEnemy;
                DoCurrentStateAction();
                yield break;
            }

            //Every now and then this error shows up:
            //"Set Destination" can only be called on an active agent that has been placed on a navmesh
            //Which is why I added many checks to make sure this doesnt happen

            //continue to go to the enemy while the enemy remains valid andout of range
            if (Agent != null && Agent.isActiveAndEnabled && Enemy != null && Agent.SetDestination(Enemy.transform.position))
            {
                if (CharAnimator != null) CharAnimator.SetInteger("AnimationState", 1);
                yield return null;
                count += Time.deltaTime;
            }
            //if enemy is no longer valid, look for a new enemy
            else
            {
                State = AIState.LookForEnemy;
                if (CharAnimator != null) CharAnimator.SetInteger("AnimationState", 0);
                //short delay before looking for new target
                yield return new WaitForSeconds(DelayBetweenAttack);
                DoCurrentStateAction();
                yield break;
            }
        }
        //make sure the agent cant move once in desired range of enemy and then attack
        if (Agent != null && Agent.isActiveAndEnabled)
        {
            Agent.velocity = Vector3.zero;
            Agent.isStopped = true;
            State = AIState.AttackEnemy;
            DoCurrentStateAction();
        }
    }

    //damages enemy while they are alive and in range, moves to enemy if they go out of range, and looks for new target when current target dies/becomes invalid
    private IEnumerator AttackEnemy()
    {
        Character enemyCharComp = null;
        if (Enemy != null)
        {
            enemyCharComp = Enemy.GetComponent<Character>();
        }        
        if (enemyCharComp != null)
        {
            //first hit the enemy (because the enemy is in the character's attack range due to the MoveToEnemy coroutine)
            //castle king has a special take damage method
            if (CharAnimator != null) CharAnimator.SetInteger("AnimationState", 2);
            if (enemyCharComp.IsKing && !enemyCharComp.IsPirate) CharacterManager._instance.CKD.TakeDamage(Damage);
            else enemyCharComp.TakeDamage(Damage);
            yield return new WaitForSeconds(TimeBetweenAttacks);
            //continously hit the enemy while they remain in range every x seconds and aren't dead
            while (Enemy != null && Vector2.Distance(transform.position, Enemy.transform.position) <= Range)
            {
                if (enemyCharComp != null)
                {
                    if (enemyCharComp.IsKing && !enemyCharComp.IsPirate) CharacterManager._instance.CKD.TakeDamage(Damage);
                    else enemyCharComp.TakeDamage(Damage);
                }
                yield return new WaitForSeconds(TimeBetweenAttacks);
            }
        }        
        //if the enemy isn't dead/invalid but out of range, go to it again
        if (enemyCharComp != null && Vector2.Distance(transform.position, Enemy.transform.position) > Range)
        {
            if (CharAnimator != null) CharAnimator.SetInteger("AnimationState", 0);
            State = AIState.GoToEnemy;
            DoCurrentStateAction();
            yield break;
        }
        //in the case the enemy dies/is no longer valid, stop trying to attack it and search for a new enemy to attack
        if (CharAnimator != null) CharAnimator.SetInteger("AnimationState", 0);
        State = AIState.LookForEnemy;
        DoCurrentStateAction();
    }
}
