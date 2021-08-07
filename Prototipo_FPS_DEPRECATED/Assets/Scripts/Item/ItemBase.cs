using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemBase : MonoBehaviour
{
    public enum ItemType
    {
        weapon,
        knife,
        utility,
    }

    [SerializeField]
    protected   ItemType        m_itemType = ItemType.weapon;

    protected   CharPlayer      m_player = null;


    [SerializeField]
    protected   bool            m_canBeUsed = true;

    protected   Animator        cmp_anim = null;




    protected virtual void Awake()
    {
        m_player = (CharPlayer)CharPlayer.Instance;

        if(TryGetComponent(out Animator anim))
        {
            cmp_anim = anim;
        }
    }

    protected virtual void Start()
    {
        
    }

    protected virtual void Update()
    {
        
        if (Input.GetMouseButtonDown((int)InputManager.Instance.Mb_primaryAction) && m_canBeUsed)
        {
            PrimaryActionDown();
        }

        if (Input.GetMouseButtonUp((int)InputManager.Instance.Mb_primaryAction))
        {
            PrimaryActionUp();
        }

        if (Input.GetMouseButtonDown((int)InputManager.Instance.Mb_secondaryAction) && m_canBeUsed)
        {
            SecondaryActionDown();
        }
    }

    protected virtual void PrimaryActionDown()
    {
        if (!m_canBeUsed)
            return;
    }

    protected virtual void PrimaryActionUp()
    {

    }

    protected virtual void SecondaryActionDown()
    {
        if (!m_canBeUsed)
            return;
    }


    public virtual void Activate()
    {
        DrawItem();
    }

    public virtual void Deactivate()
    {
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        gameObject.SetActive(false);
    }


    //DESENFUNDAR
    public void DrawItem()
    {
        //Debug.Log("DRAW: " + m_cmpAnim);
        if (cmp_anim)
        {
            m_canBeUsed = false;
            cmp_anim.SetTrigger("Draw");
        }
    }

    /*protected void StartDrawing()
    {
        //m_canBeUsed = false;
    }*/

    protected void EndDrawing()
    {
        m_canBeUsed = true;
    }

    //ENFUNDAR
    public void HolsterWeapon()
    {

    }

}
