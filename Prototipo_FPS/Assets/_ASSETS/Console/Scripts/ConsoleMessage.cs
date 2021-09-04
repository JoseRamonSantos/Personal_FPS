using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConsoleMessage : MonoBehaviour
{
    private TextMeshProUGUI m_cmpTMPro = null;

    private Image m_cmpImage = null;

    private bool m_isActivated = true;

    private void Awake()
    {
        m_cmpTMPro = GetComponentInChildren<TextMeshProUGUI>();
        m_cmpImage = GetComponentInChildren<Image>();
    }

    public void Predeactivate()
    {
        if (!m_isActivated) { return; }

        Color newColor = m_cmpImage.color;
        newColor.a = 0.75f;

        m_cmpImage.color = newColor;

    }

    public void Deactivate()
    {
        if (!m_isActivated) { return; }

        Color newColor = m_cmpImage.color;
        newColor.a = 0.5f;

        m_cmpImage.color = newColor;

        m_isActivated = false;
    }

    public void ResetText()
    {
        m_cmpTMPro.text = "";
    }

    public void AddText(string _message, string _colorHex = "#FFFFFF", bool _bold = false, bool _italic = false, bool _underline = false, bool _strikethrough = false)
    {
        string crntTxt = m_cmpTMPro.text;

        string newTxt = "";

        if (_bold) { newTxt = newTxt + "<b>"; }

        if (_italic) { newTxt = newTxt + "<i>"; }

        if (_underline) { newTxt = newTxt + "<u>"; }
        
        if (_strikethrough) { newTxt = newTxt + "<s>"; }

        newTxt = newTxt + "<color=" + _colorHex + ">";

        newTxt = newTxt + _message;

        newTxt = newTxt + "</color>";

        if (_bold) { newTxt = newTxt + "</b>"; }

        if (_italic) { newTxt = newTxt + "</i>"; }

        if (_underline) { newTxt = newTxt + "</u>"; }

        if (_strikethrough) { newTxt = newTxt + "</s>"; }

        m_cmpTMPro.text = m_cmpTMPro.text + "<color=" + _colorHex + ">" + _message + "</color>";

        m_cmpTMPro.text = crntTxt + " " + newTxt;
    }
}
