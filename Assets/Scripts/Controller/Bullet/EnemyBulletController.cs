using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletController : BulletBase
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

    }
}
