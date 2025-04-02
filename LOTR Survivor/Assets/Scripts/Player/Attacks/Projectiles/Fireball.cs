using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    private GameObject target;
    private int damage;
    private float speed;
    private GameObject fireballPrefab;

    public void Initialize(GameObject target, int damage, float speed, GameObject prefab)
    {
        this.target = target;
        this.damage = damage;
        this.speed = speed;
        this.fireballPrefab = prefab;
    }

    void Update()
    {
        if (target == null || !target.activeSelf)
        {
            DestroyAttack();
            return;
        }

        Vector3 direction = (target.transform.position - transform.position).normalized;
        transform.Translate(direction * speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, target.transform.position) < 0.2f)
        {
            HitTarget();
        }
    }

    private void HitTarget()
    {
        if (target.TryGetComponent<EnemyHealthBehaviour>(out EnemyHealthBehaviour health))
        {
            health.TakeDamage(damage);
        }

        DestroyAttack();
    }

    private void DestroyAttack()
    {
        if (ObjectPool.Instance != null)
        {
            ObjectPool.Instance.Despawn(gameObject, fireballPrefab);
        }
        else
        {
            Debug.Log("ObjectPool Instance is not present in the scene!");
            Destroy(gameObject);
        }
    }
}
