using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnimationAi : StateMachineBehaviour
{
    AIController aiController;

    /// <summary>
    /// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state.
    /// It finds the CharacterAttack component and sets IsAttacking to true.
    /// </summary>
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        aiController = animator.GetComponentInParent<Transform>().GetComponentInParent<Transform>().GetComponentInParent<AIController>();
        aiController.SetIsAttacking(true);
    }

    /// <summary>
    /// OnStateExit is called when a transition ends and the state machine finishes evaluating this state.
    /// It sets IsAttacking to false.
    /// </summary>
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        aiController.SetIsAttacking(false);
    }
}