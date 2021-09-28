using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.InputSystem;


[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Component References")]
    private Camera m_cmpMainCamera = null;
    private CharacterController m_cmpCC = null;

    [Header("Movement Settings")]
    //public float movementSpeed = 3f;

    [SerializeField]
    private float m_crntMaxSpeed = 0;
    private float m_runSpeed = 8f;
    private float m_walkSpeed = 6f;
    private float m_crouchedSpeed = 4.5f;

    [SerializeField]
    private float m_ySpeed = 0;
    private float m_gravity = -9.8f;

    [SerializeField]
    private float m_camMaxAngle = 20;
    [SerializeField]
    private float m_camMinAngle = -20;

    //Stored Values
    private Vector3 m_movementDirection = Vector3.zero;
    private float m_playerRotation = 0;
    private float m_cameraRotation = 0;
    private float m_deltaCameraRotation = 0;

    [SerializeField]
    private bool m_isMoving = false;

    [SerializeField]
    private bool m_isWalking = false;
    [SerializeField]
    private bool m_isCrouched = false;

    private float m_cameraStandPosY = 0;
    private float m_colliderStandHeight = 0;
    private float m_colliderStandPosY = 0;
    private float m_cameraCrouchPosY = 0;
    private float m_colliderCrouchHeight = 0;
    private float m_colliderCrouchPosY = 0;

    [SerializeField]
    private bool m_isJumping = false;
    [SerializeField]
    private bool m_jumpInput = false;


    public bool IsMoving { get => m_isMoving; }



    private void Awake()
    {
        m_cmpCC = GetComponent<CharacterController>();
    }

    private void Start()
    {
        m_cmpMainCamera = Camera.main;
        m_cameraStandPosY = m_cmpMainCamera.transform.position.y;

        m_colliderStandHeight = m_cmpCC.height;
        m_colliderStandPosY = m_cmpCC.center.y;
        m_cameraCrouchPosY = m_cameraStandPosY * 0.75f;
        m_colliderCrouchHeight = m_colliderStandHeight * 0.75f;
        m_colliderCrouchPosY = m_colliderStandPosY - (m_colliderStandHeight - m_colliderCrouchHeight) * 0.5f;

        DefaultMovement();
        m_isMoving = false;
    }

    void Update()
    {
        Fall();
        //Slip();

        PerformMovement();
        PerformRotation();

        UpdateAnimator(m_movementDirection);
    }

    void UpdateAnimator(Vector3 move)
    {
        // update the animator parameters
    }
    private void Fall()
    {
        if (!m_cmpCC.isGrounded)
        {
            m_ySpeed += m_gravity * Time.deltaTime;
        }
        else if (m_cmpCC.isGrounded)
        {
            m_ySpeed = m_gravity;
        }
    }

    private void PerformMovement()
    {
        Vector3 velocidadTotal = CameraDirection(m_movementDirection);

        velocidadTotal.y = m_ySpeed;

        m_cmpCC.Move(velocidadTotal * m_crntMaxSpeed * Time.deltaTime);

        //WEAPON
        if (Mathf.Abs(m_movementDirection.magnitude) < 0.05f && IsMoving)
        {
            m_isMoving = false;
        }
        else if (Mathf.Abs(m_movementDirection.magnitude) >= 0.05f && !IsMoving)
        {
            m_isMoving = true;
        }
    }

    private void PerformRotation()
    {
        //PLAYER Rotation
        transform.Rotate(transform.up * m_playerRotation);

        //CAMERA Rotation
        if (m_cmpMainCamera != null)
        {
            // Set our rotation and clamp it
            m_cameraRotation += m_deltaCameraRotation;
            m_cameraRotation = Mathf.Clamp(m_cameraRotation, m_camMinAngle, m_camMaxAngle);

            //Apply our rotation to the transform of our camera
            m_cmpMainCamera.transform.localEulerAngles = new Vector3(m_cameraRotation, 0f, 0f);
        }
    }


    private Vector3 CameraDirection(Vector3 _movementDirection)
    {
        var cameraForward = m_cmpMainCamera.transform.forward;
        var cameraRight = m_cmpMainCamera.transform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;

        return cameraForward * _movementDirection.z + cameraRight * _movementDirection.x;
    }


    public void UpdateMovementData(Vector3 _newMovementDirection)
    {
        m_movementDirection = _newMovementDirection;
    }

    public void UpdateRotationData(Vector3 _newRotation)
    {
        m_deltaCameraRotation = -_newRotation.y;

        m_playerRotation = _newRotation.x;
    }


    private void DefaultMovement()
    {
        m_isCrouched = false;
        m_isWalking = false;
        m_crntMaxSpeed = m_runSpeed;
    }

    public void StartCrouch()
    {
        m_isCrouched = true;
        m_crntMaxSpeed = m_crouchedSpeed;

        //COLLIDER HEIGHT AND POS 
        m_cmpCC.height = m_colliderCrouchHeight;

        Vector3 newColliderCenter = m_cmpCC.center;
        newColliderCenter.y = m_colliderCrouchPosY;
        m_cmpCC.center = newColliderCenter;

        //CAMERA POS
        Vector3 newCamPos = m_cmpMainCamera.transform.position;
        newCamPos.y = m_cameraCrouchPosY;

        m_cmpMainCamera.transform.position = newCamPos;
    }

    public void EndCrouch()
    {
        m_isCrouched = false;

        if (m_isWalking)
        {
            StartWalk();
        }
        else
        {
            DefaultMovement();
        }

        //COLLIDER HEIGHT AND POS 
        m_cmpCC.height = m_colliderStandHeight;

        Vector3 newCenter = m_cmpCC.center;
        newCenter.y = m_colliderStandPosY;
        m_cmpCC.center = newCenter;

        //CAMERA POS
        Vector3 newCamPos = m_cmpMainCamera.transform.position;
        newCamPos.y = m_cameraStandPosY;

        m_cmpMainCamera.transform.position = newCamPos;
    }

    public void StartWalk()
    {
        m_isWalking = true;
        m_crntMaxSpeed = m_walkSpeed;
    }

    public void EndWalk()
    {
        m_isWalking = false;

        if (m_isCrouched)
        {
            StartCrouch();
        }
        else
        {
            DefaultMovement();
        }
    }

    /*
    private CharacterController cmpCC;
    private Transform cmpCamara;
    private PlayerInput cmpPInput = null;

    private float velocidadAndar = 3;
    private float velocidadRotacion = 180;
    private float gravedad = -9.8f;
    private float velocidadSalto = 4;
    private float sensibilidadRaton = 100;

    private float velocidadPegadoAlSuelo = -5;

    private float inclinacionDeslizamiento;

    private bool deslizandose = false;
    private float velocidadY;


    private void Awake()
    {
        cmpCC = GetComponent<CharacterController>();
        cmpCamara = transform.Find("Main Camera");
    }

    private void Start()
    {
        cmpPInput = FindObjectOfType<PlayerInput>();
        inclinacionDeslizamiento = cmpCC.slopeLimit - 10;
    }

    // Update is called once per frame
    private void Update()
    {
        Caer();
        Deslizar();
        Saltar();
        Mover();
        Rotar();
        //RotarCamara();
    }

    private void Mover()
    {
        Vector3 velocidadTotal = transform.forward * velocidadAndar * zInput;
        velocidadTotal.y = velocidadY;

        cmpCC.Move(velocidadTotal * Time.deltaTime);
    }

    private void Rotar()
    {
        float xInput = Input.GetAxis("Horizontal");
        transform.Rotate(transform.up * velocidadRotacion * xInput * Time.deltaTime);
    }

    /*private void RotarCamara()
    {
        float yInput = Input.GetAxis("Mouse Y");
        cmpCamara.transform.Rotate(yInput * sensibilidadRaton * Time.deltaTime, 0, 0);
    }*/

    /*private void Caer()
    {
        if (!cmpCC.isGrounded)
        {
            velocidadY += gravedad * Time.deltaTime;
        }
        else if (cmpCC.isGrounded)
        {
            velocidadY = velocidadPegadoAlSuelo;
        }
    }

    private void Saltar()
    {
        //if (cmpCC.isGrounded && Keyboard.current.IsPressed(cmpPInput.actionEvents.))
        {
            velocidadY = velocidadSalto;
        }
    }

    private void Deslizar()
    {
        if (cmpCC.isGrounded)
        {
            RaycastHit infoImpacto;
            if (Physics.Raycast(this.transform.position, Vector3.down, out infoImpacto))
            {
                Vector3 normal = infoImpacto.normal;
                float anguloPendiente = Vector3.Angle(normal, Vector3.up);
                if (anguloPendiente > inclinacionDeslizamiento)
                {
                    print("Pendiente muy inclinada, activar deslizamiento");
                }
            }
        }
    }*/
}
