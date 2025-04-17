using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class SkillInfo : MonoBehaviour
{
    public CanvasGroup infoPanel;

    public TMP_Text skillNameText;
    public TMP_Text skillDescriptionText;

    public TMP_Text[] statsText;

    private RectTransform infoPanelRect;

    private float AnimationDuration = 0.6f;

    public static event Action OnHide;

    private void Awake()
    {
        infoPanelRect = GetComponent<RectTransform>();
    }

    private void Start()
    {
        HideSkillInfoComplete();
    }

    public void ShowSkillInfo(SkillSettings skillSettings)
    {
        ResetPanelTransform();
        AnimatePanel();
        ActivatePanel();

        SetTexts(skillSettings);
        DisplayStats(skillSettings);
    }

    private void ResetPanelTransform()
    {
        infoPanel.transform.localScale = Vector3.zero;
        infoPanel.transform.localEulerAngles = new Vector3(0, 0, -40f);
    }

    private void AnimatePanel()
    {
        infoPanel.transform.DOScale(Vector3.one, AnimationDuration)
            .SetEase(Ease.OutBack)
            .SetUpdate(true);

        infoPanel.transform.DOLocalRotate(Vector3.zero, AnimationDuration)
            .SetEase(Ease.OutBack)
            .SetUpdate(true);
    }

    private void ActivatePanel()
    {
        infoPanel.alpha = 1f;
        infoPanel.interactable = true;
        infoPanel.blocksRaycasts = true;
    }

    private void SetTexts(SkillSettings skillSettings)
    {
        skillNameText.text = skillSettings.name;
        skillDescriptionText.text = skillSettings.skillDescription;
    }

    private void DisplayStats(SkillSettings upgrades)
    {
        var stats = new List<string>();

        AddStat(stats, "Damage", upgrades.DamageUpgrade);
        AddStat(stats, "Speed", upgrades.SpeedUpgrade);
        AddStat(stats, "Rate", upgrades.CooldownUpgrade, inverse: true);
        AddStat(stats, "Range", upgrades.RangeUpgrade);
        AddStat(stats, "Aim Range", upgrades.AimRangeUpgrade);
        AddStat(stats, "Rotation", upgrades.MaxRotationUpgrade);

        for (int i = 0; i < statsText.Length; i++)
        {
            bool show = i < stats.Count;
            statsText[i].text = show ? stats[i] : "";
            statsText[i].gameObject.SetActive(show);
        }
    }

    private void AddStat(List<string> stats, string label, float value, bool inverse = false)
    {
        if (Mathf.Approximately(value, 1f)) return;
        float displayValue = inverse && value != 0f ? 1f / value : value;
        stats.Add($"{label}: x{displayValue:0.##}");
    }


    public void HideSkillInfo()
    {
        ResetRotation();
        AnimateHide(() =>
        {
            HideSkillInfoComplete();
            OnHide?.Invoke();
        });
    }

    private void ResetRotation()
    {
        infoPanel.transform.localEulerAngles = Vector3.zero;
    }

    private void AnimateHide(TweenCallback onComplete)
    {
        infoPanel.transform.DOLocalRotate(new Vector3(0, 0, -60f), 0.2f)
            .SetEase(Ease.InBack)
            .SetUpdate(true);

        infoPanel.transform.DOScale(Vector3.zero, 0.2f)
            .SetEase(Ease.InBack)
            .SetUpdate(true)
            .OnComplete(onComplete);
    }


    public void HideSkillInfoComplete()
    {
        infoPanel.alpha = 0f;
        infoPanel.interactable = false;
        infoPanel.blocksRaycasts = false;
        infoPanel.transform.localScale = Vector3.zero;

        skillNameText.text = "";
        skillDescriptionText.text = "";
    }
}
