//Class written by: Dev Patel

using UnityEngine;

//A leaf part of the composite pattern based on ISpecialEffect
//Intended to change a character's speed mod (how fast they move)
public class SpecialEffect_SpeedMod : ISpecialEffect
{
    //negative numbers reduce speed mod, positive increases
    public float minSpeedModChange;
    public float maxSpeedModChange;

    public void Effect(ICharacter character)
    {
        float speedModChange = Random.Range(minSpeedModChange, maxSpeedModChange + 1);
        (character as Character).ChangeSpeedMod(speedModChange);
    }
}
