using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HealthMaxPickupSpawner))]
public class HealthMaxPickupSpawnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        HealthMaxPickupSpawner spawner = (HealthMaxPickupSpawner)target;

        if (GUILayout.Button("Spawn Health Pickup"))
        {
            spawner.SpawnHealthPickup();
        }
    }
}
