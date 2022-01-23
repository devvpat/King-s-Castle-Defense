using UnityEngine;

public interface ICharacter
{
    void TakeDamage(int damage);
    void Attack(GameObject target);
    void Die();
}
