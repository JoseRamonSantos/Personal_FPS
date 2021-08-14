using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Char_Base : MonoBehaviour
{
    [SerializeField]
    protected GameObject m_bulletImpact = null;
    [SerializeField]
    protected int m_maxHP = 100;
    [SerializeField]
    protected int m_crntHP = 0;

    [SerializeField]
    private Transform m_targetPoint = null;

    [SerializeField]
    protected bool m_invulnerable = false;

    protected bool m_isDead = false;

    public GameObject BulletImpact { get => m_bulletImpact; }
    public int MaxHP { get => m_maxHP; }
    public int CrntHP { get => m_crntHP; set => m_crntHP = Mathf.Clamp(value, 0, MaxHP); }
    public Transform TargetPoint { get => m_targetPoint; }


    protected virtual void Start()
    {
        if (CrntHP == 0)
        {
            CrntHP = MaxHP;
        }
    }

    public virtual void ReceiveDamage(int _damage)
    {
        if (m_isDead || m_invulnerable) { return; }

        CrntHP -= _damage;

        if (CrntHP == 0)
        {
            Die();
        }
    }

    public virtual void Heal(int _ammount)
    {
        CrntHP += _ammount;
    }

    protected virtual void Die()
    {
        m_isDead = true;
    }

}
