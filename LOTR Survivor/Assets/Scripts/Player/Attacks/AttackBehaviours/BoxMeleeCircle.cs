using UnityEngine;

public class BoxMeleeCircle : AreaAttackBehaviour
{
    protected override Collider[] GetHitColliders()
    {
        return Physics.OverlapSphere(transform.position, attackSettings.Range, LayerMask.GetMask("Enemy"));
    }

    protected override Vector3 GetFXSpawnPosition()
    {
        return transform.position;
    }

    protected virtual void OnDrawGizmosSelected()
    {
        if (attackSettings == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackSettings.Range);
    }
}
