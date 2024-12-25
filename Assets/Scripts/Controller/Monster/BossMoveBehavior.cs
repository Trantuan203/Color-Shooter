using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMoveBehavior : StateMachineBehaviour
{
    internal BossController bossCtrl;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bossCtrl = animator.GetComponent<BossController>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bossCtrl.MonsterMove();
        bossCtrl.MonsterAttack();
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bossCtrl.rb2D.velocity = Vector2.zero;
        animator.ResetTrigger("IsAttacking");
    }
}
