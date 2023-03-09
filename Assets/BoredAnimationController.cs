using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoredAnimationController : StateMachineBehaviour
{
    [SerializeField]
    private float numAnimations;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float animation = Mathf.Floor(Random.Range(0, this.numAnimations));
        animator.SetFloat("Bored Animation", animation);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.normalizedTime >= 1f || animator.GetBool("Is Moving") || animator.GetBool("Is Jumping") || animator.GetBool("Is Falling") || animator.GetBool("Is Attacking"))
        {
            animator.SetBool("Is Bored", false);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Is Bored", false);
    }
}
