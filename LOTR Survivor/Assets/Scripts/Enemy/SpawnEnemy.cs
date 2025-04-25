using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance { get; private set; }

    [Header("Player Reference")]
    public Transform player;

    [Header("Spawning Settings")]
    public float spawnRadius = 10f;
    public float despawnDistance = 30f;
    public float navMeshCheckRadius = 2f;

    [Header("Wave Settings")]
    public EnemyWaveSO[] waves;
    private EnemyWaveSO currentWave;
    private int currentWaveIndex = 0;

    [Header("Timers")]
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private float finalSurviveTime = 15f;
    [SerializeField] private float warningTime = 10f;

    private List<GameObject> activeEnemies = new List<GameObject>();
    private Coroutine waveCoroutine;
    private Coroutine spawnCoroutine;
    private float waveTimer = 0f;

    private bool isSpawningPaused = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void OnEnable()
    {
        PlayerEvents.OnPlayerSpawned += AssignPlayer;
    }

    private void OnDisable()
    {
        PlayerEvents.OnPlayerSpawned -= AssignPlayer;
    }

    private void AssignPlayer(GameObject playerObj)
    {
        player = playerObj.transform;
        Debug.Log("[EnemySpawner] Player assigned.");

        if (waveCoroutine == null && waves.Length > 0)
        {
            waveCoroutine = StartCoroutine(ManageWaves());
        }
    }

    private IEnumerator ManageWaves()
    {
        while (currentWaveIndex < waves.Length)
        {
            currentWave = waves[currentWaveIndex];

            if (currentWave == null || currentWave.enemySOs.Length == 0)
            {
                Debug.LogWarning("Wave is empty or invalid. Skipping.");
                currentWaveIndex++;
                continue;
            }

            WaveCountdownTimer.Instance?.StartCountdown((int)warningTime);
            yield return new WaitForSeconds(warningTime);

            waveTimer = 0f;
            spawnCoroutine = StartCoroutine(SpawnContinuously(currentWave));

            while (waveTimer < currentWave.waveCooldown)
            {
                if (!isSpawningPaused)
                    waveTimer += Time.deltaTime;

                yield return null;
            }

            if (spawnCoroutine != null)
                StopCoroutine(spawnCoroutine);

            currentWaveIndex++;
        }

        // Final survival phase
        WaveCountdownTimer.Instance?.DisplayFinalSurviveCountdown((int)finalSurviveTime);
        yield return new WaitForSeconds(finalSurviveTime);

        GamePauseManager.Instance?.PauseGame();
        VictoryCanvas victoryCanvas = FindObjectOfType<VictoryCanvas>();
        victoryCanvas?.DisplayUI();
    }

    private IEnumerator SpawnContinuously(EnemyWaveSO wave)
    {
        while (true)
        {
            if (!isSpawningPaused)
                SpawnEnemy(wave);

            yield return new WaitForSeconds(wave.spawnInterval);
        }
    }

    private void SpawnEnemy(EnemyWaveSO wave)
    {
        if (player == null)
        {
            Debug.LogWarning("[EnemySpawner] No player assigned, cannot spawn.");
            return;
        }

        EnemySO enemyData = wave.enemySOs[Random.Range(0, wave.enemySOs.Length)];
        if (enemyData == null || enemyData.prefab == null)
        {
            Debug.LogWarning("[EnemySpawner] Invalid enemy in wave.");
            return;
        }

        float angle = Random.Range(0f, Mathf.PI * 2f);
        Vector3 spawnPos = player.position + new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * spawnRadius;

        if (NavMesh.SamplePosition(spawnPos, out NavMeshHit hit, navMeshCheckRadius, NavMesh.AllAreas))
        {
            GameObject enemy;

            if (ObjectPool.Instance != null)
            {
                enemy = ObjectPool.Instance.Spawn(enemyData.prefab, hit.position, Quaternion.identity);
            }
            else
            {
                enemy = Instantiate(enemyData.prefab, hit.position, Quaternion.identity);
            }

            if (enemy != null)
                activeEnemies.Add(enemy);
        }
        else
        {
            Debug.LogWarning("[EnemySpawner] No valid NavMesh at spawn position.");
        }
    }

    private void Update()
    {
        DespawnDistantEnemies();
    }

    private void DespawnDistantEnemies()
    {
        if (player == null) return;

        for (int i = activeEnemies.Count - 1; i >= 0; i--)
        {
            GameObject enemy = activeEnemies[i];
            if (enemy == null)
            {
                activeEnemies.RemoveAt(i);
                continue;
            }

            float dist = Vector3.Distance(player.position, enemy.transform.position);
            if (dist > despawnDistance)
            {
                if (ObjectPool.Instance != null)
                {
                    ObjectPool.Instance.Despawn(enemy, enemy.GetComponent<EnemyHealthBehaviour>().enemyData.prefab);
                }
                else
                {
                    Destroy(enemy);
                }

                activeEnemies.RemoveAt(i);
            }
        }
    }

    public void PauseSpawning() => isSpawningPaused = true;
    public void ResumeSpawning() => isSpawningPaused = false;

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
