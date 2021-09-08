using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public abstract class Weapon_Base : Item_Base
{
    protected Char_Base m_owner = null;
    protected PlayerWeaponController m_cmpWController = null;

    protected Animator m_cmpAnimator = null;
    protected AudioSource m_cmpAudioSource = null;

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

    protected bool m_triggerIsPullled = false;


    /*PROPERTIES*/
    public E_WEAPON_STATE CrntState { get => m_crntState; }
    public bool CanShoot { get => m_canShoot; }
    public bool HasChamber { get => m_data.m_hasChamber; }
    public bool HasInstantReload { get => m_data.m_hasInstantReload; }
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
    public Camera MainCamera { get => m_cmpWController.MainCamera; }
    public Camera WeaponCamera { get => m_cmpWController.WeaponCamera; }


    protected virtual void Awake()
    {
        m_cmpAnimator = GetComponentInChildren<Animator>();
        m_cmpAudioSource = GetComponentInParent<AudioSource>();
        m_cmpWController = GetComponentInParent<PlayerWeaponController>();

        m_shootSpot = transform.Find("Armature/weapon/ShootSpot");
        m_muzzleFlash = transform.Find("Armature/weapon/MuzzleFlash_" + m_weaponName).GetComponent<ParticleSystem>();

        if (!m_shootSpot)
        {
            Debug.LogError(transform.name + " - " + name + " Missing ShootSpot");
        }

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

        m_owner = GetComponentInParent<Char_Base>();
    }

    protected virtual void Start()
    {
        m_canShoot = true;

        m_crntAccRecoverDelay = 0;
    }

    protected virtual void Update()
    {
        //Debug.DrawRay(MainCamera.transform.position, MainCamera.transform.forward * 500, Color.yellow);

        UpdateFireRate();

        AccRecover();
    }

    protected override void TakeItem(Char_Base _char)
    {

    }

    #region ACC
    protected virtual void AccRecover()
    {
        CrntAccRecoverDelay -= Time.deltaTime;

        if (CrntAccRecoverDelay == 0)
        {
            CrntAcc += m_data.m_accuracyRecoverPerSecond * Time.deltaTime;
        }
    }

    protected void ResetAccRecover()
    {
        CrntAccRecoverDelay = AccRecoverDelay;
    }
    #endregion

    #region TRIGGER
    public virtual void PullTrigger()
    {
        m_triggerIsPullled = true;

        m_cmpAudioSource.PlayOneShot(m_data.m_triggerSound, 1);
    }

    public void ReleaseTrigger()
    {
        m_triggerIsPullled = false;

        if (CrntAmmo == 0 && m_crntState != E_WEAPON_STATE.RELOADING)
        {
            StartReload();
        }
    }
    #endregion

    #region FIRE RATE
    protected virtual void UpdateFireRate()
    {
        if (CanShoot) { return; }

        if (CrntFireRate > 0)
        {
            CrntFireRate -= Time.deltaTime;

            if (CrntFireRate <= 0)
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
    #endregion

    #region DAMAGE
    protected int CalculateDamage(Char_Base _target, E_HITBOX_PART _hPart)
    {
        int damage = m_data.m_damage;

        switch (_hPart)
        {
            case E_HITBOX_PART.HEAD:
                damage = Mathf.RoundToInt(damage * Hitbox.HEAD_MULT);
                break;
            case E_HITBOX_PART.CHEST:
                damage = Mathf.RoundToInt(damage * Hitbox.CHEST_MULT);
                break;
            case E_HITBOX_PART.ARM:
                damage = Mathf.RoundToInt(damage * Hitbox.ARM_MULT);
                break;
            case E_HITBOX_PART.STOMACH:
                damage = Mathf.RoundToInt(damage * Hitbox.STOMACH_MULT);
                break;
            case E_HITBOX_PART.LEG:
                damage = Mathf.RoundToInt(damage * Hitbox.LEG_MULT);
                break;
        }

        float distance = (_target.transform.position - transform.position).magnitude;

        float cPoint = Mathf.Clamp(distance / m_data.m_maxRange, 0, 1);

        float damageReducionPercent = m_data.m_distanceDamageReduction.Evaluate(cPoint);

        damage = Mathf.RoundToInt(damage * damageReducionPercent);

        //CONSOLE MESSAGE
        if (m_owner is Char_Player)
        {
            ConsoleManager.Instance.AddPlayerCauseDamageMessage(m_owner, _target, damage, _hPart, distance);
        }

        /*Debug.Log("------------------------------------");
        Debug.Log("Distance = " + charDistance);
        Debug.Log("Curve Point = " + cPoint + " (" + charDistance / m_data.m_maxRange + ")");
        Debug.Log("Damage Reduction Percent = " + damageReducionPercent);
        Debug.Log("Damage = " + damage);
        Debug.Log("------------------------------------");*/

        return damage;
    }

    protected void DoDamage(Char_Base _char, E_HITBOX_PART _hPart)
    {
        _char.ReceiveDamage(CalculateDamage(_char, _hPart));
    }
    #endregion

    #region FIRE
    protected void Fire()
    {
        if (CrntAmmo == 0) { return; }

        m_cmpAnimator.Play("Fire", 0, 0f);

        m_cmpAudioSource.PlayOneShot(m_data.m_fireSound, 0.25f);

        ResetFireRate();

        ResetAccRecover();

        DecrementAmmo();

        if (m_muzzleFlash)
        {
            m_muzzleFlash.Play();
        }

        Shoot();
    }

    protected virtual void Shoot()
    {
        RaycastHit rHit;
        Vector3 destination;

        if (Physics.Raycast(GetShootRay(), out rHit, m_data.m_maxRange, GameManager.Instance.ShootLM))
        {
            Hitbox cmpHitbox = null;
            Char_Base cmpChar = null;
            GameObject bImpactPrefab = null;

            if (rHit.transform.TryGetComponent(out cmpHitbox))
            {
                cmpChar = cmpHitbox.GetComponentInParent<Char_Base>();

                if (cmpChar)
                {
                    DoDamage(cmpChar, cmpHitbox.GetHitboxPart);
                }

                bImpactPrefab = cmpChar.BulletImpact;
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
            destination = m_shootDir * m_data.m_maxRange;
        }

        if (m_data.m_bulletTracer)
        {
            BulletTracer(destination);
        }
    }

    protected virtual Ray GetShootRay()
    {
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

    protected virtual void BulletTracer(Vector3 _destination)
    {
        Vector3 shootPos = WeaponCamera.WorldToScreenPoint(m_shootSpot.position);

        shootPos = MainCamera.ScreenToWorldPoint(shootPos);

        Debug.DrawLine(shootPos, m_shootDir, Color.magenta, 10f);

        BulletTracer bulletTracer = Instantiate(m_data.m_bulletTracer, shootPos, Quaternion.identity).GetComponent<BulletTracer>();

        bulletTracer.Init(_destination);
    }
    #endregion

    #region RELOAD & AMMO
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

        if (HasChamber)
        {
            if (CrntAmmo > 0)
            {
                m_cmpAnimator.Play("ReloadAmmoLeft", 0, 0f);

                m_cmpAudioSource.PlayOneShot(m_data.m_reloadAmmoLeftSound);
            }
            else
            {
                m_cmpAnimator.Play("ReloadOutOfAmmo", 0, 0f);

                m_cmpAudioSource.PlayOneShot(m_data.m_reloadOutOfAmmoSound);
            }
        }
        else
        {
            m_cmpAnimator.Play("Reload", 0, 0f);

            m_cmpAudioSource.PlayOneShot(m_data.m_reload);
        }
    }

    //Anim Event
    private void EndReload()
    {
        m_crntState = E_WEAPON_STATE.DEFAULT;

        m_cmpAudioSource.Stop();
    }

    //Anim Event
    protected void ReloadAmmo()
    {
        if (HasInstantReload)
        {
            InstantReload();
        }
        else
        {
            IncrementReload();
        }
    }

    protected void InstantReload()
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

        m_cmpWController.UpdateAmmoHUD();
    }

    //Anim Event
    protected void StartInsert()
    {
        m_cmpAudioSource.PlayOneShot(m_data.m_reloadInsert);
    }

    protected void IncrementReload()
    {
        CrntAmmo++;
        TotalAmmo--;

        m_cmpWController.UpdateAmmoHUD();

        if (CrntAmmo == ClipAmmo || TotalAmmo == 0)
        {
            CmpAnimator.SetTrigger("EndReload");
            m_cmpAudioSource.PlayOneShot(m_data.m_reloadClose);
        }
    }

    protected virtual void DecrementAmmo()
    {
        CrntAmmo--;

        m_cmpWController.UpdateAmmoHUD();
    }

    public virtual void AddTotalAmmo(int _amount)
    {
        TotalAmmo += _amount;
        m_cmpWController.UpdateAmmoHUD();
    }
    #endregion

    #region HOLSTER & DRAW
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
        //m_cmpAnimator.SetBool("Holster", true);
        m_cmpAnimator.Play("Holster", 0, 0);

        m_cmpAudioSource.PlayOneShot(m_data.m_holsterSound);
    }

    public void CancelHolster()
    {
        Debug.LogWarning("CANCEL HOLSTER");
        m_crntState = E_WEAPON_STATE.DEFAULT;
        //m_cmpAnimator.SetBool("Holster", false);

        m_cmpAudioSource.Stop();
    }

    public void StartDraw()
    {
        Debug.LogWarning("START DRAW");
        m_crntState = E_WEAPON_STATE.DRAWING;
        m_cmpAnimator.Play("Draw", 0, 0);

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
        m_cmpWController.ChangeWeapon();
    }
    #endregion

    public void SetIsMoving(bool _isMoving)
    {
        m_isMoving = _isMoving;
        m_cmpAnimator.SetBool("IsMoving", m_isMoving);
    }
}
