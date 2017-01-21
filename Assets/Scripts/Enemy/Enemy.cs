using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public float health;
    public float damage;

    public float deathVelocity;

    Rigidbody rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Ground")
        {         
            if (rb.velocity.magnitude > deathVelocity)
            {
                Die();
            }
        }

        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().Hurt(damage);
            Die();
        }
    }

    public void Hurt(float damage)
    {
        if (health - damage > 0)
        {
            health -= damage;
        }
        else
        {
            Die();
        }
    }

    void Die()
    {
        health = 0;
        transform.parent.GetComponent<Flock>().RemoveBoid(GetComponent<Rigidbody>());
        Destroy(gameObject);
    }
}
