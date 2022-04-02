using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{

    //生命
    //伤害
    //死亡
    [Header("Health")]
    [SerializeField] protected float maxhealth;
    [SerializeField] protected bool showOnHeadHealthBar;
    [SerializeField] dynamichealth onHeadHealthBar;
    [SerializeField] GameObject VFXDie;
    protected float health;
    private float midhealth;

    protected virtual void OnEnable()
    {
        health = maxhealth;
        if (showOnHeadHealthBar)
        {
            ShowOnHeadHealthBar();
        }
        else
        {
            HidOnHeadHealthBar();
        }
    }
    public void ShowOnHeadHealthBar()
    {
        onHeadHealthBar.gameObject.SetActive(true);
        onHeadHealthBar.Initialize(health, maxhealth);
    }
    public void HidOnHeadHealthBar()
    {
        if (onHeadHealthBar == null)
        {
            return;
        }
        onHeadHealthBar.gameObject.SetActive(false);


    }
    public virtual void Damage(int value)
    {
        if (health == 0)
        {
            return;
        }
        //midhealth=health;
        health -= value;
        if (showOnHeadHealthBar)
        {
            onHeadHealthBar.UpdataStats(health, maxhealth);
        }
        if (health <= 0)
        {
            Die();
        }
    }
    public virtual void Die()
    {
        health = 0;
        //播放死亡动画
        PoolManager.Release(VFXDie, transform.position);
        gameObject.SetActive(false);
    }
    public virtual void RestoreHealth(float value)
    {
        if (health == maxhealth)
        {
            return;
        }
        //防止以血量溢出    ---最大最小范围
        health = Mathf.Clamp(health + value, 0f, maxhealth);
        if (gameObject.activeSelf)
        {
            if (onHeadHealthBar == null)
            {
                return;
            }
            onHeadHealthBar.UpdataStats(health, maxhealth);
        }
    }
    //自动回复生命协程
    protected IEnumerator HealthRegenerateCoroutine(WaitForSeconds waitTime, float present)
    {
        while (health < maxhealth)
        {
            yield return waitTime;
            RestoreHealth(maxhealth * present);
        }
    }
}
