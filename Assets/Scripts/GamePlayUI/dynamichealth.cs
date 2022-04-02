using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class dynamichealth : MonoBehaviour
{
    public Image fillImageBack;//表面血条
    public Image fillImagefront;//动态平滑血条
    [SerializeField] float fillSpeed = 0.1f;
    [SerializeField] float DealyTime = 0.5f;
    WaitForSeconds waitForDelayFill;
    float t;
    [SerializeField] bool delyfill = true;

    //当前血量fill值
    float currentFillAmount;
    //目标血量fill值
    protected float targetFillAmount;

    float prevousFillAmount;
    //记录协程中间变量
    Coroutine bufferedFillingCoroutine;
    Canvas canvas;
    
    void Awake()
    {
        if (TryGetComponent<Canvas>(out Canvas canvas))
        {
            canvas.worldCamera = Camera.main;
        }
        waitForDelayFill = new WaitForSeconds(DealyTime);
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    public virtual void Initialize(float currentValue, float maxValue)
    {
        currentFillAmount = currentValue / maxValue;
        targetFillAmount = currentFillAmount;
        //初始化前后图片fill
        fillImageBack.fillAmount = currentFillAmount;
        fillImagefront.fillAmount = currentFillAmount;
    }
    public void UpdataStats(float currentValue, float maxValue)
    {
        //目标fill值记录下
        targetFillAmount = currentValue / maxValue;
        if (bufferedFillingCoroutine != null)
        {
            StopCoroutine(bufferedFillingCoroutine);
        }
        //状态减小--当前>目标
        if (currentFillAmount > targetFillAmount)
        {
            //前fill更新
            fillImagefront.fillAmount = targetFillAmount;
            //后fill延迟
            bufferedFillingCoroutine = StartCoroutine(BuffFillingCoroutine(fillImageBack));
        }
        //状态增加
        else if (currentFillAmount < targetFillAmount)
        {
            //后fill更新
            fillImageBack.fillAmount = targetFillAmount;
            //前fill延迟
            bufferedFillingCoroutine = StartCoroutine(BuffFillingCoroutine(fillImagefront));

        }
    }
    //延迟协程
    protected virtual IEnumerator BuffFillingCoroutine(Image image)
    {
        if (delyfill)
        {
            yield return waitForDelayFill;
        }
        prevousFillAmount = currentFillAmount;
        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * fillSpeed;
            currentFillAmount = Mathf.Lerp(prevousFillAmount, targetFillAmount, t);
            image.fillAmount = currentFillAmount;
            yield return null;
        }
    }
}
