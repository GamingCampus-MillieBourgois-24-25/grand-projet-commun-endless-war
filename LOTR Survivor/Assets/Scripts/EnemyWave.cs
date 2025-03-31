using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class EnemyWave
{
    public GameObject[] enemyPrefabs; // Array of enemy prefabs for this wave
    public int minEnemies = 10; // Minimum number of enemies in the wave
    public float spawnInterval = 1f; // Interval between spawns
}