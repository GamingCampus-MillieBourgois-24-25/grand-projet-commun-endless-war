using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
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

    // Références UI
    [SerializeField] private TMP_Text enemyCountText;
    [SerializeField] private TMP_Text goldEarnedText;
    [SerializeField] private TMP_Text levelReachedText;

    // Références managers
    [SerializeField] private MoneyManager moneyManager;
    [SerializeField] private XPManager xpManager;

    private void Awake()
    {
        originalPosition = gameOverPanel.anchoredPosition;
        gameOverPanel.gameObject.SetActive(false);
        PlayerEvents.OnPlayerSpawned += AssignPlayer;
    }

    private void OnEnable()
    {
        HealthEvents.OnGameOver += DisplayUI;
    }

    private void OnDisable()
    {
        HealthEvents.OnGameOver -= DisplayUI;
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

        UpdateStatsUI();
    }

    private void UpdateStatsUI()
    {
        if (moneyManager != null && xpManager != null)
        {
            enemyCountText.text = $"Ennemis tués : {moneyManager.SessionEnemiesKilled}";
            goldEarnedText.text = $"Gold gagné : {moneyManager.SessionGoldEarned}";
            levelReachedText.text = $"Niveau atteint : {xpManager.levelCurrent}";
        }
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
        if (player == null)
        {
            Debug.LogError("Player is null");
            return;
        }
        player.SetActive(true);
        player.GetComponent<PlayerHealthBehaviour>().Revive(amount);
    }

    private void AssignPlayer(GameObject _player)
    {
        player = _player;
    }
}
