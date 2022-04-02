using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBeiz : Character
{
    [SerializeField] int NumFire;


    GameObject bullet;

    [SerializeField] int score;
    [SerializeField] float minTime = 1;
    [SerializeField] float maxTime = 1.5f;
    [SerializeField] GameObject EnemyProject;

    [SerializeField] float onceFiretimeinterrival = 0.5f;
    [SerializeField] int sumonceFire = 5;
    [SerializeField] float PaddingX;
    [SerializeField] float PaddingY;
    Collider2D collision2D;
    float time;
    Vector2 targetposition = Vector2.zero;
    int pointindex = -1;
    public Vector2 targetPosition;
    //出生移动速度
    [SerializeField] float MoveSpeed = 6;
    public bool Isarrive = false;

    bool IsRandomMove;

    private void Awake()
    {
        collision2D = GetComponent<Collider2D>();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        // targetposition = EnemyController.GetPoint(out pointindex);
        StartCoroutine(nameof(FireCoroutine));
        // StartCoroutine(nameof(Move));
        StartCoroutine(nameof(movetargect));
        StartCoroutine(nameof(IsmoveTargectCoroutine));
    }
    private void Start()
    {
        targetposition = EnemyController.GetPoint(out pointindex);
    }
    private void Update()
    {
        collision2D.enabled = Isarrive;
        if (time != 0)
        {
            time -= Time.deltaTime;
            if (time <= 0)
            {
                time = 0;
            }
        }
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    IEnumerator FireCoroutine()
    {
        while (gameObject.activeSelf)
        {
            yield return new WaitUntil(() => { return Isarrive; });
            float intervile = Random.Range(minTime, maxTime);
            yield return new WaitForSeconds(intervile);
            for (int i = 0; i < sumonceFire; i++)
            {
                EnemyFire();
                yield return new WaitForSeconds(onceFiretimeinterrival);
            }
        }

    }
    void EnemyFire()
    {
        float angle = 360 / NumFire;
        int i = 0;

        while (i < NumFire)
        {
            for (i = 0; i < NumFire; i++)
            {
                bullet = PoolManager.Release(EnemyProject, transform.position);
                var Script = bullet.GetComponent<Transform>();
                //Script.rotation = Quaternion.Euler(Vector3.forward * i * angle);
                Script.rotation = Quaternion.Euler(0, 0, angle * i);
            }

        }
    }
    IEnumerator Move()
    {
         targetPosition = Viewport.Instance.RandomRightHalfPosition(PaddingX, PaddingY);

        while (gameObject.activeSelf)
        {
           while  (Vector3.Distance(transform.position, targetPosition) >= Mathf.Epsilon)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.fixedDeltaTime);
            } 
             targetPosition = Viewport.Instance.RandomRightHalfPosition(PaddingX, PaddingX);
            yield return new WaitForSeconds (5.0f);
        }
    }

    IEnumerator movetargect()
    {
        while (Vector2.Distance(transform.position, targetposition) >= Mathf.Epsilon && gameObject.activeSelf)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetposition, MoveSpeed * Time.deltaTime);
            yield return null;
        }
    }
    IEnumerator IsmoveTargectCoroutine()
    {
        while (gameObject.activeSelf)
        {
            if (IsRandomMove == false)
            {
                Isarrive = IsmoveTargect();
                if (Isarrive)
                {
                    print(7876);
                    StartCoroutine(nameof(Move));
                    IsRandomMove = true;
                }
            }
            yield return null;
        }
    }
    public bool IsmoveTargect() => Vector2.Distance(transform.position, targetposition) <= Mathf.Epsilon;
    public override void Die()
    {
        ScoreManager.Instance.AddScore(score);
        EnemyManager.Instance.RemoveFeomList(gameObject);
        pointindex = EnemyController.returnPoint(pointindex);

        //播放特效；音效（可有可无）
        base.Die();
    }
}

