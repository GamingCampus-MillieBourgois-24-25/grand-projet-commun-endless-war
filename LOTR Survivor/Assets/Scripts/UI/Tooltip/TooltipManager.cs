using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class TooltipManager : MonoBehaviour
{
    [Header("Singleton")]
    public static TooltipManager Instance;

    [Header("Tooltip UI Elements")]
    [SerializeField] private CanvasGroup tipPanel;
    [SerializeField] private RectTransform tipTransform;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text contentText;

    [Header("Animation Settings haha")]
    [SerializeField] private float animationDuration = 0.7f;

    private Tween currentTween;
    private bool isAnimating = false;
    private Queue<TooltipData> tooltipQueue = new Queue<TooltipData>();
    private bool pausedByTooltip = false;

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
    }

    private void Start()
    {
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

        if (isAnimating)
        {
            tooltipQueue.Enqueue(tooltipData);
            Debug.Log("Tooltip added to queue.");
        }
        else
        {
            if (!GamePauseManager.Instance.IsGamePaused)
            {
                GamePauseManager.Instance.PauseGame();
                pausedByTooltip = true;
            }
            DisplayTooltip(tooltipData);
            TooltipState.Instance.MarkTooltipAsSeen(tooltipData.tooltipID);
        }
    }

    public void HideTip()
    {
        currentTween?.Kill();

        currentTween = tipTransform.DOScale(Vector3.zero, animationDuration)
            .SetEase(Ease.InBack)
            .OnComplete(OnTipHidden)
            .SetUpdate(true);

        Debug.Log("Closing tip");
    }

    private void DisplayTooltip(TooltipData tooltipData)
    {
        Debug.Log("Showing new tip");
        isAnimating = true;

        titleText.text = tooltipData.title;
        contentText.text = tooltipData.content;

        tipPanel.alpha = 1f;
        tipPanel.interactable = true;
        tipPanel.blocksRaycasts = true;

        tipTransform.localScale = Vector3.zero;

        currentTween = tipTransform.DOScale(Vector3.one, animationDuration)
            .SetEase(Ease.OutBack)
            .SetUpdate(true);
    }

    private void OnTipHidden()
    {
        tipPanel.alpha = 0f;
        tipPanel.interactable = false;
        tipPanel.blocksRaycasts = false;
        titleText.text = "";
        contentText.text = "";

        if (pausedByTooltip)
        {
            GamePauseManager.Instance.ResumeGame();
            pausedByTooltip = false;
        }

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
        return TooltipState.Instance.HasSeenTooltip(tooltipData.tooltipID);
    }
}