using UnityEngine;
using QFramework;
using static Xeno;
using System;

public class XenoAttackCommand : AbstractCommand
{
    XenoModel xenoModel;
    FSM<State> fsm;
    Action callback;

    public XenoAttackCommand(XenoModel xenoModel, FSM<State> fsm, Action callback)
    {
        this.xenoModel = xenoModel;
        this.fsm = fsm;
        this.callback = callback;
    }

    protected override void OnExecute()
    {
        if (xenoModel.curAttackTime > 0)
        {
            callback?.Invoke();
            xenoModel.curAttackTime -= Time.deltaTime;
        }
        else fsm.ChangeState(State.Common);
    }
}
