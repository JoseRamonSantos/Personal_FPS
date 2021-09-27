using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Char_Player : Char_Base
{
    public static Char_Player Instance = null;

    private PlayerController m_cmpPController = null;
    private PlayerMovement m_cmpPMovement = null;
    private PlayerWeaponController m_cmpWController = null;

    [SerializeField]
    private HpHUD m_hpHUD = null;

    public PlayerWeaponController CmpWController { get => m_cmpWController; }


    protected override void Awake()
    {
        Instance = this;

        m_cmpPController = GetComponent<PlayerController>();
        m_cmpPMovement = GetComponent<PlayerMovement>();
        m_cmpWController = GetComponent<PlayerWeaponController>();
    }

    protected override void Start()
    {
        base.Start();
        
        if (m_hpHUD)
        {
            m_hpHUD.UpdateHpHUD(CrntHP);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            ReceiveDamage(25);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            GameManager.Instance.RestartLevel();
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            Time.timeScale = 1;
        }
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            Time.timeScale = 0.75f;
        }
        else if (Input.GetKeyDown(KeyCode.F3))
        {
            Time.timeScale = 0.5f;
        }
        else if (Input.GetKeyDown(KeyCode.F4))
        {
            Time.timeScale = 0.25f;
        }
        else if (Input.GetKeyDown(KeyCode.F5))
        {
            Time.timeScale = 0.1f;
        }
    }

    protected override void Die()
    {
        base.Die();
        m_cmpPController.enabled = false;
        m_cmpPMovement.enabled = false;
        m_cmpWController.CrntWeapon.enabled = false;
        m_cmpWController.enabled = false;
    }

    public override void DoDamage(Char_Base _target, int _dmg)
    {
        base.DoDamage(_target, _dmg);

        ConsoleManager.Instance.AddPlayerCauseDamageMessage(this, _target, _dmg, (_target.transform.position - transform.position).magnitude);
    }

    public override void DoDamage(Char_Base _target, int _dmg, E_HITBOX_PART _hPart)
    {
        base.DoDamage(_target, _dmg, _hPart);

        ConsoleManager.Instance.AddPlayerCauseDamageMessage(this, _target, _dmg, _hPart, (_target.transform.position - transform.position).magnitude);
    }

    public override void ReceiveDamage(int _damage)
    {
        base.ReceiveDamage(_damage);
        if (m_hpHUD)
        {
            m_hpHUD.UpdateHpHUD(CrntHP);
        }
    }

    public override void Heal(int _ammount)
    {
        base.Heal(_ammount);
        if (m_hpHUD)
        {
            m_hpHUD.UpdateHpHUD(CrntHP);
        }
    }
}
