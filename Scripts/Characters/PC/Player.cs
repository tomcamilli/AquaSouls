using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

	Rigidbody2D rb;
    public Camera cam;
    private Vector2 mousePos;

	private float timeBtwAttack;
	private float startTimeBtwAttack;
    private bool shift;
    private string[] effects;

    [Header("Movement")]
    public float moveSpeed = 4f;
    private Vector3 veloc;
    private float timeBtwDodge;
    public float startTimeBtwDodge;
    public float dodgeTime;

    [Header("Focus")]
    public float focus;
    private bool focusInUse;
    private bool stunned;
    private float maxFocus;
    public FocusBar focusBar;

    [Header("Attack Positions")]
    public Transform attackPos0;
    public Transform attackPos1;
    public Transform attackPos2;
    public Transform attackPos3;
    public Transform attackPos4;
    public Transform attackPos5;
    public Transform attackPos6;
    public Transform attackPos7;
    public Transform attackPos8;
    public Transform attackPos9;
    private Transform attackPos;
    public Transform shield;
    public GameObject fireImpact;
    public GameObject arcaneImpact;
    public GameObject darkImpact;
    public GameObject lightImpact;
    private GameObject impactFX;
    private bool impactEffect;
    private string damageType;
    private bool lightRelease;

    private float damageFactor;
    private float speedFactor;
    //public GameObject[] spawns;
    private List<GameObject> projCorpses = new List<GameObject>();

    private string weapon1;
    private string weapon2;
    private int whichHand;
    //private string spellColor;
	public LayerMask whatIsEnemies;

    [Header("Projectile Options")]
    public GameObject oval;
    public GameObject dagger;
    public GameObject javelin;
    public GameObject bolt;
    public GameObject firebolt;
    public GameObject flintlockBullet;
    public GameObject blunderbussBullet;
    public GameObject musketBullet;
    public GameObject arcaneBoltLight;
    public GameObject arcaneBoltHeavy;
    public GameObject arcaneBoltSmall;
    public GameObject arcaneOrb;
    public GameObject arcaneOrbSmall;
    public GameObject fire;
    public GameObject poison;
    public GameObject cold;
    public GameObject light;
    public GameObject lightScatter;
    public GameObject lightningLight;
    public GameObject lightningHeavy;
    private GameObject prefabProj;

    private bool harpoonShot;
    private bool gunReload;

	private float attackX;
	private float attackY;
    private Vector2 attackDirection;

	void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        DetermineWeapon(0);
        attackPos = attackPos2;
        prefabProj = oval;
        damageFactor = 1f;
        speedFactor = 1f;
        maxFocus = focus;
        focusInUse = false;
        focusBar.SetMaxFocus(maxFocus);

        harpoonShot = false;
        gunReload = false;
        impactEffect = false;
        lightRelease = false;
        //Debug.Log(attackPos.transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        
        moveUpdate();
        focusUpdate();
        attackUpdate();
        
    }

    void focusUpdate()
    {
        if(!stunned && !focusInUse && focus <= maxFocus)
        {
            focus += 0.2f; // 0.07 0.15
            focusBar.SetFocus(focus);
        }
        if(focus <= 0)
        {
            focus = 0;
            StartCoroutine(stunFocus());
        }
    }

    void attackUpdate()
    {
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            shift = true;
        }
        else
        {
            shift = false;
        }

        if(lightRelease && (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1)))
        {
            lightAttack();
            lightRelease = false;
        }

        ConfigAttackPos();
        if (timeBtwAttack <= 0 && focus > 0f)
        {
            //you can attack
            if(Input.GetMouseButton(0)) 
            {
                DetermineWeapon(1);
                whichHand = 2;
                Attack(weapon2);

                //Attack();

                timeBtwAttack = startTimeBtwAttack;
            }
            else if(Input.GetMouseButton(1))
            {
                DetermineWeapon(0);
                whichHand = 1;
                Attack(weapon1);

                //GetComponent<LaunchProjectile>().launch(attackDirection);

                timeBtwAttack = startTimeBtwAttack;
            }
            else if (shield.GetComponent<SpriteRenderer>().enabled == true)
            {
                shield.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
        else
        {
            timeBtwAttack -= Time.deltaTime;
        }
    }

    void moveUpdate()
    {
        Move();
        if (timeBtwDodge <= 0)
        {
            if(Input.GetKeyDown("space") && focus > 0f)
            {
                GetComponent<Dodge>().DodgeMove(veloc, moveSpeed*2f, dodgeTime);
                timeBtwDodge = startTimeBtwDodge;
                StartCoroutine(decFocus(10f));
            }
        }
        else
        {
            timeBtwDodge -= Time.deltaTime;
        }
    }

    void Move() 
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        veloc = new Vector3(x, y, 0);
        rb.velocity = veloc * moveSpeed;

    }

    public Vector3 movementDirection()
    {
        return veloc;
    }

    void ConfigAttackPos()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        Vector2 lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;
    }

    void DetermineWeapon(int num)
    {
        if (num == 0)
        {
            weapon1 = WeaponManager.instance.getWeapon(0);
            //Debug.Log(weapon1);
        }
        else if (num == 1)
        {
            weapon2 = WeaponManager.instance.getWeapon(1);
            //Debug.Log(weapon2);
        }
    }

    /*void DetermineSpellColor()
    {
        spellColor = WeaponManager.instance.getSpellColor();
    }*/

    void lightAttack()
    {
        startTimeBtwAttack = 0.8f + speedFactor;
        attackPos = attackPos6;
        prefabProj = light;
        StartCoroutine(decFocus(10f));
        effects = new string[]{"Light", "WallImpact", "ExpireImpact"};
        RangedAttack(10.0f,8,3f * damageFactor,10.0f, effects);
        Recoil(1.5f, 1.5f);
    }

    public void addProjCorpse(GameObject corpse)
    {
        projCorpses.Add(corpse);
        //corpse.GetComponent<Dodge>().randomDrift();
        //corpse.GetComponent<Dodge>().DodgeMove(veloc, moveSpeed*2f, dodgeTime);
        //corpse.GetComponent<Destructible>().health -= 1;
    }
    public void incProjCorpseAge()
    {
        for(int i=0;i<projCorpses.Count; ++i)
        {
            GameObject c = projCorpses[i];
            if (c != null)
            {
                c.GetComponent<Destructible>().corpseAge++;
                //Debug.Log(c.GetComponent<Destructible>().corpseAge);
                if(c.GetComponent<Destructible>().corpseAge > 2)
                {
                    removeProj(c);
                }
            }
        }
    }
    public void removeProj(GameObject pCorpse)
    {
        projCorpses.Remove(pCorpse);
        Destroy(pCorpse);
    }

    /*void changeReach(float num)
    {
        Vector3 newPos = new Vector3(attackPos.transform.position.x, )
        //attackPos.transform.position = new Vector3(0, num, 0);
        //attackPos.transform.position.y = num;
        Debug.Log(attackPos.transform.position);
    } */

    IEnumerator stunFocus()
    {
        // something else bad happens
            // - you stun yourself?
            // - fatigue - lower your max focus until rest?
        stunned = true;
        yield return new WaitForSeconds(2);
        stunned = false;
    }

    IEnumerator decFocus(float amount)
    {
        focus -= amount;
        focusBar.SetFocus(focus);
        //Debug.Log("Decreased focus to " + (int) focus + ", " + focusInUse);
        if(!focusInUse)
        {
            focusInUse = true;
            yield return new WaitForSeconds(1f);
            focusInUse = false;
        }
    }

    void Attack(string weapon)
    {
        if(shift)
        {
            weapon = weapon + " (Secondary)";
        }
        bool twohand = WeaponManager.instance.getTwoHand();
        bool dualwield = WeaponManager.instance.getDualWield();
        if (twohand)
        {
            damageFactor += 2f;
        }
        if (dualwield)
        {
            speedFactor -= 0.2f;
        }
        //Debug.Log(weapon);
        //string[] effects;
        damageType = "physical";

        // Arms have a type of attack (light jab, etc.)
        // Hands have a weapon in them.
        switch(weapon)
        {
            case "Empty":
                startTimeBtwAttack = 0.3f + speedFactor; // 0.8f
                attackPos = attackPos1;
                StartCoroutine(decFocus(4f));
                impactEffect = false;
                GetComponent<MerfolkGFX>().attackAnimation(whichHand, weapon); // Jab
                MeleeAttack(0.6f, 1f * damageFactor, 2);
                Recoil(2f, 1.5f);
                break;
            case "Empty (Secondary)":
                startTimeBtwAttack = 0.9f + speedFactor;
                attackPos = attackPos1;
                StartCoroutine(decFocus(8f));
                impactEffect = false;
                GetComponent<MerfolkGFX>().attackAnimation(whichHand, weapon); // Swipe
                MeleeAttack(0.6f, 3f * damageFactor, 3);
                Recoil(2.5f, 1.5f);
                break;
            case "Spear":
                startTimeBtwAttack = 0.8f + speedFactor;
                attackPos = attackPos3; // 3
                StartCoroutine(decFocus(6f));
                impactEffect = false;
                GetComponent<MerfolkGFX>().attackAnimation(whichHand, weapon); // Heavy Jab (more windup)
                MeleeAttack(0.6f, 2f * damageFactor, 2);
                Recoil(1.5f, 1.5f);
                break;
            case "Spear (Secondary)":
                startTimeBtwAttack = 0.9f + speedFactor;
                attackPos = attackPos3;
                StartCoroutine(decFocus(12f));
                impactEffect = false;
                GetComponent<MerfolkGFX>().attackAnimation(whichHand, weapon); // Jab
                MeleeAttack(0.6f, 4f * damageFactor, 3);
                Recoil(2f, 1.5f);
                break;
            case "Dagger":
                startTimeBtwAttack = 0.7f + speedFactor;
                attackPos = attackPos2;
                StartCoroutine(decFocus(5f));
                impactEffect = false;
                GetComponent<MerfolkGFX>().attackAnimation(whichHand, weapon); // Jab
                MeleeAttack(0.6f, 1f * damageFactor, 2);
                Recoil(1.5f, 1.5f);
                break;
            case "Dagger (Secondary)":
                startTimeBtwAttack = 0.8f + speedFactor;
                attackPos = attackPos5;
                prefabProj = dagger;
                StartCoroutine(decFocus(10f));
                effects = new string[]{"Dropping", "Spinning", "Drag"};
                GetComponent<MerfolkGFX>().attackAnimation(whichHand, weapon); // Throw
                RangedAttack(10.0f,20,2f * damageFactor,10.0f, effects);
                Recoil(2f, 1.5f);
                break;
            case "Sword":
                startTimeBtwAttack = 0.8f + speedFactor;
                attackPos = attackPos2;
                StartCoroutine(decFocus(7f));
                impactEffect = false;
                GetComponent<MerfolkGFX>().attackAnimation(whichHand, weapon); // Jab
                MeleeAttack(0.6f, 3f * damageFactor, 2);
                Recoil(1.5f, 1.5f);
                break;
            case "Sword (Secondary)":
                startTimeBtwAttack = 0.9f + speedFactor;
                attackPos = attackPos2;
                StartCoroutine(decFocus(14f));
                impactEffect = false;
                GetComponent<MerfolkGFX>().attackAnimation(whichHand, weapon); // Swipe
                MeleeAttack(1.2f, 5f * damageFactor, 2);
                Recoil(2f, 1.5f);
                break;
            case "Trident":
                startTimeBtwAttack = 0.8f + speedFactor;
                attackPos = attackPos4;
                StartCoroutine(decFocus(9f));
                impactEffect = false;
                GetComponent<MerfolkGFX>().attackAnimation(whichHand, weapon); // Jab
                MeleeAttack(0.6f, 3f * damageFactor, 2);
                Recoil(1.2f, 1.5f);
                break;
            case "Trident (Secondary)":
                startTimeBtwAttack = 0.9f + speedFactor;
                attackPos = attackPos4;
                StartCoroutine(decFocus(18f));
                impactEffect = false;
                GetComponent<MerfolkGFX>().attackAnimation(whichHand, weapon); // Heavy Jab (more windup)
                MeleeAttack(0.6f, 5f * damageFactor, 2);
                Recoil(1.7f, 1.5f);
                break;
            case "Torch":
                startTimeBtwAttack = 0.8f + speedFactor;
                attackPos = attackPos2;
                StartCoroutine(decFocus(6f));
                impactEffect = true;
                impactFX = fireImpact;
                damageType = "fire";
                GetComponent<MerfolkGFX>().attackAnimation(whichHand, weapon); // Swipe
                MeleeAttack(1.2f, 2f * damageFactor, 2);
                Recoil(1.5f, 1.5f);
                break;
            case "Torch (Secondary)":
                startTimeBtwAttack = 0.9f + speedFactor;
                attackPos = attackPos2;
                StartCoroutine(decFocus(12f));
                impactEffect = true;
                impactFX = fireImpact;
                damageType = "fire";
                GetComponent<MerfolkGFX>().attackAnimation(whichHand, weapon); // Jab
                MeleeAttack(0.6f, 4f * damageFactor, 2);
                Recoil(2f, 1.5f);
                break;
            case "Chain Hook":
                startTimeBtwAttack = 0.7f + speedFactor;
                attackPos = attackPos2;
                StartCoroutine(decFocus(7f));
                impactEffect = false;
                GetComponent<MerfolkGFX>().attackAnimation(whichHand, weapon); // Swipe
                MeleeAttack(1.2f, 2f * damageFactor, 2);
                Recoil(1.5f, 1.5f);
                break;
            case "Chain Hook (Secondary)":
                startTimeBtwAttack = 0.8f + speedFactor;
                attackPos = attackPos2;
                prefabProj = oval;
                StartCoroutine(decFocus(14f));
                effects = new string[]{"Returning"};
                GetComponent<MerfolkGFX>().attackAnimation(whichHand, weapon); // Throw
                RangedAttack(8.0f,4,2f * damageFactor,8.0f, effects);
                Recoil(2f, 1.5f);
                break;
            case "Rapier":
                startTimeBtwAttack = 0.7f + speedFactor;
                attackPos = attackPos2;
                StartCoroutine(decFocus(7f));
                impactEffect = false;
                GetComponent<MerfolkGFX>().attackAnimation(whichHand, weapon); // Jab
                MeleeAttack(0.6f, 2f * damageFactor, 2);
                Recoil(1.5f, 1.5f);
                break;
            case "Rapier (Secondary)":
                StartCoroutine(decFocus(14f));
                GetComponent<MerfolkGFX>().attackAnimation(whichHand, weapon); // Parry
                shield.GetComponent<SpriteRenderer>().enabled = true;
                break;
            case "Anchor":
                startTimeBtwAttack = 0.9f + speedFactor;
                attackPos = attackPos3;
                StartCoroutine(decFocus(11f));
                impactEffect = false;
                GetComponent<MerfolkGFX>().attackAnimation(whichHand, weapon); // Swipe
                MeleeAttack(1.2f, 4f * damageFactor, 2);
                Recoil(1.5f, 1.5f);
                break;
            case "Anchor (Secondary)":
                startTimeBtwAttack = 0.9f + speedFactor;
                attackPos = attackPos2;
                StartCoroutine(decFocus(22f));
                impactEffect = false;
                GetComponent<MerfolkGFX>().attackAnimation(whichHand, weapon); // Heavy Jab
                MeleeAttack(0.6f, 5f * damageFactor, 2);
                Recoil(2f, 1.5f);
                break;
            case "Tentacle":
                startTimeBtwAttack = 0.7f + speedFactor;
                attackPos = attackPos3;
                StartCoroutine(decFocus(10f));
                impactEffect = false;
                GetComponent<MerfolkGFX>().attackAnimation(whichHand, weapon); // Swipe
                MeleeAttack(1.2f, 4f * damageFactor, 2);
                Recoil(1.5f, 1.5f);
                break;
            case "Tentacle (Secondary)":
                startTimeBtwAttack = 0.7f + speedFactor;
                attackPos = attackPos6;
                StartCoroutine(decFocus(20f));
                impactEffect = false;
                GetComponent<MerfolkGFX>().attackAnimation(whichHand, weapon); // Jab
                MeleeAttack(0.6f, 4f * damageFactor, 2);
                Recoil(1.5f, 1.5f);
                break;
            case "Javelin":
                startTimeBtwAttack = 0.8f + speedFactor;
                attackPos = attackPos4;
                prefabProj = javelin;
                StartCoroutine(decFocus(7f));
                effects = new string[]{"Dropping", "Drag"};
                GetComponent<MerfolkGFX>().attackAnimation(whichHand, weapon); // Throw
                RangedAttack(8.0f,20,2.5f * damageFactor,10.0f, effects);
                Recoil(1.5f, 1.5f);
                break;
            case "Javelin (Secondary)":
                startTimeBtwAttack = 0.8f + speedFactor;
                attackPos = attackPos2;
                StartCoroutine(decFocus(10f));
                impactEffect = false;
                GetComponent<MerfolkGFX>().attackAnimation(whichHand, weapon); // Heavy Jab
                MeleeAttack(0.6f, 3f * damageFactor, 2);
                Recoil(2f, 1.5f);
                break;
            case "Boltcaster":
                startTimeBtwAttack = 1.0f + speedFactor;
                attackPos = attackPos4;
                prefabProj = bolt;
                StartCoroutine(decFocus(8f));
                effects = new string[]{"Dropping", "Drag"};
                GetComponent<MerfolkGFX>().attackAnimation(whichHand, weapon); // Shoot
                RangedAttack(10.0f,20,2f * damageFactor,10.0f, effects);
                Recoil(1.5f, 1.5f);
                break;
            case "Boltcaster (Secondary)":
                startTimeBtwAttack = 1.0f + speedFactor;
                attackPos = attackPos4;
                prefabProj = firebolt;
                StartCoroutine(decFocus(16f));
                effects = new string[]{"Explode", "Fire", "Drag"};
                GetComponent<MerfolkGFX>().attackAnimation(whichHand, weapon); // Shoot
                RangedAttack(10.0f,9,3f * damageFactor,10.0f, effects);
                Recoil(2f, 1.5f);
                // keyword "explode" - explodes on death
                break;
            case "Harpoon Gun":
                if(!harpoonShot)
                {
                    startTimeBtwAttack = 1.0f + speedFactor;
                    attackPos = attackPos4;
                    prefabProj = javelin;
                    StartCoroutine(decFocus(10f));
                    effects = new string[]{"Returning"};
                    GetComponent<MerfolkGFX>().attackAnimation(whichHand, weapon); // Shoot
                    RangedAttack(10.0f,8,3f * damageFactor,10.0f, effects);
                    Recoil(1.5f, 1.5f);
                    // keyword "returning" which creates a projectile that returns to the player (like a coin) after not hitting an enemy
                    harpoonShot = true;
                }
                break;
            case "Harpoon Gun (Secondary)":
                harpoonShot = false;
                GetComponent<MerfolkGFX>().attackAnimation(whichHand, weapon); // Reel back/Reload
                StartCoroutine(decFocus(10f));
                break;
            case "Flintlock":
                if(!gunReload)
                {
                    startTimeBtwAttack = 0.3f + speedFactor; // 1.0f
                    attackPos = attackPos3;
                    prefabProj = flintlockBullet;
                    StartCoroutine(decFocus(8f));
                    effects = new string[]{"Piercing", "Gunshot"};
                    GetComponent<MerfolkGFX>().attackAnimation(whichHand, weapon); // Shoot
                    RangedAttack(25.0f,2,3f * damageFactor,13.0f, effects); // 1 duration
                    Recoil(1.5f, 1.5f);
                    gunReload = true;
                }
                break;
            case "Flintlock (Secondary)":
                gunReload = false;
                GetComponent<MerfolkGFX>().attackAnimation(whichHand, weapon); // Reload
                StartCoroutine(decFocus(8f));
                break;
            case "Blunderbuss":
                if(!gunReload)
                {
                    startTimeBtwAttack = 1.0f + speedFactor;
                    attackPos = attackPos3;
                    prefabProj = blunderbussBullet;
                    StartCoroutine(decFocus(10f));
                    effects = new string[]{"Fan5", "Gunshot"};
                    GetComponent<MerfolkGFX>().attackAnimation(whichHand, weapon); // Shoot
                    RangedAttack(20.0f,1,3f * damageFactor,15.0f, effects);
                    Recoil(1.5f, 1.5f);
                    gunReload = true;
                }
                break;
            case "Blunderbuss (Secondary)":
                gunReload = false;
                GetComponent<MerfolkGFX>().attackAnimation(whichHand, weapon); // Reload
                StartCoroutine(decFocus(10f));
                break;
            case "Musket":
                if(!gunReload)
                {
                    startTimeBtwAttack = 1.0f + speedFactor;
                    attackPos = attackPos3;
                    prefabProj = musketBullet;
                    StartCoroutine(decFocus(10f));
                    effects = new string[]{"Piercing", "Gunshot"};
                    GetComponent<MerfolkGFX>().attackAnimation(whichHand, weapon); // Shoot
                    RangedAttack(40.0f,4,4f * damageFactor,15.0f, effects);
                    Recoil(1.5f, 1.5f);
                    gunReload = true;
                }
                break;
            case "Musket (Secondary)":
                gunReload = false;
                GetComponent<MerfolkGFX>().attackAnimation(whichHand, weapon); // Reload
                StartCoroutine(decFocus(10f));
                break;
            case "Shield":
                GetComponent<MerfolkGFX>().attackAnimation(whichHand, weapon); // Block
                shield.GetComponent<SpriteRenderer>().enabled = true;
                break;
            case "Shield (Secondary)":
                startTimeBtwAttack = 0.8f + speedFactor;
                attackPos = attackPos2;
                prefabProj = oval;
                StartCoroutine(decFocus(10f));
                impactEffect = false;
                GetComponent<MerfolkGFX>().attackAnimation(whichHand, weapon); // Bash
                MeleeAttack(0.6f, 2f * damageFactor, 2);
                Recoil(1.5f, 1.5f);
                break;
            case "Staff":
                GetComponent<MerfolkGFX>().attackAnimation(whichHand, weapon); // Possibly whack or staff spellcast
                break;
            case "Staff (Secondary)":
                break;
            case "Telekinetic Blast":
                StartCoroutine(decFocus(9f));
                GetComponent<MerfolkGFX>().attackAnimation(whichHand, weapon); // spellcast (bolt)
                break;
            case "Telekinetic Blast (Secondary)":
                GetComponent<MerfolkGFX>().attackAnimation(whichHand, weapon); // spellcast (aura)
                StartCoroutine(decFocus(18f));
                break;
            case "Arcane Bolt":
                startTimeBtwAttack = 0.8f + speedFactor;
                attackPos = attackPos2;
                prefabProj = arcaneBoltLight;
                StartCoroutine(decFocus(10f));
                effects = new string[]{"Arcane", "WallImpact", "ExpireImpact"};
                GetComponent<MerfolkGFX>().attackAnimation(whichHand, weapon); // spellcast (bolt)
                RangedAttack(10.0f,8,3f * damageFactor,10.0f, effects);
                Recoil(1.5f, 1.5f);
                break;
            case "Arcane Bolt (Secondary)":
                startTimeBtwAttack = 1.0f + speedFactor;
                attackPos = attackPos2;
                prefabProj = arcaneBoltHeavy;
                StartCoroutine(decFocus(20f));
                effects = new string[]{"Arcane", "Explode", "WallImpact", "ExpireImpact"};
                GetComponent<MerfolkGFX>().attackAnimation(whichHand, weapon); // spellcast (bolt)
                RangedAttack(10.0f,8,5f * damageFactor,10.0f, effects);
                Recoil(2f, 1.5f);
                break;
            case "Arcane Blade":
                startTimeBtwAttack = 0.8f + speedFactor;
                attackPos = attackPos3;
                StartCoroutine(decFocus(10f));
                impactEffect = true;
                impactFX = arcaneImpact;
                damageType = "arcane";
                GetComponent<MerfolkGFX>().attackAnimation(whichHand, weapon); // spellcast (aura)
                MeleeAttack(0.6f, 3f * damageFactor, 2);
                Recoil(1.5f, 1.5f);
                break;
            case "Arcane Blade (Secondary)":
                StartCoroutine(decFocus(20f));
                break;
            case "Arcane Volley":
                startTimeBtwAttack = 0.3f * speedFactor;
                attackPos = attackPos5;
                prefabProj = arcaneBoltSmall;
                StartCoroutine(decFocus(3f));
                effects = new string[]{"Arcane", "Voltaic", "Release1"};
                GetComponent<MerfolkGFX>().attackAnimation(whichHand, weapon); // spellcast (bolt)
                RangedAttack(10.0f,1,1f * damageFactor,5.0f, effects);
                Recoil(0.2f, 1.5f);
                break;
            case "Arcane Volley (Secondary)":
                StartCoroutine(decFocus(15f));
                GetComponent<MerfolkGFX>().attackAnimation(whichHand, weapon); // spellcast (aura)
                break;
            case "Arcane Seeker":
                startTimeBtwAttack = 0.8f * speedFactor;
                attackPos = attackPos5;
                prefabProj = arcaneOrb;
                StartCoroutine(decFocus(12f));
                effects = new string[]{"Arcane", "Homing", "WallImpact", "ExpireImpact"};
                GetComponent<MerfolkGFX>().attackAnimation(whichHand, weapon); // spellcast (bolt)
                RangedAttack(3.0f,15,2f * damageFactor,10.0f, effects);
                Recoil(1.5f, 1.5f);
                break;
            case "Arcane Seeker (Secondary)":
                startTimeBtwAttack = 1.0f * speedFactor;
                attackPos = attackPos5;
                prefabProj = arcaneOrbSmall;
                StartCoroutine(decFocus(24f));
                effects = new string[]{"Fan5", "Arcane", "Homing", "WallImpact", "ExpireImpact"};
                GetComponent<MerfolkGFX>().attackAnimation(whichHand, weapon); // spellcast (bolt)
                RangedAttack(2.0f,18,0.5f * damageFactor,10.0f, effects);
                Recoil(2.0f, 1.5f);
                break;
            case "Arcane Gate":
                StartCoroutine(decFocus(10f));
                GetComponent<MerfolkGFX>().attackAnimation(whichHand, weapon); // spellcast (aura)
                break;
            case "Arcane Gate (Secondary)":
                StartCoroutine(decFocus(20f));
                GetComponent<MerfolkGFX>().attackAnimation(whichHand, weapon); // spellcast (aura)
                break;
            case "Arcane Spirit":
                StartCoroutine(decFocus(13f));
                GetComponent<MerfolkGFX>().attackAnimation(whichHand, weapon); // spellcast (aura)
                break;
            case "Arcane Spirit (Secondary)":
                StartCoroutine(decFocus(26f));
                GetComponent<MerfolkGFX>().attackAnimation(whichHand, weapon); // spellcast (aura)
                break;
            case "Flamecast":
                startTimeBtwAttack = 0.2f * speedFactor;
                attackPos = attackPos2;
                prefabProj = fire;
                StartCoroutine(decFocus(2f));
                effects = new string[]{"Fan5", "Piercing", "Voltaic", "Fire", "WallImpact"};
                GetComponent<MerfolkGFX>().attackAnimation(whichHand, weapon); // spellcast (bolt)
                RangedAttack(20.0f,1,0.4f * damageFactor,3.0f, effects);
                Recoil(0.1f, 1.5f);
                break;
            case "Flamecast (Secondary)":
                startTimeBtwAttack = 1.0f * speedFactor;
                attackPos = attackPos2;
                prefabProj = fire;
                StartCoroutine(decFocus(18f));
                effects = new string[]{"Explode", "Fire"};
                GetComponent<MerfolkGFX>().attackAnimation(whichHand, weapon); // spellcast (bolt)
                RangedAttack(10.0f,8,3f * damageFactor,20.0f, effects);
                Recoil(2.0f, 1.5f);
                break;
            case "Poisonburst":
                startTimeBtwAttack = 1.2f * speedFactor;
                attackPos = attackPos2;
                prefabProj = poison;
                StartCoroutine(decFocus(10f));
                effects = new string[]{"Poison", "Mark", "WallImpact", "ExpireImpact"};
                GetComponent<MerfolkGFX>().attackAnimation(whichHand, weapon); // spellcast (bolt)
                RangedAttack(3.0f,24,3f * damageFactor,10.0f, effects);
                Recoil(1.5f, 1.5f);
                // keyword "poison" "mark" - poisons enemies and marks them for death
                break;
            case "Poisonburst (Secondary)":
                StartCoroutine(decFocus(20f));
                GetComponent<MerfolkGFX>().attackAnimation(whichHand, weapon); // spellcast (aura)
                break;
            case "Coldblast":
                startTimeBtwAttack = 1.1f * speedFactor;
                attackPos = attackPos2;
                prefabProj = cold;
                StartCoroutine(decFocus(10f));
                effects = new string[]{"Slow", "Cold", "WallImpact", "ExpireImpact"};
                GetComponent<MerfolkGFX>().attackAnimation(whichHand, weapon); // spellcast (bolt)
                RangedAttack(5.0f,16,2f * damageFactor,10.0f, effects);
                Recoil(1.5f, 1.5f);
                // keyword "slow" - slows enemies
                break;
            case "Coldblast (Secondary)":
                startTimeBtwAttack = 1.1f * speedFactor;
                attackPos = attackPos0;
                prefabProj = cold;
                StartCoroutine(decFocus(20f));
                effects = new string[]{"Slow", "Burst", "Cold"};
                GetComponent<MerfolkGFX>().attackAnimation(whichHand, weapon); // spellcast (aura)
                RangedAttack(5.0f,16,2f * damageFactor,10.0f, effects);
                Recoil(1.5f, 1.5f);
                StartCoroutine(decFocus(20f));
                break;
            case "Darkcall":
                startTimeBtwAttack = 0.2f * speedFactor;
                attackPos = attackPos6;
                StartCoroutine(decFocus(3f));
                impactEffect = true;
                impactFX = darkImpact;
                damageType = "dark";
                MeleeAttack(0.6f, 0.5f * damageFactor, 2);
                GetComponent<MerfolkGFX>().attackAnimation(whichHand, weapon); // spellcast (bolt)
                Recoil(0.1f, 1.5f);
                break;
            case "Darkcall (Secondary)":
                StartCoroutine(decFocus(17f));
                GetComponent<MerfolkGFX>().attackAnimation(whichHand, weapon); // spellcast (bolt)
                break;
            case "Blood Rite":
                StartCoroutine(decFocus(8f));
                GetComponent<MerfolkGFX>().attackAnimation(whichHand, weapon); // spellcast (bolt)
                break;
            case "Blood Rite (Secondary)":
                StartCoroutine(decFocus(16f));
                GetComponent<MerfolkGFX>().attackAnimation(whichHand, weapon); // spellcast (bolt)
                break;
            case "Crystal Shards":
                startTimeBtwAttack = 0.8f * speedFactor;
                attackPos = attackPos2;
                prefabProj = oval;
                StartCoroutine(decFocus(8f));
                effects = new string[]{"Piercing"};
                RangedAttack(14.0f,8,2f * damageFactor,10.0f, effects);
                GetComponent<MerfolkGFX>().attackAnimation(whichHand, weapon); // spellcast (bolt)
                Recoil(2.0f, 1.5f);
                break;
            case "Crystal Shards (Secondary)":
                startTimeBtwAttack = 0.8f * speedFactor;
                attackPos = attackPos2;
                prefabProj = oval;
                StartCoroutine(decFocus(16f));
                effects = new string[]{"Piercing", "Fan3"};
                RangedAttack(14.0f,8,2f * damageFactor,10.0f, effects);
                GetComponent<MerfolkGFX>().attackAnimation(whichHand, weapon); // spellcast (bolt)
                Recoil(2.0f, 1.5f);
                break;
            case "Lightshift":
                lightRelease = true;
                startTimeBtwAttack = 0.2f + speedFactor;
                attackPos = attackPos6;
                StartCoroutine(decFocus(4f));
                impactEffect = true;
                impactFX = lightImpact;
                damageType = "light";
                MeleeAttack(0.6f, 0.5f * damageFactor, 2);
                GetComponent<MerfolkGFX>().attackAnimation(whichHand, weapon); // spellcast (bolt)
                Recoil(0.1f, 1.5f);
                //lightRelease = false;
                break;
            case "Lightshift (Secondary)":
                startTimeBtwAttack = 0.8f * speedFactor;
                attackPos = attackPos0;
                prefabProj = lightScatter;
                StartCoroutine(decFocus(22f));
                effects = new string[]{"Burst", "Piercing", "Light"};
                RangedAttack(20.0f,1,2f * damageFactor,30.0f, effects);
                GetComponent<MerfolkGFX>().attackAnimation(whichHand, weapon); // spellcast (aura)
                Recoil(1.5f, 1.5f);
                break;
            case "Raise Dead":
                StartCoroutine(decFocus(12f));
                GetComponent<MerfolkGFX>().attackAnimation(whichHand, weapon); // spellcast (bolt)
                break;
            case "Raise Dead (Secondary)":
                StartCoroutine(decFocus(24f));
                GetComponent<MerfolkGFX>().attackAnimation(whichHand, weapon); // spellcast (aura)
                break;
            case "Chain Lightning":
                startTimeBtwAttack = 0.5f * speedFactor;
                attackPos = attackPos5;
                prefabProj = lightningLight;
                StartCoroutine(decFocus(11f));
                effects = new string[]{"Voltaic", "Homing", "Lightning", "WallImpact", "ExpireImpact", ""};
                RangedAttack(15.0f,4,2f * damageFactor,10.0f, effects);
                GetComponent<MerfolkGFX>().attackAnimation(whichHand, weapon); // spellcast (bolt)
                Recoil(1.5f, 1.5f);
                break;
            case "Chain Lightning (Secondary)":
                startTimeBtwAttack = 0.5f * speedFactor;
                attackPos = attackPos5;
                prefabProj = lightningHeavy;
                StartCoroutine(decFocus(22f));
                effects = new string[]{"Release3", "Homing", "Voltaic", "Lightning", "Explode", "WallImpact", "ExpireImpact"};
                RangedAttack(15.0f,4,1f * damageFactor,10.0f, effects);
                GetComponent<MerfolkGFX>().attackAnimation(whichHand, weapon); // spellcast (bolt)
                Recoil(1.5f, 1.5f);
                break;
            case "Wavebreak":
                startTimeBtwAttack = 0.8f * speedFactor;
                attackPos = attackPos2;
                prefabProj = oval;
                StartCoroutine(decFocus(9f));
                effects = new string[]{"Fan5"};
                RangedAttack(20.0f,2,2f * damageFactor,30.0f, effects);
                GetComponent<MerfolkGFX>().attackAnimation(whichHand, weapon); // spellcast (bolt)
                Recoil(1.5f, 1.5f);
                break;
            case "Wavebreak (Secondary)":
                startTimeBtwAttack = 0.8f * speedFactor;
                attackPos = attackPos2;
                prefabProj = oval;
                StartCoroutine(decFocus(18f));
                effects = new string[]{"Burst"};
                RangedAttack(20.0f,2,2f * damageFactor,30.0f, effects);
                GetComponent<MerfolkGFX>().attackAnimation(whichHand, weapon); // spellcast (aura)
                Recoil(1.5f, 1.5f);
                break;
            default:
                Debug.Log("Error");
                break;
        }
        if (twohand || dualwield)
        {
            damageFactor = 1;
            speedFactor = 1f;
        }
    }


    void MeleeAttack(float attackRadius, float damage, int impact)
    {
        // Play attack animation
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRadius*2f, whatIsEnemies);
        if(impactEffect)
        {
            Instantiate(impactFX, attackPos.position, Quaternion.identity);
        }
        for (int i = 0; i < enemiesToDamage.Length; i++)
        {
        	enemiesToDamage[i].GetComponent<Destructible>().TakeDamage(damage, attackDirection, impact, attackPos.position, damageType);
        }
    }

    void RangedAttack(float speed, int range, float damage, float impact, string[] effects)
    {
        GetComponent<LaunchProjectile>().launch(prefabProj, attackPos, speed, range, damage, impact, effects);
    }

    void Recoil(float recoilSpeed, float recoilFactor)
    {
        Vector3 recoilDir = transform.position - attackPos.transform.position;
        GetComponent<Dodge>().DodgeMove(recoilDir, recoilSpeed, recoilFactor);
    }
}
