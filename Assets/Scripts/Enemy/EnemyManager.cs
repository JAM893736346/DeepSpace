using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public class EnemyManager : Singleton<EnemyManager>
{
    public GameObject RandomEnemy => enemyList.Count == 0 ? null : enemyList[Random.Range(0, enemyList.Count)];
    public int WaveNumber => waveNumber;
    public float TimeBetweenWaves => timeBetweenSpawn;
    [SerializeField] GameObject[] enemyPrefab;
    [SerializeField] bool SpanEnemy = true;
    [SerializeField] float timeBetweenSpawn = 2f;

    [SerializeField] ItemManager itemManager;


    [SerializeField] float waitBetweenWaves = 3;
    WaitForSeconds waitTimeBetweenWaves;
    public List<GameObject> enemyList = new List<GameObject> { };
    public int waveNumInEnemy => waveNumber;
    int waveNumber = 0;
    int enemyAmount;
    WaitForSeconds waitForSeconds;
    WaitUntil waitUntil;
    [SerializeField] GameObject BossComingUI;
    protected override void Awake()
    {
        base.Awake();
        waitForSeconds = new WaitForSeconds(timeBetweenSpawn);
        waitTimeBetweenWaves = new WaitForSeconds(waitBetweenWaves);
        waitUntil = new WaitUntil(() => { return enemyList.Count == 0; });
    }
    IEnumerator Start()
    {
        while (SpanEnemy)
        {
            yield return waitUntil;
            if (waveNumber > 9)
            {
                GameManager.GameState = GameState.Scoring;
                GameWinManager.GameWinevent.Invoke();
            }
            yield return waitTimeBetweenWaves;
            yield return StartCoroutine(nameof(RandomlySpawnCoroutine));
        }

    }
    IEnumerator RandomlySpawnCoroutine()
    {
        switch (waveNumber)
        {
            case 1:
                enemyList.Add(PoolManager.Release(enemyPrefab[0], transform.position));
                yield return waitForSeconds;
                break;
            case 2:
                enemyList.Add(PoolManager.Release(enemyPrefab[0], transform.position));
                yield return waitForSeconds;
                enemyList.Add(PoolManager.Release(enemyPrefab[1], transform.position));
                yield return waitForSeconds;
                break;
            case 3:
                enemyList.Add(PoolManager.Release(enemyPrefab[1], transform.position));
                yield return waitForSeconds;
                enemyList.Add(PoolManager.Release(enemyPrefab[2], transform.position));
                yield return waitForSeconds;
                break;
            case 4:
                enemyList.Add(PoolManager.Release(enemyPrefab[2], transform.position));
                yield return waitForSeconds;
                enemyList.Add(PoolManager.Release(enemyPrefab[3], transform.position));
                yield return waitForSeconds;
                break;
            case 5:
                enemyList.Add(PoolManager.Release(enemyPrefab[4], transform.position));
                yield return waitForSeconds;
                enemyList.Add(PoolManager.Release(enemyPrefab[1], transform.position));
                yield return waitForSeconds;
                break;
            case 6:
                enemyList.Add(PoolManager.Release(enemyPrefab[4], transform.position));
                yield return waitForSeconds;
                enemyList.Add(PoolManager.Release(enemyPrefab[3], transform.position));
                break;
            case 7:
                enemyList.Add(PoolManager.Release(enemyPrefab[1], transform.position));
                yield return waitForSeconds;
                enemyList.Add(PoolManager.Release(enemyPrefab[4], transform.position));
                yield return waitForSeconds;
                enemyList.Add(PoolManager.Release(enemyPrefab[3], transform.position));
                break;
            case 8:
                enemyList.Add(PoolManager.Release(enemyPrefab[1], transform.position));
                yield return waitForSeconds;
                enemyList.Add(PoolManager.Release(enemyPrefab[2], transform.position));
                yield return waitForSeconds;
                enemyList.Add(PoolManager.Release(enemyPrefab[3], transform.position));
                yield return waitForSeconds;
                enemyList.Add(PoolManager.Release(enemyPrefab[4], transform.position));
                break;
            case 9:
                //屏幕震动
                BossComingUI.SetActive(true);
                AttackScense.Instance.CameraShake(2, 0.5f);
                yield return new WaitForSeconds(6);
                BossComingUI.SetActive(false);

                itemManager.InitDatamic(2);
                enemyList.Add(PoolManager.Release(enemyPrefab[0], transform.position));
                yield return waitForSeconds;
                enemyList.Add(PoolManager.Release(enemyPrefab[0], transform.position));
                yield return new WaitForSeconds(5);
                enemyList.Add(PoolManager.Release(enemyPrefab[1], transform.position));
                yield return new WaitForSeconds(6);
                enemyList.Add(PoolManager.Release(enemyPrefab[2], transform.position));
                yield return new WaitForSeconds(10f);
                enemyList.Add(PoolManager.Release(enemyPrefab[3], transform.position));
                yield return new WaitForSeconds(10f);
                enemyList.Add(PoolManager.Release(enemyPrefab[4], transform.position));
                yield return new WaitForSeconds(14);
                enemyList.Add(PoolManager.Release(enemyPrefab[5], transform.position));
                yield return new WaitForSeconds(5);
                enemyList.Add(PoolManager.Release(enemyPrefab[4], transform.position));
                break;
            default:

                break;
        }
        waveNumber++;
        enemyAmount = waveNumber;
    }
    public void RemoveFeomList(GameObject enemy) => enemyList.Remove(enemy);
}
