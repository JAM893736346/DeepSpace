using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScenesLoader : Singleton<ScenesLoader>
{
    [SerializeField] Image translationImage;
    [SerializeField] float fadetime = 3.5f;
    public static int PlayerNumber = 1;
    Color color;
    const string Teachplay = "teachPlay";
    const string Gameplay = "GamePlay";
    const string chooseMenuplay = "chooseMenu";
    void Load(string scence)
    {
        SceneManager.LoadScene(scence);
    }

    /// <summary>
    /// 淡入淡出效果异步加载场景
    /// </summary>
    /// <param name="scence"></param>
    /// <returns></returns>
    IEnumerator LoadCoroutine(string scence)
    {
        //记录异步加载的场景值
        var loadingOperation = SceneManager.LoadSceneAsync(scence);
        //加载完成后禁用场景
        loadingOperation.allowSceneActivation = false;
        //启用画布
        translationImage.gameObject.SetActive(true);
        //淡入效果
        while (color.a < 1)
        {
            color.a = Mathf.Clamp01(color.a + Time.unscaledDeltaTime / fadetime);
            translationImage.color = color;
            yield return null;
        }
        //加载成功后启用场景
        loadingOperation.allowSceneActivation = true;
        //淡出效果
        while (color.a > 0)
        {
            color.a = Mathf.Clamp01(color.a - Time.unscaledDeltaTime / fadetime);
            translationImage.color = color;
            yield return null;
        }
        //禁用话画布
        loadingOperation.allowSceneActivation = false;

    }

    /// <summary>
    /// 加载主场景
    /// </summary>
    public void LoadGameteachScence()
    {
        StartCoroutine(LoadCoroutine(Teachplay));
    }
     public void LoadGamePlayScence()
    {
        StartCoroutine(LoadCoroutine(Gameplay));
    }
      public void chooseMenuPlayScence()
    {
        StartCoroutine(LoadCoroutine(chooseMenuplay));
    }
}
