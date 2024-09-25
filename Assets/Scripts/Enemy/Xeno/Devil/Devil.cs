using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

public class Devil : Xeno
{
    [SerializeField] int shootCount;
    int curShootCount;
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
            go.transform.right = (PlayerController.Instance.transform.position - go.transform.position).normalized;
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
}
