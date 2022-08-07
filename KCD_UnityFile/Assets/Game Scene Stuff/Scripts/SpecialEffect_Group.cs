//Class written by: Dev Patel

using System.Collections.Generic;

//The composite part of the composite pattern based on ISpecialEffect
public class SpecialEffect_Group : ISpecialEffect
{
    protected List<ISpecialEffect> SpecialEffectLists = new List<ISpecialEffect>();

    public void Add(ISpecialEffect effect)
    {
        SpecialEffectLists.Add(effect);
    }

    public void Remove(ISpecialEffect effect)
    {
        SpecialEffectLists.Remove(effect);
    }

    public void Effect(ICharacter character)
    {
        foreach (ISpecialEffect effect in SpecialEffectLists)
        {
            effect.Effect(character);
        }
    }
}
