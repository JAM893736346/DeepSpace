using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScale  : MonoBehaviour
{
Vector3 cale;
    // Start is called before the first frame update
    void Start()
    {
       cale=transform .localScale ;
    }

    private void OnEnable()
    {
        StartCoroutine(nameof(Iscale));
    }
    // Update is called once per frame
    IEnumerator Iscale()
    {while (gameObject .activeSelf )
        {
            while (transform.localScale != 1.25f * cale)
            {
                yield return new WaitForSeconds(0.2f);
                transform.localScale += new Vector3(0.025f, 0.025f, 0f);

            }
            transform.localScale = cale;
        }
        
        //yield return null;
    }
}
