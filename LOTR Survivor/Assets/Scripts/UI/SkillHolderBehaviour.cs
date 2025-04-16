using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using System;

public class SkillHolderBehaviour : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    public SkillSettings _skillSettings;

    [SerializeField] Image selectImage;
    [SerializeField] Image skillHolderImage;
    [SerializeField] Image skillImage;

    [SerializeField] Color startingSkillColor;
    [SerializeField] Color skillColor;
    [SerializeField] Color buffColor;

    public static event Action<SkillHolderBehaviour> OnSkillSelected;
    public static event Action<SkillSettings> OnDetailsButton;

    private bool isSelected = false;

    private void Start()
    {
        Unselect();
    }

    public void UpdateData(SkillSettings skillSettings)
    {
        if (skillSettings == null)
        {
            Debug.LogWarning("Tried to update SkillHolder with null SkillSettings.");
            return;
        }

        _skillSettings = skillSettings;
        text.text = skillSettings.skillName;
        skillImage.sprite = skillSettings.skillSprite;

        switch (skillSettings.attackSettings.skillType)
        {
            case SkillType.Starting:
                skillHolderImage.color = startingSkillColor;
                break;

            case SkillType.Heal:
                skillHolderImage.color = buffColor;
                break;

            case SkillType.Attack:
            case SkillType.Buff:
            default:
                skillHolderImage.color = skillColor;
                break;
        }
    }

    public void Select()
    {
        if (isSelected) return;
        isSelected = true;
        selectImage.enabled = true;
        OnSkillSelected?.Invoke(this);
    }

    public void Unselect()
    {
        isSelected = false;
        selectImage.enabled = false;
    }

    public void ShowDetails()
    {
        OnDetailsButton?.Invoke(_skillSettings);
    }
}
