using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Pathfinding;

public class Projectile : MonoBehaviour
{
	public Rigidbody2D rb;
    
    public GameObject returnPrefab;
    public GameObject corpsePrefab;
    public GameObject releasePrefab;
    public GameObject explosion;
    public GameObject impactFX;
    private bool expired;
    private bool piercing;
    //private bool tempPiercing;
    private bool homing;
    private bool spinRight;

    private float angleSpin;
    private float angle;

    private GameObject playerObj;
    private string typeAttack;
    private string damageType;

	public float speed;
    public float lifetimeMax;
    private float lifetime;
	public float damage;
	public float impact;
    public string[] effects;


    public bool enemyShot;
	private Vector2 direction;
    private Transform deathTransform;
    private Vector3 deathPos;
    private Quaternion deathRot;

    private int targetLayer;
    private int shooterLayer;

    public bool returningWeapon;
    private bool dead;
    private List<GameObject> projCorpses = new List<GameObject>();

    public float homingRange;
    public float homingRotateSpeed;

    // Start is called before the first frame update
    void Start()
    {
        //ai = GetComponent<AIPath>();
        //GetComponent<AIDestinationSetter>().target = null;

        rb.velocity = transform.up * speed; //transform.right
        direction = transform.up * speed;
        dead = false;
        expired = false;
        homing = true;
        spinRight  = (Random.value > 0.5f);
        angleSpin = 20;
        lifetime = lifetimeMax;

        //foundTarget = false;

        playerObj = GameObject.Find("Player");

        if (enemyShot == true) 
        {
            shooterLayer = 8; 
        }
        else
        {
            shooterLayer = 10;
        }
        
    }

    void Update()
    {
        lifetime -= 0.2f;  //0.2f
        if (lifetime <= 0 && !dead)
        {
            expired = true;
            //Debug.Log("Death instance 1");
            ProjDeath();
        }
        ActiveProjEffects();
    }

    private void ActiveProjEffects()
    {
        if(effects.Contains("Voltaic"))
        {
            randomDrift(gameObject, 15f);
        }
        if(effects.Contains("Drag"))
        {
            rb.velocity = rb.velocity * 0.985f;
        }
        if (effects.Contains("Spinning"))
        {
            if (effects.Contains("Drag"))
            {
                angleSpin = angleSpin * 0.985f;
            }
            if (spinRight)
            {
                angle += angleSpin;
                if(angle > 360f)
                {
                    angle = 0f;
                }
            }
            else
            {
                angle -= angleSpin;
                if(angle < 0f)
                {
                    angle = 360f;
                }
            }
            transform.rotation = Quaternion.Euler(new Vector3(0,0,angle));
        }
        if(effects.Contains("Homing") && homing)
        {
            GameObject homingTarget = GameObject.FindGameObjectWithTag("Enemy");
            if (homingTarget != null)
            {
                float homingDist = Vector2.Distance(gameObject.transform.position, homingTarget.transform.position);
                if(homingDist <= homingRange)
                {
                    Vector3 targetDirection = homingTarget.transform.position - transform.position;
                    Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, homingRotateSpeed * Time.deltaTime, 0.0f);
                    transform.rotation = Quaternion.LookRotation(newDirection);
                    rb.velocity = targetDirection * speed;
                }
            }
        }
        if(effects.Contains("Piercing"))
        {
            piercing = true;
        }
    }

    public void setSpeed(float speedVal)
    {
        speed = speedVal;
    }
    public void setRange(int rangeVal)
    {
        lifetimeMax = (float) rangeVal;
    }
    public void setDamage(float dmgVal)
    {
        damage = dmgVal;
    }
    public void setImpact(float impactVal)
    {
        impact = impactVal;
    }
    public void setEffects(string[] effectVal)
    {
        effects = effectVal;
    }

    // if enemy shot and target.layer is enemy, don't hit it.

    void OnTriggerEnter2D (Collider2D hitInfo)
    {
        targetLayer = hitInfo.gameObject.layer;

        if(targetLayer == 9 && piercing)
        {
            //Debug.Log("Death instance 2 " + targetLayer + " " + piercing);
            ProjDeath();
        }

        if (targetLayer != shooterLayer && targetLayer != 12)
        {
            Destructible target = hitInfo.GetComponent<Destructible>();
            if (target != null)
            {
                Vector3 hitPos = gameObject.transform.position;
                typeAttack = "physical";
                checkDamageType();
                target.TakeDamage(damage, direction, impact, hitPos, typeAttack);
                if(targetLayer == 8)
                {
                    target.GetComponent<EnemyAI>().ProjectileAggro();
                }
            }
            if(!dead && (!piercing || targetLayer == 9))// && !(effects.Contains("Homing") && targetLayer == 13))
            {
                //Debug.Log("Death instance 3 " + targetLayer + " " + piercing);
                ProjDeath();
            }
        }
    }

    void checkDamageType()
    {
        if(effects.Contains("Arcane"))
        {
            typeAttack = "arcane";
            damageType = "Arcane";
        }
        if(effects.Contains("Fire"))
        {
            typeAttack = "fire";
            damageType = "Fire";
        }
        if(effects.Contains("Poison"))
        {
            typeAttack = "poison";
            damageType = "Poison";
        }
        if(effects.Contains("Cold"))
        {
            typeAttack = "cold";
            damageType = "Cold";
        }
        if(effects.Contains("Light"))
        {
            typeAttack = "light";
            damageType = "Light";
        }
        if(effects.Contains("Dark"))
        {
            typeAttack = "dark";
            damageType = "Dark";
        }
        if(effects.Contains("Lightning"))
        {
            typeAttack = "lightning";
            damageType = "Lightning";
        }
    }

    void ProjDeath()
    {
        dead = true;
        rb.velocity = new Vector2(0,0);
        deathTransform = gameObject.transform;
        deathPos = gameObject.transform.position;
        deathRot = gameObject.transform.rotation;
        DeathEffect();

        Destroy(gameObject);
    }

    void DeathEffect()
    {
        if(effects.Contains("Returning"))
        {
            GameObject.Instantiate(returnPrefab, deathPos, deathRot);
        }
        if(effects.Contains("Dropping") && targetLayer != 8 && targetLayer != 10 && targetLayer != 11)
        {
            GameObject corpse = GameObject.Instantiate(corpsePrefab, deathPos, deathRot);
            playerObj.GetComponent<Player>().addProjCorpse(corpse);
            playerObj.GetComponent<Player>().incProjCorpseAge();
            randomDrift(corpse, 10f);
        }
        if(effects.Contains("Explode"))
        {
            //circular detection thing like for melee attacks
            Instantiate(explosion, deathPos, deathRot);
        }
        if(effects.Contains("WallImpact"))
        {
            if(targetLayer == 9)
            {
                Instantiate(impactFX, deathPos, deathRot);
            }
        }
        if(effects.Contains("ExpireImpact"))
        {
            if(expired)
            {
                Instantiate(impactFX, deathPos, deathRot);
            }
        }
        if(effects.Contains("Release1"))
        {
            string[] releaseEffects = new string[]{"WallImpact", damageType, "Homing", "ExpireFX"};
            GetComponent<LaunchProjectile>().launch(releasePrefab, deathTransform, speed*2f,(int)lifetimeMax*5,damage,impact, releaseEffects);
        }
        if(effects.Contains("Release3"))
        {
            string[] releaseEffects = new string[]{"WallImpact", "Fan3", damageType, "Homing", "Piercing", "ExpireFX"};
            GetComponent<LaunchProjectile>().launch(releasePrefab, deathTransform, speed*2f,(int)lifetimeMax/2,damage/3.0f,impact/3f, releaseEffects);
        }

        // Explode
        // Poison
        // Mark
        // Slow
    }

    private void randomDrift(GameObject obj, float veloc)
    {
        Vector3 driftDir = GetRandomDir();
        obj.GetComponent<Dodge>().DodgeMove(driftDir, veloc, 0.7f);
    }

    private Vector3 GetRandomDir() 
    {
        return new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f,1f));
    }
}
