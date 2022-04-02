using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyController
{
    //生成敌人要达到的地点
    public static List<Vector2> Enemyposition = new List<Vector2>{
     new Vector2(6.5f, 3),
     new Vector2(6.5f, 0),
     new Vector2(6.5f, -3),
     new Vector2(3.5f, -3),
     new Vector2(3.5f, 0),
     new Vector2(3.5f, 3)

    };
    public static int[] markpoint = new int[6] { 0, 1, 2, 3, 4, 5 };
    static Object locker = new Object();
    /// <summary>
    /// 获得一个随机点
    /// </summary>
    /// <param name="m">传入的索引</param>
    /// <returns></returns>
    public static Vector2 GetPoint(out int pointIndex)
    {
        pointIndex = -1;
        while (true)
        {
            // lock (locker)
            // {
            pointIndex = markpoint[Random.Range(0, markpoint.Length)];
            // }
            if (pointIndex >= 0)
            {
                markpoint[pointIndex] = -1;
                break;
            }
        }
        return Enemyposition[pointIndex];
    }

    /// <summary>
    /// 重置点的索引
    /// </summary>
    /// <param name="Index"></param>
    /// <returns></returns>
    public static int returnPoint(int Index)
    {
        markpoint[Index] = Index;
        Index = -1;
        return Index;
    }

}
