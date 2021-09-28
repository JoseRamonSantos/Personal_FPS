using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Rocket : Projectile_Base
{
    public override void Activate(Char_Base _owner, int _maxDamage, int _maxRange, AnimationCurve _dDmgReduction)
    {
        base.Activate(_owner, _maxDamage, _maxRange, _dDmgReduction);

        m_impactDamage = _maxDamage;
    }

    protected override void StartCollision()
    {
        Explode();
    }

    protected override void EndLifetime()
    {
        Destroy(this.gameObject);
    }
}
