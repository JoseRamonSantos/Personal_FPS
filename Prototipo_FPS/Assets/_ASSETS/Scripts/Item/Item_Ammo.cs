using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Ammo : Item_Base
{
    private int m_ammoAmount = 20;


    protected override void TakeItem(Char_Base _char)
    {
        if (_char.GetComponent<Char_Player>())
        {
            _char.GetComponent<Char_Player>().CmpWController.CrntWeapon.AddTotalAmmo(m_ammoAmount);
        }
    }
}
