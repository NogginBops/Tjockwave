using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour {

    public GameObject monsterPrefab;
    public Transform bridgeLineSpawner;
    public Transform bridgeLineTarget;
    public float distanceToStop;
    public float speed;
    public Vector2 timeInterval;
    public bool randomColor;

    float randomInterval;
    float timer;

    List<GameObject> monsterMarchers;

	// Use this for initialization
	void Start () {

        monsterMarchers = new List<GameObject>();
        randomInterval = Random.Range(timeInterval.x, timeInterval.y);
    }
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;

        if (timer > randomInterval)
        {
            monsterMarchers.Add(Instantiate(monsterPrefab, bridgeLineSpawner.position, Quaternion.LookRotation(bridgeLineTarget.position - bridgeLineSpawner.position), transform) as GameObject);

            if(randomColor)
            {
                monsterMarchers[monsterMarchers.Count - 1].transform.GetChild(0).GetComponent<Renderer>().material.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            }
                             
            randomInterval = Random.Range(timeInterval.x, timeInterval.y);
            timer = 0;
        }

        foreach (GameObject monster in monsterMarchers)
        {
            monster.transform.position = Vector3.MoveTowards(monster.transform.position, bridgeLineTarget.position, Time.deltaTime * speed);

            float distance = (bridgeLineTarget.position - monster.transform.position).magnitude;

            if (distance <= distanceToStop)
            {
                monsterMarchers.Remove(monster);
                Destroy(monster);
            }
        }

	}
}
