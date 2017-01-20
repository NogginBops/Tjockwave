﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    Plane raycastPlane = new Plane(Vector3.up, Vector3.zero);

    public string arenaLayerMask;

    [Header("Movement settings")]

    public float speed;
    public float maxSpeed;
    public float turnSpeed;
    public float jumpForce;
    public float fallingG;

    [Tooltip("Gravity")]
    public float G;
    float staticG;

    [Header("Slam settings")]

    [Tooltip("The height that the player must be off the ground in order to do a slam attack.")]
    public float slamJumpHeight;
    public float slamForce;
    public float slamDistanceToGround;
    public GameObject slamShockWave;

    Rigidbody rb;
    Vector3 targetPosition;

    bool slamming = false;
    bool grounded = true;

    float rayDistance = 15;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        staticG = G;

    }
	
	// Update is called once per frame
	void FixedUpdate () {

        // Gravity
        if (rb.velocity.y <= 0 && grounded == false)
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
        RaycastHit hit;
        float currentPlayerHeight = 0;

        if (Physics.Raycast(ray, out hit, rayDistance, 1 << LayerMask.NameToLayer(arenaLayerMask)))
        {
            currentPlayerHeight = (hit.point - transform.position).magnitude;
        }

        if (currentPlayerHeight < slamDistanceToGround)
        {
            grounded = true;

            if (slamming == true)
            {
                Instantiate(slamShockWave, transform.position, Quaternion.identity);
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

            rb.velocity = Vector3.Lerp(rb.velocity, transform.forward, 0.1f);

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
            if (slamming == false)
            {             
                // If the player is high up enough in the air...
                if (currentPlayerHeight > slamJumpHeight)
                {
                    // ... Slam down.
                    Debug.Log("SLAM!");
                    rb.AddForce(Vector3.down * slamForce);
                    slamming = true;
                }
            }
        }
    }
}
