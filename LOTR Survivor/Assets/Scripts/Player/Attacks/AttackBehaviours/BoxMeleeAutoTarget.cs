using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxMeleeAutoTarget : BoxMelee
{
    protected override bool CanAttack()
    {
        if (attackTimer < attackSettings.Cooldown)
            return false;

        float adjustedRange = attackSettings.Range * PlayerStatsMultiplier.rangeMultiplier;
        return GetHitColliders(adjustedRange).Length > 0;
    }

}

