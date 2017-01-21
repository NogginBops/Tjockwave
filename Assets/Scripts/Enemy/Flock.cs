using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public struct FlockSettings
{
    public float neighborRadius;
    public float separationAmp;
    public float alignmentAmp;
    public float cohesionAmp;
    public float centeringAmp;
    public float followPlayerAmp;
}

public class Flock : MonoBehaviour {
    
    public Vector2 center;

    public Transform player;

    List<Rigidbody> boids;

    public FlockSettings settings;

    Collider[] neighbours;
    
    // Use this for initialization
    void Start () {
        boids = new List<Rigidbody>();
        neighbours = new Collider[20];
	}
	
    void FixedUpdate()
    {
        foreach (var boid in boids)
        {
            Vector3 pos = boid.position;

            int i = Physics.OverlapSphereNonAlloc(pos, settings.neighborRadius, neighbours, 1 << 9);

            Vector3 separation = new Vector3();
            Vector3 alignment = new Vector3();
            Vector3 cohesion = new Vector3();
            Vector3 centering = new Vector3(center.x, 0, center.y) - boid.position;
            Vector3 followPlayer = player.position - boid.position;

            foreach (var neighbour in neighbours.Take(i))
            {
                Vector3 neighbourPos = neighbour.transform.position;
                separation += -(neighbourPos - pos);
                alignment += neighbour.transform.parent.GetComponent<Rigidbody>().velocity;
                cohesion += neighbourPos;
            }

            if (i > 0)
            {
                separation /= i;
                alignment /= i;
                cohesion /= i;
            }
            
            alignment = alignment - boid.velocity;
            cohesion = cohesion - boid.position;
            
            separation.y = 0;
            alignment.y = 0;
            cohesion.y = 0;
            centering.y = 0;
            followPlayer.y = 0;

            separation.Normalize();
            alignment.Normalize();
            cohesion.Normalize();
            centering.Normalize();
            followPlayer.Normalize();
            
            Debug.DrawRay(boid.position, separation * settings.separationAmp, Color.red);
            Debug.DrawRay(boid.position, alignment * settings.alignmentAmp, Color.green);
            Debug.DrawRay(boid.position, cohesion * settings.cohesionAmp, Color.blue);
            Debug.DrawRay(boid.position, centering * settings.centeringAmp, Color.magenta);
            Debug.DrawRay(boid.position, followPlayer * settings.followPlayerAmp, Color.cyan);

            boid.AddForce(separation * settings.separationAmp);
            boid.AddForce(alignment * settings.alignmentAmp);
            boid.AddForce(cohesion * settings.cohesionAmp);
            boid.AddForce(centering * settings.centeringAmp);
            boid.AddForce(followPlayer * settings.followPlayerAmp);

            //boid.AddForce(separation + alignment + cohesion + centering);

            /*
            if (boid.velocity.magnitude > Mathf.Epsilon)
            {
                Vector2 forward = Vector2.Lerp(boid.transform.forward, boid.velocity, 0.5f);
                boid.transform.forward = new Vector3(boid.velocity.x, 0, boid.velocity.y);
            }
            */
        }
    }

    public void AddBoid(Rigidbody boid)
    {
        boids.Add(boid);
    }
    
    public void RemoveBoid(Rigidbody boid)
    {
        boids.Remove(boid);
    }
}
