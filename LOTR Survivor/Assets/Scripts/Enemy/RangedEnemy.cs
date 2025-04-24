using UnityEngine;

public class RangedEnemy : EnemyBase
{
    [Header("Projectile")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float projectileSpeed;

    protected override bool CanAttack()
    {
        return isInRange && attackTimer >= enemyData.attackCooldown && !isAttacking;
    }

    protected override void StartAttack()
    {
        isAttacking = true;
        attackTimer = 0f;

        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }
        else
        {
            ShootProjectile();
            EndAttack();
        }
    }

    public void ShootProjectile()
    {
        if (projectilePrefab != null && firePoint != null && player != null)
        {
            GameObject proj = ObjectPool.Instance.Spawn(projectilePrefab, firePoint.position, Quaternion.identity);
            EnemyProjectile enemyProj = proj.GetComponent<EnemyProjectile>();
            if (enemyProj != null)
            {
                Vector3 dir = (player.position - firePoint.position).normalized;
                enemyProj.Init(dir, projectileSpeed, enemyData.attackPower);
            }
        }
    }

    public void EndAttack()
    {
        isAttacking = false;
    }
}
