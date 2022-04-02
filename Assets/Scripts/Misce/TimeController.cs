using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : Singleton<TimeController>
{
    [SerializeField, Range(0f, 1f)] float bulletTimeScale = 0.1f;
    float defultFixDeltaTime;
    float t;
    float musicurrent;
    AudioSource mainScore;
    protected override void Awake()
    {
        base.Awake();
        defultFixDeltaTime = Time.fixedDeltaTime;

    }
    private void OnEnable()
    {
        GameManager.onPause += musicpause;
        GameManager.onunPause += musicunpause;
    }
    private void Start()
    {
        GameObject root = GameObject.Find("Playercollison");
        mainScore = root.transform.Find("Rhythm/bgm").GetComponent<AudioSource>();
        musicurrent = mainScore.pitch;
    }
    private void OnDestroy()
    {
        GameManager.onPause -= musicpause;
        GameManager.onunPause -= musicunpause;
    }
    public void BulletTime(float duration)
    {
        Time.timeScale = bulletTimeScale;
        StartCoroutine(SlowOutCoroutine(duration));
    }

    //供外部调用 刻度减小
    public void SlowIn(float induration)
    {
        StartCoroutine(SlowInCoroutine(induration));
    }
    //供外部调用 刻度增加 
    public void SlowOut(float outduration)
    {
        StopCoroutine(SlowOutCoroutine(outduration));
    }

    //供外部调用 重载子弹时间刻度先减小后增加
    public void BulletTime(float induration, float outduration)
    {
        StartCoroutine(SlowInandOutCoroutine(induration, outduration));
    }
    /// <summary>
    /// 供外部调用 重载子弹时间刻度先减小后持续一段时间再增加增加
    /// </summary>
    /// <param name="induration"></param>
    /// <param name="keepingduration"></param>
    /// <param name="outduration"></param>
    public void SlowKeepTime(float induration, float keepingduration, float outduration)
    {
        StartCoroutine(SlowInKeepAndOutCoroutine(induration, keepingduration, outduration));
    }

    #region 具体实现
    //时间刻度减小后持续一段时间恢复
    IEnumerator SlowInKeepAndOutCoroutine(float induration, float keepingduration, float outduration)
    {
        yield return StartCoroutine(SlowInCoroutine(induration));
        yield return new WaitForSecondsRealtime(keepingduration);
        StartCoroutine(SlowOutCoroutine(outduration));
    }

    //时间先减小后增加
    IEnumerator SlowInandOutCoroutine(float induration, float outduration)
    {
        yield return StartCoroutine(SlowInCoroutine(induration));
        StartCoroutine(SlowOutCoroutine(outduration));
    }

    //事件刻度缓慢减少-- 1-0.1
    IEnumerator SlowInCoroutine(float duration)
    {
        t = 0;
        while (t < 1f)
        {
            //---Time.DeltaSTime收TimeScal的影响用Time.unscaledDeltaTime
            t += Time.unscaledDeltaTime / duration;
            Time.timeScale = Mathf.Lerp(1f, bulletTimeScale, t);
            //音量倍速缩放
            mainScore.pitch = Mathf.Lerp(1f, bulletTimeScale, t);

            Time.fixedDeltaTime = defultFixDeltaTime * Time.timeScale;

            yield return null;
        }
    }

    //持续时间timesacle恢复正常-- 0.1--1
    IEnumerator SlowOutCoroutine(float duration)
    {
        t = 0;
        while (t < 1f)
        {
            //---Time.DeltaSTime收TimeScal的影响用Time.unscaledDeltaTime
            t += Time.unscaledDeltaTime / duration;
            Time.timeScale = Mathf.Lerp(bulletTimeScale, 1f, t);
            //音量倍速缩放
            mainScore.pitch = Mathf.Lerp(bulletTimeScale, 1f, t);

            Time.fixedDeltaTime = defultFixDeltaTime * Time.timeScale;

            yield return null;
        }
    }
    #endregion
    void musicpause()
    {
        if (mainScore.isPlaying)
        {
            mainScore.Pause();
        }
    }
    void musicunpause()
    {
        if (!mainScore.isPlaying)
        {
            mainScore.Play();
        }
    }
}
