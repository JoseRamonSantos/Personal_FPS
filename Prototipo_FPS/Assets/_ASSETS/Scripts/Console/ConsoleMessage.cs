using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ConsoleMessage : MonoBehaviour
{
    private TextMeshProUGUI m_cmpTMPro = null;



    private void Awake()
    {
        m_cmpTMPro = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void Deactivate()
    {

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
