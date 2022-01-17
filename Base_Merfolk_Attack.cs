using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base_Merfolk_Attack : StateMachineBehaviour
{
    private GameObject player;
    private MerfolkGFX gfx;
    private Transform target;
    private bool attack;
    private string attackType;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       player = animator.GetComponent<BodyPart>().parent;
       gfx = player.GetComponent<MerfolkGFX>();
       target = gfx.attackPos;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        attackType = gfx.attackAnim;
        if(gfx.attacking)
        {
            Debug.Log(attackType);
            animator.SetTrigger(attackType);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Debug.Log("Exiting state!!!");
        animator.ResetTrigger(attackType);
        //ResetAllTriggers(animator);
    }

    /*private void ResetAllTriggers(Animator animator)
    {
        foreach (var param in animator.parameters)
        {
            if (param.type == AnimatorControllerParameterType.Trigger)
            {
                animator.ResetTrigger(param.name);
            }
        }
    }*/
}
