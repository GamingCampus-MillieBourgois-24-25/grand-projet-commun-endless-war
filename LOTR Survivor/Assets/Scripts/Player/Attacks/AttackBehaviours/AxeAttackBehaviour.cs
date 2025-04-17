using UnityEngine;

public class AxeAttackBehaviour : AttackBehaviour
{
    protected override void Attack()
    {
        Vector3 axePosition = transform.forward * skillSettings.Range;
        GameObject axe = SpawnOrInstantiate(skillSettings.prefab, transform.position + axePosition, Quaternion.identity);

        Axe axeScript = axe.GetComponent<Axe>();
        if (axeScript != null)
        {
            axeScript.Initialize(transform);
            axeScript.SetSettings(skillSettings);
        }
    }
}
