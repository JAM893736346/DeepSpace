using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bornpoint : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float Speed = 10;
    WaitForSeconds waitForSeconds;
    [SerializeField] float delytime = 0.5f;
    Animator animator;
    // float timecurrent;
    // Vector3 transformRotate;
    private void Awake()
    {
        waitForSeconds = new WaitForSeconds(delytime);
        animator = GetComponent<Animator>();
    }
    void OnEnable()
    {
        transform.rotation = Quaternion.Euler(Vector3.zero);
        StartCoroutine(nameof(pointerCoroutine));
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    IEnumerator pointerCoroutine()
    {
        while (true)
        {
            yield return waitForSeconds;
            transform.Rotate(new Vector3(0, 0, Speed));
        }
    }
    public void PlayerAnimation()
    {
        animator.Play("Scale");
    }
}
