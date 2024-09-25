using Pathfinding;
using QFramework;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Animator), typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D), typeof(AIDestinationSetter), typeof(AIPath))]
public abstract class Enemy : MonoBehaviour, IController
{
    public enum BeHitedType
    {
        Slash,Shoot,ChargeShoot,Smash,Dash,DragonPunch,Sweep,Fury
    }
    BeHitedType beHitedType;

    protected SpriteRenderer spriteRenderer;
    protected Animator animator;
    protected Rigidbody2D rb;
    new protected BoxCollider2D collider;
    protected AIPath aiPath;
    protected AIDestinationSetter aidDestinationSetter;
    protected GameObject smashedStar;
    protected GameObject exclamation;

    EnemyModel enemyModel;
    PlayerModel playerModel;

    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        collider = GetComponent<BoxCollider2D>();
        aiPath = GetComponent<AIPath>();
        aidDestinationSetter = GetComponent<AIDestinationSetter>();
        enemyModel = GetComponent<EnemyModel>();
        playerModel = this.GetModel<PlayerModel>();
        smashedStar = transform.Find("SmashedStar").gameObject;
        exclamation = transform.Find("Exclamation").gameObject;
    }

    protected virtual void Start()
    {
        aidDestinationSetter.target = PlayerController.Instance.transform;
    }

    protected virtual void Update()
    {
        BeHitedDisplay();
    }

    public virtual void BeHited(BeHitedType beHitedType)//受击判断
    {
        Damage(beHitedType);
        if (beHitedType == BeHitedType.Slash || beHitedType == BeHitedType.Sweep) enemyModel.isTrun = !enemyModel.isTrun;
        enemyModel.curBeHitedDisplayTime = enemyModel.beHitedDisplayTime;
        this.beHitedType = beHitedType;
    }

    void BeHitedDisplay()//受击反馈：变颜色
    {
        if (enemyModel.curBeHitedDisplayTime > 0)
        {
            enemyModel.curBeHitedDisplayTime -= Time.deltaTime;
            switch (beHitedType)
            {
                case BeHitedType.Slash:
                    spriteRenderer.color = enemyModel.isTrun ? EnemyModel.beHited1 : EnemyModel.beHited2;
                    break;
                case BeHitedType.Shoot:
                    spriteRenderer.color = EnemyModel.beHited2;
                    break;
                case BeHitedType.ChargeShoot:
                    spriteRenderer.color = EnemyModel.beHited2;
                    break;
                case BeHitedType.Smash:
                    spriteRenderer.color = EnemyModel.beHited1;
                    break;
                case BeHitedType.Dash:
                    spriteRenderer.color = EnemyModel.beHited1;
                    break;
                case BeHitedType.DragonPunch:
                    spriteRenderer.color = EnemyModel.beHited1;
                    break;
                case BeHitedType.Sweep:
                    spriteRenderer.color = enemyModel.isTrun ? EnemyModel.beHited1 : EnemyModel.beHited2;
                    break;
                case BeHitedType.Fury:
                    break;
            }        
        }
        else spriteRenderer.color = EnemyModel.common;
    }

    void Damage(BeHitedType beHitedType)
    {
        switch(beHitedType) 
        {
            case BeHitedType.Slash:
                enemyModel._hp.Value -= playerModel.slashDamage;
                break;
            case BeHitedType.Shoot:
                enemyModel._hp.Value -= playerModel.shootDamage;
                break;
            case BeHitedType.ChargeShoot:
                enemyModel._hp.Value -= playerModel.chargeShootDamage;
                break;
            case BeHitedType.Smash:
                enemyModel._hp.Value -= playerModel.smashDamage;
                break;
            case BeHitedType.Dash:
                enemyModel._hp.Value -= playerModel.dashDamage;
                break;
            case BeHitedType.DragonPunch:
                enemyModel._hp.Value -= playerModel.dragonPunchDamage;
                break;
            case BeHitedType.Sweep:
                enemyModel._hp.Value -= playerModel.sweepDamage;
                break;
            case BeHitedType.Fury:
                enemyModel._hp.Value -= playerModel.furyDamage;
                break;
        }
    }

    public void BePushed(Vector2 bePushedDestination)
    {
        enemyModel.bePushedDestination = bePushedDestination;
        enemyModel.bePushedOriginalPos = transform.Position2D();
    }

    public void BeDraged(Transform player, int speed)
    {
        if (enemyModel.canBeDraged)
            transform.Translate((player.Position2D() - transform.Position2D()) * Time.deltaTime * speed);
    }

    public virtual void BeFuried(bool isStart)
    {

    }

    public virtual void Death()
    {
        Destroy(gameObject);
        LevelController.Instance.KillEnemy();
    }

    public IArchitecture GetArchitecture()
    {
        return Main.Interface;
    }
}
