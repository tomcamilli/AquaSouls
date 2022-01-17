using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dodge : MonoBehaviour
{
	Rigidbody2D rb;
    private Vector3 lastMoveDir;
    private float rollSpeed;
    private float rollFactor;
    private float rollThreshold;
    private Vector3 rollVeloc;
    public GameObject bubbles;

    private State state;
    private enum State
    {
        Normal,
        DodgeRollSliding,
    }

    // Start is called before the first frame update
    void Start()
    {
    	rb = GetComponent<Rigidbody2D>();
        state = State.Normal;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state) 
        {
        	case State.Normal:
        		break;
        	case State.DodgeRollSliding:
        		DodgeRollSlide();
        		break;
        }
    }
/*
    private bool CanMove(Vector3 dir, float distance)
    {
        return Physics2D.Raycast(transform.position, dir, distance).collider == null;
    }

    private bool TryDodge(Vector3 baseMoveDir, float distance)
    {
    	Vector3 moveDir = baseMoveDir;
    	bool canMove = CanMove(moveDir, distance);
    	if (!canMove) 
    	{
    		moveDir = new Vector3(baseMoveDir.x, 0f);
    		canMove = moveDir.x != 0f && CanMove(moveDir, distance);
    		if (!canMove)
    		{
    			moveDir = new Vector3(0f, baseMoveDir.y);
    			canMove = moveDir.y != 0f && CanMove(moveDir, distance);
    		}
    	}

    	if (canMove)
    	{
    		lastMoveDir = moveDir;
    		transform.position += moveDir * distance;
    		return true;
    	}
    	else
    	{
    		return false;
    	}
    }
*/
    //private Vector3 MoveDirection()
    //{
    //	if (rb.velocity)
    //}

    public void DodgeMove(Vector3 veloc, float dodgeSpeed, float dodgeFactor)
    {
    	rollSpeed = dodgeSpeed;
    	rollFactor = dodgeFactor;
    	rollThreshold = dodgeFactor/2f;
    	rollVeloc = veloc;
    	//if(veloc == Vector3.zero)
    	//{
    	//	rollVeloc = GetComponent<PlayerAttack>().attackDirection;
    	//}
    	state = State.DodgeRollSliding;

        //Vector3 beforeDashPos = transform.position;
    }

    private void DodgeRollSlide()
    {
        //Debug.Log("Dodge roll sliding!");
    	transform.position += rollVeloc * rollSpeed * Time.deltaTime;

        if(bubbles != null)
        {
            StartCoroutine(sendBubbles());
        }

    	rollSpeed -= rollSpeed * rollFactor * Time.deltaTime;
    	if (rollSpeed < rollThreshold) 
    	{
    		state = State.Normal;
    	}
    }

    IEnumerator sendBubbles()
    {
        yield return new WaitForSeconds(0.05f);
        Instantiate(bubbles, transform.position, Quaternion.identity);
    }

    // public void randomDrift()
    // {
    //     Vector3 driftDir = GetRandomDir();
    //     Debug.Log("Random drift!!!");
    //     DodgeMove(driftDir, 100f, 100f);
    // }

    // private Vector3 GetRandomDir() 
    // {
    //     return new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f,1f));
    // }


}
