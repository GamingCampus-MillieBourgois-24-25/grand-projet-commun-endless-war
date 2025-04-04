using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingProjectile : Attack
{
    private GameObject target;
    private ProjectileSettings settings;
    private AudioSource audioSource;

    public void Initialize(GameObject target, int damage, float speed, GameObject prefab)
    {
        base.Initialize(damage, speed, prefab);
        this.target = target;
        audioSource = GetComponent<AudioSource>();
    }

    public void SetSettings(ProjectileSettings projectileSettings)
    {
        settings = projectileSettings;
        damage = projectileSettings.Damage;
        speed = projectileSettings.Speed;
    }

    void Update()
    {
        if (target == null || !target.activeSelf)
        {
            DestroyAttack();
            return;
        }

        UpdateAttack();
    }

    protected override void UpdateAttack()
    {
        Vector3 direction = (target.transform.position - transform.position).normalized;
        transform.Translate(direction * speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, target.transform.position) < 0.2f)
        {
            HitTarget();
        }
    }

    private void HitTarget()
    {
        if (target.TryGetComponent<EnemyHealthBehaviour>(out EnemyHealthBehaviour health))
        {
            health.TakeDamage(damage);
        }

        DestroyAttack();
    }

    protected override void PlayFX()
    {
        base.PlayFX();

        if (settings.explosionPrefab != null)
        {
            GameObject fx = Instantiate(settings.explosionPrefab, transform.position, Quaternion.identity);
            ParticleSystem system = fx.GetComponent<ParticleSystem>();
            var main = system.main;
            main.stopAction = ParticleSystemStopAction.Destroy;
            main.loop = false;
        }

        if (settings.explosionSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(settings.explosionSound);
        }
    }
}
