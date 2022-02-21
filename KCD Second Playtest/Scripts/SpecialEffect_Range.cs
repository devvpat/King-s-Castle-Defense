//Class written by: Dev Patel

using UnityEngine;

//A leaf part of the composite pattern based on ISpecialEffect
//Intended to change a character's attack range (how close they need to be to an enemy to attack them)
public class SpecialEffect_Range : ISpecialEffect
{
    //negative numbers reduce attack range, positive increases
    public float minAttackRangeChange;
    public float maxAttackRangeChange;

    public void Effect(ICharacter character)
    {
        float rangeChange = Random.Range(minAttackRangeChange, maxAttackRangeChange + 1);
        (character as Character).ChangeRange(rangeChange);
    }
}
