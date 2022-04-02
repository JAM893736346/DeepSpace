using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDeactivate : MonoBehaviour
{
    [SerializeField] bool IsDestory;
    [SerializeField] float AutoTime = 3f;
    WaitForSeconds waitForSeconds;
    private void Awake()
    {
        waitForSeconds = new WaitForSeconds(AutoTime);
    }
    private void OnEnable() {
        StartCoroutine(nameof(AutoCoroutine));
    }
    IEnumerator AutoCoroutine()
    {
        yield return waitForSeconds;
        if (IsDestory)
        {
            Destroy(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
