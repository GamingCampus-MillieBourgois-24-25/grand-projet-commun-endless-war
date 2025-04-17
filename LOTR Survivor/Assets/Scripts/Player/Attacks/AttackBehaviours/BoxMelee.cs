using UnityEngine;

public class BoxMelee : AreaAttackBehaviour
{
    protected override Collider[] GetHitColliders(float adjustedRange)
    {
        Vector3 center = transform.position + transform.forward * (adjustedRange * 0.5f);
        Vector3 size = new Vector3(skillSettings.WideRange, 2f, adjustedRange);

        return Physics.OverlapBox(center, size / 2f, transform.rotation, LayerMask.GetMask("Enemy"));
    }

    protected override Vector3 GetFXSpawnPosition()
    {
        return transform.position + transform.forward * (skillSettings.Range * PlayerStatsMultiplier.rangeMultiplier * 0.5f);
    }

    protected override void PlayHitFX(float adjustedRange)
    {
        if (skillSettings.prefab != null)
        {
            Vector3 spawnPosition = GetFXSpawnPosition();
            Quaternion adjustedRotation = transform.rotation * Quaternion.Euler(0, skillSettings.RotationOffset, 0);
                
            GameObject hitEffect = Instantiate(skillSettings.prefab, spawnPosition, adjustedRotation);
            hitEffect.transform.localScale = new Vector3(skillSettings.WideRange * PlayerStatsMultiplier.rangeMultiplier, hitEffect.transform.localScale.y, adjustedRange) * skillSettings.Scale;
        }
    }

    protected virtual void OnDrawGizmosSelected()
    {
        if (skillSettings == null) return;

        Gizmos.color = Color.red;
        float adjustedRange = skillSettings.Range * PlayerStatsMultiplier.rangeMultiplier;
        Vector3 center = transform.position + transform.forward * (adjustedRange * 0.5f);
        Vector3 size = new Vector3(skillSettings.WideRange, 2f, adjustedRange);

        Gizmos.matrix = Matrix4x4.TRS(center, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, size);
    }

}
