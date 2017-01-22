using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenDestroy : MonoBehaviour
{

	public float destroyTimer;

	// Use this for initialization
	void Start ()
	{
		Destroy (gameObject, destroyTimer);
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}
