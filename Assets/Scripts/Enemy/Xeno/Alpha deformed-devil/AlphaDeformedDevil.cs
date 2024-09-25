using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

public class AlphaDeformedDevil : Xeno
{
    XenoLineDetectPlayerCommand xenoLineDetectPlayerCommand;
    Vector3 bulletScale = new Vector3(3, 3, 1);

    protected override void Awake()
    {
        base.Awake();
        xenoLineDetectPlayerCommand = new XenoLineDetectPlayerCommand(transform, xenoModel.meetTimeToFoundPlayer);
    }

    protected override void AttackOnEnter()
    {
        base.AttackOnEnter();
        GameObject go = Instantiate(bullet, attackPoint.position, Quaternion.identity);
        go.transform.localScale = bulletScale;
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
