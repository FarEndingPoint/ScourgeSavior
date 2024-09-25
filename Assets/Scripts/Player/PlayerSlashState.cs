using QFramework;
using UnityEngine;

internal class PlayerSlashState : AbstractState<PlayerController.FightState, PlayerController>
{
    PlayerAttackToAddSlotEvent addEvent;
    FightStateOnUpdateEvent updateEvent;
    Collider2D[] colliders;
    bool canhit;//一次动作只进行一次打击
    Collider2D nearestEnemy;//选最近的敌人黏着

    public PlayerSlashState(FSM<PlayerController.FightState> fsm, PlayerController target) : base(fsm, target)
    {
        addEvent.shootDelta = 0.05f;
        addEvent.furyDelta = 0.02f;
        updateEvent.callbackInAction = () =>
        {
            if (canhit && Main.Interface.GetSystem<ActionSystem>().CurActionTime <= Main.Interface.GetModel<PlayerModel>().slashHitTime)
            {
                colliders = Physics2D.OverlapCircleAll(target.transform.position, Main.Interface.GetModel<PlayerModel>().slashRadius, LayerMask.GetMask("Enemy"));
                canhit = false;
                if (colliders.Length > 0)
                {
                    Main.Interface.GetModel<PlayerModel>().canSecondJump = true;
                    nearestEnemy = colliders[0];
                    TypeEventSystem.Global.Send(addEvent);
                    nearestEnemy.GetComponent<Enemy>().BeHited(Enemy.BeHitedType.Slash);
                    for (int i = 1; i < colliders.Length; i++)
                    {
                        TypeEventSystem.Global.Send(addEvent);
                        colliders[i].GetComponent<Enemy>().BeHited(Enemy.BeHitedType.Slash);
                        if (Vector2.Distance(colliders[i].transform.Position2D(), target.transform.Position2D()) < Vector3.Distance(nearestEnemy.transform.Position2D(), target.transform.Position2D()))
                            nearestEnemy = colliders[i];
                    }
                }
            }
            if (nearestEnemy != null)
            {
                Main.Interface.GetModel<PlayerModel>().isAttach = true;
                PlayerController.Instance.rb.velocity = (nearestEnemy.transform.Position2D() - target.transform.Position2D()) * Main.Interface.GetModel<PlayerModel>().attachSpeed;
                target.rb.gravityScale = Main.Interface.GetModel<PlayerModel>().attackGravityScale;
            }
            else
            {
                Main.Interface.GetModel<PlayerModel>().isAttach = false;
                target.rb.gravityScale = Main.Interface.GetModel<PlayerModel>().airGravityScale;
            }
        };
        updateEvent.callbackOutAction = () =>
        {
            target.fightFSM.ChangeState(PlayerController.FightState.None);
        };
    }

    protected override void OnEnter()
    {
        base.OnEnter();
        PlayerController.Instance.animator.SetBool("Slash", true);
        canhit = true;
        nearestEnemy = null;
        AudioKit.PlaySound(Main.Interface.GetModel<GameData>().soundPath + "Slash");
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        TypeEventSystem.Global.Send(updateEvent);
    }

    protected override void OnExit()
    {
        base.OnExit();
        PlayerController.Instance.animator.SetBool("Slash", false);
        PlayerController.Instance.rb.gravityScale = Main.Interface.GetModel<PlayerModel>().commonGravityScale;
        Main.Interface.GetModel<PlayerModel>().isAttach = false;
    }
}