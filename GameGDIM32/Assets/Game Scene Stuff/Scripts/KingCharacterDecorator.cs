using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingCharacterDecorator : BaseCharacterDecorator
{
    public KingCharacterDecorator(ICharacter character) : base(character)
    {

    }

    public override void Attack(GameObject target)
    {
        //Special attack
    }

    public override void Die()
    {
        //Special death
        Debug.Log("Death from king decorator");
    }

    //inherits TakeDamage method from base class ("BaseCharacterDecorator")
}
