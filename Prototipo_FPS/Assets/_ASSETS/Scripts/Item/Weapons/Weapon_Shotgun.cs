using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Shotgun : Weapon_Base
{
    public override void PullTrigger()
    {
        base.PullTrigger();
        Fire();
    }

    protected override void ShootRaycast()
    {
        for (int i = 0; i < m_data.m_options.m_bulletsPerShoot; i++)
        {
            base.ShootRaycast();
        }
    }
}
