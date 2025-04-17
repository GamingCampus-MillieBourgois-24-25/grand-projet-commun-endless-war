using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeAttackBehaviour : AttackBehaviour
{
    [SerializeField] private int munitionNumber = 10;
    [SerializeField] private float munitionRadius = 90f;

    protected override void Attack()
    {
        if (munitionNumber < 1) return;

        Vector3 forward = transform.forward;
        float angleStep = CalculateAngleStep();

        for (int i = 0; i < munitionNumber; i++)
        {
            float angle = CalculateAngle(i, angleStep);
            GameObject munition = CreateMunition(angle, forward);
            InitializeProjectile(munition, angle, forward);
        }
    }

    private float CalculateAngleStep()
    {
        return munitionNumber > 1 ? munitionRadius / (munitionNumber - 1) : 0f;
    }

    private float CalculateAngle(int index, float angleStep)
    {
        return -munitionRadius / 2 + index * angleStep;
    }

    private GameObject CreateMunition(float angle, Vector3 forward)
    {
        Quaternion rotation = Quaternion.Euler(0f, angle, 0f);
        return SpawnOrInstantiate(skillSettings.prefab, transform.position, rotation);
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
