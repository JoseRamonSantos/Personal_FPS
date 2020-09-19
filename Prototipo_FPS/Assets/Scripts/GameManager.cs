using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : TemporalSingleton<GameManager>
{
    [Header("Options")]
    
    [SerializeField]
    private bool m_righthand = true;

    [SerializeField]
    private float m_mousseSensibility = 1;

    [SerializeField]
    [Range (0, 5)]
    private int m_maxUtility = 0;
    

    [Header("Game options")]
    [SerializeField]
    private int m_maxEnterHits = 5;

    [SerializeField]
    protected GameObject m_AmmoHUD = null;
    [SerializeField]
    protected TextMeshProUGUI m_crntAmmoText = null;
    [SerializeField]
    protected TextMeshProUGUI m_totalAmmoText = null;


    [SerializeField]
    private     Camera          m_mainCamera        = null;
    [SerializeField]
    private     Camera          m_weaponCamera      = null;

    //PROPERTIES
    public float    MousseSensibility       { get => m_mousseSensibility;   set => m_mousseSensibility  = value; }

    public int      MaxUtility              { get => m_maxUtility;          set => m_maxUtility         = value; }
    public Camera   MainCamera              { get => m_mainCamera;          set => m_mainCamera         = value; }
    public Camera   WeaponCamera            { get => m_weaponCamera;        set => m_weaponCamera       = value; }
                                                                    
    public int      MaxEnterHits            { get => m_maxEnterHits;        set => m_maxEnterHits       = value; }

    void Start()
    {
        //CAMERA
        m_mainCamera = Camera.main;
        WeaponCamera = MainCamera.transform.Find("WeaponCamera").GetComponent<Camera>();

        //AMMO HUD
        m_AmmoHUD = GameObject.Find("Canvas/AmmoHUD");
        m_crntAmmoText = GameObject.Find("Canvas/AmmoHUD/txt_CrntAmmo").GetComponent<TextMeshProUGUI>();
        m_totalAmmoText = GameObject.Find("Canvas/AmmoHUD/txt_TotalAmmo").GetComponent<TextMeshProUGUI>();

        DeactivateAmmoHUD();
    }


    void Update()
    {
        
    }

    public RaycastHit[] OrderRaycastHitByDistance(RaycastHit[] hits, int length)
    {
        for (int i = 0; i < length; i++)
        {
            float distance = hits[i].distance;
            //Debug.Log("--------------------");
            //Debug.Log(i + "." + i + " - [" + hits[i].transform.name + "]  " + hits[i].distance + "m <-----");

            for (int j = i + 1; j < length; j++)
            {
                //Debug.Log(i + "." + j + " - [" + hits[j].transform.name + "]  " + hits[j].distance + "m <-");
                
                if (distance > hits[j].distance)
                {
                    RaycastHit nearestRay = hits[j];

                    //Debug.Log("..........");
                    //Debug.Log("CHANGE");

                    for (int z = j; z >= i; z--)
                    {
                        //Debug.Log("-");
                        if (z == i)
                        {
                            //Debug.Log(i + "." + j + "." + z + " - " + i + " --> " + hits[z].transform.name);
                            hits[z] = nearestRay;
                        }
                        else
                        {
                            //Debug.Log(i + "." + j + "." + z + " - " + hits[z - 1].transform.name + " -->" + hits[z].transform.name);
                            hits[z] = hits[z - 1];
                        }
                    }
                    //Debug.Log("..........");
                }
            }
        }
        return hits;
    }

    public void UpdateAmmoHUD(int crntAmmo, int totalAmmo)
    {

        m_crntAmmoText.text = crntAmmo.ToString();
        m_totalAmmoText.text = totalAmmo.ToString();
    }

    public void ActivateAmmoHUD()
    {
        m_AmmoHUD.gameObject.SetActive(true);
    }

    public void DeactivateAmmoHUD()
    {
        m_AmmoHUD.gameObject.SetActive(false);
    }


}
