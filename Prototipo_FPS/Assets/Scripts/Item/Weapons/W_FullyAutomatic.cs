using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W_FullyAutomatic : W_Base
{

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        
        if (m_triggerIsPullled && m_canBeUsed)
        {
            Shoot();
        }
    }

    protected override void PullTrigger()
    {
        base.PullTrigger();
    }

    protected override void ReleaseTrigger()
    {
        base.ReleaseTrigger();

    }

}
