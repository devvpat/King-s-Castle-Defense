//Class written by: Dev Patel

//"ConcreteDecorator" for the king character that notifies when pirates/castle win
public class KingCharacterDecorator : BaseCharacterDecorator
{
    public KingCharacterDecorator(ICharacter character) : base(character)
    {

    }

    public override void Attack()
    {
        //Special attack
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
