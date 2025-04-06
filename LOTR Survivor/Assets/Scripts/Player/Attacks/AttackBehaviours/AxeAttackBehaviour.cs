using UnityEngine;

public class AxeAttackBehaviour : AttackBehaviour
{
    protected override void Attack()
    {
        Vector3 axePosition = transform.forward * attackSettings.Range;
        GameObject axe = SpawnOrInstantiate(attackSettings.prefab, transform.position + axePosition, Quaternion.identity);

        Axe axeScript = axe.GetComponent<Axe>();
        if (axeScript != null)
        {
            axeScript.Initialize(transform);
            axeScript.SetSettings(attackSettings);
        }
    }
}
