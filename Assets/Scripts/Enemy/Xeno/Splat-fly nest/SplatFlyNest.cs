using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

public class SplatFlyNest : Xeno
{
    [SerializeField] GameObject summoned;
    [SerializeField] float summonGapTime;
    float curSummonGapTime;

    protected override void CommonOnEnter()
    {
        base.CommonOnEnter();
        curSummonGapTime = summonGapTime;
    }

    protected override void CommonOnUpdate()
    {
        base.CommonOnUpdate();
        if(curSummonGapTime > 0)
        {
            curSummonGapTime -= Time.deltaTime;
        }
        else
        {
            curSummonGapTime = summonGapTime;
            Instantiate(summoned, attackPoint.position, Quaternion.identity).GetComponent<SplatFly>().isKillCount = false;
        }
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
