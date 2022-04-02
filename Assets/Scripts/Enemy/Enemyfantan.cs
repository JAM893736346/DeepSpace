using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemyfantan : Character 
{
  
      [SerializeField] int score;
        [SerializeField]GameObject hitVFX;
    [SerializeField] float minTime = 1;
    [SerializeField] float maxTime = 1.5f;
    [SerializeField] GameObject EnemyProject;
    [SerializeField] Vector2 dirction;
    [SerializeField] float onceFiretimeinterrival = 0.5f;
    [SerializeField] int sumonceFire = 5;
    Collider2D collision2D;
    float time;
    Vector2 targetposition = Vector2.zero;
    int pointindex = -1;
    //出生移动速度
    [SerializeField] float MoveSpeed = 6;
    public bool Isarrive = false;

    private void Awake()
    {
        collision2D = GetComponent<Collider2D>();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        // targetposition = EnemyController.GetPoint(out pointindex);
        StartCoroutine(nameof(FireCoroutine));
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
         PoolManager.Release(EnemyProject,transform .position,Quaternion .Euler(0,0,60));
        PoolManager.Release(EnemyProject, transform .position, Quaternion.Euler(0, 0, -60));

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
            Isarrive = IsmoveTargect();
            //print(Isarrive);
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
      protected void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Playerprojectile"))
        {
            var projectile = other.gameObject.GetComponent<Projectile>();
           int  damage = projectile.Damage * 2;
            base.Damage(damage);
            other.gameObject.tag = "Enemyprojectile";
            projectile.MoveSpeed /= 2;
            PoolManager.Release(hitVFX, other.GetContact(0).point, Quaternion.identity);
            
            other.gameObject.SetActive(false);
        }
    }
 }
 // Start is called before the first frame update
 

