using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    public Transform player; // Assign the player object in the Inspector
    public float spawnRadius = 10f; // Radius around the player to spawn enemies
    public float despawnDistance = 30f; // Distance at which enemies despawn
    public float navMeshCheckRadius = 2f; // How far to check for a valid NavMesh position
    public EnemyWaveSO[] waves; // Array of enemy waves

    private List<GameObject> activeEnemies = new List<GameObject>();
    private int currentWaveIndex = 0;
    private bool isWaveActive = false;

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
        StartCoroutine(ManageWaves());
    }

    private IEnumerator ManageWaves()
    {
        while (currentWaveIndex < waves.Length)
        {
            isWaveActive = true;
            yield return StartCoroutine(SpawnWave(waves[currentWaveIndex]));
            isWaveActive = false;
            yield return new WaitForSeconds(waves[currentWaveIndex].waveCooldown);
            currentWaveIndex++;
        }
    }

    private IEnumerator SpawnWave(EnemyWaveSO wave)
    {
        if (wave == null || wave.enemySOs.Length == 0)
        {
            Debug.LogError("Wave data is missing or empty!");
            yield break;
        }

        int enemiesSpawned = 0;
        while (enemiesSpawned < wave.minEnemies)
        {
            SpawnEnemy(wave);
            enemiesSpawned++;
            yield return new WaitForSeconds(wave.spawnInterval);
        }
    }

    private void SpawnEnemy(EnemyWaveSO wave)
    {
        if (wave.enemySOs.Length == 0)
        {
            Debug.LogError("No enemies assigned in EnemyWaveSO!");
            return;
        }

        // Pick a random enemy from the wave
        EnemySO enemyData = wave.enemySOs[Random.Range(0, wave.enemySOs.Length)];

        if (enemyData == null || enemyData.prefab == null)
        {
            Debug.LogError("EnemySO or prefab is null!");
            return;
        }

        // Generate a spawn position around the player
        float angle = Random.Range(0f, Mathf.PI * 2);
        Vector3 spawnPosition = new Vector3(
            player.position.x + Mathf.Cos(angle) * spawnRadius,
            player.position.y,
            player.position.z + Mathf.Sin(angle) * spawnRadius
        );

        // Ensure the spawn is on the NavMesh
        if (NavMesh.SamplePosition(spawnPosition, out NavMeshHit hit, navMeshCheckRadius, NavMesh.AllAreas))
        {
            spawnPosition = hit.position;
            GameObject enemy = ObjectPool.Instance.Spawn(enemyData.prefab, spawnPosition, Quaternion.identity);

            if (enemy != null)
            {
                enemy.GetComponent<EnemyHealthBehaviour>().Initialize(enemyData);
                activeEnemies.Add(enemy); // Track spawned enemy
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
            if (activeEnemies[i] == null)
            {
                activeEnemies.RemoveAt(i);
                continue;
            }

            if (Vector3.Distance(player.position, activeEnemies[i].transform.position) > despawnDistance)
            {
                ObjectPool.Instance.Despawn(activeEnemies[i], activeEnemies[i].GetComponent<EnemyHealthBehaviour>().enemyData.prefab);
                activeEnemies.RemoveAt(i);
            }
        }
    }

    private int GetActiveEnemyCount()
    {
        activeEnemies.RemoveAll(enemy => enemy == null);
        return activeEnemies.Count;
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
