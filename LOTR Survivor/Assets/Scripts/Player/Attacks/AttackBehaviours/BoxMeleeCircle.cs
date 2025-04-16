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
        if (attackSettings.prefab != null)
        {
            Vector3 spawnPosition = GetFXSpawnPosition();
            spawnPosition.y = 0;
            Quaternion adjustedRotation = transform.rotation * Quaternion.Euler(0, attackSettings.RotationOffset, 0);

            GameObject hitEffect = Instantiate(attackSettings.prefab, spawnPosition, adjustedRotation);

            hitEffect.transform.localScale = Vector3.one * adjustedRange * attackSettings.Scale;
        }
    }


    protected virtual void OnDrawGizmosSelected()
    {
        if (attackSettings == null) return;

        Gizmos.color = Color.red;
        float adjustedRange = attackSettings.Range * PlayerStatsMultiplier.rangeMultiplier;
        Gizmos.DrawWireSphere(transform.position, adjustedRange);
    }

}
