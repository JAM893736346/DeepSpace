using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile2 :EnemyProjectile
{
[SerializeField ]int Change; 
 
 
protected override  void OnEnable()
    {
        StartCoroutine(nameof(MoveDirectly));
        StartCoroutine (nameof (Rotate ));
    }
    IEnumerator MoveDirectly()
    {
        while (gameObject.activeSelf)
        {
            Move();
            yield return null;
        }
    }
    IEnumerator Rotate()
    {
      

        while (gameObject .activeSelf )
        {     int i=0;
      
                for (i = 0; i <= Change ; i++)
                {

                    transform.rotation  = Quaternion.AngleAxis(15 * i, Vector3.back);
                    yield return new WaitForSeconds(0.1f);
                }
            
           
            
           // print(22222);
           
        }
    }
}