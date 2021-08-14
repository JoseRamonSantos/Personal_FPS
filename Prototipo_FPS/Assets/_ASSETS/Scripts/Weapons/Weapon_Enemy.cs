using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Weapon_Enemy : Weapon_Base
{
    protected Transform m_playerTargetPoint = null;
    protected Transform m_enemyTargetPoint = null;

    protected override void Awake()
    {
        base.Awake();
        m_playerTargetPoint = FindObjectOfType<Char_Player>().TargetPoint;
        m_enemyTargetPoint = GetComponent<Char_Enemy>().TargetPoint;
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void UpdateFireRate()
    {
        base.UpdateFireRate();
    }

    public void Shoot()
    {
        Fire();
        m_fireLayer = 1;
    }

    protected override Ray GetRay()
    {

        m_shootDir = (m_playerTargetPoint.position - m_shootSpot.position).normalized;

        //MODIFY ACCURACY
        float accuracyModifier = 0.01f;

        if(GetComponent<NavMeshAgent>().velocity != Vector3.zero)
        {
            accuracyModifier = 0.025f;
        }

        m_shootDir.x += Random.Range(-accuracyModifier, accuracyModifier);
        m_shootDir.y += Random.Range(-accuracyModifier*2, accuracyModifier*2);
        m_shootDir.z += Random.Range(-accuracyModifier, accuracyModifier);


        CrntAcc -= m_data.m_accuracyDropPerShot;

        CrntAcc = Mathf.Clamp(CrntAcc, 0, 100);

        //RAYCAST
        Debug.DrawRay(m_shootSpot.position, m_shootDir * 500, Color.magenta, 0.1f);

        Ray shootRay = new Ray(m_shootSpot.position, m_shootDir);

        return shootRay;
    }

    protected override void BulletTracer(Vector3 _destination)
    {
        Vector3 shootPos = m_shootSpot.position;

        Vector3 shootDir = (_destination - shootPos).normalized;

        //Debug.DrawRay(shootPos, shootDir*500, Color.yellow, 0.1f);

        BulletTracer bulletTracer = Instantiate(m_bulletTracer, shootPos, Quaternion.identity).GetComponent<BulletTracer>();

        bulletTracer.Init(_destination);
    }
}
