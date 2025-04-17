using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance { get; private set; }

    public Transform player;
    public float spawnRadius = 10f;
    public float despawnDistance = 30f;
    public float navMeshCheckRadius = 2f;
    public EnemyWaveSO[] waves;

    private List<GameObject> activeEnemies = new List<GameObject>();
    private int currentWaveIndex = 0;

    private Coroutine waveCoroutine;
    private Coroutine spawnCoroutine;

    private float waveTimer;
    private bool isSpawningPaused = false;
    private EnemyWaveSO currentWave;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        if (ObjectPool.Instance == null)
        {
            Debug.LogError("ObjectPool Instance is NULL! Make sure ObjectPool is in the scene.");
            return;
        }

        if (waves == null || waves.Length == 0)
        {
            Debug.LogError("No waves assigned to EnemySpawner!");
            return;
        }

        waveCoroutine = StartCoroutine(ManageWaves());
    }

    private IEnumerator ManageWaves()
    {
        while (currentWaveIndex < waves.Length)
        {
            currentWave = waves[currentWaveIndex];

            if (currentWave == null || currentWave.enemySOs.Length == 0)
            {
                Debug.LogError("Wave data is missing or empty!");
                currentWaveIndex++;
                continue;
            }

            waveTimer = 0f;
            spawnCoroutine = StartCoroutine(SpawnContinuously(currentWave));

            while (waveTimer < currentWave.waveCooldown)
            {
                if (!isSpawningPaused)
                    waveTimer += Time.deltaTime;

                yield return null;
            }

            StopCoroutine(spawnCoroutine);
            currentWaveIndex++;
        }
    }

    private IEnumerator SpawnContinuously(EnemyWaveSO wave)
    {
        while (true)
        {
            if (!isSpawningPaused)
            {
                SpawnEnemy(wave);
            }
            yield return new WaitForSeconds(wave.spawnInterval);
        }
    }

    private void SpawnEnemy(EnemyWaveSO wave)
    {
        EnemySO enemyData = wave.enemySOs[Random.Range(0, wave.enemySOs.Length)];

        if (enemyData == null || enemyData.prefab == null)
        {
            Debug.LogError("EnemySO or prefab is null!");
            return;
        }

        float angle = Random.Range(0f, Mathf.PI * 2);
        Vector3 spawnPosition = new Vector3(
            player.position.x + Mathf.Cos(angle) * spawnRadius,
            player.position.y,
            player.position.z + Mathf.Sin(angle) * spawnRadius
        );

        if (NavMesh.SamplePosition(spawnPosition, out NavMeshHit hit, navMeshCheckRadius, NavMesh.AllAreas))
        {
            spawnPosition = hit.position;
            GameObject enemy = ObjectPool.Instance.Spawn(enemyData.prefab, spawnPosition, Quaternion.identity);

            if (enemy != null)
            {
                enemy.GetComponent<EnemyHealthBehaviour>().Initialize(enemyData);
                activeEnemies.Add(enemy);
            }
            else
            {
                Debug.LogError("Failed to spawn enemy from pool!");
            }
        }
        else
        {
            Debug.LogWarning("No valid NavMesh position found for enemy spawn.");
        }
    }

    private void Update()
    {
        DespawnDistantEnemies();
    }

    private void DespawnDistantEnemies()
    {
        for (int i = activeEnemies.Count - 1; i >= 0; i--)
        {
            GameObject enemy = activeEnemies[i];
            if (enemy == null)
            {
                activeEnemies.RemoveAt(i);
                continue;
            }

            if (Vector3.Distance(player.position, enemy.transform.position) > despawnDistance)
            {
                ObjectPool.Instance.Despawn(enemy, enemy.GetComponent<EnemyHealthBehaviour>().enemyData.prefab);
                activeEnemies.RemoveAt(i);
            }
        }
    }

    public void PauseSpawning()
    {
        isSpawningPaused = true;
    }

    public void ResumeSpawning()
    {
        isSpawningPaused = false;
    }

    private void OnDrawGizmos()
    {
        if (player != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(player.position, spawnRadius);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(player.position, despawnDistance);
        }
    }
}
