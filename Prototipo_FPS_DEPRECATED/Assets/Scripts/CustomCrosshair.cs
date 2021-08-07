using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCrosshair : MonoBehaviour
{

    [SerializeField]
    private Texture m_crosshairTexture = null;


    private void OnGUI()
    {
        Vector2 center = new Vector2(Screen.width / 2, Screen.height / 2);
        Rect auxRect = new Rect(center.x - 25 * 0.5f, center.y - 25 * 0.5f, 25, 25);
        GUI.DrawTexture(auxRect, m_crosshairTexture, ScaleMode.StretchToFill);
    }
}
