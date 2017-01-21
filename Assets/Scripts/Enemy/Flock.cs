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
    public float followPlayerAmp;
}

public class Flock : MonoBehaviour {
    
    public Vector2 center;

    public Transform player;

    List<Rigidbody> boids = new List<Rigidbody>();

    public FlockSettings settings;

    Collider[] neighbours = new Collider[20];
	
    void FixedUpdate()
    {
        foreach (var boid in boids)
        {
            Vector3 pos = boid.position;

            int i = Physics.OverlapSphereNonAlloc(pos, settings.neighborRadius, neighbours, 1 << LayerMask.NameToLayer("Enemy"));
            
            Vector3 separation = new Vector3();
            Vector3 alignment = new Vector3();
            Vector3 cohesion = new Vector3();
            Vector3 followPlayer = player.position - boid.position;

            foreach (var neighbour in neighbours.Take(i))
            {
                Vector3 neighbourPos = neighbour.transform.position;
                separation += pos - neighbourPos;
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
            cohesion = -(boid.position - cohesion);
            
            separation.y = 0;
            alignment.y = 0;
            cohesion.y = 0;
            followPlayer.y = 0;

            separation.Normalize();
            alignment.Normalize();
            cohesion.Normalize();
            followPlayer.Normalize();
            
            Debug.DrawRay(boid.position, separation * settings.separationAmp, Color.red);
            Debug.DrawRay(boid.position, alignment * settings.alignmentAmp, Color.green);
            Debug.DrawRay(boid.position, cohesion * settings.cohesionAmp, Color.blue);
            Debug.DrawRay(boid.position, followPlayer * settings.followPlayerAmp, Color.cyan);

            if (separation.sqrMagnitude > Mathf.Epsilon)
                boid.AddForce(separation * settings.separationAmp);
            if (alignment.sqrMagnitude > Mathf.Epsilon)
                boid.AddForce(alignment * settings.alignmentAmp);
            if (cohesion.sqrMagnitude > Mathf.Epsilon)
                boid.AddForce(cohesion * settings.cohesionAmp);
            if (followPlayer.sqrMagnitude > Mathf.Epsilon)
                boid.AddForce(followPlayer * settings.followPlayerAmp);
            
            if (boid.velocity.magnitude > Mathf.Epsilon)
            {
                Vector3 forward = Vector3.Lerp(boid.transform.forward, boid.velocity, 0.5f);
                forward.y = 0;
                boid.transform.forward = forward;
            }
        }
    }

    public void OnDrawGizmos()
    {
        /*foreach (var boid in boids)
        {
            //Gizmos.DrawWireSphere(boid.position, settings.neighborRadius);
        }*/
    }

    public void AddBoid(Rigidbody boid)
    {
        boids.Add(boid);
    }
    
    public void RemoveBoid(Rigidbody boid)
    {
        boids.Remove(boid);
    }

    public bool IsEmpty()
    {
        return boids.Count <= 0;
    }
}
