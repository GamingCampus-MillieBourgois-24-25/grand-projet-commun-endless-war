using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AreaAttackBehaviour : AttackBehaviour
{
    protected override void Attack()
    {
        ApplyAttackEffects();

        Collider[] hitEnemies = GetHitColliders();

        foreach (Collider enemy in hitEnemies)
        {
            EnemyHealthBehaviour health = enemy.GetComponent<EnemyHealthBehaviour>();
            if (health != null)
            {
                health.TakeDamage(attackSettings.Damage);
                ApplyStatusEffects(enemy.gameObject);
            }
        }

        PlayHitFX();
    }

    protected abstract Collider[] GetHitColliders();

    protected virtual void PlayHitFX()
    {
        if (attackSettings.prefab != null)
        {
            Vector3 spawnPosition = GetFXSpawnPosition();
            Quaternion adjustedRotation = transform.rotation * Quaternion.Euler(0, attackSettings.RotationOffset, 0);

            GameObject hitEffect = Instantiate(attackSettings.prefab, spawnPosition, adjustedRotation);
            hitEffect.transform.localScale = new Vector3(attackSettings.WideRange, hitEffect.transform.localScale.y, attackSettings.Range) * attackSettings.Scale;
        }
    }

    protected abstract Vector3 GetFXSpawnPosition();
}

