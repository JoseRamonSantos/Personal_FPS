using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W_DropAndTake : MonoBehaviour
{

    [SerializeField]
    private Collider    m_triggerCollider   = null;

    [SerializeField]
    private W_Base      cmp_wBase           = null;
    private Rigidbody   cmp_rb              = null;
    private Animator    cmp_anim            = null;

    // Start is called before the first frame update
    void Start()
    {
        cmp_wBase = GetComponent<W_Base>();
        cmp_rb = GetComponent<Rigidbody>();
        cmp_anim = GetComponent<Animator>();

        if (!m_triggerCollider)
        {
            Debug.LogError("DropAndTake.cs: triggerCollider is not assigned");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Drop()
    {
        if (!cmp_wBase)
        {
            cmp_wBase = GetComponent<W_Base>();
        }
        cmp_wBase.enabled = false;

        if (!cmp_anim)
        {
            cmp_anim = GetComponent<Animator>();
        }
        cmp_anim.enabled = false;

        if (!cmp_rb)
        {
            cmp_rb = GetComponent<Rigidbody>();
        }
        cmp_rb.isKinematic = false;

        m_triggerCollider.enabled = true;
        ChangeLayer("Default");
    }

    public void Take()
    {
        cmp_wBase.enabled = true;
        m_triggerCollider.enabled = false;
        cmp_anim.enabled = true;
        cmp_rb.isKinematic = true;
        ChangeLayer("Weapon");
    }

    private void ChangeLayer(string newLayer)
    {
        /*for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).
        }*/

        Transform[] child;
        child = GetComponentsInChildren<Transform>();

        for (int i = 0; i < child.Length; i++)
        {
            child[i].gameObject.layer = LayerMask.NameToLayer(newLayer);
        }

    }
}
