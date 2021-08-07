using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class WeaponItem
{
    public enum WeaponType
    {
        primaryWeapon,
        secondaryWeapon,
    }


    [Space]
    public      string          m_name                      = null;

    [Space]
    public      WeaponType      m_weaponType                = WeaponType.primaryWeapon;

    [Header("WEAPONS")]
    [Header("Attributes")]
    public      bool            m_hasChamber                = true;
    [Space]
    public      float           m_damage                    = 80.0f;
    public      float           m_fireRate                  = 1;
    public      float           m_range                     = 9999;
    public      float           m_accuracy                  = 100;
    public      float           m_accuracyDropPerShot       = 5;
    public      float           m_accuracyRecoverPerSecond  = 1;
    //public      float           m_weaponRecoilBack          = 0.1f;
    //public      float           m_weaponRecoilUp            = 0.1f;
    //public      float           m_recoilRecovery            = 10;
    [Space]
    public      int             m_initAmmo                  = 150;
    public      int             m_clipAmmo                  = 12;

    [Header("Shoot effects")]
    public      AudioClip       m_fireSound                 = null;
    public      GameObject      m_bulletShell               = null;
    public      AudioClip       m_bulletShellSound          = null;
    public      float           m_bulletShellSpawnForce     = 1.5f;
    
    [Header("Reload")]
    public      AudioClip       m_reloadWithAmmoSound       = null;
    public      AudioClip       m_reloadWithoutAmmoSound    = null;
    public      float           m_reloadWithAmmoTime        = 2.0f;
    public      float           m_reloadWithoutAmmoTime     = 2.5f;

    [Space]
    [Header("KNIFE")]
    [Header("Attributes")]
    public      GameObject      m_hitbox1                   = null;
    public      GameObject      m_hitbox2                   = null;

    [Header("Effects")]
    public      AudioClip       m_hit1Sound                 = null;
    public      AudioClip       m_hit2Sound                 = null;

    [Space]

    [Header("UTILITY")]
    [Header("Attributes")]
    //public      UtilityType     m_type                      = UtilityType.greenade;
    public      int             m_maxStack                  = 1;
}
