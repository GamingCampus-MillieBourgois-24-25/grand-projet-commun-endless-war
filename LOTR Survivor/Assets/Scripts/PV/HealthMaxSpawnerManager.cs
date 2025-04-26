using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthMaxSpawnerManager : MonoBehaviour
{
    [SerializeField] public GameObject healthPickupPrefab;
    [SerializeField] public int numberToSpawn = 2;

    private List<HealthMaxSpawnerPoint> allSpawners = new();

    private void Awake()
    {
        allSpawners.AddRange(FindObjectsOfType<HealthMaxSpawnerPoint>());

        if(allSpawners.Count == 0 )
        {
            Debug.LogWarning("Aucun spawner trouvé dans la scène");
            return;
        }

        SpawnRandomPickupsAtStart();
    }

    private void SpawnRandomPickupsAtStart()
    {
        numberToSpawn = Mathf.Clamp(numberToSpawn, 0, allSpawners.Count);

        List<HealthMaxSpawnerPoint> candidates = new(allSpawners);
        for(int i = 0; i < numberToSpawn; i++)
        {
            int index = Random.Range(0, candidates.Count);
            candidates[index].Spawn(healthPickupPrefab);
            candidates.RemoveAt(index);
        }
    }
}
