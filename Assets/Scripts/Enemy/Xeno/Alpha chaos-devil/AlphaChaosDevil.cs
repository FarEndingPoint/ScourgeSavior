using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphaChaosDevil : Xeno
{
    [SerializeField] float shootGapTime;
    [SerializeField] float bulletNum;
    XenoLineDetectPlayerCommand xenoLineDetectPlayerCommand;

    protected override void Awake()
    {
        base.Awake();
        xenoLineDetectPlayerCommand = new XenoLineDetectPlayerCommand(transform, xenoModel.meetTimeToFoundPlayer);
    }

    protected override void AttackOnEnter()
    {
        base.AttackOnEnter();
        StartCoroutine(Shoot());
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

    IEnumerator Shoot()
    {
        for (int i = 0; i < bulletNum; i++)
        {
            Instantiate(bullet, attackPoint.Position2D(), Quaternion.identity);
            yield return new WaitForSeconds(shootGapTime);
        }
    }
}
