using QFramework;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoSingleton<PlayerController>, IController
{
    public enum MoveState
    {
        Locomotion,FirstJump,SecondJump,Fall
    }
    public FSM<MoveState> moveFSM = new FSM<MoveState>();

    public enum FightState
    {
        None,Slash,Smash,Dash,DragonPunch,Sweep,Fury
    }
    public FSM<FightState> fightFSM = new FSM<FightState>();

    [HideInInspector] public SpriteRenderer spriteRenderer;
    [HideInInspector] public Animator animator;
    [HideInInspector] public Rigidbody2D rb;
    public BoxCollider2D bottom, left, right;
    public Transform smashCenter;
    public Blast32 blast32;

    //UI
    public Scrollbar blood;
    public Image[] bullets;
    public Image furySlot, dragonPunchSlot;
    public Scrollbar bulletSlot;
    public Animator furyLogo;

    PlayerModel playerModel;
    InputSystem inputSystem;
    ActionSystem actionSystem;

    GroundDetectorCommand groundDetectorCommand;
    PlayerLocomotionCommand playerLocomotionCommand;
    PlayerJumpCommand playerJumpCommand;
    PlayerDragonPunchCDCommand playerDragonPunchCDCommand;
    PlayerBeHitedInvincibleCommand playerBeHitedInvincibleCommand;

    void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();

        playerModel = this.GetModel<PlayerModel>();
        inputSystem = this.GetSystem<InputSystem>();
        actionSystem = this.GetSystem<ActionSystem>();

        groundDetectorCommand = new GroundDetectorCommand();
        playerLocomotionCommand = new PlayerLocomotionCommand();
        playerJumpCommand = new PlayerJumpCommand();
        playerDragonPunchCDCommand = new PlayerDragonPunchCDCommand();
        playerBeHitedInvincibleCommand = new PlayerBeHitedInvincibleCommand(spriteRenderer);
    }

    private void OnEnable()
    {
        playerModel.Init();
        actionSystem.Init();

        inputSystem.inputController.Player.Jump.performed += Jump;
        inputSystem.inputController.Player.Slash.performed += Slash;
        inputSystem.inputController.Player.Shoot.performed += Shoot;
        inputSystem.inputController.Player.ChargeShoot.performed += ChargeShoot;
        inputSystem.inputController.Player.Smash.performed += Smash;
        inputSystem.inputController.Player.Dash.performed += Dash;
        inputSystem.inputController.Player.Sweep.performed += Sweep;
        inputSystem.inputController.Player.Fury.performed += Fury;
    }

    private void OnDisable()
    {
        inputSystem.inputController.Player.Jump.performed -= Jump;
        inputSystem.inputController.Player.Slash.performed -= Slash;
        inputSystem.inputController.Player.Shoot.performed -= Shoot;
        inputSystem.inputController.Player.ChargeShoot.performed -= ChargeShoot;
        inputSystem.inputController.Player.Smash.performed -= Smash;
        inputSystem.inputController.Player.Dash.performed -= Dash;
        inputSystem.inputController.Player.Sweep.performed -= Sweep;
        inputSystem.inputController.Player.Fury.performed -= Fury;
    }

    private void Start()
    {
        RegisterEvent();

        moveFSM.AddState(MoveState.Locomotion, new PlayerLocomotionState(moveFSM, this));
        moveFSM.AddState(MoveState.FirstJump, new PlayerFirstJumpState(moveFSM, this));
        moveFSM.AddState(MoveState.SecondJump, new PlayerSecondJumpState(moveFSM, this));
        moveFSM.AddState(MoveState.Fall, new PlayerFallState(moveFSM, this));

        fightFSM.State(FightState.None);
        fightFSM.AddState(FightState.Slash, new PlayerSlashState(fightFSM, this));
        fightFSM.AddState(FightState.Smash, new PlayerSmashState(fightFSM, this));
        fightFSM.AddState(FightState.Dash, new PlayerDashState(fightFSM, this));
        fightFSM.AddState(FightState.DragonPunch, new PlayerDragonPunchState(fightFSM, this));
        fightFSM.AddState(FightState.Sweep, new PlayerSweepState(fightFSM, this));
        fightFSM.AddState(FightState.Fury, new PlayerFuryState(fightFSM, this));

        moveFSM.StartState(MoveState.Locomotion);
        fightFSM.StartState(FightState.None);
    }

    private void Update()
    {
        this.SendCommand(playerLocomotionCommand);
        this.SendCommand(groundDetectorCommand);
        this.SendCommand(playerDragonPunchCDCommand);
        this.SendCommand(playerBeHitedInvincibleCommand);
        moveFSM.Update();
        fightFSM.Update();
    }

    void RegisterEvent()
    {
        TypeEventSystem.Global.Register<MoveStateOnUpdateEvent>(e =>
        {
            if (fightFSM.CurrentStateId == FightState.Slash)
                animator.SetTrigger("EndMove");
            else animator.SetBool(e.parameterName, true);
            e.callback?.Invoke();
        }).UnRegisterWhenGameObjectDestroyed(gameObject);
        TypeEventSystem.Global.Register<FightStateOnUpdateEvent>(e =>
        {
            if (actionSystem.IsInAction())
            {
                actionSystem.ActEveryFrame();
                e.callbackInAction?.Invoke();
            }
            else
            {
                e.callbackOutAction?.Invoke();
                actionSystem.EndAction();
            }
        }).UnRegisterWhenGameObjectDestroyed(gameObject);
        TypeEventSystem.Global.Register<DragonPunchJudgeEvent>(e =>
        {
            if (!playerModel.canDragonPunch) return;
            if (playerModel.curDragonPunchInputTime > 0)
            {
                playerModel.curDragonPunchInputTime -= Time.deltaTime;
                if ((e.isSmash && Main.Interface.GetSystem<InputSystem>().inputController.Player.Dash.triggered) || (!e.isSmash && Main.Interface.GetSystem<InputSystem>().inputController.Player.Smash.triggered))
                {
                    Main.Interface.GetSystem<ActionSystem>().ClearCurAction();
                    Main.Interface.GetSystem<ActionSystem>().StartAction(Main.Interface.GetModel<PlayerModel>().dragonPunchTime - (Main.Interface.GetModel<PlayerModel>().dragonPunchInputTime - playerModel.curDragonPunchInputTime), Main.Interface.GetModel<PlayerModel>().dragonPunchWindow);
                    fightFSM.ChangeState(FightState.DragonPunch);
                }
            }
        }).UnRegisterWhenGameObjectDestroyed(gameObject);
        TypeEventSystem.Global.Register<PlayerBeHitedEvent>(e =>
        {
            if (playerModel.curBeHitedInvincibleTime > 0 || fightFSM.CurrentStateId == FightState.Fury || fightFSM.CurrentStateId == FightState.DragonPunch) return;
            else
            {
                if (playerModel.hp.Value > 0) playerModel.hp.Value--;
                playerModel.curBeHitedInvincibleTime = playerModel.beHitedInvincibleTime;
            }
        }).UnRegisterWhenGameObjectDestroyed(gameObject);
        TypeEventSystem.Global.Register<PlayerAttackToAddSlotEvent>(e =>
        {
            playerModel.bulletSlot.Value += e.shootDelta;
            playerModel.furySlot.Value += e.furyDelta;
        }).UnRegisterWhenGameObjectDestroyed(gameObject);
        playerModel.hp.Register(e =>
        {
            blood.size = 1 - (float)playerModel.hp.Value / playerModel.maxHp;
            if (playerModel.hp.Value <= 0) LevelController.Instance.Lose();
        }).UnRegisterWhenGameObjectDestroyed(gameObject);
        playerModel.curBulletsNum.Register(e =>
        {
            if (playerModel.curBulletsNum.Value == playerModel.maxBulletsNum)
                for (int i = 0; i < playerModel.maxBulletsNum; i++)
                    bullets[i].enabled = true;
            for (int i = playerModel.maxBulletsNum - 1; i >= playerModel.curBulletsNum.Value; i--)
            {
                bullets[i].enabled = false;
            }
        }).UnRegisterWhenGameObjectDestroyed(gameObject);
        playerModel.bulletSlot.Register(e =>
        {
            if (playerModel.bulletSlot.Value >= 1)
            {
                playerModel.curBulletsNum.Value = playerModel.maxBulletsNum;
                playerModel.bulletSlot.Value -= 1;
                bulletSlot.value = 0;
                AudioKit.PlaySound(Main.Interface.GetModel<GameData>().soundPath + "GunLoad");
            }
            else bulletSlot.value = playerModel.bulletSlot.Value;
        }).UnRegisterWhenGameObjectDestroyed(gameObject);
        playerModel.furySlot.Register(e =>
        {
            if (playerModel.furySlot.Value > 1) playerModel.furySlot.Value = 1;
            furyLogo.SetBool("FuryLogo", playerModel.furySlot.Value == 1);
            furySlot.rectTransform.SetSizeHeight(650 * playerModel.furySlot.Value);
        }).UnRegisterWhenGameObjectDestroyed(gameObject);
        playerModel.curDragonPunchCD.Register(e =>
        {
            if (playerModel.curDragonPunchCD.Value < 0) playerModel.curDragonPunchCD.Value = 0;
            dragonPunchSlot.rectTransform.SetSizeHeight(650 * (1 - playerModel.curDragonPunchCD.Value / playerModel.dragonPunchCD));
        }).UnRegisterWhenGameObjectDestroyed(gameObject);
    }

    void Jump(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        this.SendCommand(playerJumpCommand);
    }

    private void Slash(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        StartCoroutine(StartSlash());
    }

    IEnumerator StartSlash()
    {
        if (!actionSystem.CanEnterAction()) yield break;
        yield return actionSystem.WaitCurAction();
        actionSystem.StartAction(playerModel.slashTime, playerModel.slashWindow);      
        fightFSM.ChangeState(FightState.Slash);
    }

    private void Shoot(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (playerModel.curBulletsNum.Value > 0)
        {
            blast32.Shoot(false, Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()).ToVector2());
            playerModel.curBulletsNum.Value--;
            AudioKit.PlaySound(Main.Interface.GetModel<GameData>().soundPath + "Gun" + Random.Range(1, 5).ToString());
        }
    }

    private void ChargeShoot(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (playerModel.curBulletsNum.Value >= 3)
        {
            blast32.Shoot(true, Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()).ToVector2());
            playerModel.curBulletsNum.Value -= 3;
            AudioKit.PlaySound(Main.Interface.GetModel<GameData>().soundPath + "Grenade");
        }
    }   

    private void Smash(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        StartCoroutine(StartSmash());
    }

    IEnumerator StartSmash()
    {
        if (playerModel.canDragonPunch && fightFSM.CurrentStateId == FightState.Dash && playerModel.dashTime - actionSystem.CurActionTime <= playerModel.dragonPunchInputTime)
            yield break;
        else if (fightFSM.CurrentStateId == FightState.Slash)
            actionSystem.ClearCurAction();
        else
        {
            if (!actionSystem.CanEnterAction()) yield break;
            yield return actionSystem.WaitCurAction();
        }
        actionSystem.StartAction(playerModel.smashTime, playerModel.smashWindow);
        fightFSM.ChangeState(FightState.Smash);
    }

    private void Dash(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        StartCoroutine(StartDash());
    }

    IEnumerator StartDash()
    {
        if (playerModel.canDragonPunch && fightFSM.CurrentStateId == FightState.Smash && playerModel.smashTime - actionSystem.CurActionTime <= playerModel.dragonPunchInputTime)
            yield break;
        else if (fightFSM.CurrentStateId == FightState.Slash)
            actionSystem.ClearCurAction();
        else
        {
            if (!actionSystem.CanEnterAction()) yield break;
            yield return actionSystem.WaitCurAction();
        }
        actionSystem.StartAction(playerModel.dashTime, playerModel.dashWindow);
        fightFSM.ChangeState(FightState.Dash);
    }

    private void Sweep(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        StartCoroutine(StartSweep());
    }

    IEnumerator StartSweep()
    {
        if (fightFSM.CurrentStateId == FightState.Slash)
            actionSystem.ClearCurAction();
        else
        {
            if (!actionSystem.CanEnterAction()) yield break;
            yield return actionSystem.WaitCurAction();
        }
        actionSystem.StartAction(0, 0);
        actionSystem.SetCanEnterAction(false);
        fightFSM.ChangeState(FightState.Sweep);
    }

    private void Fury(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        StartCoroutine(StartFury());
    }

    IEnumerator StartFury()
    {
        if(playerModel.furySlot.Value < 1) yield break;
        playerModel.furySlot.Value = 0;
        actionSystem.ClearCurAction();
        actionSystem.StartAction(playerModel.furyTime, playerModel.furyWindow);
        fightFSM.ChangeState(FightState.Fury);
        yield break;
    }

    public IArchitecture GetArchitecture()
    {
        return Main.Interface;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 0.5f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(smashCenter.position, 0.8f);
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, 0.8f);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 2);
    }
}
