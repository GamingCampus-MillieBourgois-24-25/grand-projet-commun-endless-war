using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyWave", menuName = "Waves/New Enemy Wave")]
public class EnemyWaveSO : ScriptableObject
{
    public EnemySO[] enemySOs;
    public float spawnInterval;
    public float waveCooldown;
    public int maxEnemiesSpawn = 4;
}
