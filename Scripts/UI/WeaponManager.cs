using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class WeaponManager : MonoBehaviour
{
	public static WeaponManager instance;

	public Text weapon1Text;
	public Text weapon2Text;
    //public Text spellColorText;


	private string weapon1;
	private string weapon2;
	private string tempWeapon;
    //private string spellColor;
    //private string spellColorTemp;
	private string[] weapons = new string[]{"Empty", "Spear", "Dagger", "Sword", "Trident", "Torch", "Chain Hook", "Rapier", "Anchor", "Tentacle", "Javelin", "Boltcaster", "Harpoon Gun", "Flintlock", "Blunderbuss", "Musket", "Shield", "Staff", "Telekinetic Blast", "Arcane Bolt", "Arcane Blade", "Arcane Volley", "Arcane Seeker", "Arcane Gate", "Arcane Spirit", "Flamecast", "Poisonburst", "Coldblast", "Darkcall", "Blood Rite", "Crystal Shards", "Lightshift", "Raise Dead", "Chain Lightning", "Wavebreak"};
	//private string[] color = new string[]{"Default", "Red", "Orange", "Yellow", "Green", "Blue", "Violet", "White"};
    private int index1 = 1;
	private int index2 = 0;

	private bool twohanded;
    //private bool spell;

	private void Awake()
	{
		instance = this;
	}

    // Start is called before the first frame update
    void Start()
    {
    	index1 = 1;
    	weapon1 = "Spear";
    	weapon1Text.text = weapon1;
    	index2 = 0;
    	weapon2 = "Empty";
    	weapon2Text.text = weapon2;
    	twohanded = false;
        //index3 = 0;
        //spellColor = "";
        //spellColorText.text = spellColor;
    }

    // Update is called once per frame
    void Update()
    {
    	twoHandWeapon();
    	swapWeapon();
        //swapSpellColor();
    }

    public string getWeapon(int num)
    {
    	if (num == 1)
    	{
    		return weapon1;
    	}
    	else
    	{
    		return weapon2;
    	}
    }
    /*public string getSpellColor()
    {
        return spellColor;
    }*/

    public bool getTwoHand()
    {
    	return twohanded;
    }
    public string getHandGrip(int weaponNum)
    {
        string[] fistGrip = new string[]{"Empty"};
        string[] weaponGrip = new string[]{"Spear", "Dagger", "Sword", "Trident", "Torch", "Chain Hook", "Rapier", "Anchor", "Tentacle", "Javelin", "Shield", "Staff"};
        string[] shootGrip = new string[]{"Boltcaster", "Harpoon Gun", "Flintlock", "Blunderbuss", "Musket"};
        string[] spellGrip = new string[]{"Telekinetic Blast", "Arcane Bolt", "Arcane Blade", "Arcane Volley", "Arcane Seeker", "Arcane Gate", "Arcane Spirit", "Flamecast", "Poisonburst", "Coldblast", "Darkcall", "Blood Rite", "Crystal Shards", "Lightshift", "Raise Dead", "Chain Lightning", "Wavebreak"};
        if(weaponNum == 1)
        {
            if(fistGrip.Contains(weapon1))
            {
                return "Fist Grip";
            }
            else if(weaponGrip.Contains(weapon1))
            {
                return "Weapon Grip";
            }
            else if(shootGrip.Contains(weapon1))
            {
                return "Shoot Grip";
            }
            else if(spellGrip.Contains(weapon1))
            {
                return "Spell Grip";
            }
            else
            {
                return "Error on weapon 1";
            }
        }
        else if(weaponNum == 2)
        {
            if(fistGrip.Contains(weapon2))
            {
                return "Fist";
            }
            else if(weaponGrip.Contains(weapon2))
            {
                return "Weapon Grip";
            }
            else if(weaponGrip.Contains(weapon1))
            {
                return "Shoot Grip";
            }
            else if(spellGrip.Contains(weapon2))
            {
                return "Spell Grip";
            }
            else
            {
                return "Error on weapon 2";
            }
        }
        else
        {
            return "Error!";
        }
    }
    public bool getReverseSwitcher(int weaponNum)
    {
        string[] reverseSwitcher = new string[]{"Dagger", "Javelin", "Spear", "Sword", "Trident"};
        if(weaponNum == 1)
        {
            if(reverseSwitcher.Contains(weapon1))
            {
                return true;
            }
            else
            {
                return false;
            }
        }  
        else if (weaponNum == 2)
        {
            if(reverseSwitcher.Contains(weapon2))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            Debug.Log("Error");
            return false;
        }
    }
    public bool getDualWield()
    {
        if(weapon1 == weapon2)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //if(index1 != index2 || !nondual.Contains(weapon2))
    public void incWeapList(int num)
    {
        if(num == 1)
        {
            index1++;
            if (index1 >= weapons.Length)
            {
                index1 = 0;
            }
            weapon1 = weapons[index1];
        }
        else
        {
            index2++;
            if (index2 >= weapons.Length)
            {
                index2 = 0;
            }
            weapon2 = weapons[index2];
        }
    }

    public void swapWeapon()
    {
        string[] nondual = new string[]{"Spear", "Trident", "Torch", "Rapier", "Anchor", "Javelin", "Harpoon Gun", "Blunderbuss", "Musket", "Shield", "Staff"};
    	if(Input.GetKeyDown(KeyCode.O) && !twohanded) 
        {
            incWeapList(1);
            while(weapon1 == weapon2 && nondual.Contains(weapon1))
            {
                incWeapList(1);
            }
            weapon1Text.text = weapon1.ToString();
        }
        if(Input.GetKeyDown(KeyCode.P) && !twohanded)
        {
            incWeapList(2);
            while(weapon1 == weapon2 && nondual.Contains(weapon2))
            {
                incWeapList(2);
            }
            weapon2Text.text = weapon2.ToString();
        }
    }

    // string[] spellList = new string[]{"Telekinetic Blast", "Arcane Bolt", "Arcane Blade", "Arcane Volley", "Arcane Seeker", "Arcane Gate", "Arcane Spirit", "Flamecast", "Poisonburst", "Coldblast", "Darkcall", "Blood Rite", "Crystal Shards", "Lightshift", "Raise Dead", "Chain Lightning", "Wavebreak"};

    /*public void swapSpellColor()
    {
        string[] spellList = new string[]{"Telekinetic Blast", "Arcane Bolt", "Arcane Blade", "Arcane Volley", "Arcane Seeker", "Arcane Gate", "Arcane Spirit", "Flamecast", "Poisonburst", "Coldblast", "Darkcall", "Blood Rite", "Crystal Shards", "Lightshift", "Raise Dead", "Chain Lightning", "Wavebreak"};
        
        if(spellList.Contains(weapon1) || spellList.Contains(weapon2))
        {
            spellColor = color[index3];
            spellColorText.text = spellColor.ToString();
            if(Input.GetKeyDown(KeyCode.U))
            {
                index3++;
                if (index3 >= color.Length)
                {
                    index3 = 0;
                }
            }
        }
        else
        {
            spellColor = "";
            spellColorText.text = "";
        }
    } */

    public void twoHandWeapon()
    {
        string[] nontwo = new string[]{"Empty", "Dagger", "Torch", "Chain Hook", "Rapier", "Tentacle", "Javelin", "Flintlock", "Shield"};
    	if(Input.GetKeyDown(KeyCode.I) && !nontwo.Contains(weapon1)) 
        {
        	if (!twohanded)
        	{
        		weapon2Text.text = "(Two-Handed)";
        		tempWeapon = weapon2;
        		weapon2 = weapon1;
        		twohanded = true;
        	}
        	else
        	{
        		weapon2 = tempWeapon;
        		weapon2Text.text = weapon2.ToString();
        		twohanded = false;
        	}
        }
    }

    public Sprite getWeaponSprite(int weapNum)
    {
        string weap = "";
        if(weapNum == 1)
        {
            weap = weapon1;
        }
        else if (weapNum == 2)
        {
            weap = weapon2;
        }

        string weaponPath = "Art/Merfolk/Weapons/Player/" + weap;
        //Debug.Log(weaponPath);
        Sprite weapSprite = Resources.Load<Sprite>(weaponPath);
        if(weapSprite != null)
        {
            return weapSprite;
        }
        else
        {
            //return Resources.Load<Sprite>("Art/Merfolk/Weapons/Player/Dagger");
            //Debug.Log("No weapon!");
            return Resources.Load<Sprite>("Art/Merfolk/Weapons/Player/None");
        }
    }
}
