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
            Debug.Log("Cr�ation de la pool : " +  prefab.name);
        }

        if (pool[prefab].Count > 0)
        {
            GameObject obj = pool[prefab].Dequeue();
            obj.transform.SetPositionAndRotation(position, rotation);
            obj.SetActive(true);
            Debug.Log("La pool contient : " + prefab.name);
            return obj;
        }
        else
        {
            Debug.Log("La pool ne contient pas : " + prefab.name);
            return Instantiate(prefab, position, rotation);
        }
    }

    public void Despawn(GameObject obj, GameObject prefab)
    {
        if (!pool.ContainsKey(prefab))
        {
            pool[prefab] = new Queue<GameObject>();
        }

        obj.SetActive(false);
        pool[prefab].Enqueue(obj);
        Debug.Log("Adding " + obj.name + " to the pool : " + prefab.name);
    }
}