using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using System;
using DG.Tweening;

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

    [SerializeField] Image upgradeArrow;

    public static event Action<SkillHolderBehaviour> OnSkillSelected;
    public static event Action<SkillSettings> OnDetailsButton;

    private bool isSelected = false;

    private Tween arrowTween;

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

        SkillType types = skillSettings.skillType;

        if (skillSettings.acquired == false || skillSettings.skillType == SkillType.Buff)
        {
            upgradeArrow.enabled = false;
            StopArrowAnimation();
        }
        else
        {
            upgradeArrow.enabled = true;
            AnimateUpgradeArrow();

            if (skillSettings.CurrentLevel == 1)
            {
                upgradeArrow.color = Color.blue;
            }
            else if (skillSettings.CurrentLevel == 2)
            {
                upgradeArrow.color = Color.red;
            }
        }


        if (types == SkillType.Starting)
        {
            skillHolderImage.color = startingSkillColor;
        }
        else if (types == SkillType.Buff)
        {
            skillHolderImage.color = buffColor;
        }
        else
        {
            skillHolderImage.color = skillColor;
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

    private void AnimateUpgradeArrow()
    {
        if (upgradeArrow == null) return;

        upgradeArrow.rectTransform.anchoredPosition = Vector2.zero;

        arrowTween = upgradeArrow.rectTransform.DOAnchorPosY(10f, 0.5f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetUpdate(true)
            .SetEase(Ease.InOutSine);
    }

    private void StopArrowAnimation()
    {
        if (arrowTween != null && arrowTween.IsActive())
        {
            arrowTween.Kill();
        }
    }

}
