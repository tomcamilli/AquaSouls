using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{
	public static CoinManager instance;

	public Text coinText;

	int coins = 0;

	private void Awake()
	{
		instance = this;
	}

    // Start is called before the first frame update
    void Start()
    {
    	coinText.text = "Coin Count : " + coins.ToString();
    }

    public void AddCoins(int coin)
    {
    	//Debug.Log(coin + " " + coins);
    	coins += coin;
    	coinText.text = "Coin Count : " + coins.ToString();
    }

    public int GetCoins()
    {
    	return coins;
    }

    public void ClearCoins()
    {
    	coins = 0;
    	coinText.text = "Coin Count : " + coins.ToString();
    }
}
