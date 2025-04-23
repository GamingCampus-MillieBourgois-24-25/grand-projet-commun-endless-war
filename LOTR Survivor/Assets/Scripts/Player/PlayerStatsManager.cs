using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsManager : MonoBehaviour
{
    public static PlayerStatsManager Instance;

    public static event Action<SkillNameType, float> OnStatChanged;
    public float HealthBoost { get; private set; }
    public float AttackBoost { get; private set; }
    public float ShotSpeedBoost { get; private set; }
    public float RateBoost { get; private set; }
    public float XPBoost { get; private set; }
    public float RangeBoost { get; private set; }
    public float SpeedBoost { get; private set; }
    public float RegenBoost { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        LoadStats();
    }

    private string statsSavePath => Application.persistentDataPath + "/playerStats.json";

    public void SaveStats()
    {
        PlayerStatsSaveData data = new PlayerStatsSaveData
        {
            healthBoost = HealthBoost,
            attackBoost = AttackBoost,
            shotSpeedBoost = ShotSpeedBoost,
            rateBoost = RateBoost,
            xpBoost = XPBoost,
            rangeBoost = RangeBoost,
            speedBoost = SpeedBoost,
            regenBoost = RegenBoost,
        };

        string json = JsonUtility.ToJson(data, true);
        System.IO.File.WriteAllText(statsSavePath, json);
    }

    public void LoadStats()
    {
        if (!System.IO.File.Exists(statsSavePath))
            return;

        string json = System.IO.File.ReadAllText(statsSavePath);
        PlayerStatsSaveData data = JsonUtility.FromJson<PlayerStatsSaveData>(json);

        HealthBoost = data.healthBoost;
        AttackBoost = data.attackBoost;
        ShotSpeedBoost = data.shotSpeedBoost;
        RateBoost = data.rateBoost;
        XPBoost = data.xpBoost;
        RangeBoost = data.rangeBoost;
        SpeedBoost = data.speedBoost;
        RegenBoost = data.regenBoost;

        OnStatChanged?.Invoke(SkillNameType.Health, HealthBoost);
        OnStatChanged?.Invoke(SkillNameType.Damage, AttackBoost);
        OnStatChanged?.Invoke(SkillNameType.ShotSpeed, ShotSpeedBoost);
        OnStatChanged?.Invoke(SkillNameType.Rate, RateBoost);
        OnStatChanged?.Invoke(SkillNameType.XP, XPBoost);
        OnStatChanged?.Invoke(SkillNameType.Range, RangeBoost);
        OnStatChanged?.Invoke(SkillNameType.Speed, SpeedBoost);

        Debug.Log(HealthBoost);
    }

    public void Apply(SkillSlot slot)
    {
        SkillSO skill = slot.skillSO;
        float newValue = skill.value;

        Debug.Log(newValue);

        switch (skill.skillNameType)
        {
            case SkillNameType.Health:
                HealthBoost = Mathf.Max(HealthBoost, newValue);
                break;
            case SkillNameType.Damage:
                AttackBoost = Mathf.Max(AttackBoost, newValue);
                break;
            case SkillNameType.ShotSpeed:
                ShotSpeedBoost = Mathf.Max(ShotSpeedBoost, newValue);
                break;
            case SkillNameType.Rate:
                RateBoost = Mathf.Max(RateBoost, newValue);
                break;
            case SkillNameType.XP:
                XPBoost = Mathf.Max(XPBoost, newValue);
                break;
            case SkillNameType.Range:
                RangeBoost = Mathf.Max(RangeBoost, newValue);
                break;
            case SkillNameType.Speed:
                SpeedBoost = Mathf.Max(SpeedBoost, newValue);
                break;
            case SkillNameType.Regen:
                RegenBoost = Mathf.Max(RegenBoost, newValue);
                break;
            case SkillNameType.Skill:
                Debug.Log("Skill spécial appliqué : " + skill.skillText);
                break;
            default:
                Debug.LogWarning("Skill non reconnu : " + skill.skillNameType);
                break;
        }

        OnStatChanged?.Invoke(skill.skillNameType, newValue);
    }
    public void RecalculateAllStats(IEnumerable<SkillSlot> allSlots)
    {
        HealthBoost = 0f;
        AttackBoost = 0f;
        ShotSpeedBoost = 0f;
        RateBoost = 0f;
        XPBoost = 0f;
        RangeBoost = 0f;
        SpeedBoost = 0f;
        RegenBoost = 0f;

        foreach (SkillSlot slot in allSlots)
        {
            if (slot.skillSlotState == SkillSlotState.Acquired)
            {
                Apply(slot);
            }
        }
    }
}

[Serializable]
public class PlayerStatsSaveData
{
    public float healthBoost;
    public float attackBoost;
    public float shotSpeedBoost;
    public float rateBoost;
    public float xpBoost;
    public float rangeBoost;
    public float speedBoost;
    public float regenBoost;
}
