using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

public class XenoBeSmashedCommand : AbstractCommand
{
    FSM<Xeno.State> fsm;
    Rigidbody2D rb;
    XenoModel xenoModel;

    public XenoBeSmashedCommand(FSM<Xeno.State> fsm, Rigidbody2D rb, XenoModel xenoModel)
    {
        this.fsm = fsm;
        this.rb = rb;
        this.xenoModel = xenoModel;
    }

    protected override void OnExecute()
    {
        if (xenoModel.curBeSmashedTime > 0)
        {
            xenoModel.curBeSmashedTime -= Time.deltaTime;
            if (xenoModel.curBePushedTime > 0)
            {
                xenoModel.curBePushedTime -= Time.deltaTime;
                rb.velocity = (xenoModel.bePushedDestination - xenoModel.bePushedOriginalPos).normalized * xenoModel.bePushedSpeed;
            }
            else rb.velocity = Vector2.zero;
        }
        else fsm.ChangeState(Xeno.State.Common);
    }
}
