using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour {

    public GameObject monsterPrefab;
    public Transform bridgeLineSpawner;
    public Transform bridgeLineTarget;
    public float distanceToStop;
    public float speed;
    public Vector2 timeInterval;
    public bool randomColor;

    public Image credits;
    public float creditsFromScale;
    public float creditsToScale;
    public float creditsScaleSpeed;

    float randomInterval;
    float timer;

    List<GameObject> monsterMarchers;

	// Use this for initialization
	void Start () {

        monsterMarchers = new List<GameObject>();
        randomInterval = Random.Range(timeInterval.x, timeInterval.y);
        credits.gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;

        if (credits.IsActive())
        {          
            credits.rectTransform.localScale = Vector3.Lerp(credits.rectTransform.localScale, Vector3.one * creditsToScale, creditsScaleSpeed);
        }
        else
        {
            credits.rectTransform.localScale = Vector3.one * creditsFromScale;
        }

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

    public void OnPlay()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void OnCredtis()
    {
        credits.gameObject.SetActive(!credits.IsActive());
    }
}
