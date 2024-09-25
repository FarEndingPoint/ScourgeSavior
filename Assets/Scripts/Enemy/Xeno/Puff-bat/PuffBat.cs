using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuffBat : Xeno, PatrolMoveXeno
{
    [SerializeField] Transform _attackPoint;

    [SerializeField] float attackRadius;
    [SerializeField] float foundRadius;
    bool canHit;

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

    XenoCircleDetectPlayerCommand xenoCircleDetectPlayerCommand;
    XenoPatrolCommand xenoPatrolCommand;

    protected override void Awake()
    {
        base.Awake();
        xenoCircleDetectPlayerCommand = new XenoCircleDetectPlayerCommand(attackPoint, xenoModel.meetTimeToFoundPlayer, foundRadius);
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
        canHit = true;
    }

    protected override void AttackOnExit()
    {
        base.AttackOnExit();
        _patrolCenter = transform.position;
    }

    protected override bool DetectPlayer()
    {
        return this.SendCommand(xenoCircleDetectPlayerCommand);
    }

    protected override void Attack()
    {
        if (canHit && Physics2D.OverlapCircle(attackPoint.position, attackRadius, LayerMask.GetMask("Player")) != null)
        {
            canHit = false;
            TypeEventSystem.Global.Send<PlayerBeHitedEvent>();
        }
    }

    protected override void Move()
    {
        this.SendCommand(xenoPatrolCommand);
    }

    protected override void Stop()
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_attackPoint.position, attackRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_attackPoint.position, foundRadius);
    }
}
