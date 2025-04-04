using UnityEngine;

public class FireballAttackBehaviour : AttackBehaviour
{
    [SerializeField] private ProjectileSettings projectileSettings;
    [SerializeField] private float fireballSpeed = 10f;

    protected override void Attack()
    {
        GameObject nearestEnemy = FindNearestEnemy();

        if (nearestEnemy != null)
        {
            GameObject fireball = SpawnOrInstantiate(projectileSettings.prefab, transform.position, Quaternion.identity);

            HomingProjectile fireballScript = fireball.GetComponent<HomingProjectile>();
            if (fireballScript != null)
            {
                fireballScript.Initialize(nearestEnemy, damage, fireballSpeed, projectileSettings.prefab);
                if (projectileSettings != null)
                {
                    fireballScript.SetSettings(projectileSettings);
                }
            }
        }
    }

    public void SetProjectileSettings(ProjectileSettings newProjectileSettings)
    {
        projectileSettings = newProjectileSettings;
    }
}
