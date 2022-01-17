using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MerfolkGFX : MonoBehaviour
{
    public Camera cam;
    private Vector3 mousePos;
    public float scale;
    public float weaponScale;
    private bool faceLeft;

	public int rotateSpeed;
	[Header("Body Parts")]
	public GameObject torso;
	public GameObject head;
	public GameObject leftRagdollArm;
	public GameObject rightRagdollArm;
	public GameObject leftRigidArm;
	public GameObject rightRigidArm;
	public GameObject leftArm;
	public GameObject rightArm;
	public GameObject leftForearm;
	public GameObject rightForearm;
	public GameObject leftHand;
	public GameObject rightHand;

	public Sprite permLeftRigidArm;
	public Sprite permRightRigidArm;

	public GameObject leftWeapon;
	public GameObject rightWeapon;

	[Header("Attack Animations")]
	public string attackAnim;
	public bool attacking;
	public Transform attackPos;
	private bool canReverseWeapon;
	private string reverse;
	private bool canFlip;
	private float attackAngle;
	private float scaleMod;
	
	[Header("Grip Sprites")]
	public Sprite leftLooseGrip;
	public Sprite rightLooseGrip;
	public Sprite leftNormalGrip;
	public Sprite rightNormalGrip;
	public Sprite leftReverseGrip;
	public Sprite rightReverseGrip;
	public Sprite leftShootGrip;
	public Sprite rightShootGrip;
	public Sprite leftFist;
	public Sprite rightFist;

	private float currentTorsoAngle;
	private float currentHeadAngle;
	private float oldHeadAngle;
	private float currentForearmAngle;
	private float mouseAngle;


    // Start is called before the first frame update
    void Start()
    {
    	faceLeft = true;
    	canFlip = true;
    	canReverseWeapon = true;
    	reverse = "Normal";
    	attacking = false;
    	attackAngle = 0;
    	scaleMod = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        determineMerfolkAngle();
        DirectionToMouse();
        if(canFlip && !attacking)
        {
        	determineMerfolkFacing();
        }
        if(canReverseWeapon)
        {
        	determineHandGrip();
        }
        determineHeadLooking();
        determineForearmLooking();
        fixRotation();

        leftRigidArm.GetComponent<SpriteRenderer>().sprite = permLeftRigidArm;
        rightRigidArm.GetComponent<SpriteRenderer>().sprite = permRightRigidArm;
    }

    private void fixRotation()
    {
    	if(currentHeadAngle > 360f)
    	{
    		currentHeadAngle = 0;
    	}
    	if(currentHeadAngle < -360f)
    	{
    		currentHeadAngle = 0;
    	}
    	float headTorsoAngleDiff = Mathf.Abs(currentHeadAngle-currentTorsoAngle);
    	if(headTorsoAngleDiff > 160)
    	{
    		currentHeadAngle = currentTorsoAngle;
    	}
    }

    private void determineMerfolkAngle()
    {
    	Vector3 targetDirection = GetComponent<Player>().movementDirection();
    	float turnSpeed = GetComponent<Player>().moveSpeed/2f;
    	float angle = Mathf.Atan2(targetDirection.x, targetDirection.y)*Mathf.Rad2Deg;

    	float angleDiff = Mathf.Abs(angle-currentTorsoAngle);
    	if(angleDiff<180)
    	{
    		if(angle > currentTorsoAngle)
	    	{
	    		currentTorsoAngle += turnSpeed;
	    		currentHeadAngle += turnSpeed;
	    	}
	    	else if (angle < currentTorsoAngle)
	    	{
	    		currentTorsoAngle -= turnSpeed;
	    		currentHeadAngle -= turnSpeed;
	    	}
	    	else
	    	{
	    		currentTorsoAngle = angle;
	    		currentHeadAngle = angle;
	    	}
    	}
    	else
    	{
    		if(angle > currentTorsoAngle)
	    	{
	    		currentTorsoAngle -= turnSpeed;
	    		currentHeadAngle -= turnSpeed;
	    		if(currentTorsoAngle < -180f)
	    		{
	    			currentTorsoAngle = 179f;
	    		}
	    		if(currentHeadAngle < -180f)
	    		{
	    			currentHeadAngle = 179f;
	    		}
	    	}
	    	else if (angle < currentTorsoAngle)
	    	{
	    		currentTorsoAngle += turnSpeed;
	    		currentHeadAngle += turnSpeed;
	    		if(currentTorsoAngle > 180f)
	    		{
	    			currentTorsoAngle = -179f;
	    		}
	    		if(currentHeadAngle > 180f)
	    		{
	    			currentHeadAngle = -179f;
	    		}
	    	}
	    	else
	    	{
	    		currentTorsoAngle = angle;
	    		currentHeadAngle = angle;
	    	}
    	}

    	Quaternion rotateTorso = Quaternion.Euler(new Vector3(0,0,currentTorsoAngle*-1f));
    	Quaternion rotateHead = Quaternion.Euler(new Vector3(0,0,(int)currentHeadAngle*-1));
    	torso.transform.rotation = Quaternion.Slerp(torso.transform.rotation, rotateTorso, rotateSpeed * Time.deltaTime);
    	head.transform.rotation = Quaternion.Slerp(head.transform.rotation, rotateHead, rotateSpeed * Time.deltaTime);
    }

    private void determineMerfolkFacing()
    {
    	float angleDiff2 = Mathf.Abs(mouseAngle-currentTorsoAngle);
    	if((currentTorsoAngle <= 0f && mouseAngle < currentTorsoAngle) ||
    		(angleDiff2 > 180f && mouseAngle > 0f) ||
    		(currentTorsoAngle > 0f && mouseAngle < currentTorsoAngle && angleDiff2 < 180f) || mouseAngle == 0f)
    	{
    		torso.transform.localScale = new Vector3(-scale, scale, scale);
    		head.transform.localScale = new Vector3(-scale, scale, scale);

    		leftArm.transform.localScale = new Vector3(-scale, scale, scale);
    		leftForearm.transform.localScale = new Vector3(-scale, scale, scale);
    		leftHand.transform.localScale = new Vector3(-scale, scale, scale);
    		rightArm.transform.localScale = new Vector3(-scale, scale, scale);
    		rightForearm.transform.localScale = new Vector3(-scale, scale, scale);
    		rightHand.transform.localScale = new Vector3(-scale, scale, scale);

    		leftRigidArm.transform.localScale = new Vector3(-scale*scaleMod, scale*scaleMod, scale*scaleMod);
    		rightRigidArm.transform.localScale = new Vector3(-scale*scaleMod, scale*scaleMod, scale*scaleMod);

    		leftWeapon.transform.localScale = new Vector3(-scale, scale, scale);
    		rightWeapon.transform.localScale = new Vector3(-scale, scale, scale);

    		/*JointMotor2D leftMotor = leftArm.GetComponent<HingeJoint2D>().motor;
    		JointMotor2D rightMotor = leftArm.GetComponent<HingeJoint2D>().motor;
    		leftMotor.motorSpeed = -300;
    		rightMotor.motorSpeed = -300;
    		leftArm.GetComponent<HingeJoint2D>().motor = leftMotor;
    		rightArm.GetComponent<HingeJoint2D>().motor = rightMotor; */ 

    		if(!faceLeft)
    		{
    			//Debug.Log("Face left " + currentTorsoAngle + " " + (currentTorsoAngle-90f));
    			Quaternion rotateArms = Quaternion.Euler(new Vector3(0,0,currentTorsoAngle-90f));
    			//leftArm.transform.rotation = Quaternion.Slerp(leftArm.transform.rotation, rotateArms, rotateSpeed * Time.deltaTime);
    			//rightArm.transform.rotation = Quaternion.Slerp(rightArm.transform.rotation, rotateArms, rotateSpeed * Time.deltaTime);
    			leftArm.transform.rotation = rotateArms;
    			rightArm.transform.rotation = rotateArms;
    			StartCoroutine(flipLock());
    		}
    		faceLeft = true;
    	}
    	else
    	{
    		torso.transform.localScale = new Vector3(scale, scale, scale);
    		head.transform.localScale = new Vector3(scale, scale, scale);

    		leftArm.transform.localScale = new Vector3(scale, scale, scale);
    		leftForearm.transform.localScale = new Vector3(scale, scale, scale);
    		leftHand.transform.localScale = new Vector3(scale, scale, scale);
    		rightArm.transform.localScale = new Vector3(scale, scale, scale);
    		rightForearm.transform.localScale = new Vector3(scale, scale, scale);
    		rightHand.transform.localScale = new Vector3(scale, scale, scale);

    		leftRigidArm.transform.localScale = new Vector3(scale*scaleMod, scale*scaleMod, scale*scaleMod);
    		rightRigidArm.transform.localScale = new Vector3(scale*scaleMod, scale*scaleMod, scale*scaleMod);

    		leftWeapon.transform.localScale = new Vector3(scale, scale, scale);
    		rightWeapon.transform.localScale = new Vector3(scale, scale, scale);

    		if (faceLeft)
    		{
    			Quaternion rotateArms = Quaternion.Euler(new Vector3(0,0,currentTorsoAngle+90f));
    			leftArm.transform.rotation = rotateArms;
    			rightArm.transform.rotation = rotateArms;
    			StartCoroutine(flipLock());
    		}

    		faceLeft = false;
    	}
    }

    IEnumerator flipLock()
    {
        canFlip = false;
        yield return new WaitForSeconds(0.5f);
        canFlip = true;
    }

    private void DirectionToMouse()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 lookDir = mousePos - transform.position;
        //mouseAngle = -1f*Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        mouseAngle = Mathf.Atan2(lookDir.x, lookDir.y)*Mathf.Rad2Deg;
        //Debug.Log(mouseAngle);
        //rb.rotation = angle;
    }

    private void determineHeadLooking() // 0.196 and 0.905
    {
    	oldHeadAngle = currentHeadAngle;
    	float upRange = 10f;
    	float downRange = 100f;

    	float turnSpeed = GetComponent<Player>().moveSpeed/2f;

    	float angleDiff2 = Mathf.Abs(mouseAngle-currentHeadAngle);

    	//if ((angleDiff2 > 10f && angleDiff2 < 120f))
    	//{
    	if(faceLeft)
    	{
    		currentHeadAngle = (mouseAngle+90f)/2f;
    	}
    	else
   		{
  			currentHeadAngle = (mouseAngle-90f)/2f;
    	}
    	Quaternion rotateHead = Quaternion.Euler(new Vector3(0,0,currentHeadAngle*-1f));
   		head.transform.rotation = Quaternion.Slerp(head.transform.rotation, rotateHead, rotateSpeed * Time.deltaTime);
   		/*if(aimArms)
   		{
   			leftRigidArm.transform.rotation = Quaternion.Slerp(head.transform.rotation, rotateHead, rotateSpeed * Time.deltaTime);
   			rightRigidArm.transform.rotation = Quaternion.Slerp(head.transform.rotation, rotateHead, rotateSpeed * Time.deltaTime);
   		}*/
   		leftRigidArm.transform.rotation = Quaternion.Euler(new Vector3(0,0,attackAngle));
   		rightRigidArm.transform.rotation = Quaternion.Euler(new Vector3(0,0,attackAngle));
   		//Debug.Log(attackAngle);
   	}

    private void determineForearmLooking() // 0.196 and 0.905
    {
    	//currentForearmAngle = mouseAngle;
    	if(faceLeft)
    	{
    		currentForearmAngle = (mouseAngle+90f)/2f;
    	}
    	else
    	{
    		currentForearmAngle = (mouseAngle-90f)/2f;
    	}
    	Quaternion rotateArm = Quaternion.Euler(new Vector3(0,0,currentForearmAngle*-1f));
    	leftForearm.transform.rotation = Quaternion.Slerp(leftForearm.transform.rotation, rotateArm, rotateSpeed * Time.deltaTime);
    	rightForearm.transform.rotation = Quaternion.Slerp(rightForearm.transform.rotation, rotateArm, rotateSpeed * Time.deltaTime);
    } 


    private void determineHandGrip()
    {
 		//SpriteRenderer leftSpriteRenderer = leftHand.spriteRenderer;
    	string gripLeft = WeaponManager.instance.getHandGrip(2);
    	string gripRight = WeaponManager.instance.getHandGrip(1);
    	float handAngleDiff;
    	int facing;

    	leftWeapon.GetComponent<SpriteRenderer>().sprite = WeaponManager.instance.getWeaponSprite(2);
    	rightWeapon.GetComponent<SpriteRenderer>().sprite = WeaponManager.instance.getWeaponSprite(1);
    	

    	if(faceLeft)
    	{
    		handAngleDiff = Mathf.Abs(currentForearmAngle-currentTorsoAngle-90f);
    		facing = -1;
    	}
    	else
    	{
    		handAngleDiff = Mathf.Abs(currentForearmAngle-currentTorsoAngle+90f);
    		facing = 1;
    	}
    	
    	if(gripLeft == "Weapon Grip")
    	{
    		if(reverse == "Reverse")
    		{
    			StartCoroutine(lockReverse());
    		}
    		reverse = "Normal";
    		leftHand.GetComponent<SpriteRenderer>().sprite = leftNormalGrip;
    		leftWeapon.transform.localScale = new Vector3(facing*scale, scale, scale);

    		if(WeaponManager.instance.getReverseSwitcher(2) && handAngleDiff < 50f)
    		{
    			if(reverse == "Normal")
    			{
    				StartCoroutine(lockReverse());
    			}
    			reverse = "Reverse";
    			leftHand.GetComponent<SpriteRenderer>().sprite = leftReverseGrip;
    			leftWeapon.transform.localScale = new Vector3(facing*scale, -scale, scale);
    		}
    	}
    	else if(gripLeft == "Shoot Grip")
    	{
    		leftHand.GetComponent<SpriteRenderer>().sprite = leftShootGrip;
    	}
    	else if(gripLeft == "Fist")
    	{
    		leftHand.GetComponent<SpriteRenderer>().sprite = leftFist;
    	}
    	else
    	{
    		leftHand.GetComponent<SpriteRenderer>().sprite = leftLooseGrip;
    	}

    	if(gripRight == "Weapon Grip")
    	{
    		if(reverse == "Reverse")
    		{
    			StartCoroutine(lockReverse());
    		}
    		reverse = "Normal";
    		rightHand.GetComponent<SpriteRenderer>().sprite = rightNormalGrip;
    		rightWeapon.transform.localScale = new Vector3(facing*scale, scale, scale);

    		if(WeaponManager.instance.getReverseSwitcher(1) && handAngleDiff < 50f)
    		{
    			if(reverse == "Normal")
    			{
    				StartCoroutine(lockReverse());
    			}
    			reverse = "Reverse";
    			rightHand.GetComponent<SpriteRenderer>().sprite = rightReverseGrip;
    			rightWeapon.transform.localScale = new Vector3(facing*scale, -scale, scale);
    		}
    	}
    	else if(gripRight == "Shoot Grip")
    	{
    		rightHand.GetComponent<SpriteRenderer>().sprite = rightShootGrip;
    	}
    	else if(gripRight == "Fist")
    	{
    		rightHand.GetComponent<SpriteRenderer>().sprite = rightFist;
    	}
    	else
    	{
    		rightHand.GetComponent<SpriteRenderer>().sprite = rightLooseGrip;
    	}
    }

    IEnumerator lockReverse()
    {
        canReverseWeapon = false;
        //Debug.Log("Locking rotation " + Time.deltaTime);
        yield return new WaitForSeconds(2f);
        canReverseWeapon = true;
    }

    public void attackAnimation(int whichHand, string typeAttack)
    {
    	GameObject arm;
    	GameObject ragdollArm;
    	GameObject rigidArm;
    	if(whichHand == 1)
    	{
    		arm = leftArm;
    		ragdollArm = leftRagdollArm;
    		rigidArm = leftRigidArm;
    	}
    	else
    	{
    		arm = rightArm;
    		ragdollArm = rightRagdollArm;
    		rigidArm = rightRigidArm;
    	}
    	StartCoroutine(makeRigid(ragdollArm, rigidArm));
    	rigidArm.transform.rotation = Quaternion.Euler(new Vector3(0,0,attackAngle));

    	switch(typeAttack)
    	{
    		case "Spear":
    			scaleMod = 1.3f;
    			break;
    		case "Dagger":
    			scaleMod = 1f;
    			break;
    		case "Flintlock":
    			scaleMod = 1f;
    			break;
    		case "Boltcaster":
    			scaleMod = 1f;
    			break;
    		case "Boltcaster (Secondary)":
    			scaleMod = 1f;
    			typeAttack = "Boltcaster";
    			break;
    		default:
    			//Debug.Log("Not yet implemented.");
    			scaleMod = 1f;
    			break;
    	}

    	attackAnim = typeAttack;
    	
    	/*else if (typeAttack == "Jab")
    	{
    		Debug.Log(whichHand + " Jabbin! " + Time.deltaTime);
    	}
    	else if (typeAttack == "Throw")
    	{
    		Debug.Log(whichHand + " Throwin! " + Time.deltaTime);
    	}
    	else if (typeAttack == "Shoot")
    	{
    		Debug.Log("Shootin! " + Time.deltaTime);
    	}
    	else if (typeAttack == "Reload")
    	{
    		Debug.Log("Reloadin! " + Time.deltaTime);
    	}
    	else if (typeAttack == "Parry")
    	{
    		Debug.Log("Parryin! " + Time.deltaTime);
    	}
    	else if (typeAttack == "Block")
    	{
    		Debug.Log("Blockin! " + Time.deltaTime);
    	}
    	else if (typeAttack == "Bash")
    	{
    		Debug.Log("Bashin! " + Time.deltaTime);
    	}
    	else if (typeAttack == "Staffcast")
    	{
    		Debug.Log("Staffcastin! " + Time.deltaTime);
    	}
    	else if (typeAttack == "Spellcast")
    	{
    		Debug.Log("Spellcastin! " + Time.deltaTime);
    	}*/
    }

    /*IEnumerator swipeAttack(GameObject arm)
    {
    	JointMotor2D motorArm = arm.GetComponent<HingeJoint2D>().motor;
    	if(faceLeft)
    	{
    		motorArm.motorSpeed = -100000;
    		arm.GetComponent<HingeJoint2D>().motor = motorArm;
    	}
        else
        {
        	motorArm.motorSpeed = 100000;
    		arm.GetComponent<HingeJoint2D>().motor = motorArm;
        }
        yield return new WaitForSeconds(3f);
        motorArm.motorSpeed = 0;
    	arm.GetComponent<HingeJoint2D>().motor = motorArm;
    }*/

    IEnumerator makeRigid(GameObject ragdollArm, GameObject rigidArm) // -0.4, 0
    {
    	ragdollArm.SetActive(false);
    	rigidArm.SetActive(true);

    	attackAngle = currentHeadAngle*-2f;
    	
    	//Quaternion rotateArms = Quaternion.Euler(new Vector3(0,0,currentHeadAngle*-1f));
   		//leftRigidArm.transform.rotation = Quaternion.Slerp(head.transform.rotation, rotateArms, rotateSpeed * Time.deltaTime);
   		//rightRigidArm.transform.rotation = Quaternion.Slerp(head.transform.rotation, rotateArms, rotateSpeed * Time.deltaTime);
   		
   		attacking = true;
   		yield return new WaitForSeconds(0.5f); //0.5f

    	attacking = false;
    	ragdollArm.SetActive(true);
    	rigidArm.SetActive(false);
    }
}
