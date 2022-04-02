using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3 : Character
{
    [SerializeField] protected int NumFire;
    [SerializeField] protected float Firetime = 1;
   
    [SerializeField] protected GameObject EnemyProject;
    GameObject bullet;
    Transform projectileTransform;


    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(nameof(FireCoroutine));
    }
    void OnDisable()
    {
        StopAllCoroutines();
    }
    IEnumerator FireCoroutine()
    {
        while (gameObject.activeSelf)
        {
            
            yield return new WaitForSeconds(Firetime );
            EnemyFire();
        }

    }
    protected  void EnemyFire()
    {
        float angle = 360 / NumFire;
        int i = 0;
      
         while (i<NumFire )
          {
            for (i = 0; i <NumFire; i++)
            {
                bullet = BeizerPoolManger.Release(EnemyProject, transform.position);
                var Script = bullet.GetComponent<Transform>();
                //Script.rotation = Quaternion.Euler(Vector3.forward * i * angle);
                Script.rotation  = Quaternion .Euler(0,0,angle *i);
            }

        }
    }
}
