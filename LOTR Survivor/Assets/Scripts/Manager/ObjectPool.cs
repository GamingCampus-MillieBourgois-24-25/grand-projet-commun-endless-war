using UnityEngine;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;

    private Dictionary<GameObject, Queue<GameObject>> pool = new();

    void Awake()
    {
        Instance = this;
    }

    public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (!pool.ContainsKey(prefab))
        {
            pool[prefab] = new Queue<GameObject>();
        }

        GameObject obj;
        if (pool[prefab].Count > 0)
        {
            obj = pool[prefab].Dequeue();
            obj.transform.SetPositionAndRotation(position, rotation);
            obj.SetActive(true);
        }
        else
        {
            obj = Instantiate(prefab, position, rotation);
        }

        /*var health = obj.GetComponent<EnemyHealthBehaviour>();
        if (health != null)
            health.OnEnable();*/

        return obj;
    }

    public void Despawn(GameObject obj, GameObject prefab)
    {
        if (prefab == null)
        {
            Debug.Log("Prefab is null in Despawn method");
            return;
        }

        if (!pool.ContainsKey(prefab))
        {
            pool[prefab] = new Queue<GameObject>();
        }

        obj.SetActive(false);
        pool[prefab].Enqueue(obj);
    }
}