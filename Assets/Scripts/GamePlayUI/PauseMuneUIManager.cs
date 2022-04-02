using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMuneUIManager : MonoBehaviour
{
    [SerializeField] Button Continuebtn;
    [SerializeField] Button Giveupbtn;
    [SerializeField] AudioData buttonAD;

    Canvas canvas;
    protected void Awake()
    {
        canvas = GetComponent<Canvas>();
    }
    protected void OnEnable()
    {
        GameManager.onPause += OnPauseEvent;
        GameManager.onunPause += OnunPauseEvent;
        GameManager.onPause += stateCanvesHid;
        GameManager.onunPause += stateCanvesturn;
        //继续按钮返回游戏
        Continuebtn.onClick.AddListener(() => GameManager.onunPause.Invoke());
        //暂停按钮结算界面
        Giveupbtn.onClick.AddListener(() => ScenesLoader.Instance.chooseMenuPlayScence());
        Giveupbtn.onClick.AddListener(stateCanvesturn);
    }
    private void OnDestroy()
    {
        GameManager.onPause -= stateCanvesHid;
        GameManager.onunPause -= stateCanvesturn;
        GameManager.onPause -= OnPauseEvent;
        GameManager.onunPause -= OnunPauseEvent;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManager.GameState == GameState.Playing)
            {
                GameManager.GameState = GameState.Paused;
                GameManager.onPause.Invoke();
            }
            else if (GameManager.GameState == GameState.Paused)
            {
                GameManager.GameState = GameState.Playing;
                GameManager.onunPause.Invoke();
            }
        }
    }
    public void OnPauseEvent()
    {
        AudioManager.Instance.PlayRandomSFX(buttonAD);
        Time.timeScale = 0;
    }
    public void OnunPauseEvent()
    {
        AudioManager.Instance.PlayRandomSFX(buttonAD);
        Time.timeScale = 1;
    }
    protected void stateCanvesHid()
    {
        canvas.enabled = true;
    }
    protected void stateCanvesturn()
    {
        canvas.enabled = false;
    }

}
