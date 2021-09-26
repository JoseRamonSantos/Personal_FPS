using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class WeaponItem
{
    [Space]
    public string m_name = "";

    [Space]
    public E_ITEM_TYPE m_itemType = E_ITEM_TYPE.SECONDARY_WEAPON;

    [Header("Attributes")]
    public int m_damage = 50;
    //public      float           m_forceToApply              = 20.0f;
    public float m_fireRate = 60;
    public int m_maxRange = 9999;
    [Space]
    public int m_totalAmmo = 150;
    public int m_clipAmmo = 30;


    [System.Serializable]
    public class Accuracy
    {
        public float m_accuracy = 100;
        public float m_accuracyDropPerShot = 3;
        public float m_accuracyRecoverPerSecond = 50;
        public float m_accuracyRecoverDelay = 0.2f;
    }
    public Accuracy m_accuracy = null;

    [System.Serializable]
    public class Options
    {
        public int m_bulletsPerShoot = 5;
        public int m_bulletsPerBurst = 3;

        [Space]
        public AnimationCurve m_distanceDamageReduction = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 0));

        [Space]
        public bool m_hasChamber = true;
        public bool m_hasInstantReload = true;


        [Space]
        public bool m_instantiateProjectile = false;
        public GameObject m_pfProjectile = null;
    }
    public Options m_options = null;

    //[Header("RECOIL SETTINGS")]
    [System.Serializable]
    public class RecoilSettings
    {
        public float m_positionDampTime = 6;
        public float m_rotationDampTime = 9;
        [Space(10)]
        public float m_recoil1 = 35;
        public float m_recoil2 = 50;
        public float m_recoil3 = 35;
        public float m_recoil4 = 50;
        [Space(10)]
        public Vector3 m_recoilRotation = new Vector3(-35, 4, 15);
        public Vector3 m_recoilKickBack = new Vector3(0.2f, 0.2f, -0.3f);
        public Vector3 m_recoilRotation_Aim = new Vector3(0, 0, 0);
        public Vector3 m_recoilKickBack_Aim = new Vector3(0, 0, 0);
    }
    public RecoilSettings m_recoilSettings = null;


    [Space]
    public GameObject m_bulletShell = null;
    public GameObject m_bulletTracer = null;

    //[Header("SHOOT EFFECTS")]
    [System.Serializable]
    public class SoundClips
    {
        public AudioClip m_triggerSound = null;
        public AudioClip m_fireSound = null;
        public AudioClip m_bulletShellSound = null;

        [Header("Reload")]
        public AudioClip m_reloadAmmoLeftSound = null;
        public AudioClip m_reloadOutOfAmmoSound = null;
        public AudioClip m_reload = null;
        public AudioClip m_reloadInsert = null;
        public AudioClip m_reloadClose = null;

        [Space]
        public AudioClip m_holsterSound = null;
        public AudioClip m_drawSound = null;
    }
    public SoundClips m_soundClips = null;

}
