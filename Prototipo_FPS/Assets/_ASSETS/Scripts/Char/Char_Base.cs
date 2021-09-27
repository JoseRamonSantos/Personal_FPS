using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Char_Base : MonoBehaviour
{
    #region VARIABLES
    [SerializeField]
    protected Animator m_cmpAnimator = null;
    [SerializeField]
    protected GameObject m_bulletImpact = null;
    [SerializeField]
    protected int m_maxHP = 100;
    [SerializeField]
    protected int m_crntHP = 0;

    [SerializeField]
    protected bool m_invulnerable = false;

    protected bool m_isDead = false;
    #endregion

    #region PROPERTIES
    public GameObject BulletImpact { get => m_bulletImpact; }
    public int MaxHP { get => m_maxHP; }
    public int CrntHP { get => m_crntHP; set => m_crntHP = Mathf.Clamp(value, 0, MaxHP); }
    public bool IsDead { get => m_isDead; }
    #endregion


    #region METHODS
    protected virtual void Awake()
    {
        m_cmpAnimator = GetComponent<Animator>();
    }

    protected virtual void Start()
    {
        if (CrntHP == 0)
        {
            CrntHP = MaxHP;
        }
    }

    public virtual void DoDamage(Char_Base _target, int _dmg)
    {
        _target.ReceiveDamage(_dmg);
    }

    public virtual void DoDamage(Char_Base _target, int _dmg, E_HITBOX_PART _part)
    {
        _target.ReceiveDamage(_dmg);
    }

    public virtual void ReceiveDamage(int _damage)
    {
        if (m_isDead) { return; }

        Debug.Log(transform.name + " has received " + _damage + " damage. (Invulnerability =" + m_invulnerable + ")");

        if (m_cmpAnimator)
        {
            m_cmpAnimator.SetTrigger("Hit");
        }

        if (m_invulnerable) { return; }

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
        if (m_cmpAnimator)
        {
            m_cmpAnimator.ResetTrigger("Hit");
            m_cmpAnimator.Play("Die", 0, 0);
        }

        m_isDead = true;
    }
    #endregion
}
