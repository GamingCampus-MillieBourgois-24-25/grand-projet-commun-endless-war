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
        if (target == null || !target.activeSelf)
        {
            Debug.LogError("Target is invalid during initialization.");
            return;
        }
        this.target = target;
        enemyLayer = LayerMask.GetMask(enemyLayerName);
    }

    void Update()
    {
        if (target == null || !target.activeSelf)
        {
            target = ProjectileUtils.FindNearestEnemy(transform.position, skillSettings.AimRange, enemyLayer);
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
        transform.Translate(direction * skillSettings.Speed * projectileSpeedMultiplier * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, target.transform.position) < 0.05f)
        {
            HitTarget();
        }
    }

    private void HitTarget()
    {
        if (target == null || !target.activeSelf)
        {
            Debug.LogWarning("Target is no longer valid.");
            DestroyAttack();
            return;
        }

        if (target.TryGetComponent<EnemyHealthBehaviour>(out EnemyHealthBehaviour health))
        {
            int finalDamage = Mathf.RoundToInt(skillSettings.Damage * damageMultiplier);
            health.TakeDamage(finalDamage, skillSettings.damageType);
            ApplyStatusEffects(target.gameObject);
        }

        DestroyAttack();
    }
}
