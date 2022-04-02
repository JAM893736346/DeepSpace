using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBeizer : MonoBehaviour
{

    [SerializeField] int Damage;

    [SerializeField] GameObject hitVFX;
    void OnCollisionEnter2D(Collision2D other)
    {


        if (other.gameObject.CompareTag("Player"))
        {
            Character character = other.gameObject.GetComponent<Character>();

            character.Damage(Damage);
            //生成击中特效----------------------------接触点位置-------------接触点法线
            PoolManager.Release(hitVFX, other.GetContact(0).point, Quaternion.identity);
            gameObject.SetActive(false);
        }
    }
}