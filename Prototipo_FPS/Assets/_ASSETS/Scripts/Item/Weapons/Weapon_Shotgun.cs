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

    protected override void Shoot()
    {
        for (int i = 0; i < m_data.m_bulletsPerShoot; i++)
        {
            base.Shoot();
        }
    }

    protected override void ReloadAmmo()
    {
        /*int ammoReloaded = 0;

        ammoReloaded = ClipAmmo - CrntAmmo;

        if (HasChamber && CrntAmmo > 0)
        {
            ammoReloaded++;
        }

        if (ammoReloaded > TotalAmmo)
        {
            ammoReloaded -= TotalAmmo - ammoReloaded;
        }*/

        CrntAmmo ++;
        TotalAmmo --;

        m_cmpWController.UpdateAmmoHUD();

        if(CrntAmmo == ClipAmmo || TotalAmmo == 0)
        {
            CmpAnimator.SetTrigger("EndReload");
        }
    }
}
