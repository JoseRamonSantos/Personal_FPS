using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Playables;
using UnityEngine;
using UnityEngine.XR.WSA.Input;


public class CharPlayer : CharBase
{

    private     GameObject          m_weaponsContainer  = null;
    private     Transform           m_riflePos          = null;
    private     Transform           m_pistolPos         = null;
    private     Transform           m_knifePos          = null;

    [SerializeField]
    private     GameObject          m_prefKnife         = null;

    [SerializeField]
    private     ItemBase           m_crntItem          = null;
    

    [SerializeField]
    private     ItemBase[]         m_inventory         = null;

    [SerializeField]
    private     int                 m_inventoryIndex    = 0;

    private     UtilityBase[]      m_utility           = null;


    public int InventoryIndex { get => m_inventoryIndex; set => m_inventoryIndex = value; }


    public static CharBase Instance
    {
        get;
        private set;
    }


    protected override void Awake()
    {
        base.Awake();
        
        Instance = this;
    }


    protected override void Start()
    {
        base.Start();
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;

        m_weaponsContainer = GameObject.Find("WeaponsContainer");
        m_riflePos = transform.Find("MainCamera/WeaponCamera/RiflePos");
        m_pistolPos = transform.Find("MainCamera/WeaponCamera/PistolPos");
        m_knifePos = transform.Find("MainCamera/WeaponCamera/KnifePos");

        if(m_inventory.Length == 0)
        {
            m_inventory = new ItemBase[4 + GameManager.Instance.MaxUtility];
        }
        else
        {
            for (int i = 0; i < m_inventory.Length; i++)
            {
                if(m_inventory[i])
                {
                    //m_inventory[i].gameObject.SetActive(true);
                    
                    if (m_inventory[i].TryGetComponent<W_DropAndTake>(out W_DropAndTake cmpDropAndTake))
                    {
                        cmpDropAndTake.Take();
                        //m_inventory[i].GetComponent<W_DropAndTake>().Take();
                    }
                    
                    m_inventory[i].gameObject.SetActive(false);
                }
                
            }


        }

        //Deactivate inventory's items
        /*for (int i = 0; i < m_inventory.Length; i++)
        {
            if (m_inventory[i])
            {
            }
        }*/

        
        if (!m_prefKnife || m_prefKnife.GetComponent<ItemBase>() == null)
        {
            Debug.LogWarning("KNIFE is not assigned");
        }
        else
        {
            //CREATE and MOVE the knife
            GameObject knife = Instantiate(m_prefKnife);
            knife.transform.parent = m_knifePos.transform;
            knife.transform.localPosition = Vector3.zero;
            knife.transform.localRotation = Quaternion.identity;

            m_inventory[2] = knife.GetComponent<ItemBase>();
            
            ChangeToItem(2);
        }


    }

    void Update()
    {

        if (Input.GetKeyDown(InputManager.Instance.K_item1) && InventoryIndex!= 0 && m_inventory[0] != null)
        {
            ChangeToItem(0);
        }

        if (Input.GetKeyDown(InputManager.Instance.K_item2) && InventoryIndex != 1 && m_inventory[1] != null)
        {
            ChangeToItem(1);
        }

        if (Input.GetKeyDown(InputManager.Instance.K_item3) && InventoryIndex != 2 && m_inventory[2] != null)
        {
            ChangeToItem(2);
        }

        if (Input.GetKeyDown(InputManager.Instance.K_item4) && InventoryIndex != 3 && m_inventory[3] != null)
        {
            ChangeToItem(3);
        }

        NextItem();
        PreviousItem();
    }

    private void NextItem()
    {
        if ((Input.mouseScrollDelta.y < -0.1))
        {
            for (int i = 1; i < m_inventory.Length; i++)
            { 
                //Debug.Log("--------------------------");
                //Debug.Log("// i:" + i);
                //Debug.Log("Inventory Index: " + m_inventoryIndex);
                //Debug.Log("// Inventor length: " + m_inventory.Length);

                if (m_inventoryIndex + i < m_inventory.Length)
                { 
                    if (m_inventory[m_inventoryIndex + i] != null)
                    {
                        //Debug.Log("NEXT--------1: " + m_inventory[m_inventoryIndex + i]);
                        ChangeToItem(m_inventoryIndex + i);
                        break;
                    }
                }
                else if (m_inventoryIndex + i >= m_inventory.Length)
                {
                    if (m_inventory[(m_inventory.Length - m_inventoryIndex - i) * -1] != null)
                    {
                        //Debug.Log("NEXT--------2: " + m_inventory[(m_inventory.Length - m_inventoryIndex - i) * -1]);
                        ChangeToItem((m_inventory.Length - m_inventoryIndex - i) * -1);
                        break;
                    }
                }

            }

        }
    }
    
    private void PreviousItem()
    {
        if (Input.mouseScrollDelta.y > 0.1)
        {
            Debug.Log("PREVIOUS item");

            for (int i = 1; i < m_inventory.Length; i++)
            {
                Debug.Log("--------------------------");
                Debug.Log("// i:" + i);
                Debug.Log("Inventory Index: " + m_inventoryIndex);
                Debug.Log("// Inventor length: " + m_inventory.Length);

                if (m_inventoryIndex - i >= 0)
                {
                    if(m_inventory[m_inventoryIndex - i] != null)
                    {
                        Debug.Log("PREVIOUS--------1: " + m_inventory[m_inventoryIndex - i]);
                        ChangeToItem(m_inventoryIndex - i);
                        break;
                    }
                }
                else if(m_inventoryIndex - i < 0)
                {
                    if (m_inventory[i] != null)
                    {
                        Debug.Log("PREVIOUS--------2: " + m_inventory[i]);
                        ChangeToItem(i);
                        break;
                    }
                }
            }
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<W_Base>(out var newWeapon))
        {
            //PRIMARY WEAPON
            if(newWeapon.data.m_weaponType == WeaponItem.WeaponType.primaryWeapon)
            {
                if (!m_inventory[0])
                {
                    TakeWeapon(newWeapon, 0, true);
                }
                else
                {
                    Debug.Log("Press E to take " + other.name);

                    if (Input.GetKey(KeyCode.E))
                    {
                        DropWeapon(0);
                        TakeWeapon(newWeapon, 0, false);
                    }
                }
            }

            //SECONDARY WEAPON
            if (newWeapon.data.m_weaponType == WeaponItem.WeaponType.secondaryWeapon)
            {
                if (!m_inventory[1])
                {
                    TakeWeapon(newWeapon, 1, true);
                }
                else
                {
                    Debug.Log("Press E to take " + other.name);

                    if (Input.GetKey(KeyCode.E))
                    {
                        DropWeapon(1);
                        TakeWeapon(newWeapon, 1, false);
                    }
                }
            }
        }
    }


    public void DropWeapon(int weaponIndex)
    {
        m_crntItem.GetComponent<W_DropAndTake>().Drop();
        m_crntItem.transform.parent = m_weaponsContainer.transform.parent;
        m_crntItem = null;
        NextItem();
    }

    public void TakeWeapon(W_Base newWeapon, int weaponIndex, bool deactivateWeapon)
    {

        //DROP CRNT WEAPON
        if (m_inventory[weaponIndex])
        {
            GameObject oldWeapon = m_inventory[weaponIndex].gameObject;

            oldWeapon.transform.parent = m_weaponsContainer.transform.parent;
            oldWeapon.transform.localPosition = newWeapon.transform.position; 
            oldWeapon.transform.localRotation = newWeapon.transform.rotation;

            m_inventory[weaponIndex] = null;
        }

        //TAKE new weapon
        newWeapon.GetComponent<W_DropAndTake>().Take();
        
        //ATTACH to player
        if(newWeapon.data.m_weaponType == WeaponItem.WeaponType.primaryWeapon)
        {
            newWeapon.transform.parent = m_riflePos.transform;
        }
        else if (newWeapon.data.m_weaponType == WeaponItem.WeaponType.secondaryWeapon)
        {
            newWeapon.transform.parent = m_pistolPos.transform;
        }

        //MOVE
        newWeapon.transform.localPosition = Vector3.zero;
        newWeapon.transform.localRotation = Quaternion.identity;

        if (deactivateWeapon)
        {
            newWeapon.gameObject.SetActive(false);
        }

        m_inventory[weaponIndex] = newWeapon;
    }
    

    //desenfundar
    public void DrawItem()
    {

    }



    public void ChangeToItem(int newIndex)
    {
        //Aqui faltan las transicciones con las animaciones
        /*if (m_crntItem)
        {
            m_crntItem.gameObject.SetActive(false);
        }*/


        //DEACTIVATE CRNT ITEM
        if (m_crntItem)
        {
            m_crntItem.Deactivate();
            m_crntItem = null;
        }

        InventoryIndex = newIndex;
        m_crntItem = m_inventory[InventoryIndex];
        m_crntItem.gameObject.SetActive(true);
        m_crntItem.Activate();


    }
}
