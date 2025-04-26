using UnityEngine;

public class BoxMeleeCircle : AreaAttackBehaviour
{
    protected override Collider[] GetHitColliders(float adjustedRange)
    {
        return Physics.OverlapSphere(transform.position, adjustedRange, LayerMask.GetMask("Enemy"));
    }

    protected override Vector3 GetFXSpawnPosition()
    {
        return transform.position;
    }

    protected override void PlayHitFX(float adjustedRange)
    {
        VolumeManager.Instance.PlaySFX(skillSettings.spawnEvent, 0.5f);

        if (skillSettings.prefab != null)
        {
            Vector3 spawnPosition = GetFXSpawnPosition();
            Quaternion adjustedRotation = transform.rotation * Quaternion.Euler(0, skillSettings.RotationOffset, 0);

            GameObject hitEffect = Instantiate(skillSettings.prefab, spawnPosition, adjustedRotation);
            hitEffect.transform.localScale = Vector3.one * adjustedRange * skillSettings.Scale;
        }
    }

    protected virtual void OnDrawGizmosSelected()
    {
        if (skillSettings == null) return;

        Gizmos.color = Color.red;
        float adjustedRange = skillSettings.Range * rangeMultiplier;
        Gizmos.DrawWireSphere(transform.position, adjustedRange);
    }
}
