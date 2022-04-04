using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnergy : Singleton<PlayerEnergy>
{
    [SerializeField] PlayerEnergyBar energyBar;
    public const int MAX = 100;
    public const int PERCENT = 1;
    int energy = 0;
    public float InterivalObtain = 1;
    WaitForSeconds waitForSecondsObtain;

    //能量爆发消耗间隔
    public float overdeiveInterval = 0.1f;
    WaitForSeconds waitForoverdriveInterval;
    //能量爆发时不能获得能量-是否可获得能量
    bool available = true;

    protected override void Awake()
    {
        base.Awake();
        waitForoverdriveInterval = new WaitForSeconds(overdeiveInterval);
        waitForSecondsObtain = new WaitForSeconds(InterivalObtain);
        energyBar.Initialize(energy, MAX);
        Obtain(100);
    }
    private void OnEnable()
    {
        PlayerController.onOverdrive += PlayerOverdriveon;
        PlayerController.unOverdrive += PlayerOverdriveoff;
    }
    void Start()
    {
        StartCoroutine(nameof(AutoObtain));
    }
    private void OnDisable()
    {
        PlayerController.onOverdrive -= PlayerOverdriveon;
        PlayerController.unOverdrive -= PlayerOverdriveoff;
    }
    public void Obtain(int value)
    {
        if (energy == MAX || available == false || !gameObject.activeSelf)
        {
            return;
        }
        energy = Mathf.Clamp(energy + value, 0, MAX);
        energyBar.UpdataStats(energy, MAX);
    }
    //自动获得能量
    IEnumerator AutoObtain()
    {
        while (true)
        {
            if (available)
            {
                Obtain(2);
            }
            yield return waitForSecondsObtain;
        }
    }
    /// <summary>
    /// 使用能量
    /// </summary>
    /// <param name="value"></param>
    public void Use(int value)
    {
        energy -= value;
        energyBar.UpdataStats(energy, MAX);
        if (energy == 0 && !available)
        {
            //关闭能量事件
            PlayerController.unOverdrive.Invoke();
        }
    }
    /// <summary>
    /// 能量是否够
    /// </summary>
    /// <param name="value">需要的能量</param>
    /// <returns></returns>
    public bool IsEnough(int value) => energy >= value;
    void PlayerOverdriveon()
    {
        available = false;
        StartCoroutine(nameof(KeepUsingCoroutine));
    }
    void PlayerOverdriveoff()
    {
        available = true;
        StopCoroutine(nameof(KeepUsingCoroutine));
    }
    /// <summary>
    /// 每隔几秒用多少
    /// </summary>
    /// <returns></returns>
    IEnumerator KeepUsingCoroutine()
    {
        while (energy > 0 && gameObject.activeSelf)
        {
            yield return waitForoverdriveInterval;
            Use(PERCENT);
        }
    }
}
