using System.Diagnostics;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using UnityEngine.EventSystems;
using UnityEngine.Events;


public class PlayerController : Character
{
    public AttackStyle styleDisplay;
    public static AttackStyle style;
    [SerializeField] float Speed;
    [SerializeField] float scoolSpeed;
    [SerializeField] float MoveTime;
    private float h;
    private float v;
    private Rigidbody2D rig;
    private new SkeletonMecanim animation;
    private Animator animator;
    float t;
    Vector2 playervelocity;
    [SerializeField] float paddingX;
    [SerializeField] float paddingY;
    private Spine.Bone leftShoulder, rightShoulder, neck, gun;
    public bool block = false;
    [Header("攻击")]
    //是否攻击
    public bool IsAttack = false;
    //攻击次数
    int ComboStep;
    [SerializeField] int meleedmage = 5;
    //补偿速度
    [SerializeField] float attackSpeed = 2f;
    [SerializeField] float timeinterval = 8f;
    [SerializeField] float shakeTime, sharkLength;
    [SerializeField] int attackPauseTime = 10;
    float time;
    [Header("Gun")]
    public Vector3 GunDirection;
    [SerializeField] GameObject Bullt;
    [SerializeField] GameObject overDriveBullt;
    [SerializeField] float FinreIntervile = 0.2f;
    private float timer;
    [SerializeField] int angleFire = 30;
    [SerializeField] int NumFire = 3;
    float angle, offset;
    [Header("HeathBar")]
    [SerializeField] PlayerHealth_BAr healthbar;
    [SerializeField] float healthRegenerateTime;
    [SerializeField, Range(0, 1)] float healthRegeneratePersent;
    WaitForSeconds waitforHealthRegenerateTime;
    Coroutine coroutine;
    //是否会自动回复生命
    [SerializeField] bool regenrateHealth = true;
    //[Header("节奏事件-射击击剑")]
    [SerializeField] Item[] items;
    [Header("翻滚")]
    //翻滚花费能量值
    public int RollCost = 25;
    public bool IsRoll = false;
    //时间放缓持续时间
    public float RollTimeScale = 0.5f;
    //翻滚时施加一个力
    public float ADDFORCE = 2;
    [SerializeField] GameObject RollVFX;

    [Header("终极大招")]
    //开启大招
    public static UnityAction onOverdrive = delegate { };
    // 结束大招
    public static UnityAction unOverdrive = delegate { };
    [SerializeField] float Speedrate = 1.2f;
    [SerializeField] float Rollrate = 2;
    [SerializeField] float KeppTimeScaletime = 1;
    bool Isoverdrive = false;
    [SerializeField] GameObject OverdriveVFX;
    [Header("音频")]
    [SerializeField] AudioData FireMic;
    [SerializeField] AudioData RollMic;
    [SerializeField] AudioData OverDriveMic;
    private void Awake()
    {
        showOnHeadHealthBar = false;
        style = AttackStyle.melee;
        GunDirection = new Vector3(1, 0, 0);
    }

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        animation = GetComponent<SkeletonMecanim>();
        neck = animation.skeleton.FindBone("neck");
        leftShoulder = animation.skeleton.FindBone("arm_upper_far");
        rightShoulder = animation.skeleton.FindBone("arm_upper_near");
        // if (style == AttackStyle.Gun)
        // {
        gun = animation.skeleton.FindBone("item_near");
        //}
        animation.UpdateLocal += HandleUpdateLocal;
        animator = GetComponent<Animator>();
        //初始化血条bar
        healthbar.Initialize(health, maxhealth);
        waitforHealthRegenerateTime = new WaitForSeconds(healthRegenerateTime);
        StartCoroutine(nameof(RollActions));
        StartCoroutine(nameof(OverDriveCoroutine));
    }
    void HandleUpdateLocal(ISkeletonAnimation skeletonRenderer)
    {
        if (!block)
        {
            MouseLook();

        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        styleDisplay = style;
        transform.position = Viewport.Instance.PlayerMoveablePosition(transform.position, paddingX, paddingY);
        Move();

        GunStyle();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        //为所有指针订阅事件
        for (int i = 0; i < items.Length; i++)
        {
            items[i].HitEvent += ReadyFire;
            items[i].HitEvent += ReadyMelee;
        }
        onOverdrive += OnOverDrive;
        unOverdrive += unOverDrive;
    }
    private void OnDisable()
    {
        for (int i = 0; i < items.Length; i++)
        {
            items[i].HitEvent -= ReadyFire;
            items[i].HitEvent -= ReadyMelee;
        }
        onOverdrive -= OnOverDrive;
        unOverdrive -= unOverDrive;
        StopAllCoroutines();
        //玩家死亡游戏结束
        GameManager.GameState = GameState.GameOver;
        GameOverManager.Gameoverevent.Invoke();
    }
    public override void Damage(int value)
    {
        base.Damage(value);
        healthbar.UpdataStats(health, maxhealth);
        if (gameObject.activeSelf)
        {
            if (regenrateHealth)
            {
                if (coroutine != null)
                {
                    StopCoroutine(coroutine);
                }
                coroutine = StartCoroutine(HealthRegenerateCoroutine(waitforHealthRegenerateTime, healthRegeneratePersent));
            }
        }
    }
    public override void RestoreHealth(float value)
    {
        base.RestoreHealth(value);
        healthbar.UpdataStats(health, maxhealth);
    }
    public override void Die()
    {
        healthbar.UpdataStats(0, maxhealth);
        base.Die();
    }
    void Move()
    {

        v = Input.GetAxisRaw("Vertical");
        h = Input.GetAxisRaw("Horizontal");
        Vector2 target = new Vector2(h, v) * Speed;
        if (!IsAttack)
        {
            animator.SetBool("idle", false);
            animator.SetBool("run", true);
            rig.velocity = target;
        }
        else
        {
            rig.velocity = new Vector2(transform.localScale.x * attackSpeed, rig.velocity.y);
        }
        if (rig.velocity == Vector2.zero)
        {
            animator.SetBool("idle", true);
            animator.SetBool("run", false);
        }

    }
    #region 攻击方式加状态
    void GunStyle()
    {
        switch (style)
        {
            case AttackStyle.Noweapon:
                EmpeyWeapon();
                break;
            case AttackStyle.melee:
                meleeAttack();
                break;
            case AttackStyle.Gun:
                GunWeappon();
                break;
        }
    }
    void EmpeyWeapon()
    {
        //设置层级权重
        block = true;
        animator.SetLayerWeight(1, 0f);
        animator.SetLayerWeight(2, 0f);
    }
    /// <summary>
    /// 光剑攻击状态
    /// </summary>
    void meleeAttack()
    {
        block = true;
        animator.SetLayerWeight(1, 0.7f);
        animator.SetLayerWeight(2, 0);
        // if (Input.GetMouseButton(0) && !IsAttack)
        // {
        //     print(4);
        //     IsAttack = true;
        //     ComboStep++;
        //     if (ComboStep > 3)
        //     {
        //         ComboStep = 1;
        //     }
        //     time = timeinterval;
        //     animator.SetTrigger("Isattack");
        //     animator.SetInteger("meleeAttck", ComboStep);
        // }
        //重置攻击次数
        if (time != 0)
        {
            time -= Time.deltaTime;
            if (time <= 0)
            {
                time = 0;
                ComboStep = 0;
            }
        }
    }
    //光剑订阅事件
    public void ReadyMelee()
    {
        if (style == AttackStyle.Gun)
        {
            return;
        }
        //print(4);
        IsAttack = true;
        ComboStep++;
        if (ComboStep > 3)
        {
            ComboStep = 1;
        }
        time = timeinterval;
        animator.SetTrigger("Isattack");
        animator.SetInteger("meleeAttck", ComboStep);
    }
    public void AttackOver()
    {
        IsAttack = false;
    }
    /// <summary>
    /// Gun武器形态
    /// </summary>
    void GunWeappon()
    {
        IsAttack = false;
        animator.SetLayerWeight(1, 0f);
        animator.SetLayerWeight(2, 0.9f);
        block = true;
        if (timer != 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = 0;
            }
        }
        // if (Input.GetMouseButton(0))
        // {
        //     if (EventSystem.current.IsPointerOverGameObject()) return;
        //     if (timer == 0)
        //     {
        //         timer = FinreIntervile;
        //         Fire();
        //     }
        // }
    }
    //射击事件
    public void ReadyFire()
    {
        if (style == AttackStyle.melee)
        {
            return;
        }
        if (timer == 0)
        {
            timer = FinreIntervile;
            float anglefire = Vector2.Angle(leftShoulder.GetLocalPosition(), gun.GetLocalPosition());
            gun.Rotation += angleFire;
            Fire();
        }
    }

    void Fire()
    {
        //播放音频
        //切换状态
        IsAttack = false;
        animator.SetTrigger("Fire");
        int midnum = NumFire / 2;
        for (int i = 0; i < NumFire; i++)
        {
            GameObject bullet = PoolManager.Release(Isoverdrive ? overDriveBullt : Bullt, gun.GetWorldPosition(transform) + GunDirection.normalized * 0.9f);
            if (Isoverdrive)
            {
                continue;
            }
            var Script = bullet.GetComponent<PlayerProjectile>();
            if (NumFire % 2 == 1)
            {
                //奇数
                Script.moveDirction = Quaternion.AngleAxis(angleFire * (i - midnum), Vector3.forward) * GunDirection.normalized;
                // = (Quaternion.AngleAxis(bulletAngle * (i - median), Vector3.forward) * direction);
            }
            else
            {
                Script.moveDirction = Quaternion.AngleAxis(angleFire * (i - midnum) + angleFire / 2, Vector3.forward) * GunDirection.normalized;
                //bullet.GetComponent<PlayerProjectile>().SetSpeed(Quaternion.AngleAxis(bulletAngle * (i - median) + bulletAngle / 2, Vector3.forward) * direction);
            }
        }
        AudioManager.Instance.PlayRandomSFX(FireMic);
    }
    /// <summary>
    /// 拿到武器时朝鼠标看
    /// </summary>
    void MouseLook()
    {
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
        Vector3 lookPos = Camera.main.ScreenToWorldPoint(mousePos);
        if (lookPos.x < transform.position.x)
        {
            lookPos.x = lookPos.x + (transform.position.x - lookPos.x) * 2;
        }
        //鼠标朝向始终在玩家前面
        lookPos = lookPos - transform.position;
        GunDirection = new Vector3(lookPos.x, lookPos.y, 0);
        //转向的角度
        angle = Mathf.Atan2(lookPos.y, lookPos.x) * Mathf.Rad2Deg;
        offset = angle;
        if (angle > 90)
            angle = 90;
        if (angle < -30)
            angle = -30;
        if (angle > 10 || angle < 0)
            offset = angle * 1.2f;
        angle -= 12;
        //骨骼转向
        neck.Rotation = neck.Rotation + angle;
        leftShoulder.Rotation = leftShoulder.Rotation + angle + offset;
        // if (Input.GetMouseButton(0))
        // {
        //     if (EventSystem.current.IsPointerOverGameObject()) return;
        //     float anglefire = Vector2.Angle(leftShoulder.GetLocalPosition(), gun.GetLocalPosition());
        //     gun.Rotation += angleFire;
        // }
        rightShoulder.Rotation = rightShoulder.Rotation + angle;
    }

    #endregion

    #region 翻滚
    IEnumerator RollActions()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.K) && PlayerEnergy.Instance.IsEnough(RollCost) && !IsRoll)
            {
                //print("进入翻滚");
                IsRoll = true;
                AudioManager.Instance.PlayRandomSFX(RollMic);
                PlayerEnergy.Instance.Use(RollCost);
                rig.velocity += new Vector2(h, v) * ADDFORCE;
                TimeController.Instance.BulletTime(RollTimeScale, RollTimeScale);
                animator.Play("roll");
                CapsuleCollider2D collider = GetComponent<CapsuleCollider2D>();
                collider.enabled = false;
                RollVFX.SetActive(true);
                yield return new WaitForSeconds(1);
                collider.enabled = true;
                RollVFX.SetActive(false);
            }
            yield return null;
        }
    }
    //动画帧事件防止重复触发
    public void IsRollTrue()
    {
        IsRoll = false;
    }
    #endregion

    #region 终极技能
    //清空全场子弹，子弹自动跟踪，伤害增加，(子弹样式改变)叠加一层 音乐
    IEnumerator OverDriveCoroutine()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.L) && PlayerEnergy.Instance.IsEnough(PlayerEnergy.MAX))
            {
                //播放特效
                AudioManager.Instance.PlayRandomSFX(OverDriveMic);
                //屏幕震动
                AttackScense.Instance.CameraShake(0.2f, 0.2f);
                onOverdrive.Invoke();

            }
            yield return null;
        }
    }
    //大招开启时间
    void OnOverDrive()
    {
        OverdriveVFX.SetActive(true);
        Isoverdrive = true;
        Speed *= Speedrate;
        RollCost *= (int)Rollrate;
        //时间特写
        TimeController.Instance.SlowKeepTime(RollTimeScale, KeppTimeScaletime, RollTimeScale);
    }
    //大招关闭
    void unOverDrive()
    {
        OverdriveVFX.SetActive(false);
        Isoverdrive = false;
        Speed /= Speedrate;
        RollCost /= (int)Rollrate;
    }
    #endregion
    private void OnTriggerEnter2D(Collider2D other)
    {

        //光剑击中敌人
        if (other.CompareTag("Enemy") && other.GetType().ToString() == "UnityEngine.CircleCollider2D" && style == AttackStyle.melee)
        {
            if (other.gameObject.TryGetComponent<Character>(out Character character))
            {
                character.Damage(meleedmage);
                AttackScense.Instance.HitPause(attackPauseTime);
                if (ComboStep == 3)
                {
                    AttackScense.Instance.CameraShake(shakeTime, sharkLength);
                }
            }
        }
        else if (other.CompareTag("Enemyprojectile") && style == AttackStyle.melee)
        {
            GameObject projectile = other.gameObject;
            var transfo = projectile.TryGetComponent<Projectile>(out Projectile Enemyprojectile);

            //var dir = transfo.moveDirction;
            //float speed = transfo.MoveSpeed;
            //transfo.moveDirction = -dir;
            //transfo.MoveSpeed *= 2;
            if (transfo)
            {
                Vector3 dir = Enemyprojectile.moveDirction;
                float speed = Enemyprojectile.MoveSpeed;
                dir = Enemyprojectile.moveDirction;
                speed = Enemyprojectile.MoveSpeed;
                Enemyprojectile.moveDirction = -dir;
                Enemyprojectile.MoveSpeed *= 2;
                projectile.tag = "Playerprojectile";
            }

        }
    }
    public enum AttackStyle
    {
        Noweapon,
        melee,
        Gun
    }
}
