using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    IAstarAI ai;
    Rigidbody2D rb;
    private GameObject playerObj;
    private GameObject startSpot;

    public Transform attackPos;
    public GameObject projPrefab;

	private Vector3 startingPosition;
    //private Vector3 roamingPosition;

    // TO PATROL: Move around the Start Spot

    private float distFromOpponent;
    public float detectionRange; //5?

    private float highSpeed; // 2.5
    private float lowSpeed;
    //private float rotSpeed;
    

    private float timeBtwDodge;
    [Header("Dodging")]
    public float startTimeBtwDodge;
    public float dodgeTime;

    private float timeBtwAttack;
    [Header("Active Combat Abilities")]
    public float startTimeBtwAttack;
    public bool shooter;
    public float collisionDamage;
    private int colliderLayer;

    [Header("Active Movement Abilities")]
    public float moveAbilityRange; // distance to opponent in which AI retreats/lunges/flanks
    public bool cowardly;
    public bool flanker;

    [Header("Passive Abilities")]
    public bool crawl;
    public float crawlRadius;
    public LayerMask crawlOnThings;
    public bool driftEnabled;
    private bool drifting;
    private float timeBtwDrift;
    private float startTimeBtwDrift = 1.7f;

    // State Machine/enum - Use "distFromPlayer", if player is out of range, don't engage.
        // Passive - will either patrol, wait, drift about, or roam.
        // Active - will chase and fight the player.

    // HELPER FUNC "distFromPlayer": Determine how far away the player is from the enemy.
    // FUNC - "Retreat" - If a ranger, will try to dodge away from nearby opponents.
    // FUNC - "Flank" - If a flanker, will try to dodge around the side of an opponent.
    // FUNC - "Melee" - Will try to perform a melee attack on a nearby opponent.
    // FUNC - "Shoot" - Will try to launch a projectile at a distant opponent.

    private State state;
    private enum State
    {
        Passive,
        Active,
    }

    private void Start()
    {
        state = State.Passive;
        ai = GetComponent<AIPath>();
        rb = GetComponent<Rigidbody2D>();
        playerObj = GameObject.Find("Player");

    	startingPosition = transform.position;
        startSpot = new GameObject();
        startSpot.transform.position = startingPosition;

        highSpeed = ai.maxSpeed;
        lowSpeed = ai.maxSpeed/2;
        //rotSpeed = ai.rotationSpeed;

        //roamPosition = GetRoamingPosition();
    }

    private void Update()
    {
        distFromOpponent = Vector2.Distance(playerObj.transform.position, transform.position);
        if(distFromOpponent >= detectionRange)
        {
            // OUT OF RANGE: PASSIVE
            state = State.Passive;
            ai.maxSpeed = lowSpeed;
            //ai.rotationSpeed = 0f;
            drifting = true;
            if(transform.position != startingPosition)
            {
                GetComponent<AIDestinationSetter>().target = startSpot.transform;
            }
        }
        else
        {
            // WITHIN RANGE: ACTIVE
            state = State.Active;
            //ai.canSearch = true;
            ai.maxSpeed = highSpeed;
            //ai.rotationSpeed = rotSpeed;
            drifting = false;
            GetComponent<AIDestinationSetter>().target = playerObj.transform;
            checkActiveAbilities();
        }
        checkPassiveAbilities();
    }

    public void ProjectileAggro()
    {
        detectionRange = detectionRange * 2;
    }

    private void checkPassiveAbilities()
    {
        if(driftEnabled == true) { Drift(); }
        Collider2D[] nearbyCrawls = Physics2D.OverlapCircleAll(transform.position, 10, crawlOnThings);
        float distFromCrawl = Vector2.Distance(nearbyCrawls[0].transform.position, transform.position);
        if(crawl == true && distFromCrawl > crawlRadius) 
        { 
            Vector3 crawlDir = nearbyCrawls[0].transform.position - transform.position;
            GetComponent<Dodge>().DodgeMove(crawlDir, highSpeed, dodgeTime);
        }
    }

    private void Drift()
    {
        if (timeBtwDrift <= 0)
        {
            if(drifting == true)
            {
                Vector3 driftDir = GetRandomDir();
                //Debug.Log(lowSpeed); 0.5f
                GetComponent<Dodge>().DodgeMove(driftDir, lowSpeed, 0.7f); //rb.mass, lowSpeed
                timeBtwDrift = startTimeBtwDrift;
            }
        }
        else
        {
            timeBtwDrift -= Time.deltaTime;
        }
    }

    private void checkActiveAbilities()
    {
        if(distFromOpponent <= moveAbilityRange)
        {
            if(cowardly == true) { Retreat(); }
        }
        if(shooter == true) { Shoot(); } // some logic when to shoot (certain distance?)
    }

    private void Retreat()
    {
        if (timeBtwDodge <= 0)
        {
            Vector3 oppDir = transform.position - playerObj.transform.position;
            GetComponent<Dodge>().DodgeMove(oppDir, highSpeed*2f, dodgeTime);
            timeBtwDodge = startTimeBtwDodge;
        }
        else
        {
            timeBtwDodge -= Time.deltaTime;
        }
    }

    private void Shoot()
    {
        string[] effects = new string[]{""};
        if (timeBtwAttack <= 0)
        {
            Vector3 fromEnemyToPlayer = (playerObj.transform.position - transform.position).normalized;
            float dotProd = Vector2.Dot(fromEnemyToPlayer, transform.right);
            //Debug.Log(dotProd);
            if(dotProd < 0.9f)
            {
                // Increase shooter turn speed?
                GetComponent<LaunchProjectile>().launch(projPrefab, attackPos, 10f, 5, 5, 10f, effects); // fromEnemyToPlayer
                timeBtwAttack = startTimeBtwAttack;
            }
        }
        else
        {
            timeBtwAttack -= Time.deltaTime;
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
            //collision.gameObject.GetComponent<Destructible>().TakeDamage(collisionDamage, playerDir, rb.mass*1.0f, playerObj.transform.position, "physical");
            GameObject parentCollision = collision.gameObject.GetComponent<BodyPart>().parent;
            parentCollision.GetComponent<Destructible>().TakeDamage(collisionDamage, playerDir, rb.mass*1.0f, playerObj.transform.position, "physical");
        }
    }
}
