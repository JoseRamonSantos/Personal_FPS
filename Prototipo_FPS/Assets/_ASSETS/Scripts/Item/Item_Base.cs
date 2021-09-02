using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item_Base : MonoBehaviour
{


    private void OnTriggerEnter(Collider other)
    {
        Char_Base target;

        if(other.TryGetComponent(out target))
        {
            Debug.Log(target.gameObject.name + " picked item (" + gameObject.name + ")");
            TakeItem(target);
            Destroy(gameObject);
        }
    }

    protected abstract void TakeItem(Char_Base _char);
}
