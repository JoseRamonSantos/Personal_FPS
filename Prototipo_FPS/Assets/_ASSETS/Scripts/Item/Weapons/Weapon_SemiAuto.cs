using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_SemiAuto : Weapon_Base
{
    public override void PullTrigger()
    {
        base.PullTrigger();
        Fire();
    }
}
