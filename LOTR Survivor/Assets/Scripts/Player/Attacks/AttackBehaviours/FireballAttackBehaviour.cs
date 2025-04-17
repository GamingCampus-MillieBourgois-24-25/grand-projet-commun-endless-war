using UnityEngine;

public class FireballAttackBehaviour : AttackBehaviour
{
    protected override void Attack()
    {
        if (skillSettings == null || skillSettings.NumberOfAttacks <= 0)
            return;

        StartCoroutine(FireProjectilesWithDelay());
    }

    private System.Collections.IEnumerator FireProjectilesWithDelay()
    {
        for (int i = 0; i < skillSettings.NumberOfAttacks; i++)
        {
            GameObject nearestEnemy = ProjectileUtils.FindNearestEnemy(transform.position, skillSettings.Range, enemyLayer);

            if (nearestEnemy != null)
            {
                GameObject fireball = SpawnOrInstantiate(skillSettings.prefab, transform.position, Quaternion.identity);

                HomingProjectile fireballScript = fireball.GetComponent<HomingProjectile>();
                if (fireballScript != null)
                {
                    fireballScript.Initialize(nearestEnemy);
                    fireballScript.SetSettings(skillSettings);
                }
            }

            if (skillSettings.CooldownBetweenAttacks > 0 && i < skillSettings.NumberOfAttacks - 1)
            {
                yield return new WaitForSeconds(skillSettings.CooldownBetweenAttacks);
            }
        }
    }
}
