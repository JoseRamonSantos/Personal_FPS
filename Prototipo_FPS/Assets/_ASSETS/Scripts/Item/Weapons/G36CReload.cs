using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class G36CReload : MonoBehaviour
{

    public int CurrentMag = 30;
    public bool Reloading;
    public G36CMaster g36cmaster;
    


    // Start is called before the first frame update
    void Start()
    {
        
        Reloading = false;


      
        g36cmaster = this.GetComponent<G36CMaster>();
        

      

    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentMag > 30)
        {
            CurrentMag = 30;
        }


        if (Input.GetKeyDown(KeyCode.R) && !Reloading)
        {
            if (CurrentMag < 30)
            {

                Reloading = true;
                Reload();
            }
        }
   
    }

    public void Reload()
    {
       
        if(Reloading == false)
        {
            
        }
       

    }
    public void fillAmmo()
    {

        CurrentMag += 30;
        Reloading = false;
        g36cmaster.isReloading = false;

    }
 
}
