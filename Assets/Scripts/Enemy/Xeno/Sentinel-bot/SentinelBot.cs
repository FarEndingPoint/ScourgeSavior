using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

public class SentinelBot : Xeno, PatrolMoveXeno
{
    [SerializeField] float dashTime;
    float curDashTime;
    [SerializeField] float dashSpeed;
    [SerializeField] float realAttackTime;
    int attackCount;
    bool canAttack;
    Vector3 dashDirection;

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
        curDashTime = dashTime;
        attackCount = 1;
        dashDirection = (PlayerController.Instance.transform.position - attackPoint.position).normalized;
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
        if(attackCount <= 2)
        {
            if (curDashTime > 0)
            {
                curDashTime -= Time.deltaTime;
                transform.Translate(dashDirection * dashSpeed * Time.deltaTime);
                if (canAttack && curDashTime <= dashTime - realAttackTime)
                {
                    canAttack = false;
                    Instantiate(bullet, attackPoint.position, Quaternion.identity).transform.right = dashDirection;
                }
            }
            else
            {
                canAttack = true;
                curDashTime = dashTime;
                attackCount++;
                dashDirection = (PlayerController.Instance.transform.position - attackPoint.position).normalized;
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
