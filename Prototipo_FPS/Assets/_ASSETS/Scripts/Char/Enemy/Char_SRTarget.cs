using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Char_SRTarget : Char_Enemy
{
    protected override void Die()
    {
        base.Die();
        GameManager.Instance.RemoveEnemy();
        Destroy(this.gameObject);
    }
}
