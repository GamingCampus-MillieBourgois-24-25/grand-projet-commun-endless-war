using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillTreeManager : MonoBehaviour
{
    public SkillSlot[] skillSlots;
    public TMP_Text coinText;

    private void OnEnable()
    {
        SkillSlot.OnSkillSlotAcquired += HandleSkillAcquired;
    }

    private void OnDisable()
    {
        SkillSlot.OnSkillSlotAcquired -= HandleSkillAcquired;
    }

    private void Start()
    {
        foreach (SkillSlot slot in skillSlots)
        {
            slot.skillButton.onClick.AddListener(slot.TryUpgradeSkill);
        }
    }

    private void HandleSkillAcquired(SkillSlot skillSlot)
    {
        foreach(SkillSlot slot in skillSlots)
        {
            slot.Unlock();
        }
    }
}
