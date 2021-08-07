using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class InputManager : TemporalSingleton<InputManager>
{
    [Header ("Controls")]
    [SerializeField]
    private     MouseButton     mb_primaryAction    = (MouseButton)0;

    [SerializeField]
    private     MouseButton     mb_secondaryAction  = (MouseButton)1;

    [SerializeField]
    private     KeyCode         kc_moveFwd          = KeyCode.W;

    [SerializeField]
    private     KeyCode         kc_moveBwd          = KeyCode.S;

    [SerializeField]
    private     KeyCode         kc_moveRt           = KeyCode.D;

    [SerializeField]
    private     KeyCode         kc_moveLt           = KeyCode.A;

    [SerializeField]
    private     KeyCode         kc_walk             = KeyCode.LeftShift;

    [SerializeField]
    private     KeyCode         kc_crouch           = KeyCode.LeftControl;

    [SerializeField]
    private     KeyCode         kc_jump             = KeyCode.Space;

    [SerializeField]
    private     KeyCode         kc_item1            = KeyCode.Alpha1;

    [SerializeField]
    private     KeyCode         kc_item2            = KeyCode.Alpha2;

    [SerializeField]
    private     KeyCode         kc_item3            = KeyCode.Alpha3;

    [SerializeField]
    private     KeyCode         kc_item4            = KeyCode.Alpha4;

    [SerializeField]
    private     KeyCode         kc_item5            = KeyCode.Alpha5;


    [Space]

    private float m_fwdAxis = 0;
    private float m_rtAxis = 0;



    [Header ("Axis options")]
    [SerializeField]
    [Description ("Aceleration multiplier")]
    private float axis_sensivity = 1;
    
    [SerializeField]
    [Description ("Deceleration multiplier")]
    private float axis_gravity = 1;
    
    [SerializeField]
    [Description ("Value rounded to 0")]
    private float axis_dead = 0.1f;

    public float FwdAxis 
    {
        get
        {
            return m_fwdAxis;
        }

        set
        {
            value = Mathf.Clamp(value, -1, 1);

            m_fwdAxis = value;
        } 
    }
    public float RtAxis
    {
        get
        {
            return m_rtAxis;
        }

        set
        {
            value = Mathf.Clamp(value, -1, 1);

            m_rtAxis = value;
        }

    }

    public KeyCode K_jump   { get => kc_jump; set => kc_jump = value; }
    public KeyCode K_walk   { get => kc_walk; set => kc_walk = value; }
    public KeyCode K_crouch { get => kc_crouch; set => kc_crouch = value; }
    public KeyCode K_item1  { get => kc_item1; set => kc_item1 = value; }
    public KeyCode K_item2  { get => kc_item2; set => kc_item2 = value; }
    public KeyCode K_item3  { get => kc_item3; set => kc_item3 = value; }
    public KeyCode K_item4  { get => kc_item4; set => kc_item4 = value; }
    public KeyCode K_item5  { get => kc_item5; set => kc_item5 = value; }
    public MouseButton Mb_primaryAction { get => mb_primaryAction; set => mb_primaryAction = value; }
    public MouseButton Mb_secondaryAction { get => mb_secondaryAction; set => mb_secondaryAction = value; }

    public delegate void OnStart();
    public event OnStart OnPlayerRun;
    public event OnStart OnPlayerWalk;
    public event OnStart OnPlayerCrouch;
    public event OnStart OnPlayerJump;

    /*public delegate void OnCheck(bool i);
    public event OnCheck OnCheckPlayerMovement;*/


    void Start()
    {

    }

    void Update()
    {
        OnFwdAxis();
        OnRtAxis();
        //Debug.Log(FwdAxis);
        //Debug.Log(RtAxis);
    }

    private void OnFwdAxis()
    {
        if (Input.GetKey(kc_moveFwd))
        {
            if(FwdAxis < 0)
            {
                FwdAxis = 0;
            }

            if (FwdAxis < 1)
            {
                FwdAxis += Time.deltaTime * axis_sensivity;
            }
        }
        else if (Input.GetKey(kc_moveBwd))
        {
            if (FwdAxis > -1)
            {
                FwdAxis -= Time.deltaTime * axis_sensivity;
            }
        }
        else if (!Input.GetKey(kc_moveFwd) && !Input.GetKey(kc_moveBwd))
        {
            if (FwdAxis > axis_dead)
            {
                FwdAxis -= Time.deltaTime * axis_gravity;
            }
            else if (FwdAxis < -axis_dead)
            {
                FwdAxis += Time.deltaTime * axis_gravity;
            }
            else
            {
                FwdAxis = 0;
            }
        }
    }

    private void OnRtAxis()
    {
        if (Input.GetKey(kc_moveRt))
        {
            
            if (RtAxis < 1)
            {
                if (RtAxis < 0)
                {
                    RtAxis = 0;
                }

                RtAxis += Time.deltaTime * axis_sensivity;
            }
        }
        else if (Input.GetKey(kc_moveLt))
        {
            
            if (RtAxis > -1)
            {
                if (RtAxis > 0)
                {
                    RtAxis = 0;
                }
                
                RtAxis -= Time.deltaTime * axis_sensivity;
            }
        }
        else if (!Input.GetKey(kc_moveRt) && !Input.GetKey(kc_moveLt))
        {
            if (RtAxis > axis_dead)
            {
                RtAxis -= Time.deltaTime * axis_gravity;
            }
            else if (RtAxis < -axis_dead)
            {
                RtAxis += Time.deltaTime * axis_gravity;
            }
            else
            {
                RtAxis = 0;
            }
        }
    }

}
