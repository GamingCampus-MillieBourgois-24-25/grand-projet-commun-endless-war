using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxMeleeAutoTarget : BoxMelee
{
    protected override bool CanAttack()
    {
        if (attackTimer < attackSettings.Cooldown)
            return false;

        return GetHitColliders().Length > 0;
    }
}

