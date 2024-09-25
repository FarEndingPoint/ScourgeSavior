using QFramework;
using static PlayerController;
using UnityEngine;

internal class PlayerFuryState : AbstractState<FightState, PlayerController>
{
    FightStateOnUpdateEvent updateEvent;
    Enemy[] enemies;

    public PlayerFuryState(FSM<FightState> fsm, PlayerController target) : base(fsm, target)
    {
        updateEvent = new FightStateOnUpdateEvent();
        updateEvent.callbackInAction = () =>
        {
            
        };
        updateEvent.callbackOutAction = () =>
        {
            target.fightFSM.ChangeState(FightState.None);
        };
    }

    protected override void OnEnter()
    {
        base.OnEnter();
        PlayerController.Instance.animator.SetBool("Fury", true);
        PlayerController.Instance.rb.gravityScale = Main.Interface.GetModel<PlayerModel>().attackGravityScale;
        enemies = Object.FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        foreach (Enemy enemy in enemies) enemy.BeFuried(true);
        AudioKit.PlaySound(Main.Interface.GetModel<GameData>().soundPath + "Fury");
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        TypeEventSystem.Global.Send(updateEvent);
    }

    protected override void OnExit()
    {
        base.OnExit();
        PlayerController.Instance.animator.SetBool("Fury", false);
        PlayerController.Instance.rb.gravityScale = Main.Interface.GetModel<PlayerModel>().commonGravityScale;
        foreach (Enemy enemy in enemies) enemy.BeFuried(false);
    }
}