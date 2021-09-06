using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_FullAuto : Weapon_Base
{
    protected override void Update()
    {
        if(m_triggerIsPullled && CanShoot)
        {
            Fire();
        }

        base.Update();
    }
}
