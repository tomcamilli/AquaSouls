using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish_Sample_Move : StateMachineBehaviour
{

    private Transform player;
    //public Rigidbody2D rb;

    public float lungeRange;
    private float timeBtwLunge;
    public float startTimeBtwLunge;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       player = GameObject.Find("Player").transform;
       //rb = animator.GetComponent<Rigidbody2D>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Debug.Log(player.position + " " + animator.transform.position + " " + Vector2.Distance(player.position, rb.transform.position));
        if (Vector2.Distance(player.position, animator.transform.position) <= lungeRange)
        {
            //Debug.Log("Lunging distance!!!");
            if(timeBtwLunge <= 0)
            {
                //Debug.Log("LUNGE");
                animator.SetTrigger("Lunge");
                timeBtwLunge = startTimeBtwLunge;
            }
            else
            {
                timeBtwLunge -= Time.deltaTime;
            }
            
        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Lunge");
    }
}
