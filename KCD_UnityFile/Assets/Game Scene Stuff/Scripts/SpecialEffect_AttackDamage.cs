//Class written by: Dev Patel

using UnityEngine;

//A leaf part of the composite pattern based on ISpecialEffect
//Intended to change a character's attack damage (how much damage they do per attack)
public class SpecialEffect_AttackDamage : ISpecialEffect
{
    //negative numbers reduce attack damage, positive increases
    public int minAttackDamageChange;
    public int maxAttackDamageChange;

    public void Effect(ICharacter character)
    {
        int damageChange = Random.Range(minAttackDamageChange, maxAttackDamageChange + 1);
        (character as Character).ChangeAttackDamage(damageChange);
    }
}
