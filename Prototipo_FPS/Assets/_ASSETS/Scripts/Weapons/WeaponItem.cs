using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class WeaponItem
{
    public enum WeaponType
    {
        primerayWeapon,
        secondaryWeapon
    }

    [Space]
    public      string          m_name                      = "";

    [Space]
    public      WeaponType      m_weaponType                = WeaponType.secondaryWeapon;

    [Header("ATTRIBUTES")]
    public      E_FIRE_MODE     m_fireMode                  = E_FIRE_MODE.FULL_AUTO;
    public      int             m_damage                    = 50;
    //public      float           m_forceToApply              = 20.0f;
    public      int             m_range                     = 9999;
    public      float           m_accuracy                  = 100;
    public      float           m_accuracyDropPerShot       = 5;
    public      float           m_accuracyRecoverPerSecond  = 1;
    public      float           m_accuracyRecoverDelay      = 0.2f;
    public      float           m_recoilRecovery            = 10;
    public      float           m_fireRate                  = 60;
    public      bool            m_hasChamber                = true;
    public      int             m_totalAmmo                 = 150;
    public      int             m_clipAmmo                  = 30;

    [Header("SHOOT EFFECTS")]
    public      AudioClip       m_triggerSound              = null;
    public      AudioClip       m_fireSound                 = null;
    public      GameObject      m_bulletShell               = null;
    public      AudioClip       m_bulletShellSound          = null;
    
    [Header("RELOAD")]
    public      AudioClip       m_reloadAmmoLeftSound       = null;
    public      AudioClip       m_reloadOutOfAmmoSound      = null;
    
    [Space]
    public      AudioClip       m_holsterSound              = null;
    public      AudioClip       m_drawSound                 = null;

}
