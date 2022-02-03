//Class written by: Dev Patel

using UnityEngine;

//A leaf part of the composite pattern based on ISpecialEffect
//Intended to damage a character by a certain (heal if negative damage)
public class SpecialEffect_TakeDamage : ISpecialEffect
{
    //negative numbers will be treated as heals    
    public int minDamage;
    public int maxDamage;
    public Army CastleArmy;
    public Army PirateArmy;

    public void Effect(ICharacter character)
    {
        int damage = Random.Range(minDamage, maxDamage+1);
        CastleArmy.TakeDamage(damage);
        PirateArmy.TakeDamage(damage);
    }
}
