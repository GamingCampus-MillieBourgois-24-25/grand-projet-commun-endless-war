using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxMeleeAutoTarget : BoxMelee
{
    protected override bool CanAttack()
    {
        if (attackTimer < skillSettings.Cooldown)
            return false;

        float adjustedRange = skillSettings.Range * PlayerStatsMultiplier.rangeMultiplier;
        return GetHitColliders(adjustedRange).Length > 0;
    }

}

