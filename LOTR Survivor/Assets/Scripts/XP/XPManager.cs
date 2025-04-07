using System.Collections.Generic;
using UnityEngine;

public class XPManager : MonoBehaviour
{
    public static XPManager Instance;

    [Header("Parameters")]
    [SerializeField] private int maxXP = 10;

    public int currentXP = 0;
    private int pendingXp = 0;

    private bool isLevelingUp = false;
    public int CurrentLevel { get; private set; } = 1;

    public int CurrentXP
    {
        get => currentXP;
        set
        {
            currentXP = Mathf.Clamp(value, 0, maxXP);
            XPEvents.RaiseXPChanged(currentXP, maxXP);

            if (currentXP >= maxXP && !isLevelingUp)
            {
                isLevelingUp = true;
                pendingXp += currentXP - maxXP;
                LevelUp();
            }
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void OnEnable()
    {
        XPEvents.OnXPPicked += AddXP;
    }

    private void OnDisable()
    {
        XPEvents.OnXPPicked -= AddXP;
    }

    private void LevelUp()
    {
        Debug.Log("Niveau atteint ! Choix d’un buff...");
        //LevelUpUiManager.Instance.ShowBuffUi();
    }

    public void AddXP(int amount)
    {
        if (isLevelingUp)
        {
            pendingXp += amount;
            return;
        }
        
        CurrentXP += amount;
    }

    public void OnLevelUpBuffSelected()
    {

        // Simule un choix de buff : +1 à une stat bidon
        Debug.Log("Buff choisi : +1 à BidonStat");

        CurrentLevel++;
        maxXP = Mathf.RoundToInt(1.2f * maxXP);
        CurrentXP = 0;
        isLevelingUp = false;

        Debug.Log("Niveau actuel : " + CurrentLevel);

        if (pendingXp > 0)
        {
            int xpToAdd = pendingXp;
            pendingXp = 0;
            AddXP(xpToAdd);
        }
    }
}