using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance { get; private set; }

    private Dictionary<GameObject, Queue<GameObject>> pool = new Dictionary<GameObject, Queue<GameObject>>();
    private Dictionary<GameObject, GameObject> instanceToPrefab = new Dictionary<GameObject, GameObject>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        GameObject instance;

        if (pool.ContainsKey(prefab) && pool[prefab].Count > 0)
        {
            instance = pool[prefab].Dequeue();
            instance.transform.position = position;
            instance.transform.rotation = rotation;
            instance.SetActive(true);
        }
        else
        {
            instance = Instantiate(prefab, position, rotation);
        }

        instanceToPrefab[instance] = prefab;
        return instance;
    }

    public void Despawn(GameObject instance)
    {
        if (instanceToPrefab.TryGetValue(instance, out GameObject prefab))
        {
            Despawn(instance, prefab);
        }
        else
        {
            Debug.LogWarning($"Trying to despawn instance {instance.name}, but no prefab reference was found.");
            Destroy(instance); // fallback
        }
    }

    public void Despawn(GameObject instance, GameObject prefab)
    {
        instance.SetActive(false);

        if (!pool.ContainsKey(prefab))
        {
            pool[prefab] = new Queue<GameObject>();
        }

        pool[prefab].Enqueue(instance);
        instanceToPrefab.Remove(instance);
    }
}
