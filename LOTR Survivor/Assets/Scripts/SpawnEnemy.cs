using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    public Transform player; // Assign the player object in the Inspector
    public float spawnRadius = 10f; // Radius around the player to spawn enemies
    public float despawnDistance = 30f; // Distance at which enemies despawn
    public float navMeshCheckRadius = 2f; // How far to check for a valid NavMesh position
    public float timerBetweenWaves = 20f;
    public EnemyWave[] waves; // Array of enemy waves
    private List<GameObject> activeEnemies = new List<GameObject>();
    private int currentWaveIndex = 0;

    private void Start()
    {
        StartCoroutine(ManageWaves());
    }

    private IEnumerator ManageWaves()
    {
        while (currentWaveIndex < waves.Length)
        {
            EnemyWave wave = waves[currentWaveIndex];
            yield return StartCoroutine(SpawnWave(wave));
            yield return new WaitForSeconds(timerBetweenWaves); // Wait for next wave
            currentWaveIndex++;
        }
    }

    private IEnumerator SpawnWave(EnemyWave wave)
    {
        while (GetActiveEnemyCount() < wave.minEnemies)
        {
            SpawnEnemy(wave);
            yield return new WaitForSeconds(wave.spawnInterval);
        }
    }

    private void SpawnEnemy(EnemyWave wave)
    {
        if (wave.enemyPrefabs.Length == 0) return;

        GameObject enemyPrefab = wave.enemyPrefabs[Random.Range(0, wave.enemyPrefabs.Length)];
        float angle = Random.Range(0f, Mathf.PI * 2);
        Vector3 spawnPosition = new Vector3(
            player.position.x + Mathf.Cos(angle) * spawnRadius,
            player.position.y,
            player.position.z + Mathf.Sin(angle) * spawnRadius
        );

        if (NavMesh.SamplePosition(spawnPosition, out NavMeshHit hit, navMeshCheckRadius, NavMesh.AllAreas))
        {
            spawnPosition = hit.position;
            GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            activeEnemies.Add(enemy);
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
            if (activeEnemies[i] == null) // Check if the enemy has already been destroyed
            {
                activeEnemies.RemoveAt(i);
                continue;
            }

            if (Vector3.Distance(player.position, activeEnemies[i].transform.position) > despawnDistance)
            {
                Destroy(activeEnemies[i]);
                activeEnemies.RemoveAt(i);
            }
        }
    }

    private int GetActiveEnemyCount()
    {
        activeEnemies.RemoveAll(enemy => enemy == null); // Cleanup destroyed enemies
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
