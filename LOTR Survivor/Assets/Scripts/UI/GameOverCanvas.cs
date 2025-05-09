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

    // R�f�rences UI
    [SerializeField] private TMP_Text enemyCountText;
    [SerializeField] private TMP_Text goldEarnedText;
    [SerializeField] private TMP_Text levelReachedText;

    // R�f�rences managers
    [SerializeField] private MoneyManager moneyManager;
    [SerializeField] private XPManager xpManager;

    [SerializeField] private AudioClip loseClip;

    [SerializeField] private int maxRespawn = 1;

    [SerializeField] private GameObject[] gameObjects;

    private int respawnCount = 0;

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
        VolumeManager.Instance.PlaySFX(loseClip, 1f);

        GamePauseManager.Instance.PauseGame();
        gameOverPanel.gameObject.SetActive(true);

        gameOverPanel.anchoredPosition = originalPosition + Vector2.up * startYOffset;

        gameOverPanel.DOAnchorPos(originalPosition, animationDuration)
            .SetEase(animationEase)
            .SetUpdate(true);

        UpdateStatsUI();

        if (respawnCount >= maxRespawn)
        {
            foreach (GameObject go in gameObjects)
            {
                go.SetActive(false);
            }
        }
    }

    private void UpdateStatsUI()
    {
        if (moneyManager != null && xpManager != null)
        {
            enemyCountText.text = $"Ennemis tu�s : {moneyManager.SessionEnemiesKilled}";
            goldEarnedText.text = $"Gold gagn� : {moneyManager.SessionGoldEarned}";
            levelReachedText.text = $"Niveau atteint : {xpManager.levelCurrent}";
        }
    }

    public void HidePanel()
    {
        respawnCount++;
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
