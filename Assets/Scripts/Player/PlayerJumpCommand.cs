using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using static PlayerController;

public class PlayerJumpCommand : AbstractCommand
{
    protected override void OnExecute()
    {
        if (PlayerController.Instance.fightFSM.CurrentStateId == FightState.Dash || PlayerController.Instance.fightFSM.CurrentStateId == FightState.Sweep || PlayerController.Instance.fightFSM.CurrentStateId == FightState.Fury) return;
        
        if (this.GetModel<PlayerModel>().isGround && PlayerController.Instance.moveFSM.CurrentStateId == MoveState.Locomotion)
        {
            PlayerController.Instance.moveFSM.ChangeState(MoveState.FirstJump);
        }
        else if (this.GetModel<PlayerModel>().canSecondJump && PlayerController.Instance.moveFSM.CurrentStateId == MoveState.Fall)
        {
            PlayerController.Instance.moveFSM.ChangeState(MoveState.SecondJump);
            this.GetModel<PlayerModel>().canSecondJump = false;
        }
    }
}
