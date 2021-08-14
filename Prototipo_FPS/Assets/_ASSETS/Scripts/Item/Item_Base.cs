using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Base : MonoBehaviour
{


    private void OnTriggerEnter(Collider other)
    {
        Char_Base target;

        if(other.TryGetComponent<Char_Base>(out target))
        {
            TakeItem(target);
        }
    }

    private void TakeItem(Char_Base _char)
    {
        Debug.Log(_char.gameObject.name + " picked item (" + gameObject.name + ")");
        Effect(_char);
        Destroy(gameObject);
    }

    protected virtual void Effect(Char_Base _char)
    {

    }
}
