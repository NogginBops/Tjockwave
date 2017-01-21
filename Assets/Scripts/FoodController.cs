using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodController : MonoBehaviour {

    public GameObject foodPrefab;
    public int maxFood;

    [Tooltip("A decimal chance of food spawning every frame (between 0-1).")]
    public float spawnChance;
    public float spawnHeight;

    BoxCollider area;
    int currentFoodAmount;

	// Use this for initialization
	void Start () {
        area = GetComponent<BoxCollider>();
	}
	
	// Update is called once per frame
	void Update () {
        if (currentFoodAmount < maxFood)
        {
            if (Random.Range(0f, 1f) < spawnChance)
            {         
                Instantiate(foodPrefab, RandomPos(), Quaternion.identity);
                currentFoodAmount++;
            }
        }
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
