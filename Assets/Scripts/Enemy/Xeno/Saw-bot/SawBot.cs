using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

public class SawBot : Xeno
{
    XenoLineDetectPlayerCommand xenoLineDetectPlayerCommand;

    [SerializeField] Transform _attackPoint;

    bool canAttack;
    [SerializeField] float dashSpeed;
    Vector3 dashDirection;
    [SerializeField] float attackRadius;

    protected override void Awake()
    {
        base.Awake();
        xenoLineDetectPlayerCommand = new XenoLineDetectPlayerCommand(transform, xenoModel.meetTimeToFoundPlayer);
    }

    protected override bool DetectPlayer()
    {
        return this.SendCommand(xenoLineDetectPlayerCommand);
    }

    protected override void AttackOnEnter()
    {
        base.AttackOnEnter();
        canAttack = true;
        dashDirection = (PlayerController.Instance.transform.position - attackPoint.position).normalized;
    }

    protected override void Attack()
    {
        transform.Translate(dashDirection * dashSpeed * Time.deltaTime);
        if (canAttack && Physics2D.OverlapCircle(attackPoint.position, attackRadius, LayerMask.GetMask("Player")) != null)
        {
            canAttack = false;
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

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_attackPoint.position, attackRadius);
    }
}
