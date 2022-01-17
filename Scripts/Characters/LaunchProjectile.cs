using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LaunchProjectile : MonoBehaviour
{
	Rigidbody2D rb;

	private Transform firePoint;
    private string[] effectsList;
    private bool projHit;
    public GameObject gunshot;

	private GameObject projectilePrefab;
    //public GameObject returnPrefab;
    //public GameObject corpsePrefab;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void launch(GameObject proj, Transform attackPos, float speed, int range, float damage, float impact, string[] effects)//, Quaternion targetRot)
    {
        firePoint = attackPos;
        effectsList = effects;

        projectilePrefab = proj;
        projectilePrefab.GetComponent<Projectile>().setSpeed(speed);
        projectilePrefab.GetComponent<Projectile>().setRange(range);
        projectilePrefab.GetComponent<Projectile>().setDamage(damage);
        projectilePrefab.GetComponent<Projectile>().setImpact(impact);
        projectilePrefab.GetComponent<Projectile>().setEffects(effectsList);
        //projectilePrefab.GetComponent<Projectile>().setDisplaceTarget(displaceTarget);

        GameObject.Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        //shot.rb.AddForce(targetLoc * shootForce);
        ShootingEffect();
    }

    // Explode
    // Displace
    // Piercing
    // Homing
    // Poison
    // Mark
    // Slow
    // Returning
    // Dropping

    void ShootingEffect()
    {
        if(effectsList.Contains("Fan3"))
        {
            //Debug.Log(firePoint.rotation);
            Vector3 rot = firePoint.rotation.eulerAngles;
            Vector3 rotLeft = new Vector3(rot.x,rot.y,rot.z-30);
            Vector3 rotRight = new Vector3(rot.x,rot.y,rot.z+30);
            //Debug.Log(rot + " " + rotLeft + " " + rotRight);

            GameObject.Instantiate(projectilePrefab, firePoint.position, Quaternion.Euler(rotLeft));
            GameObject.Instantiate(projectilePrefab, firePoint.position, Quaternion.Euler(rotRight));
        }
        if(effectsList.Contains("Fan5"))
        {
            //Debug.Log(firePoint.rotation);
            Vector3 rot = firePoint.rotation.eulerAngles;
            Vector3 rotLeft2 = new Vector3(rot.x,rot.y,rot.z-30);
            Vector3 rotLeft1 = new Vector3(rot.x,rot.y,rot.z-15);
            Vector3 rotRight1 = new Vector3(rot.x,rot.y,rot.z+15);
            Vector3 rotRight2 = new Vector3(rot.x,rot.y,rot.z+30);
            //Debug.Log(rot + " " + rotLeft + " " + rotRight);

            GameObject.Instantiate(projectilePrefab, firePoint.position, Quaternion.Euler(rotLeft1));
            GameObject.Instantiate(projectilePrefab, firePoint.position, Quaternion.Euler(rotRight1));
            GameObject.Instantiate(projectilePrefab, firePoint.position, Quaternion.Euler(rotLeft2));
            GameObject.Instantiate(projectilePrefab, firePoint.position, Quaternion.Euler(rotRight2));
        }
        if(effectsList.Contains("Burst"))
        {
            //Debug.Log(firePoint.rotation);
            Vector3 rot = firePoint.rotation.eulerAngles;
            Vector3 rotNeg15 = new Vector3(rot.x,rot.y,rot.z-15);
            Vector3 rotNeg30 = new Vector3(rot.x,rot.y,rot.z-30);
            Vector3 rotNeg45 = new Vector3(rot.x,rot.y,rot.z-45);
            Vector3 rotNeg60 = new Vector3(rot.x,rot.y,rot.z-60);
            Vector3 rotNeg75 = new Vector3(rot.x,rot.y,rot.z-75);
            Vector3 rotNeg90 = new Vector3(rot.x,rot.y,rot.z-90);
            Vector3 rotNeg105 = new Vector3(rot.x,rot.y,rot.z-105);
            Vector3 rotNeg120 = new Vector3(rot.x,rot.y,rot.z-120);
            Vector3 rotNeg135 = new Vector3(rot.x,rot.y,rot.z-135);
            Vector3 rotNeg150 = new Vector3(rot.x,rot.y,rot.z-150);
            Vector3 rotNeg165 = new Vector3(rot.x,rot.y,rot.z-165);
            Vector3 rotPos15 = new Vector3(rot.x,rot.y,rot.z+15);
            Vector3 rotPos30 = new Vector3(rot.x,rot.y,rot.z+30);
            Vector3 rotPos45 = new Vector3(rot.x,rot.y,rot.z+45);
            Vector3 rotPos60 = new Vector3(rot.x,rot.y,rot.z+60);
            Vector3 rotPos75 = new Vector3(rot.x,rot.y,rot.z+75);
            Vector3 rotPos90 = new Vector3(rot.x,rot.y,rot.z+90);
            Vector3 rotPos105 = new Vector3(rot.x,rot.y,rot.z+105);
            Vector3 rotPos120 = new Vector3(rot.x,rot.y,rot.z+120);
            Vector3 rotPos135 = new Vector3(rot.x,rot.y,rot.z+135);
            Vector3 rotPos150 = new Vector3(rot.x,rot.y,rot.z+150);
            Vector3 rotPos165 = new Vector3(rot.x,rot.y,rot.z+165);
            Vector3 rot180 = new Vector3(rot.x,rot.y,rot.z+180);

            GameObject.Instantiate(projectilePrefab, firePoint.position, Quaternion.Euler(rot));
            GameObject.Instantiate(projectilePrefab, firePoint.position, Quaternion.Euler(rotNeg15));
            GameObject.Instantiate(projectilePrefab, firePoint.position, Quaternion.Euler(rotNeg30));
            GameObject.Instantiate(projectilePrefab, firePoint.position, Quaternion.Euler(rotNeg45));
            GameObject.Instantiate(projectilePrefab, firePoint.position, Quaternion.Euler(rotNeg60));
            GameObject.Instantiate(projectilePrefab, firePoint.position, Quaternion.Euler(rotNeg75));
            GameObject.Instantiate(projectilePrefab, firePoint.position, Quaternion.Euler(rotNeg90));
            GameObject.Instantiate(projectilePrefab, firePoint.position, Quaternion.Euler(rotNeg105));
            GameObject.Instantiate(projectilePrefab, firePoint.position, Quaternion.Euler(rotNeg120));
            GameObject.Instantiate(projectilePrefab, firePoint.position, Quaternion.Euler(rotNeg135));
            GameObject.Instantiate(projectilePrefab, firePoint.position, Quaternion.Euler(rotNeg150));
            GameObject.Instantiate(projectilePrefab, firePoint.position, Quaternion.Euler(rotNeg165));
            GameObject.Instantiate(projectilePrefab, firePoint.position, Quaternion.Euler(rotPos15));
            GameObject.Instantiate(projectilePrefab, firePoint.position, Quaternion.Euler(rotPos30));
            GameObject.Instantiate(projectilePrefab, firePoint.position, Quaternion.Euler(rotPos45));
            GameObject.Instantiate(projectilePrefab, firePoint.position, Quaternion.Euler(rotPos60));
            GameObject.Instantiate(projectilePrefab, firePoint.position, Quaternion.Euler(rotPos75));
            GameObject.Instantiate(projectilePrefab, firePoint.position, Quaternion.Euler(rotPos90));
            GameObject.Instantiate(projectilePrefab, firePoint.position, Quaternion.Euler(rotPos105));
            GameObject.Instantiate(projectilePrefab, firePoint.position, Quaternion.Euler(rotPos120));
            GameObject.Instantiate(projectilePrefab, firePoint.position, Quaternion.Euler(rotPos135));
            GameObject.Instantiate(projectilePrefab, firePoint.position, Quaternion.Euler(rotPos150));
            GameObject.Instantiate(projectilePrefab, firePoint.position, Quaternion.Euler(rotPos165));
            GameObject.Instantiate(projectilePrefab, firePoint.position, Quaternion.Euler(rot180));
        }
        if(effectsList.Contains("Gunshot"))
        {
            Instantiate(gunshot, firePoint.position, Quaternion.identity);
        }
    }

    void DeathEffect()
    {
        //if(effectsList.Contains("Returning"))
        //{
            //GameObject.Instantiate(returnPrefab, hitPoint.position, hitPoint.rotation);
        //}
        // Explode
        // Poison
        // Mark
        // Slow
        // Returning
        // Dropping
    }
}
