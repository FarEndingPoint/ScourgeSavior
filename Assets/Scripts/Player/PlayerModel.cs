using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

public class PlayerModel : AbstractModel
{
    public Vector2 inputVector;

    public int maxHp;
    public BindableProperty<int> hp;
    public float beHitedInvincibleTime;
    public float curBeHitedInvincibleTime;
    public float invincibleFlashCount;

    public float moveSpeed;
    public float moveSpeedWhenSlash;

    public Vector3 leftScale;
    public Vector3 rightScale;

    //跳跃相关
    public float commonGravityScale;
    public float airGravityScale;
    public float attackGravityScale;
    public float maxFirstJumpTime;
    public float maxSecondJumpTime;
    public bool isGround;
    public bool canSecondJump;
    public float commonJumpHeight;
    public float airJumpHeight;
    public float attackJumpHeight;

    //Slash相关
    public float slashTime;
    public float slashWindow;
    public float slashRadius;
    public bool isAttach;
    public float attachSpeed;
    public float slashHitTime;

    //Shoot相关
    public float explotionRadius;
    public float explotionHitTime;
    public int maxBulletsNum;
    public BindableProperty<int> curBulletsNum;
    public BindableProperty<float> bulletSlot;

    //Smash相关
    public float smashTime;
    public float smashWindow;
    public float smashRadius;
    public float smashHitTime;

    //Dash相关
    public float dashTime;
    public float dashWindow;
    public float dashSpeed;
    public float dashDragRadius;
    public float dashHitTime;

    //DragonPunch相关
    public float dragonPunchInputTime;
    public float curDragonPunchInputTime;
    public float dragonPunchTime;
    public float dragonPunchWindow;
    public float dragonPunchCD;
    public BindableProperty<float> curDragonPunchCD;
    public bool canDragonPunch;
    public float dragonPunchSpeed;
    public float dragonPunchDragRadius;
    public float dragonPunchSmashRadius;
    public float dragonPunchHitTime;

    //Sweep相关
    public float oneSweepTime;
    public float sweepSpeed;
    public float sweepRadius;
    public float sweepHitTime;

    //Fury相关
    public float furyTime;
    public float furyWindow;
    public float furyRadius;
    public BindableProperty<float> furySlot;
    public float furySlotDeltaOneAttack;

    //Damage
    public int slashDamage;
    public int shootDamage;
    public int chargeShootDamage;
    public int smashDamage;
    public int dashDamage;
    public int dragonPunchDamage;
    public int sweepDamage;
    public int furyDamage;

    protected override void OnInit()
    {
        
    }

    public void Init()
    {
        inputVector = Vector2.zero;

        maxHp = 30;
        hp = new BindableProperty<int>(maxHp);
        beHitedInvincibleTime = 1;
        curBeHitedInvincibleTime = 0;
        invincibleFlashCount = 2;

        leftScale = new Vector3(-1.8f, 1.8f, 1);
        rightScale = new Vector3(1.8f, 1.8f, 1);

        moveSpeed = 3;
        moveSpeedWhenSlash = 1.5f;

        commonGravityScale = 14;//无Slash、Smash和Sweep
        airGravityScale = 4;//Slash无命中敌人 or 有Smash
        attackGravityScale = 0;//Slash命中敌人 or 有Dash or 有Sweep
        maxFirstJumpTime = 0.6f;
        maxSecondJumpTime = 0.6f;
        isGround = false;
        canSecondJump = false;
        commonJumpHeight = 6f;
        airJumpHeight = 3.5f;
        attackJumpHeight = 1;

        slashTime = 0.3f;
        slashWindow = 0.15f;
        slashRadius = 0.5f;
        isAttach = false;//用来改变黏着敌人时的跳跃高度
        attachSpeed = 5;
        slashHitTime = 0.15f;

        explotionRadius = 2;
        explotionHitTime = 0.15f;
        maxBulletsNum = 20;
        curBulletsNum = new BindableProperty<int>(maxBulletsNum);
        bulletSlot = new BindableProperty<float>(0);

        smashTime = 0.4f;
        smashWindow = 0.25f;
        smashRadius = 0.8f;
        smashHitTime = 0.25f;

        dashTime = 0.3f;
        dashWindow = 0.15f;
        dashSpeed = 7;
        dashDragRadius = 0.5f;
        dashHitTime = 0.15f;

        dragonPunchInputTime = 0.07f;
        curDragonPunchInputTime = 0;
        dragonPunchTime = 0.4f;
        dragonPunchWindow = 0.2f;
        dragonPunchCD = 5;
        curDragonPunchCD = new BindableProperty<float>(0);
        canDragonPunch = true;
        dragonPunchSpeed = 7;
        dragonPunchDragRadius = 0.5f;
        dragonPunchSmashRadius = 0.8f;
        dragonPunchHitTime = 0.19f;

        oneSweepTime = 0.4f;
        sweepSpeed = 7;
        sweepRadius = 0.5f;
        sweepHitTime = 0.25f;

        furyTime = 3;
        furyWindow = 0.5f;
        furySlot = new BindableProperty<float>(0);
        furySlotDeltaOneAttack = 0.02f;

        slashDamage = 2;
        shootDamage = 2;
        chargeShootDamage = 8;
        smashDamage = 0;
        dashDamage = 1;
        dragonPunchDamage = 1;
        sweepDamage = 2;
        furyDamage = 20;
    }
}
