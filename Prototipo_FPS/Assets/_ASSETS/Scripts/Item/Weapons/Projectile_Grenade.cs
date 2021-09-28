using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Grenade : Projectile_Base
{
    protected override void StartCollision() { }

    protected override void EndLifetime()
    {
        Explode();
    }
}
