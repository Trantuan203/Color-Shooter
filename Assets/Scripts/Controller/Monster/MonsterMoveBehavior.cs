using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMoveBehavior : StateMachineBehaviour
{
    internal MonsterController monsterCtrl;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        monsterCtrl = animator.GetComponent<MonsterController>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        monsterCtrl.MonsterMove();
        monsterCtrl.MonsterAttack();
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        monsterCtrl.rb2D.velocity = Vector2.zero;
        animator.ResetTrigger("IsAttacking");
    }
}
