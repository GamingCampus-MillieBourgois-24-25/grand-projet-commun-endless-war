using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillTreeManager : MonoBehaviour
{
    public SkillSlot[] skillSlots;
    public TMP_Text coinText;

    private SkillTreeSaveManager skillTreeSaveManager;
    public Button resetButton;
    public Button quitButton;

    private void Awake()
    {
        skillTreeSaveManager = new SkillTreeSaveManager();
    }

    private void OnEnable()
    {
        SkillSlot.OnSkillSlotAcquired += HandleSkillAcquired;
        resetButton.onClick.AddListener(ResetSkillTree);
        quitButton.onClick.AddListener(QuitToHub);
    }

    private void OnDisable()
    {
        SkillSlot.OnSkillSlotAcquired -= HandleSkillAcquired;
        resetButton.onClick.RemoveListener(ResetSkillTree);
        quitButton.onClick.RemoveListener(QuitToHub);
    }

    private void Start()
    {
        skillTreeSaveManager.LoadSkillTree(skillSlots);

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
        skillTreeSaveManager.SaveSkillTree(skillSlots);
    }

    private void ResetSkillTree()
    {
        foreach (SkillSlot slot in skillSlots)
        {
            if (slot.prerequisiteSkillSlots.Count == 0)
            {
                slot.skillSlotState = SkillSlotState.Unlocked;
            }
            else
            {
                slot.skillSlotState = SkillSlotState.Locked;
            }

            slot.UpdateUI();
            slot.UpdateLinks();
        }

        skillTreeSaveManager.SaveSkillTree(skillSlots);
    }

    private void QuitToHub()
    {
        Loader.Load(Loader.Scene.HubScene);
    }
}
