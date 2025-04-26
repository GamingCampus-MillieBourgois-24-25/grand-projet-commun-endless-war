using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class HealthMaxSpawnerPoint : MonoBehaviour 
{
    [HideInInspector] public bool hasSpawned = false;

    public void Spawn(GameObject prefab)
    {
        if (prefab == null || hasSpawned) return;

        Instantiate(prefab, transform.position, Quaternion.identity);
        hasSpawned = true;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!hasSpawned)
            Gizmos.color = new Color(1f, 0.5f, 0f);
        else
            Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(transform.position, 0.5f);

        string label = hasSpawned ? "PV Max (spawned)" : "PV Max Spawner";
        UnityEditor.Handles.Label(transform.position + Vector3.up * 0.6f, label);
    }
#endif
}
