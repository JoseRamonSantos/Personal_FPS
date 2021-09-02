using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [SerializeField]
    private E_HITBOX_PART m_hitboxPart = E_HITBOX_PART.BODY;

    public static float HEAD_MULT = 2.0f;
    public static float BODY_MULT = 1.0f;
    public static float LIMB_MULT = 0.75f;

    public E_HITBOX_PART GetHitboxPart
    {
        get
        {
            return m_hitboxPart;
        }
    }
}
