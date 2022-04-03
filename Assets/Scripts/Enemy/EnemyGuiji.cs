using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGuiji : Character
{
    [SerializeField] int NumFire;
    [SerializeField] GameObject hitVFX;
    [SerializeField] int angleFire;
    [SerializeField] int score;
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
        PlayerEnergy.Instance.Obtain(6);

        //播放特效；音效（可有可无）
        base.Die();
    }

    protected virtual void EnemyFire()
    {
        int midnum = NumFire / 2;
        for (int i = 0; i < NumFire; i++)
        {

            GameObject bullet = PoolManager.Release(EnemyProject, transform.position);
            EnemyProjectile2 Script = bullet.GetComponent<EnemyProjectile2>();
            if (NumFire % 2 == 1)
            {
                //奇数
                Script.moveDirction = Quaternion.AngleAxis(angleFire * (i - midnum), Vector3.forward) * dirction;
                // = (Quaternion.AngleAxis(bulletAngle * (i - median), Vector3.forward) * direction);
            }
            else
            {
                Script.moveDirction = Quaternion.AngleAxis(angleFire * (i - midnum) + angleFire / 2, Vector3.forward) * dirction;
                //bullet.GetComponent<PlayerProjectile>().SetSpeed(Quaternion.AngleAxis(bulletAngle * (i - median) + bulletAngle / 2, Vector3.forward) * direction);
            }
        }
    }
    protected void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Playerprojectile"))
        {
            var projectile = other.gameObject.GetComponent<Projectile>();
            int damage = projectile.Damage * 3;
            base.Damage(damage);
            other.gameObject.tag = "Enemyprojectile";
            projectile.MoveSpeed /= 2;
            PoolManager.Release(hitVFX, other.GetContact(0).point, Quaternion.identity);

            other.gameObject.SetActive(false);
        }
    }
    // Start is called before the first frame update

}


