using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeAttackBehaviour : AttackBehaviour
{
    [SerializeField] protected GameObject axePrefab;
    [SerializeField] private float axeSpeed = 230f;
    [SerializeField] private float range = 8f;
    [SerializeField] private float maxRotation = 360f;

    protected override void Update()
    {
        base.Update();

        if (!CanAttack())
        {
            return;
        }

        ShootAxe();
    }

    protected void ShootAxe()
    {
        Vector3 axePosition = new Vector3(0f, 0f, range);
        GameObject axe = Instantiate(axePrefab, transform.position + axePosition, Quaternion.identity);

        Axe axeScript = axe.GetComponent<Axe>();
        if (axeScript != null)
        {
            axeScript.Initialize(damage, axeSpeed, maxRotation,this.transform);
        }
        attackTimer = 0;
    }
}
