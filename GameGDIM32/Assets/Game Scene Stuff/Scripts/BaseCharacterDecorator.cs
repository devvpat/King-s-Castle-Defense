using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacterDecorator : ICharacter
{
    protected ICharacter m_Character;

    public BaseCharacterDecorator(ICharacter character)
    {
        if (m_Character != null) m_Character = character;
    }

    public void SetCharacter(ICharacter character)
    {
        if (m_Character != null) m_Character = character;
    }

    public virtual void Attack(GameObject target)
    {
        if (m_Character != null) m_Character.Attack(target);
    }

    public virtual void Die()
    {
        if (m_Character != null) m_Character.Die();
        Debug.Log("death from base decorator");
    }

    public virtual void TakeDamage(int damage)
    {
        if (m_Character != null) m_Character.TakeDamage(damage);
    }
}
