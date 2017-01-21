using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodController : MonoBehaviour {

    public GameObject foodPrefab;
    public int maxFood;

    public float stallSpawnHeight;
    public float spawnHeight;

    public float minDelay;
    public float maxDelay;

    public float throwDirForce;
    public float throwUpForce;

    float timer;
    float randomDelay;

    BoxCollider area;
    List<Food> currentFood;

    GameObject[] stalls;

	// Use this for initialization
	void Start () {
        area = GetComponent<BoxCollider>();
        currentFood = new List<Food>();

        stalls = GameObject.FindGameObjectsWithTag("Stall");

        randomDelay = Random.Range(minDelay, maxDelay);
    }
	
	// Update is called once per frame
	void Update () {
        if (currentFood.Count < maxFood)
        {
            timer += Time.deltaTime;

            if (timer > randomDelay)
            {
                Vector3 targetPos = RandomPos();
                GameObject randomStall = stalls[Random.Range(0, stalls.Length)];

                Vector3 stallPos = new Vector3(
                    randomStall.transform.position.x, 
                    stallSpawnHeight, 
                    randomStall.transform.position.z
                    );

                Vector3 dir = (targetPos - stallPos).normalized;

                currentFood.Add(Instantiate(foodPrefab, stallPos, Quaternion.identity, transform).GetComponent<Food>());
                currentFood[currentFood.Count - 1].GetComponent<Rigidbody>().AddForce(dir * throwDirForce + Vector3.up * throwUpForce);
                randomDelay = Random.Range(minDelay, maxDelay);
                timer = 0;
            }
        }
        else
        {
            timer = 0;
        }
	}

    public void RemoveFood(Food food)
    {
        currentFood.Remove(food);
    }

    Vector3 RandomPos()
    {
        return new Vector3(
            Random.Range(area.bounds.min.x, area.bounds.max.x),
            spawnHeight, 
            Random.Range(area.bounds.min.z, area.bounds.max.z)
           );
    }
}
