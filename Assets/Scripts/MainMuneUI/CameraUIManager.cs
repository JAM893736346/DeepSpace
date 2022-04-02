using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraUIManager : MonoBehaviour
{
    public UiMananger uiMananger;
    Physics2DRaycaster physics2D;
    new Camera camera;
    float t;
    [SerializeField] float changeTime = 2.0f;
    float originSizi;
    private void Awake()
    {
        physics2D = GetComponent<Physics2DRaycaster>();
        camera = GetComponent<Camera>();
        originSizi = camera.orthographicSize;
    }
    private void OnEnable()
    {
        uiMananger.role += CameraChange;
        uiMananger.back += Camerretun;
    }
    private void OnDisable()
    {
        uiMananger.role -= CameraChange;
        uiMananger.back -= Camerretun;


    }
    void CameraChange()
    {
        StartCoroutine(nameof(SizeChangeCoroutine));
    }
    void Camerretun()
    {
        StartCoroutine(nameof(SizeReturnCoroutine));

    }
    IEnumerator SizeChangeCoroutine()
    {
        t = 0;
        physics2D.enabled = true;
        while (t < changeTime)
        {
            t += Time.deltaTime;
            camera.orthographicSize = Mathf.Lerp(originSizi, 4, t / changeTime);
            yield return null;
        }
    }
    IEnumerator SizeReturnCoroutine()
    {
        t = 0;
        physics2D.enabled = false;
        while (t < changeTime)
        {
            t += Time.deltaTime;
            camera.orthographicSize = Mathf.Lerp(4, originSizi, t / changeTime);
            yield return null;
        }
    }
}
