using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackBehaviour : MonoBehaviour
{
    [Header("Paramètres")]
    [SerializeField] protected float attackRange = 10f;
    [SerializeField] protected int damage = 10;
    [SerializeField] protected float attackCooldown = 1f;
    [SerializeField] protected LayerMask enemyLayer;

    protected float attackTimer;

    protected virtual void Update()
    {
        attackTimer += Time.deltaTime;

        if (CanAttack())
        {
            Attack();
            attackTimer = 0f;
        }
    }

    protected bool CanAttack()
    {
        return attackTimer >= attackCooldown;
    }

    protected GameObject FindNearestEnemy()
    {
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, attackRange, enemyLayer);

        if (enemiesInRange.Length == 0) return null;

        Collider nearestEnemy = enemiesInRange[0];
        float shortestDistance = Vector3.Distance(transform.position, nearestEnemy.transform.position);

        foreach (Collider enemy in enemiesInRange)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < shortestDistance)
            {
                nearestEnemy = enemy;
                shortestDistance = distance;
            }
        }

        return nearestEnemy.gameObject;
    }

    protected GameObject SpawnOrInstantiate(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (ObjectPool.Instance != null)
        {
            return ObjectPool.Instance.Spawn(prefab, position, rotation);
        }
        else
        {
            return Instantiate(prefab, position, rotation);
        }
    }

    protected abstract void Attack();
}
