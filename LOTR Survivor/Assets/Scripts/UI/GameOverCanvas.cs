using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private XPManager xpManager;

    [SerializeField] private AudioClip loseClip;

    [SerializeField] private TMP_Text goldText;
    [SerializeField] private GameObject[] respawnObject;
    [SerializeField] private TMP_Text currentGold;
 
    [SerializeField] private int respawn = 1;

    private int respawnCount = 0;
    private Vector2 currentGoldOriginalPosition;
    private Color currentGoldOriginalColor;


    private void Awake()
    {
        originalPosition = gameOverPanel.anchoredPosition;
        gameOverPanel.gameObject.SetActive(false);
        PlayerEvents.OnPlayerSpawned += AssignPlayer;

        if (currentGold != null)
        {
            currentGoldOriginalPosition = currentGold.rectTransform.anchoredPosition;
            currentGoldOriginalColor = currentGold.color;
        }
        else
        {
            Debug.LogWarning("currentGold n'est pas assigné !");
        }
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
        if (goldText != null)
            goldText.text = retryCost.ToString();
        if (currentGold != null && MoneyManager.Instance != null)
            currentGold.text = MoneyManager.Instance.GetCurrentGold().ToString();

        if (respawnObject != null && respawnCount >= respawn)
        {
            foreach (GameObject item in respawnObject)
            {
                if (item != null)
                    item.SetActive(false);
            }
        }

        if (VolumeManager.Instance != null)
            VolumeManager.Instance.PlaySFX(loseClip, 1f);

        if (GamePauseManager.Instance != null)
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
        if (MoneyManager.Instance != null && xpManager != null)
        {
            enemyCountText.text = $"Ennemis tués : {MoneyManager.Instance.SessionEnemiesKilled}";
            goldEarnedText.text = $"Gold gagné : {MoneyManager.Instance.SessionGoldEarned}";
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
        respawnCount++;
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

    public void TryRespawn()
    {
        if (respawnCount >= respawn)
        {
            return;
        }
        if (MoneyManager.Instance.GetCurrentGold() < retryCost)
        {
            ShakeCurrentGold();
            return;
        }

        HidePanel();
        MoneyManager.Instance.SpendGold(retryCost);
    }

    private void ShakeCurrentGold()
    {
        if (currentGold == null)
        {
            Debug.LogWarning("currentGold est null !");
            return;
        }

        currentGold.rectTransform.DOKill();
        currentGold.DOKill();

        currentGold.rectTransform.DOShakeAnchorPos(
            duration: 0.4f,
            strength: new Vector2(20f, 5f),
            vibrato: 10,
            randomness: 90,
            snapping: false,
            fadeOut: true
        )
        .SetUpdate(true)
        .OnComplete(() => {
            if (currentGold != null)
                currentGold.rectTransform.anchoredPosition = currentGoldOriginalPosition;
        });

        Color flashColor = Color.red;

        currentGold.DOColor(flashColor, 0.2f)
            .SetLoops(2, LoopType.Yoyo)
            .SetUpdate(true)
            .OnComplete(() => {
                if (currentGold != null)
                    currentGold.color = currentGoldOriginalColor;
            });
    }
}
