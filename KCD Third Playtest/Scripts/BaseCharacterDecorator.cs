//Class written by: Dev Patel

public class BaseCharacterDecorator : ICharacter
{
    protected ICharacter m_Character;

    public BaseCharacterDecorator(ICharacter character)
    {
        m_Character = character;
    }

    public void SetCharacter(ICharacter character)
    {
        m_Character = character;
    }

    public virtual void Attack()
    {
        if (m_Character != null) m_Character.Attack();
    }

    public virtual void Die()
    {
        if (m_Character != null) (m_Character as Character).Die();
    }

    public virtual void TakeDamage(int damage)
    {
        if (m_Character != null) m_Character.TakeDamage(damage);
    }

    public Character GetCharacter()
    {
        return (m_Character as Character);
    }
}
