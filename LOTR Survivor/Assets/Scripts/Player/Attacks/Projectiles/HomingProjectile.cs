using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingProjectile : Attack
{
    private GameObject target;
    private LayerMask enemyLayer;
    private string enemyLayerName = "Enemy";

    public void Initialize(GameObject target)
    {
        this.target = target;
        enemyLayer = LayerMask.GetMask(enemyLayerName);
    }

    void Update()
    {
        if (target == null || !target.activeSelf)
        {
            target = ProjectileUtils.FindNearestEnemy(transform.position, attackSettings.AimRange, enemyLayer);
            if (target == null)
            {
                DestroyAttack();
                return;
            }
        }

        UpdateAttack();
    }

    protected override void UpdateAttack()
    {
        Vector3 direction = (target.transform.position - transform.position).normalized;
        transform.Translate(direction * attackSettings.Speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, target.transform.position) < 0.2f)
        {
            HitTarget();
        }
    }

    private void HitTarget()
    {
        if (target.TryGetComponent<EnemyHealthBehaviour>(out EnemyHealthBehaviour health))
        {
            health.TakeDamage(attackSettings.Damage);
        }

        DestroyAttack();
    }
}
