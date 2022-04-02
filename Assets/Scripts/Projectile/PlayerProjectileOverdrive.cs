using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectileOverdrive : PlayerProjectile
{
    // Start is called before the first frame update
    [SerializeField] ProjectileGidestanceSystem GidestanceSystem;
    protected override void OnEnable()
    {

        SetTarget(EnemyManager.Instance.RandomEnemy);
        transform.rotation = Quaternion.identity;
        if (target == null)
        {
            base.OnEnable();
        }
        else
        {
            StartCoroutine(GidestanceSystem.HomingCoroutine(target));
        }
    }
}
