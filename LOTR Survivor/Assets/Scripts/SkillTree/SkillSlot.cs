using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum SkillSlotState
{
    Locked,
    Unlocked,
    Acquired
}

public class SkillSlot : MonoBehaviour
{
    public List<SkillSlot> prerequisiteSkillSlots;
    public List<Image> links;

    public SkillSO skillSO;

    public SkillSlotState skillSlotState;
    public Image skillIcon;
    public TMP_Text skillText;

    public Button skillButton;

    public Image lockedImage;
    public TMP_Text costText;

    public static event Action<SkillSlot> OnSkillSlotAcquired;

    private void OnValidate()
    {
        if (skillSO != null && lockedImage != null)
        {
            UpdateUI();
        }
    }

    public void TryUpgradeSkill()
    {
        if (skillSlotState == SkillSlotState.Unlocked)
        {
            skillSlotState = SkillSlotState.Acquired;
            UpdateUI();
            PlayAcquiredAnimation(() =>
            {
                OnSkillSlotAcquired?.Invoke(this);
                UpdateLinks();
            });
        }
    }


    public bool CanUnlockSkill()
    {
        foreach (SkillSlot skill in prerequisiteSkillSlots)
        {
            if (skill.skillSlotState == SkillSlotState.Acquired)
            {
                return true;
            }
        }

        return false;
    }

    public void Unlock()
    {
        if (skillSlotState == SkillSlotState.Locked && CanUnlockSkill())
        {
            skillSlotState = SkillSlotState.Unlocked;
            UpdateUI();
            PlayUnlockedAnimation();
        }
    }

    private void UpdateUI()
    {
        skillIcon.sprite = skillSO.skillIcon;
        skillText.text = skillSO.skillText;
        costText.text = skillSO.skillCost.ToString();

        switch (skillSlotState)
        {
            case SkillSlotState.Locked:
                skillButton.interactable = false;
                costText.enabled = false;
                lockedImage.enabled = true;
                skillIcon.color = Color.grey;
                break;

            case SkillSlotState.Unlocked:
                skillButton.interactable = true;
                costText.enabled = true;
                lockedImage.enabled = false;
                skillIcon.color = Color.white;
                break;

            case SkillSlotState.Acquired:
                skillButton.interactable = false;
                costText.enabled = false;
                lockedImage.enabled = false;
                skillIcon.color = Color.white;
                break;
        }
    }

    private void PlayAcquiredAnimation(Action onComplete)
    {
        transform.DOKill();
        transform.localScale = Vector3.one;

        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOScale(1.8f, 0.18f).SetEase(Ease.OutBack))
           .Append(transform.DOScale(1f, 0.2f).SetEase(Ease.InOutSine))
           .OnComplete(() => onComplete?.Invoke());
    }

    private void PlayUnlockedAnimation()
    {
        transform.DOKill();
        transform.localScale = Vector3.one * 0.9f;
        transform.DOScale(1f, 0.3f).SetEase(Ease.OutBounce);
    }

    private void UpdateLinks()
    {
        if (skillSlotState == SkillSlotState.Acquired) 
        { 
            foreach(Image link in links)
            {
                link.color = Color.green;
            }
        }
    }
}
