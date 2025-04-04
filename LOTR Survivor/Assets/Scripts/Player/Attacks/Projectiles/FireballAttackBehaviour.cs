using UnityEngine;

public class FireballAttackBehaviour : AttackBehaviour
{
    protected override void Attack()
    {
        GameObject nearestEnemy = ProjectileUtils.FindNearestEnemy(transform.position, attackSettings.Range, enemyLayer);

        if (nearestEnemy != null)
        {
            GameObject fireball = SpawnOrInstantiate(attackSettings.prefab, transform.position, Quaternion.identity);

            HomingProjectile fireballScript = fireball.GetComponent<HomingProjectile>();
            if (fireballScript != null)
            {
                fireballScript.Initialize(nearestEnemy);
                if (attackSettings != null)
                {
                    fireballScript.SetSettings(attackSettings);
                }
            }
        }
    }
}
