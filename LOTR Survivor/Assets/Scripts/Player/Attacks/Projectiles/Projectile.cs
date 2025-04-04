using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Attack
{
    private Vector3 direction;
    private float range;
    private Vector3 startPosition;
    private ProjectileSettings settings;
    private AudioSource audioSource;

    public void Initialize(Vector3 direction, float range, int damage, float speed, GameObject prefab)
    {
        base.Initialize(damage, speed, prefab);
        this.direction = direction.normalized;
        this.range = range;
        startPosition = transform.position;
        audioSource = GetComponent<AudioSource>();
    }
    public void SetSettings(ProjectileSettings projectileSettings)
    {
        settings = projectileSettings;
    }

    void Update()
    {
        UpdateAttack();
    }

    protected override void UpdateAttack()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(startPosition, transform.position) >= range)
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
            enemy.TakeDamage(damage);
        }
    }
}
