//Class written by: Dev Patel

using UnityEngine;

//A leaf part of the composite pattern based on ISpecialEffect
//Intended to change a character's resistance (how much less damage they receive from attacks)
public class SpecialEffect_Resistance : ISpecialEffect
{
    //negative numbers decrease resistances, positive increases
    public int minResitanceChange;
    public int maxResistanceChange;

    public void Effect(ICharacter character)
    {
        int resChange = Random.Range(minResitanceChange, maxResistanceChange + 1);
        (character as Character).ChangeResistance(resChange);
    }
}
