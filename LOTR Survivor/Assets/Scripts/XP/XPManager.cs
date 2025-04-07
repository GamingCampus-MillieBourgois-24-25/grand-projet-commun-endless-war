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
        currentXP += amount;

        if (currentXP >= maxXP)
        {
            currentXP = maxXP;
            TriggerLevelUp();
        }

        xPBarCanvas.UpdateXP(currentXP);
    }

    public void OnLevelUpBuffSelected()
    {

        // Simule un choix de buff : +1 à une stat bidon
        Debug.Log("Buff choisi : +1 à BidonStat");

        CurrentLevel++;
        currentXP = 0;
        xPBarCanvas.UpdateXP(currentXP);

        Debug.Log("Niveau actuel : " + CurrentLevel);
    }

}
