using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Weapon_Player : Weapon_Base
{
    private int m_iBurst = 0;

    protected override void Awake()
    {
        m_cmpWController = GetComponentInParent<PlayerWeaponController>();

        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    /*protected override void UpdateFireRate()
    {
        if (CrntFireRate > 0)
        {
            CrntFireRate -= Time.deltaTime;

            if (CrntFireRate == 0)
            {
                m_canShoot = true;

                switch (m_fireMode)
                {
                    case E_WEAPON_TYPE.FULL_AUTO:
                        if (m_triggerIsPullled && CrntAmmo > 0)
                        {
                            Shoot();
                        }
                        break;
                    case E_WEAPON_TYPE.SEMI_AUTO:

                        break;
                    case E_WEAPON_TYPE.BURST:
                        if (m_iBurst + 1 <= 3)
                        {
                            m_iBurst++;
                            Shoot();
                        }
                        else
                        {
                            m_iBurst = 0;
                        }
                        break;
                }
            }
        }
    }*/


    protected override Ray GetShootRay()
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
}
