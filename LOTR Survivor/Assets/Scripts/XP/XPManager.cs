using System.Collections.Generic;
using UnityEngine;

public class XPManager : MonoBehaviour
{
    public static XPManager Instance;

    [Header("Parameters")]
    [SerializeField] XPBarCanvas xPBarCanvas;
    [SerializeField] private int maxXP = 10;

    public int currentXP = 0;
    public int xpPerPickup = 1;

    private bool isLevelingUp = false;

    private int pendingXp = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public int CurrentLevel { get; private set; } = 1;

    private void Start()
    {
        xPBarCanvas.SetMaxXp(maxXP);
    }

    private void TriggerLevelUp()
    {
        Debug.Log("Niveau atteint ! Choix d’un buff...");
        LevelUpUiManager.Instance.ShowBuffUi();
    }

    public void AddXP(int amount)
    {
        if (isLevelingUp)
        {
            pendingXp += amount;
            return;
        }
        
        currentXP += amount;

        if (currentXP >= maxXP)
        {
            currentXP = maxXP;
            isLevelingUp = true;

            xPBarCanvas.UpdateXP(currentXP, () =>
            {
                TriggerLevelUp();
            });
        }

        else
        {
            xPBarCanvas.UpdateXP(currentXP);
        }
    }

    public void OnLevelUpBuffSelected()
    {

        // Simule un choix de buff : +1 à une stat bidon
        Debug.Log("Buff choisi : +1 à BidonStat");

        currentXP = 0;
        xPBarCanvas.UpdateXP(currentXP);
        CurrentLevel++;

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