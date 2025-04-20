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
        attackTimer = -100;
        for (int i = 0; i < skillSettings.NumberOfAttacks; i++)
        {
            GameObject nearestEnemy = ProjectileUtils.FindNearestEnemy(transform.position, skillSettings.Range, enemyLayer);

            if (nearestEnemy != null)
            {
                GameObject fireball = SpawnOrInstantiate(skillSettings.prefab, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);

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
        attackTimer = 0;
    }

    protected override bool CanAttack()
    {
        float actualCooldown = skillSettings.Cooldown;

        float cooldownMultiplier = PlayerStatsMultiplier.IsInitialized ? PlayerStatsMultiplier.cooldownMultiplier : 1f;
        actualCooldown *= cooldownMultiplier;

        actualCooldown = Mathf.Max(actualCooldown, skillSettings.MinCooldown);

         if (attackTimer < actualCooldown)
            return false;
         if (ProjectileUtils.FindNearestEnemy(transform.position, skillSettings.Range, enemyLayer) == null)
            return false;

        return true;
    }
}
