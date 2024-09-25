using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

public class ToxicWasp : Xeno
{
    XenoLineDetectPlayerCommand xenoLineDetectPlayerCommand;

    protected override void Awake()
    {
        base.Awake();
        xenoLineDetectPlayerCommand = new XenoLineDetectPlayerCommand(transform, xenoModel.meetTimeToFoundPlayer);
    }

    protected override void AttackOnEnter()
    {
        base.AttackOnEnter();
        GameObject go = Instantiate(bullet, attackPoint.position, Quaternion.identity);
        go.transform.right = (PlayerController.Instance.transform.position - go.transform.position).normalized;
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
