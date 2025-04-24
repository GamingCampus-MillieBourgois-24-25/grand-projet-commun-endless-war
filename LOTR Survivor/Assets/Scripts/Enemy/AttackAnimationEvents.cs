using UnityEngine;

public class AttackAnimationEvents : MonoBehaviour
{
    public RangedEnemy rangedEnemy;
    public MeleeEnemy meleeEnemy;

    public void PerformAttack()
    {
        if (rangedEnemy != null)
        {
            rangedEnemy.ShootProjectile();
        }
        else if (meleeEnemy != null)
        {
            meleeEnemy.TryApplyDamage();
        }
    }

    public void EndAttack()
    {
        if (rangedEnemy != null)
        {
            rangedEnemy.EndAttack();
        }
        else if (meleeEnemy != null)
        {
            meleeEnemy.EndAttack();
        }
    }
}
