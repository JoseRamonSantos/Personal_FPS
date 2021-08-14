using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AmmoHUD : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_textCrntAmmo = null;
    [SerializeField]
    private TextMeshProUGUI m_textTotalAmmo = null;


    public void OnUpdateAmmoHUD(int _crntAmmo, int _totalAmmo)
    {
        m_textCrntAmmo.text = _crntAmmo.ToString();
        m_textTotalAmmo.text = _totalAmmo.ToString();
    }
}
