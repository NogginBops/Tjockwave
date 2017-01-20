using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWave : MonoBehaviour {

    public float speed = 1.2f;
    public float maxSize = 10;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void FixedUpdate () {

        if (transform.localScale.x >= maxSize)
        {
            Destroy(gameObject);
        }
        else
        {
            transform.localScale += transform.localScale * (Time.fixedDeltaTime * speed);
        }      
    }
}
