using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AreaAttackBehaviour : AttackBehaviour
{
    protected override void Attack()
    {
        ApplyAttackEffects();

        float adjustedDamage = skillSettings.Damage * damageMultiplier;
        int finalDamage = Mathf.RoundToInt(adjustedDamage);

        float adjustedRange = skillSettings.Range * rangeMultiplier;

        Collider[] hitEnemies = GetHitColliders(adjustedRange);

        foreach (Collider enemy in hitEnemies)
        {
            EnemyHealthBehaviour health = enemy.GetComponent<EnemyHealthBehaviour>();
            if (health != null)
            {
                health.TakeDamage(finalDamage);
                ApplyStatusEffects(enemy.gameObject);
            }
        }

        PlayHitFX(adjustedRange);
    }

    protected abstract Collider[] GetHitColliders(float adjustedRange);

    protected virtual void PlayHitFX(float adjustedRange)
    {
        if (skillSettings.prefab != null)
        {
            Vector3 spawnPosition = GetFXSpawnPosition();
            Quaternion adjustedRotation = transform.rotation * Quaternion.Euler(0, skillSettings.RotationOffset, 0);

            GameObject hitEffect = Instantiate(skillSettings.prefab, spawnPosition, adjustedRotation);
            hitEffect.transform.localScale = new Vector3(skillSettings.WideRange * rangeMultiplier, hitEffect.transform.localScale.y, adjustedRange) * skillSettings.Scale;
        }
    }

    protected abstract Vector3 GetFXSpawnPosition();
}
