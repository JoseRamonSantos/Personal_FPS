using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(AudioSource))]
public abstract class Projectile_Base : MonoBehaviour
{
    [Space]
    [SerializeField]
    protected float m_initSpeed = 10;
    protected int m_maxRange = 50;
    protected int m_maxDamage = 50;
    protected AnimationCurve m_distanceDamageReduction = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 0));

    [Space]
    protected Rigidbody m_cmpRb = null;
    protected Collider m_cmpCollider = null;

    [Space]
    [SerializeField]
    protected LayerMask m_layerMask = 1;

    [Space]
    [SerializeField]
    protected GameObject m_explosionVFX = null;

    [Space]
    [SerializeField]
    protected Char_Base m_owner = null;

    [Space]
    [SerializeField]
    protected float m_lifeTime = 5;
    protected float m_crntTime = 0;



    protected virtual void Awake()
    {
        m_cmpRb = GetComponent<Rigidbody>();
        m_cmpCollider = GetComponent<Collider>();
    }

    protected virtual void Start()
    {
        m_cmpRb.isKinematic = false;
        m_cmpRb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

        m_cmpCollider.isTrigger = false;

        m_crntTime = m_lifeTime;
    }

    protected void Update()
    {
        m_crntTime -= Time.deltaTime;

        if(m_crntTime <= 0)
        {
            EndLifetime();
        }
    }

    protected abstract void EndLifetime();

    protected void DoDamage(Char_Base _target, int _dmg)
    {
        if (m_owner)
        {
            m_owner.DoDamage(_target, _dmg);
        }
        else
        {
            Debug.LogError(transform.name + "MISSING OWNER");
            _target.ReceiveDamage(_dmg);
        }
    }

    public void Activate(Char_Base _owner, int _maxDamage, int _maxRange, AnimationCurve _dDmgReduction)
    {
        m_owner = _owner;
        m_maxDamage = _maxDamage;
        m_maxRange = _maxRange;
        m_distanceDamageReduction = _dDmgReduction;

        m_cmpRb.AddForce(transform.forward * m_initSpeed, ForceMode.Impulse);
    }

    protected void Explode()
    {
        Debug.Log(transform.name + " EXPLODE");

        ExplosionVFX();

        Collider[] collisionList = Physics.OverlapSphere(transform.position, m_maxRange, m_layerMask);

        foreach (var item in collisionList)
        {
            Debug.Log("- " + item.transform.name);
        }

        for (int i = 0; i < collisionList.Length; i++)
        {
            Char_Base target;

            if(collisionList[i].TryGetComponent(out target))
            {
                if (m_owner)
                {
                    Debug.Log("-----> " + 0);
                    m_owner.DoDamage(target, CalculateEsplosionDmg(target));
                }
                else
                {
                    Debug.Log("-----> " + 1);
                    target.ReceiveDamage(CalculateEsplosionDmg(target));
                }
            }
        }

        Destroy(gameObject);
    }

    protected int CalculateEsplosionDmg(Char_Base _target)
    {
        int dmg = m_maxDamage;

        float distance = (_target.transform.position - transform.position).magnitude;

        float dPercent = Mathf.Clamp(distance / m_maxRange, 0, 1);

        float dDmgReductionPercent = m_distanceDamageReduction.Evaluate(dPercent);


        dmg = Mathf.RoundToInt(dmg * dDmgReductionPercent);

        return dmg;
    }

    protected void ExplosionVFX()
    {
        if (m_explosionVFX)
        {
            Instantiate(m_explosionVFX, transform.position, transform.rotation);
        }
        else
        {
            Debug.LogError(transform.name + "MISSING EXPLOSION VFX");
        }
    }
}
