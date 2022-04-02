using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile1 : EnemyProjectile
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Downplane"))
        {
           
            transform.rotation = Quaternion.AngleAxis(30, Vector3.back  );
        }
       else  if (other.gameObject.CompareTag("Upplane"))
        {
            transform.rotation = Quaternion.AngleAxis(-30, Vector3.back  );
        }
    }
}
