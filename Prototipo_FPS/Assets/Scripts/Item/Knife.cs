using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : ItemBase
{


    protected override void Awake()
    {
        base.Awake();

    }

    protected override void Start()
    {
        base.Start();

    }

    protected override void Update()
    {
        base.Update();

    }

    protected override void PrimaryActionDown()
    {
        base.PrimaryActionDown();

        Attack1();
    }



    protected virtual void Attack1()
    {
        Debug.Log("ATTACK 1" + gameObject.name);
    }

}
