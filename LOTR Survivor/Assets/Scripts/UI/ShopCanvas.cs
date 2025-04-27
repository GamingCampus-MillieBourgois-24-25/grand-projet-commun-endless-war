using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopCanvas : MonoBehaviour
{
    [Header("Canvas")]
    [SerializeField] private CanvasGroup shopCanvas;

    [Header("Content Panels")]
    [SerializeField] private RectTransform leftContent;
    [SerializeField] private RectTransform rightContent;
    [SerializeField] private RectTransform topContent;
    [SerializeField] private RectTransform bottomContent;

    [Header("Dependencies")]
    [SerializeField] private MenuManager menuManager;

    [Header("Buttons")]
    [SerializeField] private Button overlayButton;
    [SerializeField] private Button newsButton;
    [SerializeField] private Button itemsButton;
    [SerializeField] private Button shortlyButton;
    [SerializeField] private Button closeButton;

    [Header("News Tab")]
    [SerializeField] private RectTransform newsTab;
    [SerializeField] private CanvasGroup newsCanvas;

    [Header("Items Tab")]
    [SerializeField] private RectTransform itemsTab;
    [SerializeField] private CanvasGroup itemsCanvas;

    [Header("Shortly Tab")]
    [SerializeField] private RectTransform shortlyTab;
    [SerializeField] private CanvasGroup shortlyCanvas;

    [Header("Banner")]
    [SerializeField] private Image maskBig;
    [SerializeField] private Image mask;
    [SerializeField] private TMP_Text cost;
    [SerializeField] private TMP_Text header;
    [SerializeField] private TMP_Text slotName;

    [Header("Gold Display")]
    [SerializeField] private TMP_Text goldText;

    private Vector2 topStartPos;
    private Vector2 rightStartPos;
    private Vector2 leftStartPos;
    private Vector2 bottomStartPos;

    private bool newsOpen = false;
    private bool itemsOpen = false;
    private bool shortlyOpen = false;

    void Start()
    {
        shopCanvas.blocksRaycasts = false;
        shopCanvas.alpha = 0f;

        topStartPos = topContent.anchoredPosition;
        rightStartPos = rightContent.anchoredPosition;
        leftStartPos = leftContent.anchoredPosition;
        bottomStartPos = bottomContent.anchoredPosition;

        overlayButton.onClick.AddListener(HideShop);
        newsButton.onClick.AddListener(HandleNewsButton);
        itemsButton.onClick.AddListener(HandleItemsButton);
        shortlyButton.onClick.AddListener(HandleShortlyButton);

    }

    public void OpenShop()
    {
        shopCanvas.alpha = 1f;
        shopCanvas.blocksRaycasts = true;

        UpdateGoldDisplay();

        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        float duration = 2f;
        float delayLeft = 0.5f;
        float delayOthers = 0.3f;

        bottomContent.anchoredPosition = bottomStartPos;
        bottomContent.DOAnchorPos(new Vector2(bottomStartPos.x, -screenHeight - 300f), 1.2f)
            .SetEase(Ease.InCubic);

        topContent.anchoredPosition = new Vector2(-screenWidth - 300f, topStartPos.y);
        rightContent.anchoredPosition = new Vector2(screenWidth + 300f, rightStartPos.y);
        leftContent.anchoredPosition = new Vector2(leftStartPos.x, screenHeight + 300f);

        topContent.DOAnchorPos(topStartPos, duration)
            .SetEase(Ease.OutCubic)
            .SetDelay(delayOthers);

        rightContent.DOAnchorPos(rightStartPos, duration)
            .SetEase(Ease.OutCubic)
            .SetDelay(delayOthers);

        leftContent.DOAnchorPos(leftStartPos, duration)
            .SetEase(Ease.OutCubic)
            .SetDelay(delayOthers + delayLeft);
        closeButton.interactable = true;
    }

    public void HideShop()
    {
        CloseAllTabs();

        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        float duration = 1.5f;
        float delayLeft = 0.3f;
        float delayOthers = 0.2f;

        closeButton.interactable = false;

        topContent.DOAnchorPos(new Vector2(-screenWidth - 300f, topStartPos.y), duration)
            .SetEase(Ease.InCubic);

        rightContent.DOAnchorPos(new Vector2(screenWidth + 300f, rightStartPos.y), duration)
            .SetEase(Ease.InCubic);

        leftContent.DOAnchorPos(new Vector2(leftStartPos.x, screenHeight + 300f), duration)
            .SetEase(Ease.InCubic)
            .SetDelay(delayLeft);

        bottomContent.anchoredPosition = new Vector2(bottomStartPos.x, -screenHeight - 300f);
        bottomContent.DOAnchorPos(bottomStartPos, duration)
            .SetEase(Ease.OutCubic)
            .SetDelay(delayOthers + delayLeft);

        DOVirtual.DelayedCall(duration + delayLeft + delayOthers + 0.4f, () =>
        {
            shopCanvas.DOFade(0f, 0.3f).OnComplete(() =>
            {
                shopCanvas.blocksRaycasts = false;
                shopCanvas.alpha = 0f;
                menuManager.mainMenu.SetActive(true);
            });
        });
    }

    private void HandleNewsButton()
    {
        if (newsOpen)
            CloseTab(newsTab, newsCanvas, () => newsOpen = false);
        else
            OpenTab(newsTab, newsCanvas, () => newsOpen = true);
    }

    private void HandleItemsButton()
    {
        if (itemsOpen)
            CloseTab(itemsTab, itemsCanvas, () => itemsOpen = false);
        else
            OpenTab(itemsTab, itemsCanvas, () => itemsOpen = true);
    }

    private void HandleShortlyButton()
    {
        if (shortlyOpen)
            CloseTab(shortlyTab, shortlyCanvas, () => shortlyOpen = false);
        else
            OpenTab(shortlyTab, shortlyCanvas, () => shortlyOpen = true);
    }

    private void OpenTab(RectTransform tab, CanvasGroup canvasGroup, System.Action onOpened)
    {
        CloseAllTabs();
        ResetBanner();

        if (tab != null)
        {
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            onOpened?.Invoke();

            float screenWidth = Screen.width;
            Vector2 startPos = new Vector2(screenWidth + 300f, tab.anchoredPosition.y);
            tab.anchoredPosition = startPos;

            tab.DOAnchorPosX(rightStartPos.x, 1.2f)
                .SetEase(Ease.OutCubic)
                .SetDelay(0.3f);
        }
    }

    private void CloseTab(RectTransform tab, CanvasGroup canvasGroup, System.Action onClosed)
    {
        ResetBanner();

        if (tab != null)
        {
            float screenWidth = Screen.width;
            tab.DOAnchorPosX(screenWidth + 300f, 1.2f)
                .SetEase(Ease.InCubic)
                .OnComplete(() =>
                {
                    canvasGroup.blocksRaycasts = false;
                    canvasGroup.alpha = 0f;
                    canvasGroup.interactable = false;
                    onClosed?.Invoke();
                });
        }
    }

    private void CloseAllTabs()
    {
        ResetBanner();
        if (newsOpen)
            CloseTab(newsTab, newsCanvas, () => newsOpen = false);

        if (itemsOpen)
            CloseTab(itemsTab, itemsCanvas, () => itemsOpen = false);

        if (shortlyOpen)
            CloseTab(shortlyTab, shortlyCanvas, () => shortlyOpen = false);
    }

    private void ResetBanner()
    {
        if (maskBig != null)
        {
            var color = maskBig.color;
            color.a = 0f;
            maskBig.color = color;
        }

        if (mask != null)
        {
            var color = mask.color;
            color.a = 0f;
            mask.color = color;
        }

        cost.text = "";
        header.text = "";
        slotName.text = "";
    }

    private void UpdateGoldDisplay()
    {
        if (goldText != null)
        {
            goldText.text = MoneyManager.Instance.GetCurrentGold().ToString();
        }
        else
        {
            Debug.LogWarning("Gold Text is not assigned in the ShopCanvas!");
        }
    }
}
