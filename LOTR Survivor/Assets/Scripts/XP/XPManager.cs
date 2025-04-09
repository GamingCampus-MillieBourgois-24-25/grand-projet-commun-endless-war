using System.Buffers.Text;
using System.Collections.Generic;
using UnityEngine;

public class XPManager : MonoBehaviour
{
    public static XPManager Instance;

    [Header("Parameters")]
    private int maxXP = 0;
    private int currentXP = 0;

    [SerializeField] private int baseXP = 10;
    [SerializeField] private float xpGrowthFactor = 1.2f;

    private int pendingXp = 0;

    private bool isLevelingUp = false;
    private int currentLevel = 1;

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

    private void Start()
    {
        maxXP = ComputeMaxXPForLevel(currentLevel);
        XPEvents.RaiseXPChanged(currentXP, maxXP);
    }

    public void LevelUp()
    {
        isLevelingUp = true;
        XPEvents.RaiseLevelChanged(currentLevel);
    }

    public void AddXP(int amount)
    {
        if (isLevelingUp)
        {
            pendingXp += amount;
            return;
        }
        
        currentXP += amount;
        CheckLevelUP();
    }

    private void CheckLevelUP()
    {
        if (currentXP >= maxXP)
        {
            pendingXp += currentXP - maxXP;
            currentXP = maxXP;
            LevelUp();
        }
        XPEvents.RaiseXPChanged(currentXP, maxXP);
    }

    private int ComputeMaxXPForLevel(int level)
    {
        return Mathf.RoundToInt(baseXP * Mathf.Pow(xpGrowthFactor, level - 1));
    }

    public void OnLevelUpBuffSelected()
    {
        currentLevel++;
        maxXP = ComputeMaxXPForLevel(currentLevel);
        currentXP = 0;
        isLevelingUp = false;

        XPEvents.RaiseXPChanged(currentXP, maxXP);
        Debug.Log("Niveau actuel : " + currentLevel);

        if (pendingXp > 0)
        {
            int xpToAdd = pendingXp;
            pendingXp = 0;
            AddXP(xpToAdd);
        }
    }
}