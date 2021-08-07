using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W_SemiAutomatic : W_Base
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
            m_canBeUsed = false;
        }
    }

    protected override void PullTrigger()
    {
        base.PullTrigger();

        /*Shoot();
        Debug.Log(data.m_name + ": SHOOT");

        m_canBeUsed = false;*/
    }

    protected override void ReleaseTrigger()
    {
        base.ReleaseTrigger();

        m_canBeUsed = true;
    }

}
