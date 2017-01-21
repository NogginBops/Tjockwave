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
    List<Food> currentFood;

	// Use this for initialization
	void Start () {
        area = GetComponent<BoxCollider>();
        currentFood = new List<Food>();
    }
	
	// Update is called once per frame
	void Update () {
        if (currentFood.Count < maxFood)
        {
            if (Random.Range(0f, 1f) < spawnChance)
            {
                currentFood.Add(Instantiate(foodPrefab, RandomPos(), Quaternion.identity, transform).GetComponent<Food>());
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

    public void RemoveFood(Food food)
    {
        currentFood.Remove(food);
    }
}
