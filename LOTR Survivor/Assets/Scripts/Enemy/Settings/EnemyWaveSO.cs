using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyWave", menuName = "Waves/New Enemy Wave")]
public class EnemyWaveSO : ScriptableObject
{
    public EnemyWaveEntry[] waveEntries;
    public float spawnInterval;
    public float waveCooldown;
}
