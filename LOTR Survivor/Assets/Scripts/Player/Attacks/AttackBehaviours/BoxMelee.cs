using UnityEngine;

public class BoxMelee : AreaAttackBehaviour
{
    protected override Collider[] GetHitColliders()
    {
        float bonusRange = PlayerStatsMultiplier.rangeMultiplier;
        Vector3 center = transform.position + transform.forward * (attackSettings.Range * bonusRange * 0.5f);
        Vector3 size = new Vector3(attackSettings.WideRange, 2f, attackSettings.Range * bonusRange);

        return Physics.OverlapBox(center, size / 2f, transform.rotation, LayerMask.GetMask("Enemy"));
    }

    protected override Vector3 GetFXSpawnPosition()
    {
        return transform.position + transform.forward * (attackSettings.Range * PlayerStatsMultiplier.rangeMultiplier * 0.5f);
    }

    protected virtual void OnDrawGizmosSelected()
    {
        if (attackSettings == null) return;

        Gizmos.color = Color.red;
        Vector3 center = transform.position + transform.forward * (attackSettings.Range * 0.5f);
        Vector3 size = new Vector3(attackSettings.WideRange, 2f, attackSettings.Range);

        Gizmos.matrix = Matrix4x4.TRS(center, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, size);
    }
}
