using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using static PlayerController;

public class PlayerLocomotionCommand : AbstractCommand
{
    protected override void OnExecute()
    {
        if (PlayerController.Instance.fightFSM.CurrentStateId == FightState.Dash || PlayerController.Instance.fightFSM.CurrentStateId == FightState.Sweep || PlayerController.Instance.fightFSM.CurrentStateId == FightState.Fury)
        {
            PlayerController.Instance.rb.velocity = Vector2.zero;
            return;
        }

        this.GetModel<PlayerModel>().inputVector = this.GetSystem<InputSystem>().inputController.Player.Locomotion.ReadValue<Vector2>();
        this.GetModel<PlayerModel>().inputVector.y = 0;
        this.GetModel<PlayerModel>().inputVector.Normalize();

        if (Main.Interface.GetModel<PlayerModel>().isGround || (PlayerController.Instance.fightFSM.CurrentStateId == FightState.None))
            PlayerController.Instance.rb.velocity = this.GetModel<PlayerModel>().inputVector * this.GetModel<PlayerModel>().moveSpeed;
        else PlayerController.Instance.rb.velocity = this.GetModel<PlayerModel>().inputVector * this.GetModel<PlayerModel>().moveSpeedWhenSlash;

        PlayerController.Instance.animator.SetFloat("Speed", PlayerController.Instance.rb.velocity.sqrMagnitude);

        if (PlayerController.Instance.fightFSM.CurrentStateId == FightState.Smash) return;
        if (this.GetModel<PlayerModel>().inputVector.x < 0)
            PlayerController.Instance.transform.localScale = this.GetModel<PlayerModel>().leftScale;
        else if (this.GetModel<PlayerModel>().inputVector.x > 0)
            PlayerController.Instance.transform.localScale = this.GetModel<PlayerModel>().rightScale;
    }
}
