using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Attack
{
    private Vector3 direction;
    private Vector3 startPosition;

    public void Initialize(Vector3 newDirection)
    {
        direction = newDirection.normalized;
        startPosition = transform.position;
    }

    void Update()
    {
        UpdateAttack();
    }

    protected override void UpdateAttack()
    {
        transform.Translate(direction * attackSettings.Speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(startPosition, transform.position) >= attackSettings.Range)
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
            DestroyAttack();
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
            enemy.TakeDamage(attackSettings.Damage);
        }
    }
}
