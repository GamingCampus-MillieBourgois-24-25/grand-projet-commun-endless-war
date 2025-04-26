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
    [Header("Skill Data")]
    public SkillSO skillSO;
    public SkillSlotState skillSlotState;
    public List<SkillSlot> prerequisiteSkillSlots;

    [Header("UI References")]
    public Button skillButton;
    public Image skillIcon;
    public TMP_Text skillText;
    public TMP_Text costText;
    public Image lockedImage;
    public Image backgroundImage;

    [Header("Link Visuals")]
    public List<Image> links;

    [Header("Colors")]
    public Color unlockColor;
    public Color lockColor;

    [Header("Audio & FX")]
    [SerializeField] private GameObject unlockParticle;
    public AudioSource audioSource;
    public AudioClip unlockClip;
    public AudioClip lockClip;

    public static event Action<SkillSlot> OnSkillSlotAcquired;
    public static event Action<SkillSlot> OnInsufficientFunds;

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
            if (MoneyManager.Instance.GetCurrentGold() < skillSO.skillCost)
            {
                OnInsufficientFunds?.Invoke(this);
                VolumeManager.Instance.PlaySFX(lockClip, 0.5f);
                return;
            }
            MoneyManager.Instance.SpendGold(skillSO.skillCost);
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
            UpdateLinks();
        }
    }

    public void UpdateUI()
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
                backgroundImage.color = lockColor;
                break;

            case SkillSlotState.Unlocked:
                skillButton.interactable = true;
                costText.enabled = true;
                lockedImage.enabled = false;
                skillIcon.color = skillSO.skillColor;
                backgroundImage.color = lockColor;
                break;

            case SkillSlotState.Acquired:
                skillButton.interactable = false;
                costText.enabled = false;
                lockedImage.enabled = false;
                skillIcon.color = skillSO.skillColor;
                backgroundImage.color = unlockColor;
                break;
        }
    }

    private void PlayAcquiredAnimation(Action onComplete)
    {
        transform.DOKill();
        transform.localScale = Vector3.one;

        Sequence seq = DOTween.Sequence();

        if (unlockParticle != null)
        {
            GameObject particleEffect = Instantiate(unlockParticle, transform);
            particleEffect.transform.localScale = Vector3.zero;

            seq.Join(particleEffect.transform.DOScale(50f, 0.25f).SetEase(Ease.OutBack))
               .Append(particleEffect.transform.DOScale(0f, 0.25f).SetEase(Ease.InBack))
               .AppendCallback(() => Destroy(particleEffect));
        }

        seq.Insert(0, transform.DOScale(1.8f, 0.18f).SetEase(Ease.OutBack));
        seq.Insert(0.18f, transform.DOScale(1f, 0.2f).SetEase(Ease.InOutSine));
        seq.OnComplete(() => onComplete?.Invoke()); VolumeManager.Instance.PlaySFX(unlockClip, 0.5f);
        ;
    }


    private void PlayUnlockedAnimation()
    {
        transform.DOKill();
        transform.localScale = Vector3.one * 0.9f;
        transform.DOScale(1f, 0.3f).SetEase(Ease.OutBounce);
    }

    public void UpdateLinks()
    {
        foreach (Image link in links)
        {
            if (skillSlotState == SkillSlotState.Acquired)
            {
                link.color = Color.green;
            }
            else if (skillSlotState == SkillSlotState.Unlocked)
            {
                link.color = Color.grey;
            }
            else
            {
                link.color = Color.red;
            }
        }
    }
    public void ResetState()
    {
        skillSlotState = (prerequisiteSkillSlots.Count == 0) ? SkillSlotState.Unlocked : SkillSlotState.Locked;
        UpdateUI();
        UpdateLinks();
    }
}
