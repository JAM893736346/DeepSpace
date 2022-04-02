using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public int WaveCount = 1;
    public int waveCountNext;
    [SerializeField] Transform pointer;
    [SerializeField] List<GameObject> items;
    [SerializeField] AudioSource bgmcontroller;
    [SerializeField] AudioClip[] bgmClip;
    [SerializeField] item_Data[] item_s;
    item_Data itemdata;
    public bool IsPlayingover = false;
    int Count = 0;
    [SerializeField] EnemyManager enemyManager;
    private void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            items.Add(transform.GetChild(i).gameObject);
        }
        InitDatamic(0);
        
    }
    private void OnEnable() {
        GameOverManager.Gameoverevent+=bgmcontrollerPause;
        GameWinManager.GameWinevent+=bgmcontrollerPause;
    }
    private void OnDisable()
    {
        StopAllCoroutines();
        GameOverManager.Gameoverevent-=bgmcontrollerPause;
        GameWinManager.GameWinevent-=bgmcontrollerPause;
    }
    private void Update()
    {
        if (!IsPlayingover)
        {
            if ((WaveCount == itemdata.itemDatas.Count) && TransformRotation.Instance.GetInspectorRotationValueMethod(pointer).z == 0)
            {
                Count++;
                if (Count == 2)
                {
                    //如果变盘转完没败全部敌人失败
                    GameManager.GameState = GameState.GameOver;
                    GameOverManager.Gameoverevent.Invoke();
                    return;
                }
                IsPlayingover = true;
                InitDatamic(1);
            }
        }

    }
    //初始化表盘对应的音乐和音乐数据
    public void InitDatamic(int Index)
    {
        StopAllCoroutines();
        pointer.gameObject.SetActive(false);
        bgmcontroller.clip = bgmClip[Index];
        if (!bgmcontroller.isPlaying) { bgmcontroller.Play(); }
        itemdata = item_s[Index];
        pointer.gameObject.SetActive(true);
        WaveCount = 1;
        waveCountNext = 1;
        StartCoroutine(nameof(WaveCountCoroutine));
        StartCoroutine(nameof(StartBornCoroutine));
        for (int i = 0; i < itemdata.itemDatas[1].statetable.Length; i++)
        {
            if (itemdata.itemDatas[1].statetable[i] == 1)
            {
                if (!items[i].gameObject.activeSelf)
                {
                    items[i].gameObject.SetActive(true);
                }
            }
        }
    }
    IEnumerator WaveCountCoroutine()
    {
        while (true)
        {
            //播放动作
            if (TransformRotation.Instance.GetInspectorRotationValueMethod(pointer).z == -20)
            {
                if (GameObject.FindWithTag("Player").TryGetComponent<Animator>(out Animator animator))
                {
                    animator.Play("gunReload1");
                }
            }
            //清场
            if (TransformRotation.Instance.GetInspectorRotationValueMethod(pointer).z == -10)
            {
                WaveCount = waveCountNext;

                //清场
                foreach (var item in items)
                {
                    if (item.activeSelf)
                    {
                        item.SetActive(false);
                    }
                }

                yield return new WaitForSeconds(0.7f);
            }
            yield return null;
        }

    }
    IEnumerator StartBornCoroutine()
    {
        while (!(WaveCount >= itemdata.itemDatas.Count))
        {
            //等到转完一轮再生成
            if (TransformRotation.Instance.GetInspectorRotationValueMethod(pointer).z == 0)
            {

                for (int i = 0; i < itemdata.itemDatas[WaveCount].statetable.Length; i++)
                {
                    if (itemdata.itemDatas[WaveCount].statetable[i] == 1)
                    {
                        items[i].gameObject.SetActive(true);
                    }
                    yield return new WaitForSeconds(0.1f);
                }
                waveCountNext++;
            }
            yield return new WaitUntil(() => WaveCount == waveCountNext);

        }
    }
    void bgmcontrollerPause()
    {
        bgmcontroller.Pause();
    }
}
