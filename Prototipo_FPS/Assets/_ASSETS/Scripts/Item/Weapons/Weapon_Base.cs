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
    [SerializeField]
    protected bool m_canShoot = false;

    [SerializeField]
    protected E_WEAPON_STATE m_crntState = E_WEAPON_STATE.DEFAULT;

    [SerializeField]
    protected float m_crntFireRate = 0;

    [SerializeField]
    protected float m_crntAcc = 100;
    [SerializeField]
    protected float m_crntAccRecoverDelay = 0;

    [Header("Recoil_Transform")]
    [SerializeField]
    protected Transform m_recoilPositionTranform;
    [SerializeField]
    protected Transform m_recoilRotationTranform;

    protected bool aim;
    [Header("Recoil settings")]
    [SerializeField]
    protected Vector3 m_crntRecoil1;
    [SerializeField]
    protected Vector3 m_crntRecoil2;
    [SerializeField]
    protected Vector3 m_crntRecoil3;
    [SerializeField]
    protected Vector3 m_crntRecoil4;
    [SerializeField]
    protected Vector3 m_rotationOutput;

    protected int m_crntAmmo = 0;
    protected int m_totalAmmo = 0;

    [SerializeField]
    protected bool m_triggerIsPullled = false;


    /*PROPERTIES*/
    public E_WEAPON_STATE CrntState { get => m_crntState; }
    public bool CanShoot { get => m_canShoot; }
    public bool HasChamber { get => m_data.m_options.m_hasChamber; }
    public bool HasInstantReload { get => m_data.m_options.m_hasInstantReload; }
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
    public float CrntFireRate { get => m_crntFireRate; set => m_crntFireRate = Mathf.Clamp(value, 0f, 60f / FireRate); }
    public int FireRate { get => m_data.m_fireRate; }
    public float CrntAcc { get => m_crntAcc; set => m_crntAcc = Mathf.Clamp(value, 0f, 100f); }
    public float AccRecoverDelay { get => m_data.m_accuracy.m_accuracyRecoverDelay; }
    public float CrntAccRecoverDelay { get => m_crntAccRecoverDelay; set => m_crntAccRecoverDelay = Mathf.Clamp(value, 0f, AccRecoverDelay); }
    public float AccRecoverPerSecond { get => m_data.m_accuracy.m_accuracyRecoverPerSecond; }
    public Animator CmpAnimator { get => m_cmpAnimator; }
    public Camera MainCamera { get => m_cmpWController.MainCamera; }
    public Camera WeaponCamera { get => m_cmpWController.WeaponCamera; }


    protected virtual void Awake()
    {
        if (!m_recoilPositionTranform)
        {
            m_recoilPositionTranform = transform.Find("Armature/weapon/ShootSpot");
        }
        if (!m_recoilRotationTranform)
        {
            m_recoilRotationTranform = transform.Find("Armature/weapon/ShootSpot");
        }

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

        CrntAcc = 100;

        TotalAmmo = m_data.m_totalAmmo;

        if (HasChamber)
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

    void FixedUpdate()
    {
        RecoilRecover();
    }

    protected void PlaySound(AudioClip _aClip, float _volume = 1)
    {
        if (m_cmpAudioSource && _aClip)
        {
            m_cmpAudioSource.PlayOneShot(_aClip, _volume);
        }
        else
        {
            if (!m_cmpAudioSource)
            {
                Debug.LogError(transform.name + " MISSING AudioSource");
            }
            else
            {
                Debug.LogError(transform.name + " MISSING AudioClip");
            }
        }
    }

    protected override void TakeItem(Char_Base _char)
    {

    }

    #region ACC & RECOIL
    protected virtual void AccRecover()
    {
        CrntAccRecoverDelay -= Time.deltaTime;

        if (CrntAccRecoverDelay == 0)
        {
            CrntAcc += AccRecoverPerSecond * Time.deltaTime;
        }
    }

    protected void ResetAccRecover()
    {
        CrntAccRecoverDelay = AccRecoverDelay;
    }

    private void RecoilRecover()
    {
        WeaponItem.RecoilSettings rSettings = m_data.m_recoilSettings;

        m_crntRecoil1 = Vector3.Lerp(m_crntRecoil1, Vector3.zero, rSettings.m_recoil1 * Time.deltaTime);
        m_crntRecoil2 = Vector3.Lerp(m_crntRecoil2, m_crntRecoil1, rSettings.m_recoil2 * Time.deltaTime);
        m_crntRecoil3 = Vector3.Lerp(m_crntRecoil3, Vector3.zero, rSettings.m_recoil3 * Time.deltaTime);
        m_crntRecoil4 = Vector3.Lerp(m_crntRecoil4, m_crntRecoil3, rSettings.m_recoil4 * Time.deltaTime);

        m_recoilPositionTranform.localPosition = Vector3.Slerp(m_recoilPositionTranform.localPosition, m_crntRecoil3, rSettings.m_positionDampTime * Time.fixedDeltaTime);
        m_rotationOutput = Vector3.Slerp(m_rotationOutput, m_crntRecoil1, rSettings.m_rotationDampTime * Time.fixedDeltaTime);
        m_recoilRotationTranform.localRotation = Quaternion.Euler(m_rotationOutput);
    }

    public void AddRecoil()
    {
        WeaponItem.RecoilSettings rSettings = m_data.m_recoilSettings;

        if (aim == true)
        {
            m_crntRecoil1 += new Vector3(
                rSettings.m_recoilRotation_Aim.x,
                Random.Range(-rSettings.m_recoilRotation_Aim.y, rSettings.m_recoilRotation_Aim.y),
                Random.Range(-rSettings.m_recoilRotation_Aim.z, rSettings.m_recoilRotation_Aim.z));

            m_crntRecoil3 += new Vector3(Random.Range(
                -rSettings.m_recoilKickBack_Aim.x, rSettings.m_recoilKickBack_Aim.x),
                Random.Range(-rSettings.m_recoilKickBack_Aim.y, rSettings.m_recoilKickBack_Aim.y),
                rSettings.m_recoilKickBack_Aim.z);
        }
        if (aim == false)
        {
            m_crntRecoil1 += new Vector3(
                rSettings.m_recoilRotation.x,
                Random.Range(-rSettings.m_recoilRotation.y, rSettings.m_recoilRotation.y),
                Random.Range(-rSettings.m_recoilRotation.z, rSettings.m_recoilRotation.z));

            m_crntRecoil3 += new Vector3(
                Random.Range(-rSettings.m_recoilKickBack.x, rSettings.m_recoilKickBack.x),
                Random.Range(-rSettings.m_recoilKickBack.y, rSettings.m_recoilKickBack.y),
                rSettings.m_recoilKickBack.z);
        }
    }
    #endregion

    #region TRIGGER
    public virtual void PullTrigger()
    {
        m_triggerIsPullled = true;

        PlaySound(m_data.m_soundClips.m_triggerSound);
    }

    public void ReleaseTrigger()
    {
        m_triggerIsPullled = false;

        //m_cmpAudioSource.PlayOneShot(m_data.m_triggerSound, 1);

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

        CrntFireRate -= Time.deltaTime;

        if (CrntFireRate <= 0)
        {
            m_canShoot = true;
        }
    }

    protected void ResetFireRate()
    {
        m_canShoot = false;
        CrntFireRate = 60f / FireRate;
        Debug.Log("------------- " + CrntFireRate + " // " + 60f / FireRate + " (" + FireRate + ")");
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

        float dPercent = Mathf.Clamp(distance / m_data.m_maxRange, 0, 1);

        float damageReducionPercent = m_data.m_options.m_distanceDamageReduction.Evaluate(dPercent);

        damage = Mathf.RoundToInt(damage * damageReducionPercent);

        /*Debug.Log("------------------------------------");
        Debug.Log("Distance = " + charDistance);
        Debug.Log("Curve Point = " + cPoint + " (" + charDistance / m_data.m_maxRange + ")");
        Debug.Log("Damage Reduction Percent = " + damageReducionPercent);
        Debug.Log("Damage = " + damage);
        Debug.Log("------------------------------------");*/

        return damage;
    }

    protected void DoDamage(Char_Base _target, E_HITBOX_PART _hPart)
    {
        m_owner.DoDamage(_target, CalculateDamage(_target, _hPart), _hPart);
    }
    #endregion

    #region FIRE
    protected void Fire()
    {
        if (CrntAmmo == 0) { return; }

        m_cmpAnimator.Play("Fire", 0, 0f);

        PlaySound(m_data.m_soundClips.m_fireSound, 0.25f);

        ResetFireRate();

        ResetAccRecover();

        DecreaseCrntAmmo(1);

        if (m_muzzleFlash)
        {
            m_muzzleFlash.Play();
        }

        if (m_data.m_options.m_projectile)
        {
            ShootProjectile();
        }
        else
        {
            ShootRaycast();
        }

        AddRecoil();
    }

    protected virtual void ShootProjectile()
    {
        Projectile_Base cmpProjectile;

        if (m_data.m_options.m_pfProjectile)
        {
            Ray ray = GetShootRay();
            Debug.DrawRay(ray.origin, ray.direction, Color.red, 5f);

            GameObject goProjectile = Instantiate(m_data.m_options.m_pfProjectile, m_shootSpot.transform.position, Quaternion.LookRotation(ray.direction));

            if (goProjectile.TryGetComponent(out cmpProjectile))
            {
                cmpProjectile.Activate(m_owner, m_data.m_damage, m_data.m_maxRange, m_data.m_options.m_distanceDamageReduction);
                Debug.Log(transform.name + " INSTANTIATE PROJECTILE " + cmpProjectile.name);
            }
            else
            {
                Debug.LogError(transform.name + " INVALID PROJECTILE. Projectile has been destroyed");
                Destroy(goProjectile);
            }
        }
        else
        {
            Debug.LogError(transform.name + " MISSING PROJECTILE");
        }
    }

    protected virtual void ShootRaycast()
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

                if (cmpChar && !cmpChar.IsDead)
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
            }

            destination = rHit.point;
        }
        else
        {
            destination = m_shootDir * m_data.m_maxRange;
        }

        if (m_data.m_options.m_bulletTracer)
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


        CrntAcc -= m_data.m_accuracy.m_accuracyDropPerShot;

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

        BulletTracer bulletTracer = Instantiate(m_data.m_options.m_bulletTracer, shootPos, Quaternion.identity).GetComponent<BulletTracer>();

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
                m_cmpAnimator.Play("ReloadAmmoLeft", 0, 0);

                PlaySound(m_data.m_soundClips.m_reloadAmmoLeftSound);
            }
            else
            {
                m_cmpAnimator.Play("ReloadOutOfAmmo", 0, 0);

                PlaySound(m_data.m_soundClips.m_reloadOutOfAmmoSound);
            }
        }
        else
        {
            m_cmpAnimator.Play("Reload", 0, 0f);

            PlaySound(m_data.m_soundClips.m_reload);
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

        IncreaseCrntAmmo(ammoReloaded);
        DecreaseTotalAmmo(ammoReloaded);

        m_cmpWController.UpdateAmmoHUD();
    }

    //Anim Event
    protected void StartInsert()
    {
        PlaySound(m_data.m_soundClips.m_reloadInsert);
    }

    protected void IncrementReload()
    {
        IncreaseCrntAmmo(1);
        DecreaseTotalAmmo(1);

        m_cmpWController.UpdateAmmoHUD();

        if (CrntAmmo == ClipAmmo || TotalAmmo == 0)
        {
            CmpAnimator.SetTrigger("EndReload");

            PlaySound(m_data.m_soundClips.m_reloadClose);
        }
    }

    protected virtual void DecreaseCrntAmmo(int _amount)
    {
        if (GameManager.Instance.InfiniteAmmo == E_INFINITE_AMMO.INFINITE_CLIP) { return; }

        CrntAmmo -= _amount;

        m_cmpWController.UpdateAmmoHUD();
    }
    protected virtual void IncreaseCrntAmmo(int _amount)
    {
        CrntAmmo += _amount;

        m_cmpWController.UpdateAmmoHUD();
    }

    public void IncreaseTotalAmmo(int _amount)
    {
        TotalAmmo += _amount;
        m_cmpWController.UpdateAmmoHUD();
    }

    public void DecreaseTotalAmmo(int _amount)
    {
        if (GameManager.Instance.InfiniteAmmo == E_INFINITE_AMMO.INFINITE_TOTAL) { return; }
        TotalAmmo -= _amount;
        m_cmpWController.UpdateAmmoHUD();
    }
    #endregion

    #region HOLSTER & DRAW
    /*public void Holster()
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
    }*/

    public void StartHolster()
    {
        EndReload();
        Debug.LogWarning("START HOLSTER");
        m_crntState = E_WEAPON_STATE.HOLSTERING;
        //m_cmpAnimator.SetBool("Holster", true);
        m_cmpAnimator.Play("Holster", 0, 0);

        PlaySound(m_data.m_soundClips.m_holsterSound);
    }

    /*public void CancelHolster()
    {
        Debug.LogWarning("CANCEL HOLSTER");
        m_crntState = E_WEAPON_STATE.DEFAULT;
        //m_cmpAnimator.SetBool("Holster", false);

        m_cmpAudioSource.Stop();

        StartDraw();
    }*/

    public void StartDraw()
    {
        Debug.LogWarning("START DRAW");
        m_crntState = E_WEAPON_STATE.DRAWING;
        m_cmpAnimator.Play("Draw", 0, 0);

        PlaySound(m_data.m_soundClips.m_drawSound);
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
