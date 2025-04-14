using UnityEngine;

public class FireballAttackBehaviour : AttackBehaviour
{
    protected override void Attack()
    {
        if (attackSettings == null || attackSettings.NumberOfAttacks <= 0)
            return;

        StartCoroutine(FireProjectilesWithDelay());
    }

    private System.Collections.IEnumerator FireProjectilesWithDelay()
    {
        for (int i = 0; i < attackSettings.NumberOfAttacks; i++)
        {
            GameObject nearestEnemy = ProjectileUtils.FindNearestEnemy(transform.position, attackSettings.Range, enemyLayer);

            if (nearestEnemy != null)
            {
                GameObject fireball = SpawnOrInstantiate(attackSettings.prefab, transform.position, Quaternion.identity);

                HomingProjectile fireballScript = fireball.GetComponent<HomingProjectile>();
                if (fireballScript != null)
                {
                    fireballScript.Initialize(nearestEnemy);
                    fireballScript.SetSettings(attackSettings);
                }
            }

            if (attackSettings.CooldownBetweenAttacks > 0 && i < attackSettings.NumberOfAttacks - 1)
            {
                yield return new WaitForSeconds(attackSettings.CooldownBetweenAttacks);
            }
        }
    }
}
