//Class written by: Dev Patel

using UnityEngine;

[CreateAssetMenu(menuName = "Data/Character Data", fileName = "Character Data")]
public class CharacterData : ScriptableObject
{
    //starting health
    public int StartingHealth;
    //how much damage the character does per attack
    public int AttackDamage;
    //character's attack range
    public float AttackRange;
    //how much resistance this character has to damage
    public int Armor;
    public bool IsPirate;
    //cost to spawn
    public int Cost;
    public float SpeedMod;
    public bool IsKing;
}
