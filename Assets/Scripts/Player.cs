using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    Plane raycastPlane = new Plane(Vector3.up, Vector3.zero);

    public string arenaLayerMask;
    
    public float health;

    public FoodBoostData settings;
    FoodBoostData baseSettings;

    [Header("Food settings")]
    public int foodCounter;

    [Header("Shockwave prefabs")]
    public GameObject slamShockwaveParticles;
    public GameObject slamShockWave;

    public CameraScript cameraScript;

    public Animator playerAnimation;
    
    [Tooltip("The stats the player will have at every food count. Element 0 is 1 food.")]
    public FoodBoostData[] foodBoostData;

    float baseHealth;
    float baseSpeed;
    float staticG;

    Rigidbody rb;
    Vector3 targetPosition;

    bool slamming = false;
    bool grounded = true;

    float spaceTimer;

    float currentPlayerHeight = 0;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();

        baseSettings = settings;
        staticG = settings.G;
        baseHealth = health;
        baseSpeed = settings.speed;

    }

    void Update()
    {
        // Space Timer
        if (Input.GetKeyUp(KeyCode.Space))
        {
            spaceTimer = 0;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (grounded)
            {
                rb.AddForce(Vector3.up * settings.jumpForce, ForceMode.Impulse);
                playerAnimation.SetTrigger("IsJumping");
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (slamming == false && foodCounter > 0)
            {
                // If the player is high up enough in the air...
                if (currentPlayerHeight > settings.slamMinJumpHeight)
                {
                    // ... Slam down.
                    Debug.Log("SLAM!");
                    rb.AddForce(Vector3.down * settings.slamDownForce);

                    // Changes the physics layer of the player and its children.
                    gameObject.layer = LayerMask.NameToLayer("PlayerSlam");
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        transform.GetChild(i).gameObject.layer = LayerMask.NameToLayer("PlayerSlam");
                    }

                    slamming = true;

                    playerAnimation.SetBool("Grounded", true);
                }
            }
        }

        playerAnimation.SetBool("Grounded", grounded);
        
        playerAnimation.SetBool("IsRunning", rb.velocity.magnitude > 0.001f);
        
    }

    // Update is called once per frame
    void FixedUpdate () {
        // Gravity
        if (Input.GetKey(KeyCode.Space) && rb.velocity.y > 0 && spaceTimer < settings.spaceDuration)
        {
            spaceTimer += Time.fixedDeltaTime;
            settings.G = settings.spaceG;
        }
        else if (rb.velocity.y <= 0 && grounded == false)
        {
            settings.G = settings.fallingG;
        }
        else
        {
            settings.G = staticG;
        }

        rb.AddForce(Vector3.down * settings.G, ForceMode.Acceleration);

        // Measures the height the player is off the ground.
        Ray ray = new Ray(transform.position, Vector3.down);

        raycastPlane.Raycast(ray, out currentPlayerHeight);

        if (currentPlayerHeight < settings.slamDistanceToGround)
        {
            grounded = true;

            if (slamming == true)
            {
                ShockWave shockWave = Instantiate(slamShockWave, transform.position, Quaternion.identity).GetComponent<ShockWave>();
                shockWave.force = settings.shockForce;
                shockWave.maxSize = settings.shockMaxSize;
                shockWave.speed = settings.shockSpeed;

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

            rb.AddForce(dir * settings.speed, ForceMode.Force);

            Vector2 vel2d = Vector2.Lerp(rb.velocity.xz(), transform.forward.xz(), 0.1f);

            rb.velocity = new Vector3(vel2d.x, rb.velocity.y, vel2d.y);

            Quaternion rotation = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.fixedDeltaTime * settings.turnSpeed);
        }
        else
        {
            targetPosition = transform.position;
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
            Die();
        }

        UIController.Instance.SetPlayerHealth(health / baseHealth);
    }

    public void Die()
    {
        Debug.Log("WASTED!");
    }

    public void Eat()
    {
        foodCounter++;
        if (foodCounter - 1 < foodBoostData.Length)
        {
            ReplaceSettings(foodBoostData[foodCounter - 1]);
        }
        else
        {
            foodCounter--;
        }

        UIController.Instance.SetFoodPercentage((int)((foodCounter / (float)(foodBoostData.Length)) * 100));
    }

    public void ReturnToBaseValues()
    {
        foodCounter = 0;
        ReplaceSettings(baseSettings);
        UIController.Instance.SetFoodPercentage(0);
    }

    [System.Serializable]
    public struct FoodBoostData
    {
        public Vector3 scale;

        [Header("Movement settings")]

        public float speed;
        public float turnSpeed;
        public float jumpForce;

        public float spaceDuration;

        [Tooltip("Gravity")]
        public float G;
        [Tooltip("The amount of gravity that will work on the player when you hold space.")]
        public float spaceG;
        [Tooltip("The amount of gravity that will work on the player when you fall.")]
        public float fallingG;

        [Header("Slam settings")]

        [Tooltip("The height that the player must be off the ground in order to do a slam attack.")]
        public float slamMinJumpHeight;
        public float slamDownForce;
        public float slamDistanceToGround;

        public float shockForce;
        public float shockSpeed;
        public float shockMaxSize;
    }

    void ReplaceSettings(FoodBoostData newData)
    {
        settings = newData;
        transform.localScale = newData.scale;
    }
}
