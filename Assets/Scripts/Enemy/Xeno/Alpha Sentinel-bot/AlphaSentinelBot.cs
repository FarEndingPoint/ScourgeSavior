using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

public class AlphaSentinelBot : Xeno, PatrolMoveXeno
{
    [SerializeField] float shootTime;
    float curShootTime;
    [SerializeField] float realAttackTime;
    int attackCount;
    bool canAttack;
    Vector3 attackDirection;

    [SerializeField] float _patrolRadius;
    [SerializeField] float _patrolSpeed;
    [SerializeField] float _patrolTime;
    Vector3 _patrolPoint;
    float _curPatrolTime;
    Vector3 _patrolCenter;
    public float patrolRadius => _patrolRadius;
    public float patrolSpeed => _patrolSpeed;
    public float patrolTime => _patrolTime;
    public Vector3 patrolPoint { get { return _patrolPoint; } set { _patrolPoint = value; } }
    public float curPatrolTime { get { return _curPatrolTime; } set { _curPatrolTime = value; } }
    public Vector3 patrolCenter { get { return _patrolCenter; } set { _patrolCenter = value; } }

    XenoLineDetectPlayerCommand xenoLineDetectPlayerCommand;
    XenoPatrolCommand xenoPatrolCommand;

    protected override void Awake()
    {
        base.Awake();
        xenoLineDetectPlayerCommand = new XenoLineDetectPlayerCommand(transform, xenoModel.meetTimeToFoundPlayer);
        xenoPatrolCommand = new XenoPatrolCommand(this, transform, xenoModel);
    }

    protected override void CommonOnEnter()
    {
        base.CommonOnEnter();
        xenoPatrolCommand.StartPartrol();
    }

    protected override void AttackOnEnter()
    {
        base.AttackOnEnter();
        curShootTime = shootTime;
        attackCount = 1;
        attackDirection = (PlayerController.Instance.transform.position - attackPoint.position).normalized;
        canAttack = true;
    }

    protected override void AttackOnExit()
    {
        base.AttackOnExit();
        _patrolCenter = transform.position;
    }

    protected override bool DetectPlayer()
    {
        return this.SendCommand(xenoLineDetectPlayerCommand);
    }

    protected override void Attack()
    {
        if (attackCount <= 2)
        {
            if (curShootTime > 0)
            {
                curShootTime -= Time.deltaTime;
                if (canAttack && curShootTime <= shootTime - realAttackTime)
                {
                    canAttack = false;
                    Instantiate(bullet, attackPoint.position, Quaternion.identity).transform.right = attackDirection;
                }
            }
            else
            {
                canAttack = true;
                curShootTime = shootTime;
                attackCount++;
                attackDirection = (PlayerController.Instance.transform.position - attackPoint.position).normalized;
            }
        }
    }

    protected override void Move()
    {
        this.SendCommand(xenoPatrolCommand);
    }

    protected override void Stop()
    {

    }
}
