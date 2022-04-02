using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScale_S : MonoBehaviour
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
            while (transform.localScale != 1.45f * cale)
            {
                yield return new WaitForSeconds(0.2f);
                transform.localScale += new Vector3(0.03f, 0.03f, 0f);

            }
            transform.localScale = cale;
        }
        
        //yield return null;
    }
}
