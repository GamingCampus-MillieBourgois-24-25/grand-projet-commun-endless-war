using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTreeEvents : MonoBehaviour
{
    private void OnEnable()
    {
        SkillSlot.OnSkillSlotAcquired += HandleSkillAcquired;
    }

    private void OnDisable()
    {
        SkillSlot.OnSkillSlotAcquired -= HandleSkillAcquired;
    }

    private void HandleSkillAcquired(SkillSlot slot)
    {

    }
}
