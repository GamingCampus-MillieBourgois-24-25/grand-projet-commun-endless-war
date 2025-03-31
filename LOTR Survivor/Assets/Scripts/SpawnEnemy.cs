using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs; // Array of enemy prefabs
    public Transform player; // Assign the player object in the Inspector
    public float spawnRadius = 10f; // Radius around the player to spawn enemies
    public float spawnRate = 1f; // How often enemies spawn
    public int enemiesPerWave = 5; // Number of enemies per wave
    public float navMeshCheckRadius = 2f; // How far to check for a valid NavMesh position

    private void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            for (int i = 0; i < enemiesPerWave; i++)
            {
                SpawnEnemy();
            }
            yield return new WaitForSeconds(spawnRate);
        }
    }

    private void SpawnEnemy()
    {
        if (enemyPrefabs.Length == 0) return; // Ensure there are enemy prefabs

        // Get a random enemy prefab from the array
        GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

        // Get a random angle in radians
        float angle = Random.Range(0f, Mathf.PI * 2);

        // Calculate spawn position using sine and cosine
        Vector3 spawnPosition = new Vector3(
            player.position.x + Mathf.Cos(angle) * spawnRadius,
            player.position.y,
            player.position.z + Mathf.Sin(angle) * spawnRadius
        );

        // Check if the position is on the NavMesh
        if (NavMesh.SamplePosition(spawnPosition, out NavMeshHit hit, navMeshCheckRadius, NavMesh.AllAreas))
        {
            spawnPosition = hit.position; // Adjust position to be on the NavMesh
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }
    }

    private void OnDrawGizmos()
    {
        if (player != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(player.position, spawnRadius);
        }
    }
}
