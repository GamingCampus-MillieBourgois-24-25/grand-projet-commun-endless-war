using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance { get; private set; }

    public XPManager xpManager;

    [Header("Gold Settings")]
    [SerializeField] private int enemiesPerGold = 10;
    private int enemiesKilled = 0;
    private int currentGold = 0;

    private const string GOLD_PREF_KEY = "PlayerGold";

    // Stats de session
    private int sessionEnemiesKilled = 0;
    private int sessionGoldEarned = 0;

    public int SessionEnemiesKilled => sessionEnemiesKilled;
    public int SessionGoldEarned => sessionGoldEarned;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Supprime le duplicata
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Persiste entre les scènes
    }

    private void Start()
    {
        LoadGold();
    }

    public void OnEnemyKilled()
    {
        enemiesKilled++;
        sessionEnemiesKilled++;

        if (enemiesKilled >= enemiesPerGold)
        {
            int goldEarned = xpManager != null ? xpManager.levelCurrent : 1;
            currentGold += goldEarned;
            sessionGoldEarned += goldEarned;

            enemiesKilled = 0;

            Debug.Log($"+{goldEarned} Gold gagné ! Total: {currentGold}");
            SaveGold();
        }
    }

    private void LoadGold()
    {
        currentGold = PlayerPrefs.GetInt(GOLD_PREF_KEY, 0);
    }

    private void SaveGold()
    {
        PlayerPrefs.SetInt(GOLD_PREF_KEY, currentGold);
        PlayerPrefs.Save();
    }

    public int GetCurrentGold()
    {
        return currentGold;
    }

    public void AddGold(int amount)
    {
        currentGold += amount;
        SaveGold();
    }

    public void SpendGold(int amount)
    {
        if (currentGold >= amount)
        {
            currentGold -= amount;
            SaveGold();
        }
        else
        {
            Debug.Log("Pas assez de gold !");
        }
    }
}
