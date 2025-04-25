using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    private Vector3 direction;
    private Vector3 startPosition;

    private float speed;
    private float range;
    private int damage;

    private bool hasHit = false;

    public void Init(Vector3 newDirection, float projectileSpeed, int attackDamage, float maxRange = 20f)
    {
        direction = newDirection.normalized;
        speed = projectileSpeed;
        damage = attackDamage;
        range = maxRange;
        startPosition = transform.position;
        hasHit = false;
    }

    private void FixedUpdate()
    {
        if (hasHit) return;

        transform.Translate(direction * speed * Time.fixedDeltaTime, Space.World);

        if (Vector3.Distance(startPosition, transform.position) >= range)
        {
            DespawnSafely();
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (hasHit) return;

        if (collider.CompareTag("Player"))
        {
            if (collider.TryGetComponent<PlayerHealthBehaviour>(out PlayerHealthBehaviour health))
            {
                health.TakeDamage(damage);
            }
            hasHit = true;
        }
        else if (collider.CompareTag("Walls"))
        {
            hasHit = true;
        }

        if (hasHit)
        {
            DespawnSafely();
        }
    }

    private void DespawnSafely()
    {
        if (ObjectPool.Instance != null)
        {
            ObjectPool.Instance.Despawn(gameObject);
        }
        else
        {
            Debug.LogWarning("ObjectPool.Instance is null. Destroying projectile manually.");
            Destroy(gameObject);
        }
    }
}
