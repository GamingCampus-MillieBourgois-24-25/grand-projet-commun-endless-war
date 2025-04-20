using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxMeleeAutoTarget : BoxMelee
{
    protected override bool CanAttack()
    {
        float actualCooldown = skillSettings.Cooldown;

        float cooldownMultiplier = PlayerStatsMultiplier.IsInitialized ? PlayerStatsMultiplier.cooldownMultiplier : 1f;
        actualCooldown *= cooldownMultiplier;

        actualCooldown = Mathf.Max(actualCooldown, skillSettings.MinCooldown);

        if (attackTimer < actualCooldown)
            return false;

        float adjustedRange = skillSettings.Range * rangeMultiplier;

        return GetHitColliders(adjustedRange).Length > 0;
    }
}
