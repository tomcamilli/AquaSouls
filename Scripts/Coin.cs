using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Coin : MonoBehaviour
{
	IAstarAI ai;
    Rigidbody2D rb;
    private GameObject playerObj;

    public int coinValue;

    private float distFromPlayer;
    public float detectionRange; //5?

    private State state;
    private enum State
    {
        Passive,
        Active,
    }

    // Start is called before the first frame update
    void Start()
    {
        state = State.Passive;
        ai = GetComponent<AIPath>();
        rb = GetComponent<Rigidbody2D>();
        playerObj = GameObject.Find("Player");

        Vector3 startDir = GetRandomDir();
       	GetComponent<Dodge>().DodgeMove(startDir, ai.maxSpeed/8, 0.7f);
    }

    // Update is called once per frame
    void Update()
    {
        distFromPlayer = Vector2.Distance(playerObj.transform.position, transform.position);
        if(distFromPlayer >= detectionRange)
        {
            // OUT OF RANGE: PASSIVE
            state = State.Passive;
            GetComponent<AIDestinationSetter>().target = transform;
        }
        else
        {
            // WITHIN RANGE: ACTIVE
            state = State.Active;
            GetComponent<AIDestinationSetter>().target = playerObj.transform;
        }
    }

    private Vector3 GetRandomDir() 
    {
    	return new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f,1f));
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        Vector3 playerDir = playerObj.transform.position - transform.position;
        //colliderLayer = collision.gameObject.layer;
        if(collision.gameObject.layer == 10)
        {
        	CoinManager.instance.AddCoins(coinValue);
        	Destroy(gameObject);
        	//collision.gameObject.GetComponent<Player>().UpdateCoins(coinValue);
        }
    }
}
