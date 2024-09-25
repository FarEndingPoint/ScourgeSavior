using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallState : AbstractState<PlayerController.MoveState, PlayerController>
{
    MoveStateOnUpdateEvent updateEvent;

    public PlayerFallState(FSM<PlayerController.MoveState> fsm, PlayerController target) : base(fsm, target)
    {
        updateEvent = new MoveStateOnUpdateEvent();
        updateEvent.parameterName = "Fall";
        updateEvent.callback = () =>
        {
            if (Main.Interface.GetModel<PlayerModel>().isGround)
                PlayerController.Instance.moveFSM.ChangeState(PlayerController.MoveState.Locomotion);
        };
    }

    protected override void OnEnter()
    {
        base.OnEnter();
        PlayerController.Instance.animator.SetBool("Fall", true);
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        TypeEventSystem.Global.Send(updateEvent);
    }

    protected override void OnExit()
    {
        base.OnExit();
        PlayerController.Instance.animator.SetBool("Fall", false);
    }
}
