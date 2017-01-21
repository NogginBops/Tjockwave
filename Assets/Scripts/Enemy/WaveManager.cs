using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour {
    
    [System.Serializable]
    public class Wave
    {
        public int enemyCount;
        public float waveDuration;
        public GameObject enemyPrefab;
    }

    public struct Enemy
    {
        public GameObject prefab;
        public Flock flock;

        public Enemy(GameObject prefab, Flock flock)
        {
            this.prefab = prefab;
            this.flock = flock;
        }
    }

    public float enemySpawnDelay;

    public Vector3 spawnOffset;

    public Transform player;

    public FlockSettings flockSettigns;
    
    public List<Wave> Waves;

    Queue<Enemy> spawnQueue = new Queue<Enemy>();

    float timer;

    int currWave = 0;
    
	// Use this for initialization
	void Start () {
        StartCoroutine(SpawnEnemies());

        LoadWave(currWave);
    }
	
	// Update is called once per frame
	void Update () {

        timer += Time.deltaTime;

        Waves[currWave].waveDuration -= Time.deltaTime;

        if (Waves[currWave].waveDuration <= 0)
        {
            currWave++;

            LoadWave(currWave);
        }

	}

    void LoadWave(int wave)
    {
        GameObject flockGO = new GameObject("Flock " + wave, typeof(Flock));

        flockGO.transform.SetParent(this.transform);

        Flock flock = flockGO.GetComponent<Flock>();

        flock.settings = flockSettigns;

        flock.player = player;

        //Queue up enemies to spawn
        for (int i = 0; i < Waves[wave].enemyCount; i++)
        {
            spawnQueue.Enqueue(new Enemy(Waves[wave].enemyPrefab, flock));
        }
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            if (spawnQueue.Count > 0) {

                Enemy enemy = spawnQueue.Dequeue();

                Vector2 randPos = Random.insideUnitCircle * Random.Range(1, 8);

                GameObject enemyGO = Instantiate(enemy.prefab, spawnOffset + new Vector3(randPos.x, 0, randPos.y), Quaternion.identity);

                enemyGO.transform.SetParent(enemy.flock.transform);

                //enemyGO.GetComponent<Rigidbody>().velocity = new Vector3(randPos.x, 0, randPos.y) * 4;

                enemy.flock.AddBoid(enemyGO.GetComponent<Rigidbody>());
            }

            yield return new WaitForSeconds(enemySpawnDelay);
        }
    }
}
