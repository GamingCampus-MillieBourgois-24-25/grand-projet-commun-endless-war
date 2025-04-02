using UnityEngine;

public class AxeAttackBehaviour : AttackBehaviour
{
    [SerializeField] private GameObject axePrefab;
    [SerializeField] private float axeSpeed = 230f;
    [SerializeField] private float range = 8f;
    [SerializeField] private float maxRotation = 360f;

    protected override void Attack()
    {
        Vector3 axePosition = new Vector3(0f, 0f, range);
        GameObject axe = SpawnOrInstantiate(axePrefab, transform.position + axePosition, Quaternion.identity);

        Axe axeScript = axe.GetComponent<Axe>();
        if (axeScript != null)
        {
            axeScript.Initialize(damage, axeSpeed, maxRotation, this.transform, axePrefab);
        }
    }
}
