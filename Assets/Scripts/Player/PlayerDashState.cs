using QFramework;
using UnityEngine;
using UnityEngine.InputSystem;

internal class PlayerDashState : AbstractState<PlayerController.FightState, PlayerController>
{
    PlayerAttackToAddSlotEvent addEvent;
    DragonPunchJudgeEvent dragonPunchJudgeEvent;
    FightStateOnUpdateEvent updateEvent;
    Collider2D[] colliders;
    bool canhit;//一次动作只进行一次打击
    Vector2 targetPoint, originalPos;

    public PlayerDashState(FSM<PlayerController.FightState> fsm, PlayerController target) : base(fsm, target)
    {
        addEvent.shootDelta = 0.025f;
        addEvent.furyDelta = 0.01f;
        dragonPunchJudgeEvent.isSmash = false;
        updateEvent.callbackInAction = () =>
        {
            TypeEventSystem.Global.Send(dragonPunchJudgeEvent);
            target.rb.velocity = (targetPoint - originalPos).normalized * Main.Interface.GetModel<PlayerModel>().dashSpeed;
            colliders = Physics2D.OverlapCircleAll(target.transform.position, Main.Interface.GetModel<PlayerModel>().dashDragRadius, LayerMask.GetMask("Enemy"));
            if (canhit && Main.Interface.GetSystem<ActionSystem>().CurActionTime <= Main.Interface.GetModel<PlayerModel>().dragonPunchHitTime)
            {
                canhit = false;
                if (colliders.Length > 0)
                {
                    Main.Interface.GetModel<PlayerModel>().canSecondJump = true;
                    for (int i = 0; i < colliders.Length; i++)
                    {
                        TypeEventSystem.Global.Send(addEvent);
                        colliders[i].GetComponent<Enemy>().BeHited(Enemy.BeHitedType.Dash);
                    }
                }
            }
            //整个Dash时间都会Drag敌人
            for (int i = 0; i < colliders.Length; i++)
                colliders[i].GetComponent<Enemy>().BeDraged(target.transform, 30);
        };
        updateEvent.callbackOutAction = () =>
        {
            target.fightFSM.ChangeState(PlayerController.FightState.None);
        };
    }

    protected override void OnEnter()
    {
        base.OnEnter();
        PlayerController.Instance.animator.SetBool("Dash", true);
        canhit = true;
        targetPoint = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        originalPos = PlayerController.Instance.transform.Position2D();
        PlayerController.Instance.rb.gravityScale = Main.Interface.GetModel<PlayerModel>().attackGravityScale;
        Main.Interface.GetModel<PlayerModel>().curDragonPunchInputTime = Main.Interface.GetModel<PlayerModel>().dragonPunchInputTime;
        AudioKit.PlaySound(Main.Interface.GetModel<GameData>().soundPath + "Dash");
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        TypeEventSystem.Global.Send(updateEvent);
    }

    protected override void OnExit()
    {
        base.OnExit();
        PlayerController.Instance.animator.SetBool("Dash", false);
        PlayerController.Instance.rb.gravityScale = Main.Interface.GetModel<PlayerModel>().commonGravityScale;
    }
}