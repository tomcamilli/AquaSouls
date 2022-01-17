using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish_Sample_Lunge : StateMachineBehaviour
{

    //private Transform player;
    //private Rigidbody2D rb;
    //public Rigidbody2D rb;
    //public Transform enemy;
    //public GameObject bubbles;

    //public float lungeSpeed;
    //public float lungeTime;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //player = GameObject.Find("Player").transform;
        //rb = animator.GetComponent<Rigidbody2D>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<EnemySpecialAttacks>().Lunge();
        //StartCoroutine(sendBubbles());
        /*Vector3 playerDir1 = player.position - enemy.position;
        Vector3 playerDir2 = player.position - rb.transform.position;

        Debug.Log("Lunging " + playerDir1 + " " + playerDir2);

        //Debug.Log("Lunging!!!" + " " + playerDir + " " + lungeSpeed + " " + Time.deltaTime);

        enemy.position += playerDir2 * lungeSpeed * Time.deltaTime; */
        //enemy.GetComponent<Dodge>().DodgeMove(playerDir, lungeSpeed, lungeTime);
        //Vector3 playerDir = player.position - rb.transform.position;
        //rb.transform.GetComponent<Dodge>().DodgeMove(playerDir, lungeSpeed, lungeTime);
        //rb.transform.position += playerDir * lungeSpeed * Time.deltaTime;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
    }

    //IEnumerator sendBubbles()
    //{
    //    yield return new WaitForSeconds(0.05f);
    //    Instantiate(bubbles, enemy.transform.position, Quaternion.identity);
    //}
}
