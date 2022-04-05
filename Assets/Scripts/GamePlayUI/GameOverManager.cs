using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    public static UnityAction Gameoverevent;
    [SerializeField] Button ReStart;
    [SerializeField] Button Return;
    Canvas canvas;
    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        canvas.enabled = false;
    }
    private void OnEnable()
    {
        Gameoverevent += GameOverEvent;
        ReStart.onClick.AddListener(() => ScenesLoader.Instance.LoadGamePlayScence());
        ReStart.onClick.AddListener(Canvasdisable);
        Return.onClick.AddListener(() => ScenesLoader.Instance.chooseMenuPlayScence());
        Return.onClick.AddListener(Canvasdisable);
    }
    private void OnDestroy() {
        Gameoverevent -= GameOverEvent;
    }
    private void Update()
    {

    }
    void GameOverEvent()
    {
         Time.timeScale = 0;
        canvas.enabled = true;
    }
    public void Canvasdisable()
    {
        canvas.enabled = false;

    }
}
