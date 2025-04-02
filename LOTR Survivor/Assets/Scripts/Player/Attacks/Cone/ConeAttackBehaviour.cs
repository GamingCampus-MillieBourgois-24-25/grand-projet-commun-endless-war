using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeAttackBehaviour : AttackBehaviour
{
    [SerializeField] private GameObject munitionPrefab;
    [SerializeField] private float munitionSpeed = 10f;
    [SerializeField] private int munitionNumber = 10;
    [SerializeField] private float munitionRadius = 90f;
    [SerializeField] private float range = 10f;

    protected override void Attack()
    {
        Vector3 forward = transform.forward;

        float angleStep = munitionRadius / (munitionNumber - 1);

        for (int i = 0; i < munitionNumber; i++)
        {
            float angle = -munitionRadius / 2 + i * angleStep;

            Quaternion rotation = Quaternion.Euler(0f, angle, 0f);

            GameObject munition = SpawnOrInstantiate(munitionPrefab, transform.position, rotation);

            Projectile projectile = munition.GetComponent<Projectile>();
            if (projectile != null)
            {
                projectile.Initialize(rotation * forward, range, damage, munitionSpeed, munitionPrefab);
            }
        }
    }
}
