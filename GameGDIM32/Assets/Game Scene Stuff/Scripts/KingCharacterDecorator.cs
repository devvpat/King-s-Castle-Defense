//Class written by: Dev Patel

using UnityEngine;
using UnityEngine.AI;

//"ConcreteDecorator" for the king character that notifies when pirates/castle win
public class KingCharacterDecorator : BaseCharacterDecorator
{
    //what percent of starting health should last defense be called
    private float LastDefenseHealth = 0.75f;    //at 75% of king's starting health...
    //what percent of starting health should the king start fleeing
    private float FleeHealth = 0.4f;    //at 40% of king's starting health...

    private Character KingChar;

    //list of troops used in LastDefense method, names are based on the list of CharacterNames in the CharacterManager
    private string[] LastDefenseSpawnList = { "C3", "C4", "C3" };

    private float FleeAccelChange = 1.5f;

    //bounds for random location chosen in flee method
    private float[] Flee_X = { 15, 20 };
    private float[] Flee_Y = { -5.9f, 6.9f };

    public KingCharacterDecorator(ICharacter character) : base(character)
    {
        KingChar = m_Character.GetCharacter();
    }

    public override void TakeDamage(int damage)
    {
        //first take damage like any character would do normally
        base.TakeDamage(damage);
        //if a castle king, check if health has reached a special abilit threshold
        if (!m_Character.GetCharacter().IsPirate)
        {
            //sets king's ai state to last defense once below a certain threshold and if the king previously had the stayput state
            if (KingChar.Health <= (int)(LastDefenseHealth * KingChar.CharacterStats.StartingHealth) &&
                KingChar.State == Character.AIState.King_StayPut)
            {
                KingChar.State = Character.AIState.King_LastDefense;
                DoCurrentKingStateAction();
            }
            //sets king's ai state to flee once below another threshold and if the king previously had the last defense state
            if (KingChar.Health <= (int)(FleeHealth * KingChar.CharacterStats.StartingHealth) && 
                KingChar.State == Character.AIState.King_LastDefense)
            {
                KingChar.State = Character.AIState.King_Flee;
                DoCurrentKingStateAction();
            }
        }
    }

    //depending on the king's state, do something
    private void DoCurrentKingStateAction()
    {
        switch (KingChar.State)
        {
            case Character.AIState.King_LastDefense:
                LastDefense();
                break;
            case Character.AIState.King_Flee:
                Flee();
                break;
        }
    }

    //spawns a few knights (exact number and type defined in a string array above)
    private void LastDefense()
    {
        foreach (string s in LastDefenseSpawnList)
        {
            CharacterManager._instance.ForceSpawn(s);
        }
    }

    //move to a random location with given constraints/bounds as a last resort to live.
    //this is not meant to be super "good" but as a last second stall to try and win before the king dies
    private void Flee()
    {
        NavMeshAgent agent = KingChar.Agent;
        agent.acceleration += FleeAccelChange;
        //set king's animation to walking
        if (KingChar.CharAnimator != null) KingChar.CharAnimator.SetInteger("AnimationState", 1);
        Vector3 FleeLocation = new Vector3(Random.Range(Flee_X[0], Flee_X[1]), Random.Range(Flee_Y[0], Flee_Y[1]), agent.gameObject.transform.position.z);
        if (agent != null && agent.isActiveAndEnabled) agent.SetDestination(FleeLocation);
    }

    public override void Die()
    {
        base.Die();
        if ((m_Character as Character).CharacterStats.IsPirate)
        {
            GameplayManager._instance.OnCastleWin();
        }
        else
        {
            GameplayManager._instance.OnPirateWin();
        }
    }
}
