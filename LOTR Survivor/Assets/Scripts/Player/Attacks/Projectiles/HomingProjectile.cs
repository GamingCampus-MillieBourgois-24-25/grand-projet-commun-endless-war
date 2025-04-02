using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingProjectile : Attack
{
    private GameObject target;

    public void Initialize(GameObject target, int damage, float speed, GameObject prefab)
    {
        base.Initialize(damage, speed, prefab);
        this.target = target;
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
}
