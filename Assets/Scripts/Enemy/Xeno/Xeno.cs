using QFramework;
using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(XenoModel))]
public abstract class Xeno : Enemy
{
    public enum State
    {
        Common,BeforeAttack,Attack,BeSmashed,BeCriticalSmashed,BeFuried,AfterFury
    };
    protected FSM<State> fsm = new FSM<State>();

    protected Transform attackPoint;
    [SerializeField] protected GameObject bullet;

    protected XenoModel xenoModel;
    
    protected XenoBeShootedCommand xenoBeShootedCommand;
    protected XenoBeSmashedCommand xenoBeSmashedCommand;
    protected XenoAttackCommand xenoAttackCommand;

    protected override void Awake()
    {
        base.Awake();
        attackPoint = transform.Find("AttackPoint");
        xenoModel = GetComponent<XenoModel>();
        xenoBeShootedCommand = new XenoBeShootedCommand(transform, rb, xenoModel);
        xenoBeSmashedCommand = new XenoBeSmashedCommand(fsm, rb, xenoModel);
        xenoAttackCommand = new XenoAttackCommand(xenoModel, fsm, Attack);
    }

    protected override void Start()
    {
        base.Start();
        fsm.State(State.Common).OnEnter(CommonOnEnter)
        .OnUpdate(CommonOnUpdate)
        .OnExit(CommonOnExit);
        fsm.State(State.BeforeAttack).OnEnter(BeforeAttackOnEnter)
        .OnUpdate(BeforeAttackOnUpdate)
        .OnExit(BeforeAttackOnExit);
        fsm.State(State.Attack).OnEnter(AttackOnEnter)
        .OnUpdate(AttackOnUpdate)
        .OnExit(AttackOnExit);
        fsm.State(State.BeSmashed).OnEnter(BeSmashedOnEnter)
        .OnUpdate(BeSmashedOnUpdate)
        .OnExit(BeSmashedOnExit);
        fsm.State(State.BeCriticalSmashed).OnEnter(BeCriticalSmashedOnEnter)
        .OnUpdate(BeCriticalSmashedOnUpdate)
        .OnExit(BeCriticalSmashedOnExit);
        fsm.State(State.BeFuried).OnEnter(BeFuriedOnEnter)
        .OnExit(BeFuriedOnExit);
        fsm.State(State.AfterFury).OnEnter(AfterFuryOnEnter)
        .OnUpdate(AfterFuryOnUpdate)
        .OnExit(AfterFuryOnExit);

        fsm.StartState(State.Common);
    }

    protected override void Update()
    {
        base.Update();
        fsm.Update();
        this.SendCommand(xenoBeShootedCommand);
    }

    public override void BeHited(BeHitedType hitType)
    {
        base.BeHited(hitType);
        if (hitType == BeHitedType.Smash || hitType == BeHitedType.DragonPunch)
        {
            if(fsm.CurrentStateId == State.BeforeAttack || fsm.CurrentStateId == State.Attack) 
                fsm.ChangeState(State.BeCriticalSmashed);
            else if(fsm.CurrentStateId != State.BeCriticalSmashed)
                fsm.ChangeState(State.BeSmashed);
        }
    }

    public virtual void BeShooted(bool isChargeShooted, Vector2 shootPoint)
    {
        xenoModel.beShootedPoint = shootPoint;
        xenoModel.beShootedOriginalPos = transform.Position2D();
        xenoModel.canMove = false;
        if (isChargeShooted)
            xenoModel.curBeChargeShootedTime = xenoModel.beChargeShootedTime;
        else
            xenoModel.curBeShootedTime = xenoModel.beShootedTime;
        Stop();
    }

    public override void BeFuried(bool isStart)
    {
        if (isStart) fsm.ChangeState(State.BeFuried);
        else fsm.ChangeState(State.AfterFury);
    }

    protected virtual void CommonOnEnter()
    {
        animator.SetBool("Common", true);
    }

    protected virtual void CommonOnUpdate()
    {
        if(xenoModel.canMove) Move();
        if (xenoModel.isFoundPlayer = DetectPlayer()) fsm.ChangeState(State.BeforeAttack);
    }

    protected virtual void CommonOnExit()
    {
        animator.SetBool("Common", false);
        Stop();
    }

    private void BeforeAttackOnEnter()
    {
        animator.SetBool("Attack", true);
        exclamation.SetActive(true);
        xenoModel.curBeforeAttackTime = xenoModel.beforeAttackTime;
    }

    private void BeforeAttackOnUpdate()
    {
        if (xenoModel.curBeforeAttackTime > 0) xenoModel.curBeforeAttackTime -= Time.deltaTime;
        else fsm.ChangeState(State.Attack);
    }

    private void BeforeAttackOnExit()
    {
        if (xenoModel.curBeforeAttackTime > 0)
        {
            animator.SetBool("Attack", false);
            exclamation.SetActive(false);
        }
    }

    protected virtual void AttackOnEnter()
    {
        xenoModel.curAttackTime = xenoModel.attackTime;
    }

    protected virtual void AttackOnUpdate()
    {
        this.SendCommand(xenoAttackCommand);
    }

    protected virtual void AttackOnExit()
    {
        animator.SetBool("Attack", false);
        exclamation.SetActive(false);
    }

    protected virtual void BeSmashedOnEnter()
    {
        animator.SetBool("BeSmashed", true);
        xenoModel.curBeSmashedTime = xenoModel.beSmashedTime;
        xenoModel.curBePushedTime = xenoModel.bePushedTime;
    }

    protected virtual void BeSmashedOnUpdate()
    {
        this.SendCommand(xenoBeSmashedCommand);
    }

    protected virtual void BeSmashedOnExit()
    {
        animator.SetBool("BeSmashed", false);
    }

    protected virtual void BeCriticalSmashedOnEnter()
    {
        animator.SetBool("BeSmashed", true);
        xenoModel.curBeSmashedTime = xenoModel.beCriticalSmashedTime;
        xenoModel.curBePushedTime = xenoModel.bePushedTime;
        smashedStar.SetActive(true);
    }

    protected virtual void BeCriticalSmashedOnUpdate()
    {
        this.SendCommand(xenoBeSmashedCommand);
    }

    protected virtual void BeCriticalSmashedOnExit()
    {
        animator.SetBool("BeSmashed", false);
        smashedStar.SetActive(false);
    }

    protected virtual void BeFuriedOnEnter()
    {
        if(animator != null) animator.speed = 0;
    }

    protected virtual void BeFuriedOnExit()
    {
        if (animator != null) animator.speed = 1;
        BeHited(BeHitedType.Fury);
    }

    protected virtual void AfterFuryOnEnter()
    {
        animator.SetBool("BeSmashed", true);
        xenoModel.curafterFuryTime = xenoModel.afterFuryTime;
    }

    protected virtual void AfterFuryOnUpdate()
    {
        if (xenoModel.curafterFuryTime > 0)
            xenoModel.curafterFuryTime -= Time.deltaTime;
        else fsm.ChangeState(State.Common);
    }

    protected virtual void AfterFuryOnExit()
    {
        animator.SetBool("BeSmashed", false);
    }

    protected virtual bool DetectPlayer()
    {
        return false;
    }

    protected virtual void Attack()//TODO:子类各自定义攻击行为
    {

    }

    protected virtual void Stop()//TODO:子类各自定义停止移动行为
    {

    }

    protected virtual void Move()//TODO:子类各自定义移动行为
    {

    }
}
