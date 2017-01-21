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
        public Flock flock;
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

    public List<BoxCollider> SpawnAreas;

    Queue<Enemy> spawnQueue = new Queue<Enemy>();

    float timer;

    public int currWave = 0;
    
	// Use this for initialization
	void Start () {
        StartCoroutine(SpawnEnemies());

        LoadWave(currWave);
    }
	
	// Update is called once per frame
	void Update () {

        if (currWave + 1 >= Waves.Count)
        {
            return;
        }

        timer += Time.deltaTime;

        Waves[currWave].waveDuration -= Time.deltaTime;

        if (Waves[currWave].waveDuration <= 0)
        {
            currWave++;

            LoadWave(currWave);
        }
        else if (Waves[currWave].flock.IsEmpty() && spawning == false)
        {
            StartCoroutine(LoadWaveDelayed(2));
        }
	}

    bool spawning;
    
    IEnumerator LoadWaveDelayed(int seconds)
    {
        spawning = true;

        yield return new WaitForSeconds(seconds);
        
        currWave++;

        LoadWave(currWave);

        spawning = false;
    }

    void LoadWave(int wave)
    {
        GameObject flockGO = new GameObject("Flock " + wave, typeof(Flock));

        flockGO.transform.SetParent(this.transform);

        Flock flock = flockGO.GetComponent<Flock>();

        flock.settings = flockSettigns;

        flock.player = player;

        Waves[wave].flock = flock;

        // Spawn initial enemy
        SpawnEnemy(new Enemy(Waves[wave].enemyPrefab, flock));

        //Queue up enemies to spawn
        for (int i = 0; i < Waves[wave].enemyCount - 1; i++)
        {
            spawnQueue.Enqueue(new Enemy(Waves[wave].enemyPrefab, flock));
        }
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            if (spawnQueue.Count > 0) {
                
                SpawnEnemy(spawnQueue.Dequeue());

            }

            yield return new WaitForSeconds(enemySpawnDelay);
        }
    }

    void SpawnEnemy(Enemy enemy)
    {
        BoxCollider spawnArea = SpawnAreas[Random.Range(0, SpawnAreas.Count)];

        Vector3 pos = Random.insideUnitSphere;

        pos.x *= spawnArea.size.x;
        pos.y *= spawnArea.size.y;
        pos.z *= spawnArea.size.z;

        pos += spawnArea.center;

        GameObject enemyGO = Instantiate(enemy.prefab, spawnArea.transform.position + pos, Quaternion.identity);

        enemyGO.transform.SetParent(enemy.flock.transform);

        enemyGO.transform.localScale = Vector3.one * Random.Range(0.4f, 1.6f);

        enemy.flock.AddBoid(enemyGO.GetComponent<Rigidbody>());
    }
}
