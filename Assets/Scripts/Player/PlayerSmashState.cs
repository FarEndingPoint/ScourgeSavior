using QFramework;
using UnityEngine;
using UnityEngine.InputSystem;

internal class PlayerSmashState : AbstractState<PlayerController.FightState, PlayerController>
{
    PlayerAttackToAddSlotEvent addEvent;
    DragonPunchJudgeEvent dragonPunchJudgeEvent;
    FightStateOnUpdateEvent updateEvent;
    Collider2D[] colliders;
    bool canhit;
    Vector2 targetPoint;

    public PlayerSmashState(FSM<PlayerController.FightState> fsm, PlayerController target) : base(fsm, target)
    {
        addEvent.shootDelta = 0.025f;
        addEvent.furyDelta = 0.01f;
        dragonPunchJudgeEvent.isSmash = true;
        updateEvent.callbackInAction = () =>
        {
            TypeEventSystem.Global.Send(dragonPunchJudgeEvent);
            if (canhit && Main.Interface.GetSystem<ActionSystem>().CurActionTime <= Main.Interface.GetModel<PlayerModel>().smashHitTime)
            {
                colliders = Physics2D.OverlapCircleAll(PlayerController.Instance.smashCenter.position, Main.Interface.GetModel<PlayerModel>().smashRadius, LayerMask.GetMask("Enemy", "EnemyBullet"));
                canhit = false;
                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].gameObject.layer == LayerMask.NameToLayer("EnemyBullet"))
                    {
                        GameObject.Destroy(colliders[i].gameObject);
                        continue;
                    }
                    TypeEventSystem.Global.Send(addEvent);
                    Enemy enemy = colliders[i].GetComponent<Enemy>();
                    enemy.BePushed(targetPoint);
                    enemy.BeHited(Enemy.BeHitedType.Smash);
                }
            }
        };
        updateEvent.callbackOutAction = () =>
        {
            PlayerController.Instance.fightFSM.ChangeState(PlayerController.FightState.None);
        };
    }

    protected override void OnEnter()
    {
        base.OnEnter();
        PlayerController.Instance.animator.SetBool("Smash", true);
        canhit = true;
        PlayerController.Instance.rb.gravityScale = Main.Interface.GetModel<PlayerModel>().airGravityScale;
        targetPoint = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        int smashDirection = targetPoint.x >= PlayerController.Instance.transform.Position2D().x ? 1 : -1;
        PlayerController.Instance.transform.localScale = smashDirection == 1 ? Main.Interface.GetModel<PlayerModel>().rightScale : Main.Interface.GetModel<PlayerModel>().leftScale;
        Main.Interface.GetModel<PlayerModel>().curDragonPunchInputTime = Main.Interface.GetModel<PlayerModel>().dragonPunchInputTime;
        AudioKit.PlaySound(Main.Interface.GetModel<GameData>().soundPath + "Smash");
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        TypeEventSystem.Global.Send(updateEvent);
    }

    protected override void OnExit()
    {
        base.OnExit();
        PlayerController.Instance.animator.SetBool("Smash", false);
        PlayerController.Instance.rb.gravityScale = Main.Interface.GetModel<PlayerModel>().commonGravityScale;
    }
}