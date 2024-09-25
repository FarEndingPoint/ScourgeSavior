using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

public class HornedDevil : Xeno
{
    [SerializeField] int shootCount;
    int curShootCount;
    Vector2 target;
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
        target = PlayerController.Instance.transform.Position2D();
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
            go.transform.right = (target - go.transform.Position2D()).normalized;
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
