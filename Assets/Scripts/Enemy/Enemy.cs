using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public float health;
    public float damage;

    public float deathVelocity;

    public GameObject deathParticles;

    CameraShake camShake;
    
	// Use this for initialization
	void Start () {
        camShake = Camera.main.GetComponent<CameraShake>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Enemy")
        {
            if (collision.gameObject.tag != "Shockwave")
            {
                if (collision.impulse.sqrMagnitude > (deathVelocity * deathVelocity))
                {
                    Hurt(health);
                }           
            }
        }         

        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().Hurt(damage);
            Die();
            camShake.ShakeCamera(0.1f, 0.1f);
        }
    }

    public void Hurt(float damage)
    {
        health -= damage;
        if (health <= 0)
        { 
            Die();
            UIController.Instance.AddSlimeKill();
        }
    }

    void Die()
    {
        health = 0;
        transform.parent.GetComponent<Flock>().RemoveBoid(GetComponent<Rigidbody>());
        Destroy(gameObject);
        Instantiate(deathParticles, transform.position, deathParticles.transform.rotation);
    }
}
