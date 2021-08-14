using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HpHUD : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_textCrntHp = null;


    public void UpdateHpHUD(int _crntHp)
    {
        m_textCrntHp.text = _crntHp.ToString();
    }
}
