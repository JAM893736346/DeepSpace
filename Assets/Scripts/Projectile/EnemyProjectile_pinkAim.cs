using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile_pinkAim : EnemyProjectile
{
    //private TrailRenderer trail;
    private void Awake()
    {
        //trail = GetComponentInChildren<TrailRenderer>();
        SetTarget(GameObject.FindGameObjectWithTag("Player"));
    }
    protected override void OnEnable()
    {

        StartCoroutine(nameof(MoveDirectionCoroutine));
        base.OnEnable();
    }

    private void OnDisable()
    {
        //trail.Clear();
    }
    IEnumerator MoveDirectionCoroutine()
    {
        yield return null;
        if (target.activeSelf)
        {
            moveDirction = (target.transform.position - transform.position).normalized;
        }
    }
}
