using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Rocket : Projectile_Base
{
    protected override void EndLifetime()
    {
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (m_owner)
        {
            if (collision.transform.gameObject == m_owner.gameObject) { return; }
        }

        Explode();
    }
}
