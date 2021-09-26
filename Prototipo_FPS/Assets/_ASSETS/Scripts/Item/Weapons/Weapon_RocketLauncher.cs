using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_RocketLauncher : Weapon_Base
{
    public override void PullTrigger()
    {
        base.PullTrigger();
        Fire();
    }

}
