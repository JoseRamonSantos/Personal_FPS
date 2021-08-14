using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class PlayerController : MonoBehaviour
{

    [Header("Sub Behaviours")]
    private PlayerWeaponController m_cmpPlayer;
    private PlayerMovement m_cmpPlayerMovement;
    //public PlayerAnimationBehaviour playerAnimationBehaviour;
    //public PlayerVisualsBehaviour playerVisualsBehaviour;


    [Header("Input Settings")]
    private float m_movementAceleration = 6f;
    private float m_movementDeceleration = 9f;
    [SerializeField]
    private float m_mouseSensibility = 1f;
    private Vector3 m_rawInputMovement = Vector3.zero;
    private Vector3 m_smoothInputMovement = Vector3.zero;
    private Vector3 m_rawInputRotation = Vector3.zero;
    private Vector3 m_smoothInputRotation = Vector3.zero;

    //Action Maps
    private string m_actionMapPlayerControls = "Player Controls";
    private string m_actionMapMenuControls = "Menu Controls";

    //Current Control Scheme
    private string m_currentControlScheme;



    private void Awake()
    {
        m_cmpPlayerMovement = GetComponent<PlayerMovement>();
        m_cmpPlayer = GetComponent<PlayerWeaponController>();
    }

    private void Start()
    {

    }

    //Update Loop - Used for calculating frame-based data
    void Update()
    {
        OnMovement();
        OnLook();
        OnShoot();
        OnCrouch();
        OnWalk();
    }

    #region INPUT ACTIONS
    //INPUT SYSTEM ACTION METHODS --------------

    //This is called from PlayerInput; when a joystick or arrow keys has been pushed.
    //It stores the input Vector as a Vector3 to then be used by the smoothing function.


    public void OnMovement()
    {
        float hAxis = Input.GetAxis("Horizontal");
        float vAxis = Input.GetAxis("Vertical");

        m_rawInputMovement = new Vector3(hAxis, 0, vAxis);
        
        if (m_rawInputMovement == Vector3.zero)
        {
            m_smoothInputMovement = Vector3.Lerp(m_smoothInputMovement, m_rawInputMovement, Time.deltaTime * m_movementDeceleration);
        }
        else
        {
            m_smoothInputMovement = Vector3.Lerp(m_smoothInputMovement, m_rawInputMovement, Time.deltaTime * m_movementAceleration);
        }

        m_cmpPlayerMovement.UpdateMovementData(m_smoothInputMovement);
    }

    public void OnLook()
    {
        float xAxis = Input.GetAxis("Mouse X");
        float yAxis = Input.GetAxis("Mouse Y");

        m_rawInputRotation = new Vector3(xAxis, yAxis, 0);

        m_smoothInputRotation = m_rawInputRotation * m_mouseSensibility;

        m_cmpPlayerMovement.UpdateRotationData(m_smoothInputRotation);
    }

    public void OnShoot()
    {

    }

    public void OnCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            m_cmpPlayerMovement.StartCrouch();
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            m_cmpPlayerMovement.EndCrouch();
        }
    }

    public void OnWalk()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            m_cmpPlayerMovement.StartWalk();
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            m_cmpPlayerMovement.EndWalk();
        }
    }
    #endregion

}
