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
        infoPanel.transform.localScale = Vector3.zero;
        infoPanel.transform.localEulerAngles = new Vector3(0, 0, -60f);

        infoPanel.transform.DOScale(Vector3.one, 0.3f)
            .SetEase(Ease.OutBack)
            .SetUpdate(true);

        infoPanel.transform.DOLocalRotate(Vector3.zero, 0.3f)
            .SetEase(Ease.OutBack)
            .SetUpdate(true);

        infoPanel.alpha = 1f;
        infoPanel.interactable = true;
        infoPanel.blocksRaycasts = true;

        skillNameText.text = skillSettings.name;
        skillDescriptionText.text = skillSettings.skillDescription;
    }

    public void HideSkillInfo()
    {
        infoPanel.transform.localEulerAngles = Vector3.zero;
        infoPanel.transform.DOLocalRotate(new Vector3(0, 0, -60f), 0.2f)
            .SetEase(Ease.InBack)
            .SetUpdate(true);


        infoPanel.transform.DOScale(Vector3.zero, 0.2f)
            .SetEase(Ease.InBack)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                HideSkillInfoComplete();
                OnHide?.Invoke();
            });
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
