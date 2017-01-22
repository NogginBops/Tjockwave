using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AS_FollowEnemy : MonoBehaviour
{
	public GameObject target;
	public float speed;
	public GameObject lookTarget;

	void Update ()
	{
		if (target == null)
			target = GameObject.FindGameObjectWithTag ("Player");

		/*if (lookTarget == null)
			lookTarget = GameObject.FindGameObjectsWithTag ("Player");*/

		//transform.LookAt (lookTarget);
		float step = speed * Time.deltaTime;
		transform.position = Vector3.MoveTowards (transform.position, target.transform.position, step);
	}
}
