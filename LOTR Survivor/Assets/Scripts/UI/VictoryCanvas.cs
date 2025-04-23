using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryCanvas : MonoBehaviour
{
    [SerializeField] private RectTransform victoryPanel;
    [SerializeField] private float animationDuration = 0.8f;
    [SerializeField] private float startYOffset = 800f;
    [SerializeField] private Ease animationEase = Ease.OutBounce;

    private Vector2 originalPosition;

    private void Awake()
    {
        originalPosition = victoryPanel.anchoredPosition;
        victoryPanel.gameObject.SetActive(false);
    }

    public void QuitToHub()
    {
        Loader.Load(Loader.Scene.HubScene);
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void DisplayUI()
    {
        GamePauseManager.Instance.PauseGame();
        victoryPanel.gameObject.SetActive(true);

        victoryPanel.anchoredPosition = originalPosition + Vector2.up * startYOffset;

        victoryPanel.DOAnchorPos(originalPosition, animationDuration)
            .SetEase(animationEase)
            .SetUpdate(true);
    }
}
