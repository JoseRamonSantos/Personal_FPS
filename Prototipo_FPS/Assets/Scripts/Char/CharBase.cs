using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharBase : MonoBehaviour
{

    protected       float       m_maxLife           = 100;
    protected       float       m_currentLife       = 0;



    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
        m_currentLife = m_maxLife;
    }


    void Update()
    {
        
    }

    public virtual void ReceiveDamage(float damage)
    {
        m_currentLife -= damage;

        if(m_currentLife <= 0)
        {
            m_currentLife = 0;
            Die();
        }
    }


    protected virtual void Die()
    {

    }


}
