using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballAttackBehaviour : AttackBehaviour
{
    [SerializeField] protected GameObject fireballPrefab;
    [SerializeField] protected float fireballSpeed = 10f;

    protected override void Update()
    {
        base.Update();

        if (!CanAttack())
        {
            return;
        }

        GameObject nearestEnemy = FindNearestEnemy();

        if (nearestEnemy != null)
        {
            ShootFireball(nearestEnemy);
            attackTimer = 0;
        }
    }

    protected void ShootFireball(GameObject target)
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
