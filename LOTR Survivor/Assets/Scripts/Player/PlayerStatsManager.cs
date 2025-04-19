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

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ApplySkill(SkillSO skill)
    {
        switch (skill.skillNameType)
        {
            case SkillNameType.Health:
                HealthBoost = Mathf.Max(HealthBoost, skill.value);
                break;
            case SkillNameType.Damage:
                AttackBoost = Mathf.Max(AttackBoost, skill.value);
                break;
            case SkillNameType.ShotSpeed:
                ShotSpeedBoost = Mathf.Max(ShotSpeedBoost, skill.value);
                break;
            case SkillNameType.Rate:
                RateBoost = Mathf.Max(RateBoost, skill.value);
                break;
            case SkillNameType.XP:
                XPBoost = Mathf.Max(XPBoost, skill.value);
                break;
            case SkillNameType.Range:
                RangeBoost = Mathf.Max(RangeBoost, skill.value);
                break;
            case SkillNameType.Speed:
                SpeedBoost = Mathf.Max(SpeedBoost, skill.value);
                break;
            case SkillNameType.Skill:
                break;
        }

        OnStatChanged?.Invoke(skill.skillNameType, skill.value);
    }

    public void LoadStatsFromSave(List<SkillSO> skills)
    {
        foreach (var skill in skills)
        {
            ApplySkill(skill);
        }
    }
}
