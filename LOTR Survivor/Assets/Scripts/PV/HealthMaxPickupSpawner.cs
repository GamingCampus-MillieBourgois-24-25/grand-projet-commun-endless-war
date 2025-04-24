using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthMaxPickupSpawner : MonoBehaviour
{
    public GameObject healthPickupPrefab;

    public void SpawnHealthPickup()
    {
        if(healthPickupPrefab == null )
        {
            Debug.LogWarning("Assign a prefab in the inspector");
            return;
        }

        Vector3 spawnPosition = transform.position + transform.forward * 2f;
        Instantiate(healthPickupPrefab, spawnPosition, Quaternion.identity);
    }
}
