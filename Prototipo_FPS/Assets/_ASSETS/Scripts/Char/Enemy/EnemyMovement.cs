using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    protected bool m_debug = false;

    private NavMeshAgent m_cmpAgent = null;
    private Animator m_cmpAnimator = null;

    [SerializeField]
    private Vector3 m_destination = Vector3.zero;

    [SerializeField]
    private Transform m_target = null;

    [SerializeField]
    private bool m_trackEnabled = false;

    private float m_rotationSpeed = 270;

    [SerializeField]
    private float m_runSpeed = 4;
    [SerializeField]
    private float m_walkSpeed = 4;

    private bool m_isRunning = false;

    [SerializeField]
    private bool m_crouched = false;


    private void Awake()
    {
        m_cmpAgent = GetComponent<NavMeshAgent>();
        m_cmpAnimator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        m_destination = transform.position;
        if (m_walkSpeed == 0)
        {
            m_walkSpeed = m_runSpeed * 0.75f;
        }
        Stand();
    }

    private void Update()
    {
        if (m_debug) { return; }

        MoveToPos();
        TrackTarget();
        UpdateAnimation();
    }

    //MOVE
    private void MoveToPos()
    {

        if (!m_cmpAgent.pathPending && m_cmpAgent.remainingDistance > m_cmpAgent.stoppingDistance && m_destination != m_cmpAgent.destination)
        {
            m_cmpAgent.SetDestination(m_destination);
        }
    }

    public bool IsInTheDestinationPos()
    {
        float rDistance = (m_destination - transform.position).magnitude;

        return rDistance <= m_cmpAgent.stoppingDistance;
    }

    public void NewDestination(Vector3 _newDestination, float _stoppingD = 0.1f, bool _run = true)
    {
        if (_run)
        {
            StartRun();
        }
        else
        {
            StartWalk();
        }

        m_cmpAgent.stoppingDistance = _stoppingD;

        NavMeshHit NVHit;
        NavMesh.SamplePosition(_newDestination, out NVHit, 10, m_cmpAgent.areaMask);

        m_destination = NVHit.position;

        if (m_debug) { return; }
        m_cmpAgent.SetDestination(m_destination);
    }



    public void Stand()
    {
        m_crouched = false;
    }

    public void Crouch()
    {
        m_crouched = true;
    }

    private void StartRun()
    {
        m_isRunning = true;
        m_cmpAgent.speed = m_runSpeed;
    }

    private void StartWalk()
    {
        m_isRunning = false;
        m_cmpAgent.speed = m_walkSpeed;
    }

    //LOOK
    private void TrackTarget()
    {
        if (m_trackEnabled && m_target)
        {
            Vector3 lookDir = m_target.transform.position - transform.position;
            lookDir.Normalize();

            Quaternion desiredRotation = Quaternion.LookRotation(lookDir);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRotation, m_rotationSpeed * Time.deltaTime);
        }
    }

    public void EnableTrack(Transform _newTarget)
    {
        m_target = _newTarget;
        m_trackEnabled = true;
    }

    public void DisnableTrack()
    {
        m_target = null;
        m_trackEnabled = false;
    }

    //ANIM
    private void UpdateAnimation()
    {
        float animSpeed;

        if (m_cmpAgent.velocity != Vector3.zero)
        {
            animSpeed = m_cmpAgent.speed / m_runSpeed;
        }
        else
        {
            animSpeed = 0;
        }

        m_cmpAnimator.SetFloat("Speed", animSpeed);
        m_cmpAnimator.SetBool("Crouch", m_crouched);
    }
}
