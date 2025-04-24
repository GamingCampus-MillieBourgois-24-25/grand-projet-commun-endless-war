using UnityEngine;

public class MeleeEnemy : EnemyBase
{
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
            TryApplyDamage();
            EndAttack();
        }
    }

    public void TryApplyDamage()
    {
        float dist = Vector3.Distance(transform.position, player.position);
        if (dist <= enemyData.aggroRange && player.TryGetComponent<PlayerHealthBehaviour>(out PlayerHealthBehaviour health))
        {
            health.TakeDamage(enemyData.attackPower);
        }
    }

    public void EndAttack()
    {
        isAttacking = false;
    }
}
