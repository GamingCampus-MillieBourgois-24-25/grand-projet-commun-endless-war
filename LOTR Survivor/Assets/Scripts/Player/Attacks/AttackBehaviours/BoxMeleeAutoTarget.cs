using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxMeleeAutoTarget : BoxMelee
{
    protected override bool CanAttack()
    {
        if (attackTimer < attackSettings.Cooldown)
            return false;

        Vector3 boxCenter = transform.position + transform.forward * (attackSettings.Range * 0.5f);
        Quaternion rotation = transform.rotation;
        Vector3 boxSize = new Vector3(attackSettings.WideRange, 2f, attackSettings.Range);

        Collider[] hits = Physics.OverlapBox(
            boxCenter,
            boxSize / 2f,
            rotation,
            LayerMask.GetMask("Enemy")
        );

        return hits.Length > 0;
    }
}
