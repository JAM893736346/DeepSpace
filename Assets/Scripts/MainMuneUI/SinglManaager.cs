using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class SinglManaager : Singleton<SinglManaager>
{
    [SerializeField] GameObject tipimage;
    Button sure;
    Button cancel;
    Collider2D surecollider;
    Collider2D cancelcollider;
    protected override void Awake()
    {
        base.Awake();
        sure = tipimage.transform.GetChild(0).GetComponent<Button>();
        cancel = tipimage.transform.GetChild(1).GetComponent<Button>();
        surecollider=GameObject.Find("Player1").GetComponent<Collider2D>();
        cancelcollider=GameObject.Find("Player2").GetComponent<Collider2D>();
    }
    private void Start() {
        sure.onClick.AddListener(() => ScenesLoader.Instance.LoadGameteachScence());
    }
   public void HidTip()
    {
        tipimage.SetActive(false);
         surecollider.enabled=true;
        cancelcollider.enabled=true;
    }
   public void turnTip()
    {
        tipimage.SetActive(true);
        surecollider.enabled=false;
        cancelcollider.enabled=false;
    }
    /// <summary>
    /// 添加确认事件
    /// </summary>
    /// <param name="name"></param>
    public void sureAddListener(UnityAction name)
    {
        sure.onClick.AddListener(name);
    }
    /// <summary>
    /// 添加取消事件
    /// </summary>
    /// <param name="name"></param>
    public void CancelAddListener(UnityAction name)
    {
        cancel.onClick.AddListener(name);
    }
}
