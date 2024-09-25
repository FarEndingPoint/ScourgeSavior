using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockPrism : Xeno
{
    XenoLineDetectPlayerCommand xenoLineDetectPlayerCommand;
    [SerializeField] float laserCenterDistance;

    protected override void Awake()
    {
        base.Awake();
        xenoLineDetectPlayerCommand = new XenoLineDetectPlayerCommand(transform, xenoModel.meetTimeToFoundPlayer);
    }

    protected override void AttackOnEnter()
    {
        base.AttackOnEnter();
        Vector3 dir = (PlayerController.Instance.transform.position - attackPoint.position).normalized;
        Vector3 pos = attackPoint.position + laserCenterDistance * dir;
        Instantiate(bullet, pos, Quaternion.identity).transform.right = dir;
    }

    protected override bool DetectPlayer()
    {
        return this.SendCommand(xenoLineDetectPlayerCommand);
    }

    protected override void Attack()
    {
        
    }

    protected override void Move()
    {
        
    }

    protected override void Stop()
    {
        
    }
}
