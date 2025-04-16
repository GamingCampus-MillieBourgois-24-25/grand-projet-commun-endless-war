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

    protected override void PlayHitFX()
    {
        if (attackSettings.prefab != null)
        {
            Vector3 spawnPosition = GetFXSpawnPosition();
            spawnPosition.y = 0;
            Quaternion adjustedRotation = transform.rotation * Quaternion.Euler(0, attackSettings.RotationOffset, 0);

            GameObject hitEffect = Instantiate(attackSettings.prefab, spawnPosition, adjustedRotation);

            hitEffect.transform.localScale = Vector3.one * attackSettings.Range * attackSettings.Scale;
        }
    }

    protected virtual void OnDrawGizmosSelected()
    {
        if (attackSettings == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackSettings.Range);
    }
}
