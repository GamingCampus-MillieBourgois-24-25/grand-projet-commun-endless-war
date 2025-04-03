using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyWave", menuName = "Waves/New Enemy Wave")]
public class EnemyWaveSO : ScriptableObject
{
    public EnemySO[] enemySOs;  // Array of enemy types in the wave
    public int minEnemies;      // Minimum enemies per wave
    public float spawnInterval; // Time between enemy spawns
    public float waveCooldown;  // Time before the next wave starts
}
