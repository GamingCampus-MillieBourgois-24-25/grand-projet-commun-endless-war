using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager Instance;

    [SerializeField] private CanvasGroup tipPanel;
    [SerializeField] private RectTransform tipTransform;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text contentText;
    [SerializeField] private float animationDuration = 0.3f;
    [SerializeField] private TooltipState tooltipState;

    private Tween currentTween;
    private bool isAnimating = false;
    private Queue<TooltipData> tooltipQueue = new Queue<TooltipData>();

    public event System.Action OnTooltipClosed;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        HideTip();
    }

    public void ShowTip(TooltipData tooltipData)
    {
        if (tooltipData == null)
        {
            Debug.LogError("Tooltip data is null.");
            return;
        }

        if (IsTooltipSeen(tooltipData))
        {
            Debug.Log("Tooltip already seen, not showing.");
            return;
        }

        if (!isAnimating)
        {
            DisplayTooltip(tooltipData);
            tooltipState.MarkTooltipAsSeen(tooltipData.tooltipID);
        }
        else
        {
            tooltipQueue.Enqueue(tooltipData);
            Debug.Log("Tooltip added to queue.");
        }
    }

    public void HideTip()
    {
        currentTween?.Kill();

        isAnimating = true;

        currentTween = tipTransform.DOScale(Vector3.zero, animationDuration)
            .SetEase(Ease.InBack)
            .OnComplete(OnTipHidden)
            .SetUpdate(true);

        Debug.Log("Closing tip");
    }

    private void DisplayTooltip(TooltipData tooltipData)
    {
        Debug.Log("Showing new tip");

        titleText.text = tooltipData.title;
        contentText.text = tooltipData.content;

        tipPanel.alpha = 1f;
        tipPanel.interactable = true;
        tipPanel.blocksRaycasts = true;

        tipTransform.localScale = Vector3.zero;

        currentTween = tipTransform.DOScale(Vector3.one, animationDuration)
            .SetEase(Ease.OutBack)
            .SetUpdate(true);

        Time.timeScale = 0f;
    }

    private void OnTipHidden()
    {
        tipPanel.alpha = 0f;
        tipPanel.interactable = false;
        tipPanel.blocksRaycasts = false;
        titleText.text = "";
        contentText.text = "";

        Time.timeScale = 1f;

        isAnimating = false;

        OnTooltipClosed?.Invoke();

        if (tooltipQueue.Count > 0)
        {
            TooltipData nextTooltip = tooltipQueue.Dequeue();
            ShowTip(nextTooltip);
        }
    }

    private bool IsTooltipSeen(TooltipData tooltipData)
    {
        return tooltipState.HasSeenTooltip(tooltipData.tooltipID);
    }
}
