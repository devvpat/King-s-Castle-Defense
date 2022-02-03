//Class written by: Dev Patel

//For the composite pattern based on ICharacter
public interface ICharacter
{
    void TakeDamage(int damage);
    void Attack();
    void Die();
    Character GetCharacter();
}
