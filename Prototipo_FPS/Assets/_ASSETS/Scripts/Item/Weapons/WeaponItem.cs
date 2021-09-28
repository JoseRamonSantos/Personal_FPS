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
    public E_WEAPON_TYPE m_wType = E_WEAPON_TYPE.RIFLE;

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
        public float m_accuracyDropPerShot = 3;
        public float m_accuracyRecoverPerSecond = 50;
        public float m_accuracyRecoverDelay = 0.2f;

        public Accuracy()
        {
            m_accuracyDropPerShot = 3;
            m_accuracyRecoverPerSecond = 50;
            m_accuracyRecoverDelay = 0.2f;
        }
    }
    public Accuracy m_accuracy = null;

    [System.Serializable]
    public class Options
    {
        public int m_bulletsPerShoot = 5;

        [Space]
        public AnimationCurve m_distanceDamageReduction = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 0));

        [Space]
        public bool m_hasChamber = true;
        public bool m_hasInstantReload = true;

        [Space]
        public bool m_bulletTracer = false;
        public GameObject m_pfBulletTracer = null;
        public bool m_projectile = false;
        public GameObject m_pfProjectile = null;
        public bool m_bulletShell = false;
        public GameObject m_pfBulletShell = null;

        public Options()
        {
            m_bulletsPerShoot = 5;
            m_distanceDamageReduction = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 0));
            m_hasChamber = true;
            m_hasInstantReload = true;
            m_projectile = false;
            m_pfProjectile = null;
            m_pfBulletTracer = null;
            m_bulletShell = false;
            m_pfBulletShell = null;
        }
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

        public RecoilSettings()
        {
            m_positionDampTime = 6;
            m_rotationDampTime = 9;
            m_recoil1 = 35;
            m_recoil2 = 50;
            m_recoil3 = 35;
            m_recoil4 = 50;
            m_recoilRotation = new Vector3(-35, 4, 15);
            m_recoilKickBack = new Vector3(0.2f, 0.2f, -0.3f);
            m_recoilRotation_Aim = new Vector3(0, 0, 0);
            m_recoilKickBack_Aim = new Vector3(0, 0, 0);
        }
    }
    public RecoilSettings m_recoilSettings = null;


    //[Header("SHOOT EFFECTS")]
    [System.Serializable]
    public class SoundClips
    {
        public AudioClip m_fireSound = null;
        public AudioClip m_triggerSound = null;
        public AudioClip m_bulletShellSound = null;
        [Space]
        public AudioClip m_holsterSound = null;
        public AudioClip m_drawSound = null;

        [Header("Reload")]
        public AudioClip m_reloadAmmoLeftSound = null;
        public AudioClip m_reloadOutOfAmmoSound = null;
        public AudioClip m_reload = null;
        public AudioClip m_reloadInsert = null;
        public AudioClip m_reloadClose = null;

        public SoundClips()
        {
            m_fireSound = null;
            m_triggerSound = null;
            m_bulletShellSound = null;
            m_holsterSound = null;
            m_drawSound = null;
            m_reloadAmmoLeftSound = null;
            m_reloadOutOfAmmoSound = null;
            m_reload = null;
            m_reloadInsert = null;
            m_reloadClose = null;
        }

    }
    public SoundClips m_soundClips = null;

    public WeaponItem()
    {
        m_name = "New Weapon";

        m_wType = E_WEAPON_TYPE.RIFLE;
        m_damage = 50;
        m_fireRate = 60;
        m_maxRange = 9999;
        m_totalAmmo = 150;
        m_clipAmmo = 30;

        m_accuracy = new Accuracy();
        m_options = new Options();
        m_recoilSettings = new RecoilSettings();
        m_soundClips = new SoundClips();
    }
}
