using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

public class AlphaIgnis : Xeno
{
    [SerializeField] float realAttackTime;
    [SerializeField] float attackDashSpeed;
    bool canShoot;
    XenoLineDetectPlayerCommand xenoLineDetectPlayerCommand;

    protected override void Awake()
    {
        base.Awake();
        xenoLineDetectPlayerCommand = new XenoLineDetectPlayerCommand(transform, xenoModel.meetTimeToFoundPlayer);
    }

    protected override void AttackOnEnter()
    {
        base.AttackOnEnter();
        canShoot = true;
    }

    protected override bool DetectPlayer()
    {
        return this.SendCommand(xenoLineDetectPlayerCommand);
    }

    protected override void Attack()
    {
        transform.Translate((PlayerController.Instance.transform.position - transform.position) * attackDashSpeed * Time.deltaTime);
        if (canShoot && xenoModel.curAttackTime <= xenoModel.attackTime - realAttackTime)
        {
            canShoot = false;
            GameObject go1 = Instantiate(bullet, attackPoint.Position2D(), Quaternion.identity);
            GameObject go2 = Instantiate(bullet, attackPoint.Position2D(), Quaternion.identity);
            GameObject go3 = Instantiate(bullet, attackPoint.Position2D(), Quaternion.identity);
            go1.transform.right = (PlayerController.Instance.transform.Position2D() - go1.transform.Position2D()).normalized;
            go2.transform.right = (PlayerController.Instance.transform.Position2D() - go2.transform.Position2D()).normalized;
            go3.transform.right = (PlayerController.Instance.transform.Position2D() - go3.transform.Position2D()).normalized;
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
