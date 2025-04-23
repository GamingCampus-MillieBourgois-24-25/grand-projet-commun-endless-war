using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillTreeManager : MonoBehaviour
{
    public SkillSlot[] skillSlots;
    public TMP_Text coinText;
    public RectTransform shakeTarget;
    private Image shakeImage;
    private Color originalColor;
    Vector3 originalPos;

    private SkillTreeSaveManager skillTreeSaveManager;
    public Button resetButton;
    public Button quitButton;

    private void Awake()
    {
        skillTreeSaveManager = new SkillTreeSaveManager();
        shakeImage = shakeTarget.GetComponent<Image>();
        originalColor = shakeImage.color;
        originalPos = shakeTarget.anchoredPosition;
    }

    private void OnEnable()
    {
        SkillSlot.OnSkillSlotAcquired += HandleSkillAcquired;
        resetButton.onClick.AddListener(ResetSkillTree);
        quitButton.onClick.AddListener(QuitToHub);
        MoneyManager.OnGoldChanged += UpdateCoinText;
        SkillSlot.OnInsufficientFunds += HandleInsufficientFunds;
    }

    private void OnDisable()
    {
        SkillSlot.OnSkillSlotAcquired -= HandleSkillAcquired;
        resetButton.onClick.RemoveListener(ResetSkillTree);
        quitButton.onClick.RemoveListener(QuitToHub);
        MoneyManager.OnGoldChanged -= UpdateCoinText;
        SkillSlot.OnInsufficientFunds -= HandleInsufficientFunds;
    }

    private void Start()
    {
        UpdateCoinText(MoneyManager.Instance.GetCurrentGold());
        skillTreeSaveManager.LoadSkillTree(skillSlots);

        foreach (SkillSlot slot in skillSlots)
        {
            slot.skillButton.onClick.AddListener(slot.TryUpgradeSkill);
        }
    }

    private void HandleSkillAcquired(SkillSlot skillSlot)
    {
        foreach (SkillSlot slot in skillSlots)
        {
            slot.Unlock();
        }

        skillTreeSaveManager.SaveSkillTree(skillSlots);
        PlayerStatsManager.Instance.Apply(skillSlot);
        PlayerStatsManager.Instance.SaveStats();
    }


    private void ResetSkillTree()
    {
        int refund = 0;
        foreach (SkillSlot slot in skillSlots)
        {
            if (slot.skillSlotState == SkillSlotState.Acquired)
                refund += slot.skillSO.skillCost;

            slot.ResetState();
        }


        MoneyManager.Instance.AddGold(refund);
        skillTreeSaveManager.SaveSkillTree(skillSlots);
        PlayerStatsManager.Instance.RecalculateAllStats(skillSlots);
        PlayerStatsManager.Instance.SaveStats();

    }

    private void QuitToHub()
    {
        Loader.Load(Loader.Scene.HubScene);
    }

    private void UpdateCoinText(int value)
    {
        coinText.text = value.ToString();

        coinText.transform.DOKill();
        coinText.transform.localScale = Vector3.one;

        coinText.transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0), 0.2f, 10, 1);
    }

    private void HandleInsufficientFunds(SkillSlot skillSlot)
    {
        if (shakeTarget == null || shakeImage == null) return;

        shakeTarget.DOKill();
        shakeImage.DOKill();

        shakeTarget.DOShakeAnchorPos(
            duration: 0.4f,
            strength: new Vector2(20f, 5f),
            vibrato: 10,
            randomness: 90,
            snapping: false,
            fadeOut: true
        ).OnComplete(() => shakeTarget.anchoredPosition = originalPos);

        Color flashColor = Color.red;

        shakeImage.DOColor(flashColor, 0.2f)
            .SetLoops(2, LoopType.Yoyo)
            .OnComplete(() => shakeImage.color = originalColor);
    }

}
