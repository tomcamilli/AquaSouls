using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticles : MonoBehaviour
{

	public float duration;

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, duration);
    }
}
