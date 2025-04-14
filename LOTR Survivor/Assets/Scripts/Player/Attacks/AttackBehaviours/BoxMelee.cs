using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxMelee : AttackBehaviour
{
    protected override void Attack()
    {
        ApplyAttackEffects();

        Vector3 boxCenter = transform.position + transform.forward * (attackSettings.Range * 0.5f);
        Quaternion rotation = transform.rotation;
        Vector3 boxSize = new Vector3(attackSettings.WideRange, 2f, attackSettings.Range);

        Collider[] hitEnemies = Physics.OverlapBox(
            boxCenter,
            boxSize / 2f,
            rotation,
            LayerMask.GetMask("Enemy")
        );

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
            Vector3 spawnPosition = transform.position + transform.forward * attackSettings.Range * 0.5f;
            Quaternion adjustedRotation = transform.rotation * Quaternion.Euler(0, attackSettings.RotationOffset, 0);

            GameObject hitEffect = Instantiate(attackSettings.hitPrefab, spawnPosition, adjustedRotation);
            hitEffect.transform.localScale = new Vector3(attackSettings.WideRange, hitEffect.transform.localScale.y, attackSettings.Range) * attackSettings.Scale;

            if (attackSettings.hitClip != null)
            {
                OneShotAudio.PlayClip(attackSettings.hitClip, transform.position, VolumeManager.Instance.GetSFXVolume());
            }
        }
    }

    protected virtual void OnDrawGizmosSelected()
    {
        if (attackSettings == null) return;

        Gizmos.color = Color.red;
        Vector3 boxCenter = transform.position + transform.forward * (attackSettings.Range * 0.5f);
        Vector3 boxSize = new Vector3(attackSettings.WideRange, 2f, attackSettings.Range);
        Gizmos.matrix = Matrix4x4.TRS(boxCenter, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, boxSize);
    }
}
