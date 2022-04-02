using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class GameWinManager : MonoBehaviour
{
    public static UnityAction GameWinevent;
    [SerializeField] Text scoretext;
    [SerializeField] Button winbtn;
    Canvas canvas;
    private void Awake()
    {
        Time.timeScale = 1;
        canvas = GetComponent<Canvas>();
        canvas.enabled = false;
        winbtn.onClick.AddListener(() => ScenesLoader.Instance.chooseMenuPlayScence());
        winbtn.onClick.AddListener(Canvasdisable);
        winbtn.onClick.AddListener(Canvasdisable);
    }
    private void OnEnable()
    {
        GameWinevent += Winevent;
    }
    private void Update()
    {

    }
    private void OnDestroy()
    {
        GameWinevent -= Winevent;

    }
    void Winevent()
    {
        Time.timeScale = 0;
        scoretext.text = "Score:" + ScoreManager.Instance.Score.ToString();
        canvas.enabled = true;
    }
    public void Canvasdisable()
    {
        canvas.enabled = false;
    }
}
