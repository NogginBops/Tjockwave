using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    public Transform pivot;
    public Transform cameraTransform;

    public Transform player;

    new Camera camera;

    CameraShake cameraShake;

    Vector3 originPos;

    public float rotationAmp;
    
    // Use this for initialization
    void Start()
    {
        camera = cameraTransform.GetComponent<Camera>();

        cameraShake = cameraTransform.GetComponent<CameraShake>();

        originPos = transform.position;
    }
	
	// Update is called once per frame
	void Update () {

        transform.position = (originPos + player.position) / 2;

        float xOffset = (originPos - transform.position).x;
        
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, xOffset * rotationAmp, transform.eulerAngles.z);
    }

    public void ShockwaveCameraEffect()
    {
        cameraShake.ShakeCamera(0.4f, 0.4f);
    }
}
