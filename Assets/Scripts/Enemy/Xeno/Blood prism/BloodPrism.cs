using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

public class BloodPrism : Xeno
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
        Vector3 pos = attackPoint.position + laserCenterDistance * (new Vector3(1, 1, 0)).normalized;
        Instantiate(bullet, pos, Quaternion.identity).transform.right = (new Vector3(1, 1, 0)).normalized;
        pos = attackPoint.position + laserCenterDistance * (new Vector3(-1, 1, 0)).normalized;
        Instantiate(bullet, pos, Quaternion.identity).transform.right = (new Vector3(-1, 1, 0)).normalized;
        pos = attackPoint.position + laserCenterDistance * (new Vector3(1, -1, 0)).normalized;
        Instantiate(bullet, pos, Quaternion.identity).transform.right = (new Vector3(1, -1, 0)).normalized;
        pos = attackPoint.position + laserCenterDistance * (new Vector3(-1, -1, 0)).normalized;
        Instantiate(bullet, pos, Quaternion.identity).transform.right = (new Vector3(-1, -1, 0)).normalized;
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
