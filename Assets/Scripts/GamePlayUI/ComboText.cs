using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboText : MonoBehaviour
{
    static Text comboText;
    GameObject childAni;
    Text childtext;
    Animation childanimation;
    int currentScore;


    void Awake()
    {
        comboText = GetComponent<Text>();
        childAni = transform.GetChild(0).gameObject;
        childtext = childAni.GetComponent<Text>();
        childanimation = childAni.GetComponent<Animation>();

    }

    void Start()
    {
        ScoreManager.Instance.ResetCombo();
        StartCoroutine(nameof(ChildPlayer));
        StartCoroutine(nameof(IsPlayCoroutine));
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    public static void UpdateText(int combo) => comboText.text = "Combo<size=90><color=\"#FF1493\">  " + combo.ToString() + "</color></size>";

    public static void ScaleText(Vector3 targetScale) => comboText.rectTransform.localScale = targetScale;
    /// <summary>
    /// 播放时机
    /// </summary>
    /// <returns></returns>
    IEnumerator IsPlayCoroutine()
    {
        while (true)
        {
            if (ScoreManager.Instance.Combo % 5 == 0 && ScoreManager.Instance.Combo != 0 && currentScore != ScoreManager.Instance.Combo)
            {
                currentScore = ScoreManager.Instance.Combo;
                childAni.SetActive(true);
            }
            yield return null;
        }
    }
    //播放逻辑处理
    IEnumerator ChildPlayer()
    {
        while (true)
        {
            if (childAni.activeSelf)
            {
                childtext.text = "+" + "<color=\"#800080\"><size=80>" + currentScore.ToString() + "</size></color>" + "X2";
                if (!childanimation.IsPlaying("ScoreAnimation"))
                {
                    //增加combo双倍得分
                    ScoreManager.Instance.AddScore(currentScore * 2);
                    childAni.SetActive(false);
                }
            }
            yield return null;
        }
    }
}
