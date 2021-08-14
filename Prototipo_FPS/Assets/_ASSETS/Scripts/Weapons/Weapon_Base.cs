using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class Weapon_Base : MonoBehaviour
{
    [SerializeField]
    protected int m_fireLayer = 0;

    protected Animator m_cmpAnimator = null;
    protected AudioSource m_cmpAudioSource = null;

    [SerializeField]
    private LayerMask m_shootLM = 0;

    [SerializeField]
    protected string m_weaponName = "";
    protected WeaponList m_weaponList = null;
    [SerializeField]
    protected WeaponItem m_data = null;

    [SerializeField]
    protected Transform m_shootSpot = null;
    protected Vector3 m_shootDir = Vector3.zero;
    [SerializeField]
    protected ParticleSystem m_muzzleFlash = null;
    [SerializeField]
    protected GameObject m_bulletTracer = null;

    protected bool m_isMoving = false;
    protected bool m_canShoot = false;

    [SerializeField]
    protected E_WEAPON_STATE m_crntState = E_WEAPON_STATE.DEFAULT;

    protected float m_crntFireRate = 0;

    [SerializeField]
    protected float m_crntAcc = 100;
    [SerializeField]
    protected float m_crntAccRecoverDelay = 0;

    protected int m_crntAmmo = 0;
    protected int m_totalAmmo = 0;


    /*PROPERTIES*/
    public E_WEAPON_STATE CrntState { get => m_crntState; }
    public bool CanShoot { get => m_canShoot; }
    public bool HasChamber { get => m_data.m_hasChamber; }
    public int ClipAmmo { get => m_data.m_clipAmmo; }
    public int CrntAmmo
    {
        get => m_crntAmmo;

        set
        {
            if (HasChamber)
            {
                value = Mathf.Clamp(value, 0, ClipAmmo + 1);
            }
            else
            {
                value = Mathf.Clamp(value, 0, ClipAmmo);
            }
            m_crntAmmo = value;
        }
    }
    public int TotalAmmo { get => m_totalAmmo; set => m_totalAmmo = Mathf.Clamp(value, 0, int.MaxValue); }
    public float CrntFireRate { get => m_crntFireRate; set => m_crntFireRate = Mathf.Clamp(value, 0, 60 / FireRate); }
    public float FireRate { get => m_data.m_fireRate; }
    public float CrntAcc { get => m_crntAcc; set => m_crntAcc = Mathf.Clamp(value, 0, 100); }
    public float AccRecoverDelay { get => m_data.m_accuracyRecoverDelay; }
    public float CrntAccRecoverDelay { get => m_crntAccRecoverDelay; set => m_crntAccRecoverDelay = Mathf.Clamp(value, 0, AccRecoverDelay); }
    public Animator CmpAnimator { get => m_cmpAnimator; }



    protected virtual void Awake()
    {
        m_cmpAnimator = GetComponentInChildren<Animator>();
        m_cmpAudioSource = GetComponentInParent<AudioSource>();

        m_weaponList = Resources.Load<WeaponList>("Weapons/WeaponList");

        for (int i = 0; i < m_weaponList.weaponList.Count; i++)
        {
            if (m_weaponName.Equals(m_weaponList.weaponList[i].m_name, System.StringComparison.Ordinal))
            {
                m_data = m_weaponList.weaponList[i];
                break;
            }
        }

        CrntAcc = m_data.m_accuracy;

        TotalAmmo = m_data.m_totalAmmo;

        if (m_data.m_hasChamber)
        {
            CrntAmmo = ClipAmmo + 1;
        }
        else
        {
            CrntAmmo = ClipAmmo;
        }
    }


    protected virtual void Start()
    {
        m_canShoot = true;

        m_crntAccRecoverDelay = 0;
    }

    // Update is called once per frame
    private void Update()
    {
        //Debug.DrawRay(MainCamera.transform.position, MainCamera.transform.forward * 500, Color.yellow);

        UpdateFireRate();

        AccRecover();
    }

    protected virtual void AccRecover()
    {
        CrntAccRecoverDelay -= Time.deltaTime;

        if (CrntAccRecoverDelay == 0)
        {
            CrntAcc = Mathf.Lerp(CrntAcc, 100, m_data.m_accuracyRecoverPerSecond * Time.deltaTime);
        }
    }

    protected void ResetAccRecover()
    {
        CrntAccRecoverDelay = AccRecoverDelay;
    }

    /*FIRE RATE*/
    protected virtual void UpdateFireRate()
    {
        if (CrntFireRate > 0)
        {
            CrntFireRate -= Time.deltaTime;

            if (CrntFireRate == 0)
            {
                m_canShoot = true;
            }
        }
    }

    protected void ResetFireRate()
    {
        m_canShoot = false;
        CrntFireRate = 60 / FireRate;

    }

    protected IEnumerator RoutineFire()
    {
        yield return new WaitForSeconds(0.05f);

        Fire();
    }

    protected void Fire()
    {
        if (CrntAmmo == 0) { return; }

        m_cmpAnimator.Play("Fire", m_fireLayer, 0f);

        m_cmpAudioSource.PlayOneShot(m_data.m_fireSound, 0.25f);

        ResetFireRate();

        ResetAccRecover();

        DecrementAmmo();

        if (m_muzzleFlash)
        {
            m_muzzleFlash.Play();
        }

        RaycastHit rHit;
        Vector3 destination;

        //SHOOT
        if (Physics.Raycast(GetRay(), out rHit, m_data.m_range, m_shootLM))
        {
            Char_Base cmpChar = null;

            GameObject bImpactPrefab = null;

            if (rHit.transform.TryGetComponent(out cmpChar) && cmpChar)
            {
                cmpChar.ReceiveDamage(m_data.m_damage);

                bImpactPrefab = cmpChar.BulletImpact;

                /*if (impactPrefab)
                {
                    Instantiate(impactPrefab, rHit.point, Quaternion.LookRotation(rHit.normal), cmpChar.transform.parent);
                }*/
            }
            else
            {
                bImpactPrefab = GameManager.Instance.BISurfaceG;

            }

            if (bImpactPrefab)
            {
                GameObject bImpact;

                bImpact = Instantiate(bImpactPrefab, rHit.point, Quaternion.LookRotation(rHit.normal));
                bImpact.transform.parent = rHit.transform;
            }

            destination = rHit.point;
        }
        else
        {
            destination = m_shootDir * m_data.m_range;
        }

        //BULLET TRACER
        if (m_bulletTracer)
        {
            BulletTracer(destination);
        }
    }


    protected virtual Ray GetRay()
    {

        return new Ray();
    }

    protected virtual void BulletTracer(Vector3 _destination)
    {

    }

    /*RELOAD && AMMO*/
    public void StartReload()
    {
        if (HasChamber)
        {
            if (CrntAmmo == ClipAmmo + 1) { return; }
        }
        else
        {
            if (CrntAmmo == ClipAmmo) { return; }

        }

        if (TotalAmmo == 0) { return; }

        if (m_crntState == E_WEAPON_STATE.DRAWING)
        {
            EndDraw();
        }

        m_crntState = E_WEAPON_STATE.RELOADING;

        if (CrntAmmo > 0)
        {
            m_cmpAnimator.Play("ReloadAmmoLeft", m_fireLayer, 0f);

            m_cmpAudioSource.PlayOneShot(m_data.m_reloadAmmoLeftSound);
        }
        else
        {
            m_cmpAnimator.Play("ReloadOutOfAmmo", m_fireLayer, 0f);

            m_cmpAudioSource.PlayOneShot(m_data.m_reloadOutOfAmmoSound);
        }
    }

    //Anim Event
    protected virtual void ReloadAmmo()
    {
        int ammoReloaded = 0;

        ammoReloaded = ClipAmmo - CrntAmmo;

        if (HasChamber && CrntAmmo > 0)
        {
            ammoReloaded++;
        }

        if (ammoReloaded > TotalAmmo)
        {
            ammoReloaded -= TotalAmmo - ammoReloaded;
        }

        CrntAmmo += ammoReloaded;
        TotalAmmo -= ammoReloaded;
    }

    protected virtual void DecrementAmmo()
    {
        CrntAmmo--;
    }

    //Anim Event
    private void EndReload()
    {
        m_crntState = E_WEAPON_STATE.DEFAULT;

        m_cmpAudioSource.Stop();
    }

    public virtual void AddTotalAmmo(int _amount)
    {
        TotalAmmo += _amount;
    }


    /*HOLSTER*/
    public void Holster()
    {
        if (m_crntState != E_WEAPON_STATE.HOLSTERING)
        {
            StartHolster();
        }
        else
        {
            CancelHolster();
            StartDraw();
        }
    }

    public void StartHolster()
    {
        EndReload();
        Debug.LogWarning("START HOLSTER");
        m_crntState = E_WEAPON_STATE.HOLSTERING;
        m_cmpAnimator.SetBool("Holster", true);

        m_cmpAudioSource.PlayOneShot(m_data.m_holsterSound);
    }

    public void CancelHolster()
    {
        Debug.LogWarning("CANCEL HOLSTER");
        m_crntState = E_WEAPON_STATE.DEFAULT;
        m_cmpAnimator.SetBool("Holster", false);

        m_cmpAudioSource.Stop();
    }

    public void StartDraw()
    {
        Debug.LogWarning("START DRAW");
        m_crntState = E_WEAPON_STATE.DRAWING;

        m_cmpAudioSource.PlayOneShot(m_data.m_drawSound);
    }

    //Anim Event
    public void EndDraw()
    {
        m_crntState = E_WEAPON_STATE.DEFAULT;
        m_cmpAudioSource.Stop();
    }

    //Anim Event
    public virtual void EndHolster()
    {
        m_crntState = E_WEAPON_STATE.DEFAULT;
    }


    /*MOVING*/
    public void SetIsMoving(bool _isMoving)
    {
        m_isMoving = _isMoving;
        m_cmpAnimator.SetBool("IsMoving", m_isMoving);
    }

}
