using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyGFX : MonoBehaviour
{
	public AIPath aiPath;
    public float scale;
    public bool faceRight;
    // 360

    void Start()
    {
        transform.rotation = Quaternion.Euler(0,0,270);
    }

    // Update is called once per frame
    void Update()
    {
        if(aiPath.desiredVelocity.x >= 0.01f) // moving to the right
        {
            faceRight = true;
        	transform.localScale = new Vector3(scale, -scale, scale);
            //transform.rotation = Quaternion.Euler(0,0,90);
        }
        else if(aiPath.desiredVelocity.x <= -0.01f) // moving to the left
        {
            faceRight = false;
        	transform.localScale = new Vector3(scale, scale, scale);
            //transform.rotation = Quaternion.Euler(0,0,270);
        }
    }

    public bool facingRight()
    {
        return faceRight;
    }
}
