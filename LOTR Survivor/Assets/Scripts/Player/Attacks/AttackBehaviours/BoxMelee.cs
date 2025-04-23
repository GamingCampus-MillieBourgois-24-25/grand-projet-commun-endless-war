using UnityEngine;

public class BoxMelee : AreaAttackBehaviour
{
    protected override Collider[] GetHitColliders(float adjustedRange)
    {
        Vector3 center = transform.position + transform.forward * (adjustedRange * 0.5f) + new Vector3(0,0.5f,0);
        Vector3 size = new Vector3(skillSettings.WideRange, 2f, adjustedRange);
        size.x = Mathf.Abs(size.x);
        size.y = Mathf.Abs(size.y);
        size.z = Mathf.Abs(size.z);

        Collider[] results = Physics.OverlapBox(center, size / 2f, transform.rotation, LayerMask.GetMask("Enemy"));

        return results.Length > 0 ? results : null;
    }

    protected override Vector3 GetFXSpawnPosition()
    {
        return transform.position + transform.forward * (skillSettings.Range * rangeMultiplier * 0.2f) + new Vector3(0, 0.5f, 0);
    }

    protected override void PlayHitFX(float adjustedRange)
    {
        if (skillSettings.prefab != null)
        {

            Vector3 spawnPosition = GetFXSpawnPosition();
            Quaternion adjustedRotation = transform.rotation * Quaternion.Euler(0, skillSettings.RotationOffset, 0);

            GameObject hitEffect = Instantiate(skillSettings.prefab, spawnPosition, adjustedRotation);
            hitEffect.transform.localScale = new Vector3(skillSettings.WideRange * rangeMultiplier, hitEffect.transform.localScale.y, adjustedRange) * skillSettings.Scale;
        }
    }

    protected virtual void OnDrawGizmosSelected()
    {
        if (skillSettings == null) return;

        float adjustedRange = skillSettings.Range * rangeMultiplier;

        Gizmos.color = Color.red;
        Vector3 center = transform.position + transform.forward * (adjustedRange * 0.5f) + new Vector3(0, 0.5f, 0);
        Vector3 size = new Vector3(skillSettings.WideRange, 2f, adjustedRange);

        Gizmos.matrix = Matrix4x4.TRS(center, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, size);
    }
}
