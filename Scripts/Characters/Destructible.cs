using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pathfinding;

public class Destructible : MonoBehaviour
{
    Rigidbody2D rb;
    public AIPath aiPath;
    public Transform GFX;

    [Header("Particle Effects")]
    public GameObject blood;
    public GameObject arcane;
    public GameObject fire;
    public GameObject poison;
    public GameObject cold;
    public GameObject dark;
    public GameObject light;
    public GameObject lightning;
    private GameObject attackFX;
    public GameObject deathFX;
    private bool poisoned;
    private bool burning;

    [Header("Corpse Materials")]
    public Material normalMaterial;
    public Material arcaneMaterial;
    public Material fireMaterial;
    public Material poisonMaterial;
    public Material coldMaterial;
    public Material darkMaterial;
    public Material lightningMaterial;
    private Material corpseMaterial;

    [Header("Health")]
	public float health;
    private float maxHealth;

    public HealthBar healthBar;
    public float invulnerableDuration;
    private float invulnerableTime;

    [Header("Looting")]
    public float lootRadius;
    public int coinValue;
    public GameObject coin1;
    public GameObject coin5;
    public GameObject coin10;
    public GameObject coin25;
    public GameObject coin100;

    [Header("Corpse")]
    public bool leaveCorpse;
    public Transform corpse;
    public int corpseAge;

    private GameObject playerObj;

    private int receiverLayer;


    private State state;
    private enum State
    {
        Normal,
        Invulnerable,
    }

    // Start is called before the first frame update
    void Start()
    {
        state = State.Normal;
        rb = GetComponent<Rigidbody2D>();
        receiverLayer = gameObject.layer;
        playerObj = GameObject.Find("Player");
        maxHealth = health;
        poisoned = false;
        corpseMaterial = normalMaterial;
        attackFX = blood;
        invulnerableTime = invulnerableDuration;
        if(receiverLayer == 10)
        {
            healthBar.SetMaxHealth(maxHealth);
        }
        corpseAge = 0;
    }

    // Update is called once per frame
    void Update()
    {
        loot();
        switch (state)
        {
            case State.Normal:
                invulnerableTime = invulnerableDuration;
                break;
            case State.Invulnerable:
                invulnerableTime = invulnerableTime - 1;
                if(invulnerableTime <= 0)
                {
                    state = State.Normal;
                }
                break;
        }
        checkDeath();
        // checkAge();
    }

    public void TakeDamage(float damage, Vector2 direction, float impact, Vector3 pos, string typeAttack)
    {
        if(state != State.Invulnerable)
        {
            DetermineFX(typeAttack);
            if(typeAttack != "poison" && typeAttack != "fire")
            {
                Instantiate(attackFX, pos, Quaternion.identity);
            }
            Instantiate(attackFX, transform.position, Quaternion.identity);
        	health -= damage;
            rb.AddForce(direction * impact);
        	if(receiverLayer == 10)
            {
                healthBar.SetHealth(health);
                state = State.Invulnerable;
            }
            if(typeAttack == "poison" && damage > 0f && !poisoned)
            {
                StartCoroutine(poisonDamage(damage, direction, 0f, transform.position, typeAttack));
            }
            if(typeAttack == "fire" && damage > 0f && !burning)
            {
                StartCoroutine(fireDamage(damage, direction, 0f, transform.position, typeAttack));
            }
        }
    }

    private void DetermineFX(string typeAttack)
    {
        switch(typeAttack)
        {
            case "physical":
                attackFX = blood;
                corpseMaterial = normalMaterial;
                break;
            case "arcane":
                attackFX = arcane;
                corpseMaterial = arcaneMaterial;
                break;
            case "fire":
                attackFX = fire;
                corpseMaterial = fireMaterial;
                break;
            case "poison":
                attackFX = poison;
                corpseMaterial = poisonMaterial;
                break;
            case "cold":
                attackFX = cold;
                corpseMaterial = coldMaterial;
                break;
            case "dark":
                attackFX = dark;
                corpseMaterial = darkMaterial;
                break;
            case "light":
                attackFX = light;
                break;
            case "lightning":
                attackFX = lightning;
                corpseMaterial = lightningMaterial;
                break;
            default:
                Debug.Log("Error");
                break;
        }
    }

    IEnumerator poisonDamage(float damage, Vector2 direction, float impact, Vector3 pos, string typeAttack)
    {
        Debug.Log("Poisoned enemy for " + damage);
        poisoned = true;
        yield return new WaitForSeconds(2f);
        poisoned = false;
        TakeDamage(damage-1f, direction, impact, pos, typeAttack);
    }

    IEnumerator fireDamage(float damage, Vector2 direction, float impact, Vector3 pos, string typeAttack)
    {
        Debug.Log("Burned enemy for " + damage);
        burning = true;
        yield return new WaitForSeconds(1f);
        burning = false;
        TakeDamage(damage-2f, direction, impact, pos, typeAttack);
    }

    private void loot()
    {
        float distFromOpponent = Vector2.Distance(playerObj.transform.position, gameObject.transform.position);
        if (receiverLayer != 10 && receiverLayer != 8 && coinValue != 0 && distFromOpponent <= lootRadius)//(Input.GetKeyDown(KeyCode.X))
        {
            if (Input.GetKeyDown(KeyCode.X))//(receiverLayer != 10 && receiverLayer != 5 && coinValue != 0 && distFromOpponent <= 1.5)
            {
                dropCoins();
                coinValue = 0;
            }
        }
    }

    // private void checkAge()
    // {
    //     if(corpseAge > 5)
    //     {
    //         //Debug.Log(corpseAge);
    //         Destroy(gameObject);
    //     }
    // }

    private void checkDeath()
    {
        if(leaveCorpse == true && health <= 0f)
        {
            // Create corpse.
            //Debug.Log("Creating corpse");
            Vector3 deathPos = gameObject.transform.position;
            //Quaternion deathRot = gameObject.transform.rotation;
            Vector3 deathRot = gameObject.transform.rotation.eulerAngles;
            deathRot = new Vector3(deathRot.x,deathRot.y,deathRot.z-90);

            if(receiverLayer == 10)
            {
                // Respawn point
                gameObject.transform.position = Vector3.zero;
                // Reset player info
                health = maxHealth;
                healthBar.SetHealth(health);
                // Drop coins
                coinValue = CoinManager.instance.GetCoins();
                CoinManager.instance.ClearCoins();
            }
            else
            {
                Destroy(gameObject);
            }
            // 0.2
            bool faceRight = false;
            float s = 1;
            if(receiverLayer == 8)
            {
                faceRight = GFX.GetComponent<EnemyGFX>().facingRight();
                s = GFX.GetComponent<EnemyGFX>().scale - 0.25f;// -1.5f;
            }
            if(faceRight) // moving to the right
            {
                corpse.transform.localScale = new Vector3(s, -s, 1);
            }
            else // moving to the left
            {
                corpse.transform.localScale = new Vector3(s, s, 1);
            } 

            corpse.GetComponent<SpriteRenderer>().material = corpseMaterial;
            corpse.GetComponent<Destructible>().setCoins(coinValue);
            Instantiate(corpse, deathPos, Quaternion.Euler(deathRot));
            corpse.transform.localScale = new Vector3(s, s, s);
        }
        else if (leaveCorpse == false && health <= 0f)
        {
            dropCoins();
            if(deathFX != null)
            {
                Instantiate(deathFX, gameObject.transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }

    public void setCoins(int val)
    {
        coinValue = val;
    }

    public void dropCoins()
    {
        int v = coinValue;
        Debug.Log("Dropping coins! " + v);
        while(v >= 100)
        {
            Instantiate(coin100, gameObject.transform.position, Quaternion.identity);
            v -= 100;
        }
        while(v >= 25)
        {
            Instantiate(coin25, gameObject.transform.position, Quaternion.identity);
            v -= 25;
        }
        while(v >= 10)
        {
            Instantiate(coin10, gameObject.transform.position, Quaternion.identity);
            v -= 10;
        }
        while(v >= 5)
        {
            Instantiate(coin5, gameObject.transform.position, Quaternion.identity);
            v -= 5;
        }
        while(v > 0)
        {
            Instantiate(coin1, gameObject.transform.position, Quaternion.identity);
            v -= 1;
        }
        //Instantiate(spawn1, Vector3.zero, Quaternion.identity);
    }
    private Vector3 GetRandomDir() 
    {
        return new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f,1f));
    }
}
