using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpecialAttacks : MonoBehaviour
{
	private GameObject playerObj;
	public Transform parent;

	public float lungeSpeed;
	public float lungeTime;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Lunge()
    {
    	playerObj = GameObject.Find("Player");
    	Vector3 playerDir = playerObj.transform.position - parent.transform.position;
    	parent.GetComponent<Dodge>().DodgeMove(playerDir, lungeSpeed, lungeTime);
    }
}
