using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerController;

public class PlayerSecondJumpState : AbstractState<PlayerController.MoveState, PlayerController>
{
    MoveStateOnUpdateEvent updateEvent;
    float curJumpTime;
    Vector2 velocity;

    public PlayerSecondJumpState(FSM<PlayerController.MoveState> fsm, PlayerController target) : base(fsm, target)
    {
        updateEvent = new MoveStateOnUpdateEvent();
        updateEvent.parameterName = "SecondJump";
        updateEvent.callback = () =>
        {
            if (PlayerController.Instance.fightFSM.CurrentStateId == FightState.Dash || PlayerController.Instance.fightFSM.CurrentStateId == FightState.Sweep || PlayerController.Instance.fightFSM.CurrentStateId == FightState.Fury)
                PlayerController.Instance.moveFSM.ChangeState(MoveState.Locomotion);

            velocity.x = target.rb.velocity.x;
            if (target.fightFSM.CurrentStateId != FightState.Slash && target.fightFSM.CurrentStateId != FightState.Smash)
                velocity.y = Main.Interface.GetModel<PlayerModel>().commonJumpHeight;
            else if (!Main.Interface.GetModel<PlayerModel>().isAttach || target.fightFSM.CurrentStateId == FightState.Smash)
                velocity.y = Main.Interface.GetModel<PlayerModel>().airJumpHeight;
            else velocity.y = Main.Interface.GetModel<PlayerModel>().attackJumpHeight;
            target.rb.velocity = velocity;

            curJumpTime -= Time.deltaTime;
            if (curJumpTime < 0 || Main.Interface.GetSystem<InputSystem>().inputController.Player.Jump.ReadValue<float>() <= 0)
                PlayerController.Instance.moveFSM.ChangeState(MoveState.Fall);
        };
    }

    protected override void OnEnter()
    {
        base.OnEnter();
        PlayerController.Instance.animator.SetBool("SecondJump", true);
        PlayerController.Instance.animator.SetBool("Fall", false);
        curJumpTime = Main.Interface.GetModel<PlayerModel>().maxSecondJumpTime;
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        TypeEventSystem.Global.Send(updateEvent);
    }

    protected override void OnExit()
    {
        base.OnExit();
        PlayerController.Instance.animator.SetBool("SecondJump", false);
    }
}
