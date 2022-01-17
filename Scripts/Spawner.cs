using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

	public GameObject spawn1;
	public GameObject spawn2;
	public GameObject spawn3;
    public GameObject spawn4;
    public GameObject spawn5;
    public GameObject spawn6;
    public GameObject spawn7;
    public GameObject spawn8;
    public GameObject spawn9;
    public GameObject spawn0;
	private float timeBtwSpawn;
	public float startTimeBtwSpawn;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timeBtwSpawn <= 0)
        {
        	if(Input.GetKeyDown("1")) 
        	{
        		Instantiate(spawn1, Vector3.zero, Quaternion.identity);
        		timeBtwSpawn = startTimeBtwSpawn;
        	}
            if(Input.GetKeyDown("2"))
            {
                Instantiate(spawn2, Vector3.zero, Quaternion.identity);
                timeBtwSpawn = startTimeBtwSpawn;
            }
            if(Input.GetKeyDown("3"))
            {
                Instantiate(spawn3, Vector3.zero, Quaternion.identity);
                timeBtwSpawn = startTimeBtwSpawn;
            }
            if(Input.GetKeyDown("4"))
            {
                Instantiate(spawn4, Vector3.zero, Quaternion.identity);
                timeBtwSpawn = startTimeBtwSpawn;
            }
            if(Input.GetKeyDown("5"))
            {
                Instantiate(spawn5, Vector3.zero, Quaternion.identity);
                timeBtwSpawn = startTimeBtwSpawn;
            }
            if(Input.GetKeyDown("6"))
            {
                Instantiate(spawn6, Vector3.zero, Quaternion.identity);
                timeBtwSpawn = startTimeBtwSpawn;
            }
            if(Input.GetKeyDown("7"))
            {
                Instantiate(spawn7, Vector3.zero, Quaternion.identity);
                timeBtwSpawn = startTimeBtwSpawn;
            }
            if(Input.GetKeyDown("8"))
            {
                Instantiate(spawn8, Vector3.zero, Quaternion.identity);
                timeBtwSpawn = startTimeBtwSpawn;
            }
            if(Input.GetKeyDown("9"))
            {
                Instantiate(spawn9, Vector3.zero, Quaternion.identity);
                timeBtwSpawn = startTimeBtwSpawn;
            }
            if(Input.GetKeyDown("0"))
            {
                Instantiate(spawn0, Vector3.zero, Quaternion.identity);
                timeBtwSpawn = startTimeBtwSpawn;
            }
        }
        else
        {
        	timeBtwSpawn -= Time.deltaTime;
        }
    }
}
