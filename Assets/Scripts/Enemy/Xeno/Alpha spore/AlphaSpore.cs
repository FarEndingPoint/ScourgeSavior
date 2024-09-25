using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

public class AlphaSpore : Xeno
{
    XenoLineDetectPlayerCommand xenoLineDetectPlayerCommand;
    [SerializeField] float moveTime;
    [SerializeField] float moveSpeed;
    float curMoveTime;
    int dir = 1;

    protected override void Awake()
    {
        base.Awake();
        xenoLineDetectPlayerCommand = new XenoLineDetectPlayerCommand(transform, xenoModel.meetTimeToFoundPlayer);
    }

    protected override void CommonOnEnter()
    {
        base.CommonOnEnter();
        curMoveTime = moveTime;
        dir = -dir;
    }

    protected override void AttackOnEnter()
    {
        base.AttackOnEnter();
        for (int i = 0; i < 13; i++)
        {
            GameObject go = Instantiate(bullet, attackPoint.position, Quaternion.identity);
            go.transform.rotation = Quaternion.Euler(0, 0, i * 30);
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
        transform.localScale = dir == 1 ? xenoModel.rightScale : xenoModel.leftScale;
        if (curMoveTime > -0)
        {
            transform.Translate(transform.right * dir * moveSpeed * Time.deltaTime);
            curMoveTime -= Time.deltaTime;
        }
        else
        {
            curMoveTime = moveTime;
            dir = -dir;
        }
    }

    protected override void Stop()
    {

    }
}
