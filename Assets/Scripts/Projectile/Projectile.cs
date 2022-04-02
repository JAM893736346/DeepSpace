using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]public float MoveSpeed = 10f;

    [SerializeField] public int Damage;
    public Vector3 moveDirction ;
    //碰撞特效
    [SerializeField] GameObject hitVFX;

    protected GameObject target;
    protected virtual void OnEnable()
    {
        StartCoroutine(nameof(MoveDirectly));
    }
    IEnumerator MoveDirectly()
    {
        while (gameObject.activeSelf)
        {
            Move();
            yield return null;
        }
    }
    public void Move() => transform.Translate(moveDirction * MoveSpeed * Time.deltaTime);
    protected virtual void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent<Character>(out Character character))
        {
            character.Damage(Damage);
            //生成击中特效----------------------------接触点位置-------------接触点法线
            PoolManager.Release(hitVFX, other.GetContact(0).point, Quaternion.identity);
            gameObject.SetActive(false);
        }
    }
    protected void SetTarget(GameObject target)
    {
        this.target = target;
    }
}
