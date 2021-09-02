using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Weapon_Player : Item_Weapon_Base
{
    private E_FIRE_MODE m_fireMode = E_FIRE_MODE.FULL_AUTO;
    private int m_iBurst = 0;

    //Components
    private PlayerWeaponController m_cmpWController = null;

    private bool m_triggerIsPullled = false;


    public Camera MainCamera { get => m_cmpWController.MainCamera; }
    public Camera WeaponCamera { get => m_cmpWController.WeaponCamera; }



    protected override void Awake()
    {
        m_cmpWController = GetComponentInParent<PlayerWeaponController>();

        base.Awake();

        m_fireMode = m_data.m_fireMode;
    }

    protected override void Start()
    {
        base.Start();
        m_fireLayer = 0;
    }


    /*TRIGGER*/
    public void PullTrigger()
    {
        m_triggerIsPullled = true;

        m_cmpAudioSource.PlayOneShot(m_data.m_triggerSound, 1);

        StartCoroutine(RoutineFire());
    }

    public void ReleaseTrigger()
    {
        StopCoroutine(RoutineFire());

        m_triggerIsPullled = false;

        if (CrntAmmo == 0 && m_crntState != E_WEAPON_STATE.RELOADING)
        {
            StartReload();
        }
    }

    protected override void UpdateFireRate()
    {
        if (CrntFireRate > 0)
        {
            CrntFireRate -= Time.deltaTime;

            if (CrntFireRate == 0)
            {
                m_canShoot = true;

                switch (m_fireMode)
                {
                    case E_FIRE_MODE.FULL_AUTO:
                        if (m_triggerIsPullled && CrntAmmo > 0)
                        {
                            Fire();
                        }
                        break;
                    case E_FIRE_MODE.SEMI_AUTO:

                        break;
                    case E_FIRE_MODE.BURST:
                        if (m_iBurst + 1 <= 3)
                        {
                            m_iBurst++;
                            Fire();
                        }
                        else
                        {
                            m_iBurst = 0;
                        }
                        break;
                }
            }
        }
    }


    protected override Ray GetRay()
    {
        //Ray frontRay = MainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        m_shootDir = MainCamera.transform.forward;

        //MODIFY ACCURACY
        float accuracyModifier = (100 - CrntAcc) * 0.001f;

        m_shootDir.x += Random.Range(-accuracyModifier, accuracyModifier);
        m_shootDir.y += Random.Range(-accuracyModifier, accuracyModifier);
        m_shootDir.z += Random.Range(-accuracyModifier, accuracyModifier);


        CrntAcc -= m_data.m_accuracyDropPerShot;

        CrntAcc = Mathf.Clamp(CrntAcc, 0, 100);

        //RAYCAST
        //Debug.DrawRay(MainCamera.transform.position, m_shootDir * 500, Color.red, 0.1f);

        Ray shootRay = new Ray(MainCamera.transform.position, m_shootDir);

        return shootRay;
    }


    protected override void BulletTracer(Vector3 _destination)
    {
        Vector3 shootPos = WeaponCamera.WorldToScreenPoint(m_shootSpot.position);

        shootPos = MainCamera.ScreenToWorldPoint(shootPos);

        Debug.DrawLine(shootPos, m_shootDir, Color.magenta, 10f);

        BulletTracer bulletTracer = Instantiate(m_data.m_bulletTracer, shootPos, Quaternion.identity).GetComponent<BulletTracer>();

        bulletTracer.Init(_destination);
    }

    /*RELOAD && AMMO*/
    //Anim Event
    protected override void ReloadAmmo()
    {
        base.ReloadAmmo();

        m_cmpWController.UpdateAmmoHUD();
    }

    protected override void DecrementAmmo()
    {
        base.DecrementAmmo();

        m_cmpWController.UpdateAmmoHUD();
    }

    public override void AddTotalAmmo(int _amount)
    {
        base.AddTotalAmmo(_amount);

        m_cmpWController.UpdateAmmoHUD();
    }

    /*HOLSTER*/
    //Anim Event
    public override void EndHolster()
    {
        base.EndHolster();
        m_cmpWController.ChangeWeapon();
    }
}
