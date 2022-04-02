using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Item : MonoBehaviour
{

    bool IsClick = false;
    [SerializeField] Sprite Trueimage;
    [SerializeField] Sprite Falseimage;
    PlayerController playerController;
    SpriteRenderer idlesprit;
    [SerializeField] GameObject pointer;
    //  public static UnityAction ButtonEvent = delegate { };
    public event UnityAction HitEvent = delegate { };

    Animator animator;
    private void Awake()
    {
        idlesprit = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        //pointer = GameObject.Find("Pointer");
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }
    private void OnEnable()
    {
        IsClick = false;
        idlesprit.sprite = Trueimage;
        animator.Play("itemAnimation");
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    // Update is called once per frame
    public void StartCoroutine()
    {
        StartCoroutine(nameof(GetSpaceCoroutine));
    }
    public void StopCoroutine()
    {
        if (IsClick == false)
        {
            idlesprit.sprite = Falseimage;
            ScoreManager.Instance.EmptyCombo();
        }
        StopCoroutine(nameof(GetSpaceCoroutine));
    }
    IEnumerator GetSpaceCoroutine()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.J) && gameObject.activeSelf)
            {
                IsClick = true;
                //播放伸缩动画
                pointer.GetComponent<bornpoint>().PlayerAnimation();
                //调用攻击逻辑
                HitEvent.Invoke();
                //连击
                ScoreManager.Instance.AddCombo(1);
                //增加能量
                PlayerEnergy.Instance.Obtain(PlayerEnergy.PERCENT);
                gameObject.SetActive(false);
            }
            yield return new WaitForFixedUpdate();
        }
    }
}
