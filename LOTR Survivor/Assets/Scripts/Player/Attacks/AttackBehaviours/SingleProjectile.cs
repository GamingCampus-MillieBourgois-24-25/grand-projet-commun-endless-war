using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleProjectileAttackBehaviour : AttackBehaviour
{
    [SerializeField] private float projectileAngle = 0f;

    protected override void Attack()
    {
        Vector3 forward = transform.forward;
        GameObject munition = CreateMunition(projectileAngle, forward);
        InitializeProjectile(munition, projectileAngle, forward);
    }

    private GameObject CreateMunition(float angle, Vector3 forward)
    {
        Quaternion rotation = Quaternion.Euler(0f, angle, 0f);
        return SpawnOrInstantiate(skillSettings.prefab, transform.position + new Vector3(0, 0.5f, 0), rotation);
    }

    private void InitializeProjectile(GameObject munition, float angle, Vector3 forward)
    {
        if (munition == null) return;

        Projectile projectile = munition.GetComponent<Projectile>();
        if (projectile != null)
        {
            Vector3 direction = Quaternion.Euler(0f, angle, 0f) * forward;
            projectile.Initialize(direction);
            projectile.SetSettings(skillSettings);
        }
    }
}
