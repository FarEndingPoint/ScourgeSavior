using QFramework;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerController;

internal class PlayerSweepState : AbstractState<FightState, PlayerController>
{
    PlayerAttackToAddSlotEvent addEvent;
    Collider2D[] colliders, teleportColliders;
    float timer;
    bool canhit, canTeleport;
    int sweepDirection;

    public PlayerSweepState(FSM<FightState> fsm, PlayerController target) : base(fsm, target)
    {
        addEvent.shootDelta = 0.05f;
        addEvent.furyDelta = 0.04f;
    }

    protected override void OnEnter()
    {
        base.OnEnter();
        PlayerController.Instance.animator.SetBool("Sweep", true);
        PlayerController.Instance.rb.gravityScale = Main.Interface.GetModel<PlayerModel>().attackGravityScale;
        canhit = true;
        canTeleport = false;
        timer = Main.Interface.GetModel<PlayerModel>().oneSweepTime;
        Vector2 target = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        sweepDirection = target.x >= PlayerController.Instance.transform.Position2D().x ? 1 : -1;
        PlayerController.Instance.transform.localScale = sweepDirection == 1 ? Main.Interface.GetModel<PlayerModel>().rightScale : Main.Interface.GetModel<PlayerModel>().leftScale;
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (Main.Interface.GetSystem<InputSystem>().inputController.Player.Sweep.ReadValue<float>() > 0)
        {
            PlayerController.Instance.rb.velocity = Vector2.right * sweepDirection * Main.Interface.GetModel<PlayerModel>().sweepSpeed;
            if (timer > 0)
            {
                PlayerController.Instance.animator.SetBool("Sweep", true);
                timer -= Time.deltaTime;
                colliders = Physics2D.OverlapCircleAll(PlayerController.Instance.transform.position, Main.Interface.GetModel<PlayerModel>().sweepRadius, LayerMask.GetMask("Enemy"));
                if (canhit && timer <= Main.Interface.GetModel<PlayerModel>().sweepHitTime)
                {
                    AudioKit.PlaySound(Main.Interface.GetModel<GameData>().soundPath + "Sweep");
                    canhit = false;
                    if (colliders.Length > 0)
                    {
                        canTeleport = true;
                        for (int i = 0; i < colliders.Length; i++)
                        {
                            TypeEventSystem.Global.Send(addEvent);
                            colliders[i].GetComponent<Enemy>().BeHited(Enemy.BeHitedType.Sweep);
                        }
                    }
                }
                for (int i = 0; i < colliders.Length; i++)
                    colliders[i].GetComponent<Enemy>().BeDraged(PlayerController.Instance.transform, 5);
            }
            else
            {
                canhit = true;
                timer = Main.Interface.GetModel<PlayerModel>().oneSweepTime;
                PlayerController.Instance.animator.SetBool("Sweep", false);
            }
            if(canTeleport && Main.Interface.GetSystem<InputSystem>().inputController.Player.Jump.WasPressedThisFrame())
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                teleportColliders = Physics2D.OverlapCircleAll(mousePos, 0.001f);
                bool posIsOk = true;
                for (int i = 0; i < teleportColliders.Length; i++)
                    if (teleportColliders[i].gameObject.layer == LayerMask.NameToLayer("UI") || teleportColliders[i].gameObject.layer == LayerMask.NameToLayer("Ground"))
                    {
                        posIsOk = false; 
                        break;
                    }
                if (posIsOk)
                {
                    PlayerController.Instance.transform.position = mousePos;
                    Main.Interface.GetSystem<ActionSystem>().SetCanEnterAction(true);
                    PlayerController.Instance.fightFSM.ChangeState(FightState.None);
                    Main.Interface.GetSystem<ActionSystem>().EndAction();
                }
            }
        }
        else
        {
            Main.Interface.GetSystem<ActionSystem>().SetCanEnterAction(true);
            PlayerController.Instance.fightFSM.ChangeState(FightState.None);            
            Main.Interface.GetSystem<ActionSystem>().EndAction();
        }
    }

    protected override void OnExit()
    {
        base.OnExit();
        PlayerController.Instance.animator.SetBool("Sweep", false);
        PlayerController.Instance.rb.gravityScale = Main.Interface.GetModel<PlayerModel>().commonGravityScale;
        Main.Interface.GetSystem<ActionSystem>().SetCanEnterAction(true);
    }
}