using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Char_Enemy : Char_Base
{

    public override void ReceiveDamage(int _damage)
    {
        base.ReceiveDamage(_damage);
    }

    protected override void Die()
    {
        base.Die();

        this.enabled = false;
    }
}
