using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    Plane raycastPlane = new Plane(Vector3.up, Vector3.zero);

    public string arenaLayerMask;
    
    public float health;

    [Header("Movement settings")]

    public float speed;
    public float turnSpeed;
    public float jumpForce;

    [Tooltip("Gravity")]
    public float G;
    [Tooltip("The amount of gravity that will work on the player when you hold space.")]
    public float spaceG;
    [Tooltip("The amount of gravity that will work on the player when you fall.")]
    public float fallingG;
    float staticG;

    [Header("Slam settings")]

    [Tooltip("The height that the player must be off the ground in order to do a slam attack.")]
    public float slamMinJumpHeight;
    public float slamDownForce;
    public float slamDistanceToGround;
    public GameObject slamShockWave;

    public GameObject slamShockwaveParticles;
    
    public CameraScript cameraScript;

    [Header("Food settings")]
    public int foodCounter;

    public float healthMultiplier;
    public float speedDivisor;
    public float scaleMultiplier;

    [Tooltip("The stats the player will have at every food count. Element 0 is 1 food.")]
    public FoodBoostData[] foodBoostData;

    float baseHealth;
    float baseSpeed;

    Rigidbody rb;
    Vector3 targetPosition;

    bool slamming = false;
    bool grounded = true;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();

        staticG = G;
        baseHealth = health;
        baseSpeed = speed;

    }
    
	// Update is called once per frame
	void FixedUpdate () {

        // Gravity
        if (Input.GetKey(KeyCode.Space) && rb.velocity.y > 0)
        {
            G = spaceG;
        }
        else if (rb.velocity.y <= 0 && grounded == false)
        {
            G = fallingG;
        }
        else
        {
            G = staticG;
        }

        rb.AddForce(Vector3.down * G * rb.mass);

        // Measures the height the player is off the ground.
        Ray ray = new Ray(transform.position, Vector3.down);
        float currentPlayerHeight = 0;

        raycastPlane.Raycast(ray, out currentPlayerHeight);

        if (currentPlayerHeight < slamDistanceToGround)
        {
            grounded = true;

            if (slamming == true)
            {
                Instantiate(slamShockWave, transform.position, Quaternion.identity);
                Instantiate(slamShockwaveParticles, transform.position, Quaternion.Euler(new Vector3(90, 0, 0)));

                cameraScript.ShockwaveCameraEffect();

                // Changes the physics layer of the player and its children.
                gameObject.layer = LayerMask.NameToLayer("Player");
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.layer = LayerMask.NameToLayer("Player");
                }

                ReturnToBaseValues();
                slamming = false;
            }
        }
        else
        {
            grounded = false;
        }

        // -- MOVE TO MOUSE --
        if (Input.GetMouseButton(0))
        {
            Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            float dist;

            if (raycastPlane.Raycast(cameraRay, out dist))
            {
                targetPosition = cameraRay.GetPoint(dist);
            }
            
            Vector3 dir = (targetPosition - transform.position);
            dir.y = 0;
            dir.Normalize();

            rb.AddForce(dir * speed, ForceMode.Force);

            Vector2 vel2d = Vector2.Lerp(rb.velocity.xz(), transform.forward.xz(), 0.1f);

            rb.velocity = new Vector3(vel2d.x, rb.velocity.y, vel2d.y);

            Quaternion rotation = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * turnSpeed);
        }
        else
        {
            targetPosition = transform.position;
        }

        // -- JUMP --
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (grounded)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (slamming == false && foodCounter > 0)
            {             
                // If the player is high up enough in the air...
                if (currentPlayerHeight > slamMinJumpHeight)
                {
                    // ... Slam down.
                    Debug.Log("SLAM!");
                    rb.AddForce(Vector3.down * slamDownForce);

                    // Changes the physics layer of the player and its children.
                    gameObject.layer = LayerMask.NameToLayer("PlayerSlam");
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        transform.GetChild(i).gameObject.layer = LayerMask.NameToLayer("PlayerSlam");
                    }

                    slamming = true;
                }
            }
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
            health = 0;
            Debug.Log("WASTED!");
        }
    }

    public void Eat()
    {
        foodCounter++;
        if (foodCounter - 1 < foodBoostData.Length)
        {       
            health = foodBoostData[foodCounter - 1].health;
            speed = foodBoostData[foodCounter - 1].speed;
            transform.localScale = foodBoostData[foodCounter - 1].scale;
            slamDistanceToGround = foodBoostData[foodCounter - 1].slamDistanceToGround;
        }
    }

    public void ReturnToBaseValues()
    {
        foodCounter = 0;
        health = baseHealth;
        speed = baseSpeed;
        transform.localScale = Vector3.one;
        slamDistanceToGround = transform.localScale.x;
    }

    [System.Serializable]
    public struct FoodBoostData
    {
        public float health;
        public float speed;
        public Vector3 scale;
        public float slamDistanceToGround;
    }
}
