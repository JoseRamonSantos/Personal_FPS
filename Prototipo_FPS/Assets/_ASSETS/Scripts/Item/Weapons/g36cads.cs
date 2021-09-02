using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class g36cads : MonoBehaviour
{
    public G36CMaster g36cmaster;
    //public GameObject ScopeDefault;

    // Start is called before the first frame update
    void Start()
    {
        g36cmaster = this.GetComponent<G36CMaster>();


    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            //StartCoroutine(waittodisable());
            g36cmaster.AimDownSights();


        }
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            //StartCoroutine(waittoenable());
            g36cmaster.isAiming = false;


        }

    }
    IEnumerator waittoenable()
    {

        yield return new WaitForSeconds(.5f);
        //ScopeDefault.SetActive(true);

    }
    IEnumerator waittodisable()
    {
        yield return new WaitForSeconds(.2f);
        //ScopeDefault.SetActive(false);
    }



}