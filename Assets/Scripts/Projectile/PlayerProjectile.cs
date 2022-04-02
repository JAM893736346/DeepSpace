using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : Projectile
{
    private void Awake()
    {
        // moveDirction = new Vector3(1, 0, 0);
        if (moveDirction != Vector3.right)
        {
            transform.GetChild(0).rotation = Quaternion.FromToRotation(Vector2.right, moveDirction);
        }
    }
    protected override void OnEnable()
    {
        // moveDirction = PlayerController.GunDirection.normalized;
        base.OnEnable();
    }
    private void Update()
    {

    }
    private void OnDisable()
    {
        if (transform.GetChild(0).TryGetComponent<TrailRenderer>(out TrailRenderer trail))
        {
            trail.Clear();
        }
        else
        {
            return;
        }
    }
    protected override void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            base.OnCollisionEnter2D(other);
        }
    }
}
