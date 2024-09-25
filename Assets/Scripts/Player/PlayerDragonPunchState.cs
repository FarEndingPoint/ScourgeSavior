using UnityEngine;
using QFramework;
using UnityEngine.InputSystem;

public class PlayerDragonPunchState : AbstractState<PlayerController.FightState, PlayerController>
{
    PlayerAttackToAddSlotEvent addEvent;
    FightStateOnUpdateEvent updateEvent;
    Collider2D[] smashColliders, dashColliders;
    bool canhit;//一次动作只进行一次打击
    Vector2 targetPoint, originalPos;

    public PlayerDragonPunchState(FSM<PlayerController.FightState> fsm, PlayerController target) : base(fsm, target)
    {
        addEvent.shootDelta = 0.075f;
        addEvent.furyDelta = 0.04f;
        updateEvent.callbackInAction = () =>
        {
            target.rb.velocity = (targetPoint - originalPos).normalized * Main.Interface.GetModel<PlayerModel>().dragonPunchSpeed;
            smashColliders = Physics2D.OverlapCircleAll(target.transform.position, Main.Interface.GetModel<PlayerModel>().dragonPunchSmashRadius, LayerMask.GetMask("Enemy", "EnemyBullet"));
            dashColliders = Physics2D.OverlapCircleAll(target.transform.position, Main.Interface.GetModel<PlayerModel>().dragonPunchDragRadius, LayerMask.GetMask("Enemy"));
            if (canhit && Main.Interface.GetSystem<ActionSystem>().CurActionTime <= Main.Interface.GetModel<PlayerModel>().dashHitTime)
            {
                canhit = false;
                for (int i = 0; i < smashColliders.Length; i++)
                {
                    if (smashColliders[i].gameObject.layer == LayerMask.NameToLayer("EnemyBullet"))
                    {
                        GameObject.Destroy(smashColliders[i].gameObject);
                        continue;
                    }
                    TypeEventSystem.Global.Send(addEvent);
                    Enemy enemy = smashColliders[i].GetComponent<Enemy>();
                    enemy.BePushed(targetPoint);
                    enemy.BeHited(Enemy.BeHitedType.DragonPunch);
                }
                if(dashColliders.Length > 0)
                {
                    Main.Interface.GetModel<PlayerModel>().canSecondJump = true;
                }
            }
            //整个Dash时间都会Drag敌人
            for (int i = 0; i < dashColliders.Length; i++)
                dashColliders[i].GetComponent<Enemy>().BeDraged(target.transform, 30);
        };
        updateEvent.callbackOutAction = () =>
        {
            target.fightFSM.ChangeState(PlayerController.FightState.None);
        };
    }

    protected override void OnEnter()
    {
        base.OnEnter();
        PlayerController.Instance.animator.SetBool("DragonPunch", true);
        canhit = true;
        targetPoint = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        originalPos = PlayerController.Instance.transform.Position2D();
        int smashDirection = targetPoint.x >= PlayerController.Instance.transform.Position2D().x ? 1 : -1;
        PlayerController.Instance.transform.localScale = smashDirection == 1 ? Main.Interface.GetModel<PlayerModel>().rightScale : Main.Interface.GetModel<PlayerModel>().leftScale;
        PlayerController.Instance.rb.gravityScale = Main.Interface.GetModel<PlayerModel>().attackGravityScale;
        DragonPunchEnterCD();
        AudioKit.PlaySound(Main.Interface.GetModel<GameData>().soundPath + "DragonPunch");
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        TypeEventSystem.Global.Send(updateEvent);
    }

    protected override void OnExit()
    {
        base.OnExit();
        PlayerController.Instance.animator.SetBool("DragonPunch", false);
        PlayerController.Instance.rb.gravityScale = Main.Interface.GetModel<PlayerModel>().commonGravityScale;
    }

    void DragonPunchEnterCD()
    {
        Main.Interface.GetModel<PlayerModel>().curDragonPunchCD.Value = Main.Interface.GetModel<PlayerModel>().dragonPunchCD;
    }
}   
