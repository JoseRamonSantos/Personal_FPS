using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public enum LocomotionState
    {
        run,
        walk,
        crouch,
    }

    private CharacterController cmp_cC = null;

    private float m_cCStandHeight = 0;
    [SerializeField]
    private float m_cCCrouchedHeight = 0;

    private Vector3 m_cCStandCenter = Vector3.zero;
    private Vector3 m_cCCrouchedCenter = Vector3.zero;

    private Vector3 m_moveDir = Vector3.zero;

    [SerializeField]
    private LocomotionState m_crntLS = LocomotionState.run;

    [Space]

    [SerializeField]
    private float m_runSpeed = 3;
    [SerializeField]
    private float m_walkSpeed = 2;
    [SerializeField]
    private float m_crouchSpeed = 1;
    [SerializeField]
    private float m_jumpSpeed = 5;
    private float m_crntSpeed = 0;
    private float m_gravity = -9.8f;
    private float m_ySpeed = 0;

    private float m_maxTimeToTopSpeed = 5;
    private float m_currentTimeToTopSpeed = 0;

    [Space]

    [SerializeField]
    private float m_GravityMultiplier = 1;


    private void Awake()
    {
        cmp_cC = GetComponent<CharacterController>();
    }

    void Start()
    {
        Run();

        m_cCStandHeight = cmp_cC.height;
        m_cCStandCenter = cmp_cC.center;

        if(m_cCCrouchedHeight == 0)
        {
            m_cCCrouchedHeight = m_cCStandHeight * 0.75f;
        }

        float yCenter = m_cCStandCenter.y - ((m_cCStandHeight - m_cCCrouchedHeight) * 0.5f);

        m_cCCrouchedCenter = new Vector3(m_cCStandCenter.x, yCenter, m_cCStandCenter.z);
    }


    void Update()
    {
        Move();

    }

    private void FixedUpdate()
    {
        Fall();
        Jump();
    }

    private void Fall()
    {
        if (cmp_cC.isGrounded)
        {
            m_ySpeed = -9.8f;
        }
        else
        {
            m_moveDir += Physics.gravity * m_GravityMultiplier * Time.fixedDeltaTime;
        }
    }

    private void Jump()
    {
        if (cmp_cC.isGrounded && Input.GetKeyDown(InputManager.Instance.K_jump))
        {
            Debug.Log("JUMP");
            m_ySpeed = m_jumpSpeed;
        }
    }

    private void Move()
    {

        if (Input.GetKey(InputManager.Instance.K_walk))
        {
            Walk();
        }
        
        if (Input.GetKey(InputManager.Instance.K_crouch))
        {
            Crouch();
        }
        
        if (Input.GetKeyUp(InputManager.Instance.K_crouch))
        {
            Uncrouch();
        }
        
        if(!Input.GetKey(InputManager.Instance.K_walk) && !Input.GetKey(InputManager.Instance.K_crouch))
        {
            Run();
        }


        float zInput = Input.GetAxis("Vertical");
        float xInput = Input.GetAxis("Horizontal");


        Vector3 desiredDirection = transform.forward * zInput + transform.right * xInput;
        //desiredDirection.x = Mathf.Clamp(desiredDirection.x , -1, 1);
        //desiredDirection.z = Mathf.Clamp(desiredDirection.z , -1, 1);
        desiredDirection.Normalize();
        desiredDirection.y = m_ySpeed;

        //Debug.Log(desiredDirection.magnitude);

        Vector3 currentDirection = Vector3.zero;
        currentDirection = Vector3.Lerp(currentDirection, desiredDirection, 1);


        //DEBUG
        if (zInput != 0 || xInput != 0)
        {
            //Debug.Log(currentDirection);
        }

        cmp_cC.Move(currentDirection * m_crntSpeed * Time.deltaTime);


    }

    public void Run()
    {
        m_crntLS = LocomotionState.run;
        m_crntSpeed = m_runSpeed;
    }

    public void Walk()
    {
        m_crntLS = LocomotionState.walk;
        m_crntSpeed = m_walkSpeed;
    }

    public void Crouch()
    {
        m_crntLS = LocomotionState.crouch;
        m_crntSpeed = m_crouchSpeed;

        cmp_cC.height = m_cCCrouchedHeight;
        cmp_cC.center = m_cCCrouchedCenter;
    }

    public void Uncrouch()
    {
        m_crntLS = LocomotionState.run;
        m_crntSpeed = m_runSpeed;

        cmp_cC.height = m_cCStandHeight;
        cmp_cC.center = m_cCStandCenter;
    }
}
