using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotionState : AbstractState<PlayerController.MoveState, PlayerController>
{
    //float soundPlayGapTime = 1;
    //float curSoundPlayGapTime;

    public PlayerLocomotionState(FSM<PlayerController.MoveState> fsm, PlayerController target) : base(fsm, target)
    {
    }

    protected override void OnEnter()
    {
        base.OnEnter();
        //curSoundPlayGapTime = 0;
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (!Main.Interface.GetModel<PlayerModel>().isGround)
            PlayerController.Instance.moveFSM.ChangeState(PlayerController.MoveState.Fall);
        //else if (PlayerController.Instance.fightFSM.CurrentStateId == PlayerController.FightState.None && Time.timeScale > 0 && Main.Interface.GetModel<PlayerModel>().inputVector.x != 0)
        //{
        //    if (curSoundPlayGapTime > 0) curSoundPlayGapTime -= Time.deltaTime;
        //    else
        //    {
        //        curSoundPlayGapTime = soundPlayGapTime;
        //        AudioKit.PlaySound(Main.Interface.GetModel<GameData>().soundPath + "Locomotion" + Random.Range(1, 3).ToString());
        //    }
        //}
        //else curSoundPlayGapTime = 0;
    }
}
