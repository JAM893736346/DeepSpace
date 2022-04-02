using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UiMananger : MonoBehaviour
{
    public event UnityAction role = delegate { };
    public event UnityAction start = delegate { };
    public event UnityAction exit = delegate { };
    public event UnityAction back = delegate { };
    bool Isrole;
    GameObject button1;
    GameObject button2;
    GameObject button3;
    public GameObject backobj;

    Button startButton;
    Button roleButton;
    Button exitButton;
    Button backButton;
    AnimatorStateInfo stateInfo;
    Animator animator;
    [SerializeField] AudioData buttonAD;
    private void Awake()
    {
        button1 = transform.GetChild(0).gameObject;
        button2 = transform.GetChild(1).gameObject;
        button3 = transform.GetChild(2).gameObject;
        startButton = button1.GetComponent<Button>();
        roleButton = button2.GetComponent<Button>();
        exitButton = button3.GetComponent<Button>();
        backButton = backobj.GetComponent<Button>();
        animator = GetComponent<Animator>();
        exitButton.onClick.AddListener(() => Application.Quit());
    }
    private void OnEnable()
    {
        role += PlayerAnimation;
        back += backEvent;
    }
    private void OnDestory()
    {
        role -= PlayerAnimation;
        back -= backEvent;

    }
    private void Start()
    {
        roleButton.onClick.AddListener(() => role.Invoke());

        backButton.onClick.AddListener(() => back.Invoke());

        startButton.onClick.AddListener(() => ScenesLoader.Instance.LoadGameteachScence());
    }
    private void Update()
    {
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
    }
    /// <summary>
    /// 播放动画
    /// </summary>
    void PlayerAnimation()
    {
        StartCoroutine(nameof(PlyeAnimationCoroutine));
    }
    IEnumerator PlyeAnimationCoroutine()
    {
        animator.SetBool("IsRole", true);
        startButton.interactable = false;
        roleButton.interactable = false;
        exitButton.interactable = false;
        //  print(2);
        //等待直到动画播完
        yield return new WaitUntil(() => { return stateInfo.IsName("ButtonScale") && stateInfo.normalizedTime >= 1.0f; });
        backobj.SetActive(true);
        animator.SetBool("IsRole", false);
        //禁用按钮
        button1.SetActive(false);
        button2.SetActive(false);
        button3.SetActive(false);
    }
    void backEvent()
    {
        StartCoroutine(nameof(backAnimationCoroutine));
    }
    IEnumerator backAnimationCoroutine()
    {
        animator.SetBool("Isreturn", true);
        button1.SetActive(true);
        button2.SetActive(true);
        button3.SetActive(true);
        backobj.SetActive(false);

        yield return new WaitUntil(() => { return stateInfo.IsName("ButtonReturn") && stateInfo.normalizedTime >= 1.0f; });
        animator.SetBool("Isreturn", false);
        startButton.interactable = true;
        roleButton.interactable = true;
        exitButton.interactable = true;
    }
    public void OnClickPlayer1()
    {
        ScenesLoader.PlayerNumber = 1;
        backButton.interactable = false;
        SinglManaager.Instance.turnTip();
    }
    public void OnClickPlayer2()
    {
        ScenesLoader.PlayerNumber = 2;
        backButton.interactable = false;
        SinglManaager.Instance.turnTip();
    }
    public void Cancel()
    {
        SinglManaager.Instance.HidTip();
        backButton.interactable = true;
    }
}
