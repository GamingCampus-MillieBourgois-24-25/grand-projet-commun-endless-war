using UnityEngine;

[System.Serializable]
public class EnemyWaveEntry
{
    public EnemySO enemySO;
    public int spawnWeight = 1;
    public int maxInGroup = 1;
    public int maxThisEnemy = 999;
}
