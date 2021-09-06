using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class WeaponItem
{
    [Space]
    public      string          m_name                      = "";

    [Space]
    public      E_ITEM_TYPE      m_itemType                = E_ITEM_TYPE.SECONDARY_WEAPON;

    [Header("ATTRIBUTES")]
    public      int             m_damage                    = 50;
    //public      float           m_forceToApply              = 20.0f;
    public      int             m_bulletsPerShoot           = 7;
    public      int             m_bulletsPerBurst           = 3;
    public      int             m_maxRange                  = 9999;
    public      AnimationCurve  m_distanceDamageReduction   = null;
    public      float           m_accuracy                  = 100;
    public      float           m_accuracyDropPerShot       = 3;
    public      float           m_accuracyRecoverPerSecond  = 50;
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
    public      GameObject      m_bulletTracer              = null;

    [Header("RELOAD")]
    public      AudioClip       m_reloadAmmoLeftSound       = null;
    public      AudioClip       m_reloadOutOfAmmoSound      = null;
    
    [Space]
    public      AudioClip       m_holsterSound              = null;
    public      AudioClip       m_drawSound                 = null;
}
