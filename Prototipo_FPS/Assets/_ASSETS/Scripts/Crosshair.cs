using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(CanvasScaler))]
public class Crosshair : MonoBehaviour
{
    [SerializeField]
    private RectTransform m_crosshairUp = null;
    [SerializeField]
    private RectTransform m_crosshairDown = null;
    [SerializeField]
    private RectTransform m_crosshairRight = null;
    [SerializeField]
    private RectTransform m_crosshairLeft = null;
    [SerializeField]
    private RectTransform m_crosshairDot = null;

    [SerializeField]
    private Image[] m_crosshairImagesList = null;

    [Header("Crosshair options")]
    [SerializeField]
    private float m_size = 2;
    [SerializeField]
    private float m_gap = 2;
    [SerializeField]
    private float m_thickness = 2;
    [SerializeField]
    private Color m_color = Color.green;

    [Space]

    [SerializeField]
    private bool m_dot = false;
    [SerializeField]
    private bool m_onlyDot = false;
    [SerializeField]
    private bool m_tShape = false;



    private void UpdateValues()
    {
        //SIZE
        Vector2 vAxis = Vector2.zero;
        vAxis.x = m_thickness;
        vAxis.y = m_size;

        m_crosshairUp.sizeDelta = vAxis;
        m_crosshairDown.sizeDelta = vAxis;

        Vector2 hAxis = Vector2.zero;
        hAxis.x = m_size;
        hAxis.y = m_thickness;

        m_crosshairRight.sizeDelta = hAxis;
        m_crosshairLeft.sizeDelta = hAxis;

        Vector3 dot = Vector2.zero;
        dot.x = m_thickness;
        dot.y = m_thickness;

        m_crosshairDot.sizeDelta = dot;

        //POSITION
        Vector3 newUpPos = Vector3.zero;
        newUpPos.y = m_size * 0.5f + m_gap;
        m_crosshairUp.localPosition = newUpPos;

        Vector3 newDownPos = Vector3.zero;
        newDownPos.y = -(m_size * 0.5f + m_gap);
        m_crosshairDown.localPosition = newDownPos;

        Vector3 newRtPos = Vector3.zero;
        newRtPos.x = m_size * 0.5f + m_gap;
        m_crosshairRight.localPosition = newRtPos;

        m_crosshairLeft.localPosition = -newRtPos;


        if (m_onlyDot)
        {
            m_dot = false;
            m_tShape = false;

            m_crosshairDot.gameObject.SetActive(true);
            m_crosshairUp.gameObject.SetActive(false);
            m_crosshairDown.gameObject.SetActive(false);
            m_crosshairRight.gameObject.SetActive(false);
            m_crosshairLeft.gameObject.SetActive(false);
        }
        else
        {
            m_crosshairDot.gameObject.SetActive(false);
            m_crosshairUp.gameObject.SetActive(true);
            m_crosshairDown.gameObject.SetActive(true);
            m_crosshairRight.gameObject.SetActive(true);
            m_crosshairLeft.gameObject.SetActive(true);

            //T Shape
            if (m_tShape)
            {
                m_crosshairUp.gameObject.SetActive(false);
            }

            if (m_dot)
            {
                m_crosshairDot.gameObject.SetActive(true);
            }

        }
    }

    private void ModifyColor()
    {
        if (m_crosshairImagesList.Length <= 0) { return; }

        for (int i = 0; i < m_crosshairImagesList.Length; i++)
        {
            m_crosshairImagesList[i].color = m_color;
        }
    }

    private void OnValidate()
    {
        UpdateValues();
        ModifyColor();
    }
}
