using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Explotion : StateMachineBehaviour
{
    bool canhit;
    float timer;
    Collider2D[] colliders;
    PlayerAttackToAddSlotEvent addEvent;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        canhit = true;
        timer = Main.Interface.GetModel<PlayerModel>().explotionHitTime;
        addEvent.shootDelta = 0;
        addEvent.furyDelta = 0.1f;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (timer > 0) timer -= Time.deltaTime;
        else if (canhit)
        {
            colliders = Physics2D.OverlapCircleAll(animator.transform.Position2D(), Main.Interface.GetModel<PlayerModel>().explotionRadius, LayerMask.GetMask("Enemy"));
            canhit = false;
            for (int i = 0; i < colliders.Length; i++)
            {
                TypeEventSystem.Global.Send(addEvent);
                colliders[i].transform.GetComponent<Enemy>().BeHited(Enemy.BeHitedType.ChargeShoot);
                if (colliders[i].transform.TryGetComponent<Xeno>(out Xeno xeno))
                    xeno.BeShooted(true, animator.transform.Position2D());
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Destroy(animator.gameObject);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
