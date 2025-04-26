using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PauseCanvas : MonoBehaviour
{
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button quitButton;

    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private RectTransform pausePanel;

    [SerializeField] private float animationDuration = 0.8f;
    [SerializeField] private float startYOffset = 800f;
    [SerializeField] private Ease animationEase = Ease.OutBounce;

    private Vector2 originalPosition;

    private void Awake()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        pauseButton.onClick.AddListener(ShowPauseMenu);
        resumeButton.onClick.AddListener(HidePauseMenu);
        quitButton.onClick.AddListener(QuitToHub);

        originalPosition = pausePanel.anchoredPosition;
        pausePanel.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        GamePauseManager.Instance.OnGamePaused += DisablePauseButton;
        GamePauseManager.Instance.OnGameResumed += EnablePauseButton;
    }

    private void OnDisable()
    {
        GamePauseManager.Instance.OnGamePaused -= DisablePauseButton;
        GamePauseManager.Instance.OnGameResumed -= EnablePauseButton;
    }

    private void EnablePauseButton()
    {
        pauseButton.enabled = true;
    }

    private void DisablePauseButton()
    {
        pauseButton.enabled = false;
    }

    private void ShowPauseMenu()
    {
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        pausePanel.gameObject.SetActive(true);

        pausePanel.anchoredPosition = originalPosition + Vector2.up * startYOffset;

        pausePanel.DOAnchorPos(originalPosition, animationDuration)
            .SetEase(animationEase)
            .SetUpdate(true);

        GamePauseManager.Instance.PauseGame();
    }

    private void HidePauseMenu()
    {
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        pausePanel.DOAnchorPos(originalPosition + Vector2.up * startYOffset, animationDuration)
            .SetEase(animationEase)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                canvasGroup.alpha = 0f;
                pausePanel.gameObject.SetActive(false);
                GamePauseManager.Instance.ResumeGame();
            });
    }


    private void QuitToHub()
    {
        Loader.Load(Loader.Scene.HubScene);
    }
}
