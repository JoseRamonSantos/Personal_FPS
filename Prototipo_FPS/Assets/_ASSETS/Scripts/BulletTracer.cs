using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTracer : MonoBehaviour
{
    private Rigidbody m_cmpRigidbody = null;

    [SerializeField]
    private float m_speed = 10;

    private Vector3 m_destination = Vector3.zero;

    private bool m_initialized = false;

    private void Awake()
    {
        m_cmpRigidbody = GetComponent<Rigidbody>();
    }

    public void Init(Vector3 _destination)
    {
        m_destination = _destination;

        Vector3 direction = (m_destination - transform.position).normalized;
        Quaternion desiredRotation = Quaternion.LookRotation(direction);

        if(transform.rotation != desiredRotation)
        {
            transform.rotation = desiredRotation;
        }

        Vector3 distanceToTarget = _destination - transform.position;

        float distance = distanceToTarget.magnitude;

        m_cmpRigidbody.velocity = transform.forward * m_speed;

        float speed = m_cmpRigidbody.velocity.magnitude;

        float predition = distance / speed;

        Destroy(this.gameObject, predition);

        m_initialized = true;
    }

    private void Update()
    {
        if (!m_initialized) { return; }
    }
}
