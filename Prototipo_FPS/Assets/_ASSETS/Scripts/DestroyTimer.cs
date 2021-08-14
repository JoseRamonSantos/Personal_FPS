using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTimer : MonoBehaviour
{
    [SerializeField]
    private float m_time = 3;

    void Start()
    {
        Destroy(this.gameObject, Mathf.Abs(m_time));
    }
}
