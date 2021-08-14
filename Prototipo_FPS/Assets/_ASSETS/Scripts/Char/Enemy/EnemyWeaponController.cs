using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponController : MonoBehaviour
{
    private Weapon_Enemy m_crntWeapon = null;

    public Weapon_Enemy CrntWeapon { get => m_crntWeapon; }



    private void Awake()
    {
        m_crntWeapon = GetComponent<Weapon_Enemy>();
    }


    public void Shoot()
    {
        CrntWeapon.Shoot();
    }
}
