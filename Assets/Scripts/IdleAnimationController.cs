using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleAnimationController : StateMachineBehaviour
{
    [SerializeField]
    private float timeUntilBored;
    private float lastIdleTime;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this.lastIdleTime = Time.time;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Time.time - this.lastIdleTime >= this.timeUntilBored)
        {
            animator.SetBool("Is Bored", true);
        }
    }
}
