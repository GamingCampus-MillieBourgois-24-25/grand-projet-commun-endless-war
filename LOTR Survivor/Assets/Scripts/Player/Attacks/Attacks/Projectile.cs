using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Attack
{
    private Vector3 direction;
    private Vector3 startPosition;

    [SerializeField] bool destroyOnHit = true;

    public void Initialize(Vector3 newDirection)
    {
        direction = newDirection.normalized;
        startPosition = transform.position;

        transform.rotation = Quaternion.LookRotation(direction);
    }

    void Update()
    {
        UpdateAttack();
    }

    protected override void UpdateAttack()
    {
        transform.Translate(direction * skillSettings.Speed * projectileSpeedMultiplier * Time.deltaTime, Space.World);

        if (Vector3.Distance(startPosition, transform.position) >= skillSettings.Range * rangeMultiplier)
        {
            DestroyAttack();
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        HitTarget(collider);
    }

    private void HitTarget(Collider collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            DealDamage(collider);
            if (destroyOnHit)
            {
                DestroyAttack();
            }
        }

        if (collider.CompareTag("Walls"))
        {
            DestroyAttack();
        }
    }

    private void DealDamage(Collider collider)
    {
        EnemyHealthBehaviour enemy = collider.GetComponent<EnemyHealthBehaviour>();
        if (enemy != null)
        {
            int finalDamage = Mathf.RoundToInt(skillSettings.Damage * damageMultiplier);
            enemy.TakeDamage(finalDamage, skillSettings.damageType);
        }
    }
}
