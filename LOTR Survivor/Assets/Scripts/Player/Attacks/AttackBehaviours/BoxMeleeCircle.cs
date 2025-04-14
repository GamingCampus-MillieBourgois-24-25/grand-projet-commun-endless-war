using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxMeleeCircle : AttackBehaviour
{
    protected override void Attack()
    {
        ApplyAttackEffects();

        Vector3 attackPosition = transform.position;
        float radius = attackSettings.Range;

        Collider[] hitEnemies = Physics.OverlapSphere(attackPosition, radius, LayerMask.GetMask("Enemy"));

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

    protected virtual void PlayHitFX()
    {
        if (attackSettings.hitPrefab != null)
        {
            Vector3 spawnPosition = transform.position;
            Quaternion adjustedRotation = transform.rotation * Quaternion.Euler(0, attackSettings.RotationOffset, 0);

            GameObject hitEffect = Instantiate(attackSettings.hitPrefab, spawnPosition, adjustedRotation);
            hitEffect.transform.localScale = new Vector3(attackSettings.WideRange, hitEffect.transform.localScale.y, attackSettings.Range) * attackSettings.Scale;

            //if (attackSettings.hitClip != null)
            //{
            //    OneShotAudio.PlayClip(attackSettings.hitClip, transform.position, VolumeManager.Instance.GetSFXVolume());
            //}
        }
    }

    protected virtual void OnDrawGizmosSelected()
    {
        if (attackSettings == null) return;

        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, attackSettings.Range);
    }
}
