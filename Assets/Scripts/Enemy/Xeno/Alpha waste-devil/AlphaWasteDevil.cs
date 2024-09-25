using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

public class AlphaWasteDevil : Xeno
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
        for (int i = 0; i < 3; i++)
        {
            GameObject go = Instantiate(bullet, attackPoint.position, Quaternion.identity);
            go.transform.right = transform.up;
            go.transform.rotation *= Quaternion.Euler(0, 0, (1 - i) * 30);
        }
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
        if (!aiPath.canMove) aiPath.canMove = true;
        else transform.localScale = aidDestinationSetter.target.position.x >= transform.position.x ? xenoModel.rightScale : xenoModel.leftScale;
    }

    protected override void Stop()
    {
        aiPath.canMove = false;
    }
}
