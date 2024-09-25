using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplatFly : Xeno
{
    [SerializeField] float attackRadius;
    XenoCircleDetectPlayerCommand xenoCircleDetectPlayerCommand;
    bool canHit;
    [HideInInspector] public bool isKillCount = true;

    protected override void Awake()
    {
        base.Awake();
        xenoCircleDetectPlayerCommand = new XenoCircleDetectPlayerCommand(attackPoint, xenoModel.meetTimeToFoundPlayer, attackRadius);
    }

    protected override void AttackOnEnter()
    {
        base.AttackOnEnter();
        canHit = true;
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
        if (!aiPath.canMove) aiPath.canMove = true;
        else transform.localScale = aidDestinationSetter.target.position.x >= transform.position.x ? xenoModel.rightScale : xenoModel.leftScale;
    }

    protected override void Stop()
    {
        aiPath.canMove = false;
    }

    public override void Death()
    {
        Destroy(gameObject);
        if (isKillCount) LevelController.Instance.KillEnemy();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
