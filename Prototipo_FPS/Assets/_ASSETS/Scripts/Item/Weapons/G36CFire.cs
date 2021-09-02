using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class G36CFire : MonoBehaviour
{
    public G36CMaster g36cmaster;

    public float range = 100f;
    public AudioSource singleShot;
    public AudioClip single;


    public G36CReload reload;
    public bool autofire;

    public float fireRate;
    public float LastTimeFired;

    public AudioSource noammo;
   
    public bool OfflineMode;

    public GameObject bulletPath;

    public New_Weapon_Recoil_Script recoil;

    void Start()
    {
        recoil = this.GetComponent<New_Weapon_Recoil_Script>();
        reload = this.GetComponent<G36CReload>();
        g36cmaster = this.GetComponent<G36CMaster>();
        singleShot.clip = single;
    }

   
    void Update()
    {
    
      
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (Time.time - LastTimeFired > 1 / fireRate)
            {
                LastTimeFired = Time.time;
                if (reload.CurrentMag >= 1) //if we have one bullet in the chamber
                {
                    fireRate = 10;
                    Shoot();
                }
                else 
                {
                    noammo.Play();
                    fireRate = 1;
                }


            }
        }
      

    }
    void Shoot()
    {
        if (reload.Reloading == true)
        {
            return;
        }
        recoil.Fire();
        singleShot.Play();
        reload.CurrentMag -= 1;
        RaycastHit hit;
        if (Physics.Raycast(bulletPath.transform.position, bulletPath.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);

        }
    }


    IEnumerator rapidFire()
    {
       
            yield return new WaitForSeconds(.25f);
            if (Input.GetKey(KeyCode.Mouse0))
            {
                autofire = true;
            }
            else
            {
                autofire = false;
            }

        

    }


}
