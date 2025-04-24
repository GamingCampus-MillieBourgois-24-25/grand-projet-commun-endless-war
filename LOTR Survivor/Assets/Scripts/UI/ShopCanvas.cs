using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ShopCanvas : MonoBehaviour
{
    [SerializeField] private CanvasGroup shopCanvas;
    [SerializeField] private RectTransform leftContent;
    [SerializeField] private RectTransform rightContent;
    [SerializeField] private RectTransform topContent;
    [SerializeField] private RectTransform bottomContent;

    [SerializeField] private MenuManager menuManager;

    [SerializeField] private Button overlayButton;
    [SerializeField] private Button newsButton;

    [SerializeField] private RectTransform newsTab;
    [SerializeField] private CanvasGroup newsCanvas;

    private Vector2 topStartPos;
    private Vector2 rightStartPos;
    private Vector2 leftStartPos;
    private Vector2 bottomStartPos;

    private bool newsOpen = false;

    void Start()
    {
        shopCanvas.blocksRaycasts = false;
        shopCanvas.alpha = 0f;
        topStartPos = topContent.anchoredPosition;
        rightStartPos = rightContent.anchoredPosition;
        leftStartPos = leftContent.anchoredPosition;
        bottomStartPos = bottomContent.anchoredPosition;

        overlayButton.onClick.AddListener(HideShop);
        overlayButton.onClick.AddListener(CloseNewsTab);
        newsButton.onClick.AddListener(HandleNewsButton);
    }

    public void OpenShop()
    {
        shopCanvas.alpha = 1f;
        shopCanvas.blocksRaycasts = true;

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
    }

    public void HideShop()
    {
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        float duration = 1.5f;
        float delayLeft = 0.3f;
        float delayOthers = 0.2f;

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
        {
            CloseNewsTab();
        }
        else
        {
            OpenNewsTab();
        }
    }

    private void OpenNewsTab()
    {
        if (newsTab != null)
        {
            newsCanvas.blocksRaycasts = true;
            newsCanvas.alpha = 1f;
            newsCanvas.interactable = true;
            newsOpen = true;

            float screenWidth = Screen.width;
            Vector2 startPos = new Vector2(screenWidth + 300f, newsTab.anchoredPosition.y);
            newsTab.anchoredPosition = startPos;

            newsTab.DOAnchorPosX(rightStartPos.x, 1.2f)
                .SetEase(Ease.OutCubic)
                .SetDelay(0.3f);
        }
    }

    private void CloseNewsTab()
    {
        if (newsTab != null && newsOpen)
        {
            float screenWidth = Screen.width;
            newsTab.DOAnchorPosX(screenWidth + 300f, 1.2f)
                .SetEase(Ease.InCubic)
                .OnComplete(() =>
                {
                    newsCanvas.blocksRaycasts = false;
                    newsCanvas.alpha = 0f;
                    newsCanvas.interactable = false;
                    newsOpen = false;
                });
        }
    }
}
