using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_MedKit : Item_Base
{
    private float m_healPercent = 20;

    protected override void Effect(Char_Base _char)
    {
        _char.Heal(Mathf.RoundToInt(_char.MaxHP * m_healPercent * 0.01f));
    }
}
