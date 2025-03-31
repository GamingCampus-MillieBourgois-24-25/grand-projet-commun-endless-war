using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehaviour : MonoBehaviour
{
    [Header("Paramètres")]
    [SerializeField] private float attackRange = 5f;
    [SerializeField] private int damage = 10;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private float fireballSpeed = 10f;

    private float attackTimer;

    void Update()
    {
        attackTimer += Time.deltaTime;

        if (!CanAttack()) return;

        GameObject nearestEnemy = FindNearestEnemy();

        if (nearestEnemy != null)
        {
            ShootFireball(nearestEnemy);
            attackTimer = 0;
        }
    }

    private bool CanAttack()
    {
        return attackTimer >= attackCooldown;
    }

    private GameObject FindNearestEnemy()
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

    private void ShootFireball(GameObject target)
    {
        GameObject fireball = Instantiate(fireballPrefab, transform.position, Quaternion.identity);
        Fireball fireballScript = fireball.GetComponent<Fireball>();

        if (fireballScript != null)
        {
            fireballScript.Initialize(target, damage, fireballSpeed);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
