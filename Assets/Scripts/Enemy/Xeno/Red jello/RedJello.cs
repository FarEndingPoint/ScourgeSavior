using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

public class RedJello : Xeno
{
    [SerializeField] int shootCount;
    int curShootCount;
    Vector3 dir;
    XenoLineDetectPlayerCommand xenoLineDetectPlayerCommand;

    protected override void Awake()
    {
        base.Awake();
        xenoLineDetectPlayerCommand = new XenoLineDetectPlayerCommand(transform, xenoModel.meetTimeToFoundPlayer);
    }

    protected override void AttackOnEnter()
    {
        base.AttackOnEnter();
        curShootCount = 0;
        dir = (PlayerController.Instance.transform.position - attackPoint.position).normalized;
    }

    protected override bool DetectPlayer()
    {
        return this.SendCommand(xenoLineDetectPlayerCommand);
    }

    protected override void Attack()
    {
        if (xenoModel.curAttackTime <= xenoModel.attackTime - (curShootCount * (xenoModel.attackTime / shootCount)))
        {
            curShootCount++;
            GameObject go = Instantiate(bullet, attackPoint.position, Quaternion.identity);
            go.transform.right = dir;
        }
    }

    protected override void Move()
    {

    }

    protected override void Stop()
    {

    }
}
