using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : Projectile
{
    [SerializeField] float moverate = 0.6f;
    private void Awake()
    {
        // PlayerController.onOverdrive += onOverDriveProjectile;
        // PlayerController.unOverdrive += onOverDriveProjectile;
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnDestroy()
    {
        // PlayerController.onOverdrive -= onOverDriveProjectile;
        // PlayerController.unOverdrive -= onOverDriveProjectile;
    }
    protected override void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            base.OnCollisionEnter2D(other);
        }
    }
    public void onOverDriveProjectile() => MoveSpeed *= moverate;
    public void unOverDriveProjectile() => MoveSpeed /= moverate;
}
