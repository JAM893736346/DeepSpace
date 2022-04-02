using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileGidestanceSystem : MonoBehaviour
{
    [SerializeField] Projectile projectile;
    [SerializeField] float minVallisitcAngle = -60;
    [SerializeField] float maxVallisitcAngle = 60f;
    float ballisiticAngle;
    public IEnumerator HomingCoroutine(GameObject target)
    {
        ballisiticAngle = Random.Range(minVallisitcAngle, maxVallisitcAngle);
        while (gameObject.activeSelf)
        {
            if (target.activeSelf)
            {
                Vector3 targectDirction = target.transform.position - transform.position;
                //返回其 Tan 为 y/x 的角度（以弧度为单位）。
                // 返回值是 X 轴与 2D 向量（从零开始，在 (x,y) 处终止）之间的 角度。
                print("进入循环");
                // 角度有问题
                var angle = Mathf.Atan2(targectDirction.y, targectDirction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.rotation *= Quaternion.Euler(0, 0, ballisiticAngle);
                //移动子弹
                projectile.Move();
            }
            else
            {
                projectile.Move();
            }
            yield return null;
        }
    }
}
