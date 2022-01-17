using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFloat : MonoBehaviour
{

	Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(rb.mass <= 1.0f)
        {
        	float floatVel = 1.2f - rb.mass;
        	rb.AddForce(Vector2.up * floatVel);
        }
    }
}
