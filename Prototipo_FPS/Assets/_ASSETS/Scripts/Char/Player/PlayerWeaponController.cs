using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    public static PlayerWeaponController Instance = null;

    private PlayerMovement m_cmpPlayerMovement = null;

    [SerializeField]
    private AmmoHUD m_ammoHUD = null;

    private Camera m_mainCamera = null;
    private Camera m_weaponCamera = null;

    [SerializeField]
    private Weapon_Base[] m_weaponsList = null;
    [SerializeField]
    private Weapon_Base m_crntWeapon = null;

    [SerializeField]
    private int m_iCrntWeapon;
    [SerializeField]
    private int m_iNextWeapon;

    private GameObject m_grenadePrefab = null;
    private GameObject m_handGrenade = null;
    [SerializeField]
    private int m_grenadesNumber = 1;

    public Camera MainCamera { get => m_mainCamera; }
    public Camera WeaponCamera { get => m_weaponCamera; }

    public Weapon_Base CrntWeapon { get => m_crntWeapon; }



    private void Awake()
    {
        Instance = this;

        m_cmpPlayerMovement = GetComponent<PlayerMovement>();

        m_mainCamera = transform.Find("MainCamera").GetComponent<Camera>();
        m_weaponCamera = transform.Find("MainCamera/WeaponCamera").GetComponent<Camera>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        m_weaponsList = GetComponentsInChildren<Weapon_Base>();

        if (m_weaponsList.Length > 0)
        {
            for (int i = 0; i < m_weaponsList.Length; i++)
            {
                m_weaponsList[i].gameObject.SetActive(false);
            }

            ToCustomWeapon(0);
        }
        else
        {
            Debug.LogError(transform.name + " - " + name + " MISSING Weapons List");
        }
    }

    void Update()
    {
        ManageWeapons();

        if (CrntWeapon)
        {
            bool canPullTrigger = CrntWeapon.CanShoot && CrntWeapon.CrntState == E_WEAPON_STATE.DEFAULT;

            if (Input.GetMouseButtonDown(0) && canPullTrigger)
            {
                CrntWeapon.PullTrigger();
            }

            if (Input.GetMouseButtonUp(0))
            {
                CrntWeapon.ReleaseTrigger();
            }

            bool canReload = CrntWeapon.CrntState == E_WEAPON_STATE.DEFAULT || CrntWeapon.CrntState == E_WEAPON_STATE.DRAWING;

            if (Input.GetKeyDown(KeyCode.R) && canReload)
            {
                CrntWeapon.StartReload();
            }
        }

        SetIsMoving();
    }

    public void SetIsMoving()
    {
        if (m_cmpPlayerMovement)
        {
            CrntWeapon.SetIsMoving(m_cmpPlayerMovement.IsMoving);
        }
    }

    private void StartThrowGrenade()
    {
        //m_grenadesNumber--;
        CrntWeapon.CmpAnimator.Play("GrenadeThrow", 0, 0f);
    }

    //AnimEvent
    private void ThrowGrenade()
    {
        m_handGrenade.SetActive(true);

        Instantiate(m_grenadePrefab, m_handGrenade.transform.position, m_handGrenade.transform.rotation);

    }

    /*WEAPON AMMO HUD*/
    public void UpdateAmmoHUD()
    {
        if (CrntWeapon == null || m_ammoHUD == null)
        {
            return;
        }

        m_ammoHUD.OnUpdateAmmoHUD(CrntWeapon.CrntAmmo, CrntWeapon.TotalAmmo);
    }

    public void ManageWeapons()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ToCustomWeapon(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ToCustomWeapon(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ToCustomWeapon(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ToCustomWeapon(3);
        }

        if (Input.mouseScrollDelta.y > 0)
        {
            ToNextWeapon();
        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            ToPreviousWeapon();
        }
    }

    public void ToCustomWeapon(int _iNewWeapon)
    {
        if (m_weaponsList.Length <= _iNewWeapon) { return; }

        if (CrntWeapon && m_iNextWeapon == _iNewWeapon) { return; }

        m_iNextWeapon = _iNewWeapon;

        if (CrntWeapon)
        {

            CrntWeapon.Holster();
        }
        else
        {
            ChangeWeapon();
        }

    }
    public void ToNextWeapon()
    {
        int iNWeapon;

        if (m_iCrntWeapon + 1 < m_weaponsList.Length)
        {
            iNWeapon = m_iCrntWeapon + 1;
        }
        else
        {
            iNWeapon = 0;
        }

        if (iNWeapon == m_iCrntWeapon) { return; }

        ToCustomWeapon(iNWeapon);
    }

    public void ToPreviousWeapon()
    {
        int iPWeapon;

        if (m_iCrntWeapon - 1 >= 0)
        {
            iPWeapon = m_iCrntWeapon - 1;
        }
        else
        {
            if (m_iCrntWeapon == m_weaponsList.Length - 1) { return; }

            iPWeapon = m_weaponsList.Length - 1;
        }

        if (iPWeapon == m_iCrntWeapon) { return; }

        ToCustomWeapon(iPWeapon);
    }

    public void ChangeWeapon()
    {
        CrntWeapon?.gameObject.SetActive(false);

        m_iCrntWeapon = m_iNextWeapon;
        NewWeapon(m_weaponsList[m_iCrntWeapon]);
        CrntWeapon.gameObject.SetActive(true);
        CrntWeapon.StartDraw();

        UpdateAmmoHUD();
    }

    private void NewWeapon(Weapon_Base _newWeapon)
    {
        m_crntWeapon = _newWeapon;
    }

}
