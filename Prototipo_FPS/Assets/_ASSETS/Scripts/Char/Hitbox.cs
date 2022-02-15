using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [SerializeField]
    private E_HITBOX_PART m_hitboxPart = E_HITBOX_PART.CHEST;

    public static float HEAD_MULT = 2.0f;
    public static float CHEST_MULT = 1.0f;
    public static float ARM_MULT = 1.0f;
    public static float STOMACH_MULT = 1.25f;
    public static float LEG_MULT = 0.75f;

    public E_HITBOX_PART GetHitboxPart
    {
        get
        {
            return m_hitboxPart;
        }
    }
}
