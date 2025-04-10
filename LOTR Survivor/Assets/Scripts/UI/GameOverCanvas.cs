using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GameOverCanvas : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] private RectTransform gameOverPanel;
    [SerializeField] float amount = 0.7f;
    [SerializeField] int retryCost = 0;

    [SerializeField] private float animationDuration = 0.8f;
    [SerializeField] private float startYOffset = 800f;
    [SerializeField] private Ease animationEase = Ease.OutBounce;

    private Vector2 originalPosition;

    private void Awake()
    {
        HealthEvents.OnGameOver += DisplayUI;
        originalPosition = gameOverPanel.anchoredPosition;
        gameOverPanel.gameObject.SetActive(false);
    }
    public void QuitToHub()
    {
        Loader.Load(Loader.Scene.HubScene);
    }

    public void Retry()
    {
        HidePanel();
    }

    public void DisplayUI()
    {
        GamePauseManager.Instance.PauseGame();
        gameOverPanel.gameObject.SetActive(true);

        gameOverPanel.anchoredPosition = originalPosition + Vector2.up * startYOffset;

        gameOverPanel.DOAnchorPos(originalPosition, animationDuration)
            .SetEase(animationEase)
            .SetUpdate(true);
    }

    public void HidePanel()
    {
        gameOverPanel.DOAnchorPos(originalPosition + Vector2.down * startYOffset, 0.5f)
            .SetEase(Ease.InBack)
            .SetUpdate(true)
            .OnComplete(OnHideComplete);
    }

    private void OnHideComplete()
    {
        GamePauseManager.Instance.ResumeGame();
        gameOverPanel.gameObject.SetActive(false);
        player.SetActive(true);
        player.GetComponent<PlayerHealthBehaviour>().Revive(amount);
    }
}
