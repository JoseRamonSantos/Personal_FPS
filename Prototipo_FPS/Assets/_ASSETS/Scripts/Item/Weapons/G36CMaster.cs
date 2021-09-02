using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;

public class G36CMaster: MonoBehaviour
{
    public Animator anim;
    public bool isAiming;
    public bool isReloading;

    G36CReload reload;


    void Start()
    {
        reload = this.GetComponent<G36CReload>();
        anim = this.GetComponent<Animator>();
    }

    void Update()
    {
        if (!isAiming)
        {
            anim.SetBool("Ads", false);
        }

        if (!Input.GetKey(KeyCode.W) && !isAiming && !Input.GetKey(KeyCode.LeftShift))
        {
            anim.SetBool("isIdle", true);
        }
        if (Input.GetKey(KeyCode.W) && !isAiming && !Input.GetKey(KeyCode.LeftShift))
        {
            anim.SetBool("isIdle", false);
            StartCoroutine(wait());
        }
        else
        {
            anim.SetBool("isWalking", false);
            // StopAllCoroutines();
        }
        if (!isAiming && Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKey(KeyCode.W))
            {
                anim.SetBool("isIdle", false);
                StartCoroutine(waitRun());
            }

        }
        else
        {
            anim.SetBool("isRunning", false);
            //StopAllCoroutines();
        }


        if (reload.Reloading == true)
        {
            isReloading = true;

            anim.SetBool("Reload", true);
        }
        else
        {
            isReloading = false;
            anim.SetBool("Reload", false);

        }
    }
       

    public void AimDownSights()
    {
        if(isReloading == false)
        {
            isAiming = true;
            anim.SetBool("Ads", true);
        }
       

    }
    IEnumerator wait()
    {
        yield return new WaitForSeconds(.1f);
        if (Input.GetKey(KeyCode.W))
        {
            anim.SetBool("isWalking", true);
        }
    }
    IEnumerator waitRun()
    {
        yield return new WaitForSeconds(.1f);

        anim.SetBool("isRunning", true);

    }


}


