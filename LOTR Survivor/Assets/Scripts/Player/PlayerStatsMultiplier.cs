using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsMultiplier : MonoBehaviour
{
    public static float damageMultiplier => CalculateTotal(damageBuffs);
    public static float speedMultiplier => CalculateTotal(speedBuffs);
    public static float cooldownMultiplier => CalculateTotal(cooldownBuffs);
    public static float rangeMultiplier => CalculateTotal(rangeBuffs);
    public static float projectileSpeedMultiplier => CalculateTotal(projectileSpeedBuffs);

    private static PlayerStatsMultiplier instance;
    public static bool IsInitialized => instance != null;

    private static List<float> damageBuffs = new List<float>();
    private static List<float> speedBuffs = new List<float>();
    private static List<float> cooldownBuffs = new List<float>();
    private static List<float> rangeBuffs = new List<float>();
    private static List<float> projectileSpeedBuffs = new List<float>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void AddBuff(BuffType buffType, float multiplier)
    {
        if (instance == null)
        {
            Debug.LogWarning("PlayerStatsMultiplier instance is not available!");
            return;
        }

        switch (buffType)
        {
            case BuffType.Damage:
                instance.AddBuffToList(damageBuffs, multiplier);
                break;
            case BuffType.Speed:
                instance.AddBuffToList(speedBuffs, multiplier);
                break;
            case BuffType.Cooldown:
                instance.AddBuffToList(cooldownBuffs, multiplier);
                break;
            case BuffType.Range:
                instance.AddBuffToList(rangeBuffs, multiplier);
                break;
            case BuffType.ProjectileSpeed:
                instance.AddBuffToList(projectileSpeedBuffs, multiplier);
                break;
        }
    }

    private void AddBuffToList(List<float> buffList, float multiplier)
    {
        if (!buffList.Contains(multiplier))
            buffList.Add(multiplier);
    }

    public static void ApplyBuff(BuffEffect buffEffect)
    {
        if (instance == null)
        {
            Debug.LogWarning("PlayerStatsMultiplier instance is not available!");
            return;
        }

        switch (buffEffect.buffType)
        {
            case BuffType.Damage:
                instance.BuffDamage(buffEffect.multiplier, buffEffect.duration);
                break;
            case BuffType.Speed:
                instance.BuffSpeed(buffEffect.multiplier, buffEffect.duration);
                break;
            case BuffType.Cooldown:
                instance.BuffCooldown(buffEffect.multiplier, buffEffect.duration);
                break;
            case BuffType.Range:
                instance.BuffRange(buffEffect.multiplier, buffEffect.duration);
                break;
            case BuffType.ProjectileSpeed:
                instance.BuffProjectileSpeed(buffEffect.multiplier, buffEffect.duration);
                break;
        }
    }

    public void BuffDamage(float multiplier, float duration)
    {
        StartCoroutine(ApplyBuff(damageBuffs, multiplier, duration));
    }

    public void BuffSpeed(float multiplier, float duration)
    {
        StartCoroutine(ApplyBuff(speedBuffs, multiplier, duration));
    }

    public void BuffCooldown(float multiplier, float duration)
    {
        StartCoroutine(ApplyBuff(cooldownBuffs, multiplier, duration));
    }

    public void BuffRange(float multiplier, float duration)
    {
        StartCoroutine(ApplyBuff(rangeBuffs, multiplier, duration));
    }

    public void BuffProjectileSpeed(float multiplier, float duration)
    {
        StartCoroutine(ApplyBuff(projectileSpeedBuffs, multiplier, duration));
    }

    private static IEnumerator ApplyBuff(List<float> buffList, float multiplier, float duration)
    {
        buffList.Add(multiplier);
        yield return new WaitForSeconds(duration);
        buffList.Remove(multiplier);
    }

    private static float CalculateTotal(List<float> buffs)
    {
        float total = 1f;
        foreach (var buff in buffs)
        {
            total *= buff;
        }
        return total;
    }
}
