using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct s_HitData
{
    public  RaycastHit      m_hit;
    public  e_ObjectType    m_objectType;
    public  float           m_objectWidth;//Only if type==normal
    public  e_material      m_material;
}

[System.Serializable]
public enum e_ObjectType
{
    normal,
    wallbang,
    character,
}

[System.Serializable]
public enum e_material
{
    concrete,
    plastic,
    wood, 
    metal,

}

[RequireComponent(typeof(W_DropAndTake))]
public class W_Base : ItemBase
{

    [SerializeField]
    protected   bool                m_startOnTheGround      = true;

    protected   WeaponList          m_weaponList            = null;
    public      WeaponItem          data                    = null;

    [SerializeField]
    protected   Transform           m_shootSpot             = null;

    protected   W_DropAndTake       cmp_DropAndTake         = null;

    protected   bool                m_reloading             = false;
    [SerializeField]
    protected   bool                m_canShoot              = true;

    protected   float               m_crntRealoadTime       = 0;
    protected   float               m_reloadTime            = 0;

    protected   float               m_crntFireRate          = 0;

    protected   float               m_crntAccuracy          = 100;

    protected   int                 m_totalAmmo             = 0;
    protected   int                 m_crntAmmo              = 0;
    protected   int                 m_clipAmmo              = 0;

    protected   bool                m_triggerIsPullled      = false;

    [SerializeField]
    protected   s_HitData[]         m_hitData               = new s_HitData[5];

    protected   RaycastHit[]        m_enterHits             = new RaycastHit[5];
    protected   RaycastHit[]        m_exitHits              = new RaycastHit[4];

    [SerializeField]
    private LineRenderer m_bulletTrail = null;

    protected override void Awake()
    {
        base.Awake();

        cmp_DropAndTake = GetComponent<W_DropAndTake>();
    }


    protected override void Start()
    {
        base.Start();

        m_weaponList = Resources.Load<WeaponList>("WeaponList");

        for (int i = 0; i < m_weaponList.weaponList.Count; i++)
        {
            if (this.gameObject.name.Equals(m_weaponList.weaponList[i].m_name, System.StringComparison.Ordinal))
            {
                data = m_weaponList.weaponList[i];
                break;
            }
        }

        m_crntFireRate = data.m_fireRate;
        m_reloadTime = data.m_reloadWithAmmoTime;

        m_shootSpot = transform.Find("ShootSpot");



        m_crntAccuracy = data.m_accuracy;
        m_crntAccuracy = Mathf.Clamp(m_crntAccuracy, 0, 100);

        //Raycasts
        m_hitData = new s_HitData[GameManager.Instance.MaxEnterHits];
        m_enterHits = new RaycastHit[GameManager.Instance.MaxEnterHits];
        m_exitHits = new RaycastHit[GameManager.Instance.MaxEnterHits - 1];

        //AMMO
        m_totalAmmo = data.m_initAmmo;
        m_clipAmmo = data.m_clipAmmo;

        if (data.m_hasChamber)
        {
            m_crntAmmo = m_clipAmmo + 1;
        }
        else
        {
            m_crntAmmo = m_clipAmmo;
        }

        if (m_startOnTheGround)
        {
            cmp_DropAndTake.Drop();
        }

    }

    protected override void Update()
    {
        base.Update();

        m_crntAccuracy = Mathf.Lerp(m_crntAccuracy, data.m_accuracy, data.m_accuracyRecoverPerSecond * Time.deltaTime);

        UpdateFireRate();

        //REAL Trajectory
        Camera mainCam = GameManager.Instance.MainCamera;
        Debug.DrawLine(mainCam.transform.position, mainCam.transform.position + mainCam.transform.forward * data.m_range, Color.magenta);
    }


    protected override void PrimaryActionDown()
    {
        base.PrimaryActionDown();
        
        PullTrigger();
    }


    protected override void PrimaryActionUp()
    {
        base.PrimaryActionUp();
        
        ReleaseTrigger();
    }

    protected override void SecondaryActionDown()
    {
        base.SecondaryActionDown();
    }

    protected virtual void UpdateFireRate()
    {
        if (m_canShoot)
            return;


        //Round/seg
        if (m_crntFireRate < 1 / data.m_fireRate)
        {
            m_canShoot = false;
            m_crntFireRate += Time.deltaTime;
        }
        else
        {
            m_canShoot = true;
        }
    }

    protected virtual void ResetFireRate()
    {
        m_canShoot = false;
        m_crntFireRate = 0;
    }

    protected bool CheckClipAmmo()
    {
        if(m_clipAmmo > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //ACTIVATE & DEACTIVATE
    public override void Activate()
    {
        base.Activate();
        
        GameManager.Instance.ActivateAmmoHUD();
        GameManager.Instance.UpdateAmmoHUD(m_crntAmmo, m_totalAmmo);
    }

    public override void Deactivate()
    {
        base.Deactivate();

        GameManager.Instance.DeactivateAmmoHUD();
    }



    protected virtual void PullTrigger()
    {
        m_triggerIsPullled = true;
    }

    protected virtual void ReleaseTrigger()
    {
        m_triggerIsPullled = false;
    }

    public int CompareHits(RaycastHit x, RaycastHit y)
    {
        return x.distance.CompareTo(y.distance);
    }

    protected virtual void Shoot()
    {
        if (!CheckClipAmmo() || !m_canShoot)
            return;

        Debug.Log("-----");
        Debug.Log("shoot");
        Debug.Log("-----");

        Camera mainCam = GameManager.Instance.MainCamera;
        int enterHits = 0;
        int exitHits = 0;

        enterHits= Physics.RaycastNonAlloc(ShootEnterRaycast(), m_enterHits, data.m_range);
        Debug.Log("EnterHits: " + enterHits);

        m_enterHits = GameManager.Instance.OrderRaycastHitByDistance(m_enterHits, enterHits);

        for (int i = 0; i < enterHits; i++)
        {
            m_hitData[i].m_hit = m_enterHits[i];
        }

        //Lanzamos un Raycast desde el punto final hasta el punto de lanzamiento
        if(enterHits > 0)
        {
            Vector3 reverseDirection = mainCam.transform.position - m_enterHits[enterHits - 1].point;
            reverseDirection.Normalize();

            Ray reverseRay = new Ray(m_enterHits[enterHits - 1].point, reverseDirection);
            float range = Vector3.Distance(mainCam.transform.position, m_enterHits[enterHits - 1].point);

            Debug.Log(range);

            exitHits = Physics.RaycastNonAlloc(reverseRay, m_exitHits, range);
            Debug.Log("ExitHits: " + exitHits);

            m_exitHits = GameManager.Instance.OrderRaycastHitByDistance(m_exitHits, exitHits);
        }


        #region Enter & Exit hits Debug
        if (enterHits == 0)
        {
            Debug.Log("Did not hit");
        }

        for (int i = 0; i < enterHits; i++)
        {
            Debug.Log("ENTER HIT " + i + " [" + m_hitData[i].m_hit.transform.name + "] : " + m_hitData[i].m_hit.distance);
        }

        for (int i = 0; i < exitHits; i++)
        {
            Debug.Log("EXIT HIT " + i + " [" + m_exitHits[i].transform.name + "] : " + Vector3.Distance(m_exitHits[i].point, mainCam.transform.position));
        }
        #endregion

        /*Raycast
        {
            GameManager.Instance.SurfaceImpact(m_shootHit);
            
            Damage(m_shootHit);

        }*/

        //AddShootFX(m_hitData[enterHits - 1].m_hit.point);
        ResetFireRate();
    }


    protected virtual Ray ShootEnterRaycast()
    {
        Camera mainCam = GameManager.Instance.MainCamera;

        //MODIFY ACCURACY
        float accuracyModifier = (100 - m_crntAccuracy) * 0.001f;


        /*directionForward.x += Random.Range(-accuracyModifier, accuracyModifier);
        directionForward.y += Random.Range(-accuracyModifier, accuracyModifier);
        directionForward.z += Random.Range(-accuracyModifier, accuracyModifier);

        m_crntAccuracy -= data.m_accuracyDropPerShot;

        m_crntAccuracy = Mathf.Clamp(m_crntAccuracy, 0, 100);*/


        //RAYCAST

        Ray shootRay = new Ray(mainCam.transform.position, mainCam.transform.forward);

        return shootRay;
    }

    public void AddShootFX(Vector3 hitPoint)
    {
        GameObject bulletTrail = Instantiate(m_bulletTrail.gameObject, m_shootSpot.position, Quaternion.identity);

        LineRenderer lineR = bulletTrail.GetComponent<LineRenderer>();

        lineR.SetPosition(0, m_shootSpot.position);
        lineR.SetPosition(1, hitPoint);


        //ADD SHOOT FX
        /*Camera weaponCam = GameManager.Instance.WeaponCamera;
        //Calculamos el centro de la pantalla (principal)
        Vector3 centerPos = mainCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, float.MaxValue));

        //Calculamos la posicion del arma en la la pantalla del arma y transladamos la posicion a la pantalla principal
        Vector3 shootSpotScreen = weaponCam.WorldToScreenPoint(m_shootSpot.position);
        Vector3 shootSpotPos = mainCam.ScreenToWorldPoint(shootSpotScreen);*/

    }

    public void Damage(RaycastHit hit)
    {
        CharBase target = hit.transform.GetComponentInParent<CharBase>();

        if (target == null)
            return;

        //GameManager.Instance.PlayerImpact(hit);

        /*if (hit.transform.CompareTag("HeadHitbox"))
        {
            //Debug.Log("HEAD SHOOT");
            m_damageMultiplier = m_headDamageMultiplier;
        }
        else if (hit.transform.CompareTag("TorsoHitbox"))
        {
            //Debug.Log("TORSO SHOOT");
            m_damageMultiplier = m_torsoDamageMultiplier;
        }
        else if (hit.transform.CompareTag("LimbHitbox"))
        {
            //Debug.Log("LIMB SHOOT");
            m_damageMultiplier = m_limbsDamageMultiplier;
        }

        int damage = (int)(data.m_damage * m_damageMultiplier);*/
        
        int damage = (int)(data.m_damage);

        int distance = (int)Vector3.Distance(this.transform.position, hit.transform.position);

        if (distance > data.m_range)
        {
            Debug.Log("OUT OF RANGE");
            //HACERLO CON VARIABLES
            damage -= (int)((data.m_range - distance) * 0.25f);
        }

        //Debug.Log(damage);

        target.ReceiveDamage(damage);
    }

    protected virtual void StartReload()
    {
        m_reloading = true;
        
        if (m_crntAmmo > 0)
        {
            //SET TRIGGER
            //SOUND
            //m_maxReloadTime = data.m_reloadWithAmmoTime;
        }
        else
        {
            //SET TRIGGER
            //SOUND
            //m_reloadTime = data.m_reloadWithoutAmmoTime;
        }
    }

    protected virtual void Reloading()
    {
        if (m_reloading)
        {
            m_crntRealoadTime += Time.deltaTime;

            if(m_crntRealoadTime >= m_reloadTime)
            {
                EndReload();
            }
        }
    }

    protected virtual void EndReload()
    {
        m_reloading = false;
        m_crntRealoadTime = 0;
    }

    protected virtual void ReloadAmmo()
    {
        if (data.m_hasChamber)
        {
            if (m_totalAmmo + m_crntAmmo >= data.m_clipAmmo + 1)
            {
                m_totalAmmo -= data.m_clipAmmo - m_crntAmmo;
                m_crntAmmo = data.m_clipAmmo;
            }
            else
            {
                m_crntAmmo += m_totalAmmo;
                m_totalAmmo = 0;
            }
        }
        else
        {
            if (m_totalAmmo + m_crntAmmo >= data.m_clipAmmo)
            {
                m_totalAmmo -= data.m_clipAmmo - m_crntAmmo;
                m_crntAmmo = data.m_clipAmmo;
            }
            else
            {
                m_crntAmmo += m_totalAmmo;
                m_totalAmmo = 0;
            }
        }

        GameManager.Instance.UpdateAmmoHUD(m_crntAmmo, m_totalAmmo);
    }


}
