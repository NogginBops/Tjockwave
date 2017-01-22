using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWave : MonoBehaviour {

    public float speed = 1.2f;
    public float maxSize = 10;
    public float force = 100;

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
            transform.localScale += transform.localScale * (Time.deltaTime * speed);
        }      
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Vector3 shockWave = collision.gameObject.transform.position - transform.position;
            collision.gameObject.GetComponent<Rigidbody>().AddForce(shockWave.normalized * force);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, transform.localScale.x);
    }
}
