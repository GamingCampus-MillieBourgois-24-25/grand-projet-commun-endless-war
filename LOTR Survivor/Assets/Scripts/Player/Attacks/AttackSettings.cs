using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

[CreateAssetMenu(menuName = "Attack/AttackSettings", fileName = "NewAttackSettings")]
public class AttackSettings : ScriptableObject
{
    [Header("Base Stats")]
    [Tooltip("Damage")]
    public int BaseDamage = 10;

    [Tooltip("Speed")]
    public float BaseSpeed = 10f;

    [Tooltip("Cooldown")]
    public float BaseCooldown = 1f;

    [Tooltip("Range")]
    public float BaseRange = 10f;

    [Tooltip("Scale")]
    public float Scale = 1f;

    [Tooltip("Aim Range")]
    public float BaseAimRange = 5f;

    [Tooltip("Max Rotation")]
    public float BaseMaxRotation = 360f;

    [Tooltip("Offset de rotation pour l'effet de coup")]
    public float RotationOffset = 0f;

    [Tooltip("Range")]
    public float WideRange = 10f;

    public StatusEffect[] attackEffects;
    public StatusEffect[] statusEffects;
    public BuffEffect[] buffEffects;

    [Header("Skill Type")]
    public SkillType skillType;

    [Tooltip("Number of Projectiles")]
    public int NumberOfAttacks = 10;

    [Tooltip("Cooldown Between Attacks")]
    public float CooldownBetweenAttacks = 5f;

    [Header("VFX / SFX")]
    public GameObject prefab;

    [Tooltip("Prefab Hit")]
    public GameObject hitPrefab;

    [Tooltip("Spawn Event (FMOD)")]
    public EventReference spawnEvent;

    [Tooltip("Hit Event (FMOD)")]
    public EventReference hitEvent;

    [Header("Upgrade Multipliers")]
    public float DamageUpgrade = 1f;
    public float SpeedUpgrade = 1f;
    public float CooldownUpgrade = 1f;
    public float RangeUpgrade = 1f;
    public float AimRangeUpgrade = 1f;
    public float MaxRotationUpgrade = 1f;
    public float HealthBoost = 1f;

    [Header("Upgrade Limits")]
    public int MaxDamage = 999;
    public float MaxSpeed = 100f;
    public float MinCooldown = 0.1f;
    public float MaxRange = 50f;
    public float MaxAimRange = 30f;
    public float MaxMaxRotation = 360f;

    public int Damage;
    public float Speed;
    public float Cooldown;
    public float Range;
    public float AimRange;
    public float MaxRotation;

    public int GetDamage(int level)
    {
        float value = BaseDamage * Mathf.Pow(DamageUpgrade, level - 1);
        return Mathf.Min(Mathf.RoundToInt(value), MaxDamage);
    }

    public float GetSpeed(int level)
    {
        float value = BaseSpeed * Mathf.Pow(SpeedUpgrade, level - 1);
        return Mathf.Min(value, MaxSpeed);
    }

    public float GetCooldown(int level)
    {
        float value = BaseCooldown * Mathf.Pow(CooldownUpgrade, level - 1);
        return Mathf.Max(value, MinCooldown);
    }

    public float GetRange(int level)
    {
        float value = BaseRange * Mathf.Pow(RangeUpgrade, level - 1);
        return Mathf.Min(value, MaxRange);
    }

    public float GetAimRange(int level)
    {
        float value = BaseAimRange * Mathf.Pow(AimRangeUpgrade, level - 1);
        return Mathf.Min(value, MaxAimRange);
    }

    public float GetMaxRotation(int level)
    {
        float value = BaseMaxRotation * Mathf.Pow(MaxRotationUpgrade, level - 1);
        return Mathf.Min(value, MaxMaxRotation);
    }

    public AttackSettings Upgrade(int level = 1)
    {
        Damage = GetDamage(level);
        Speed = GetSpeed(level);
        Cooldown = GetCooldown(level);
        Range = GetRange(level);
        AimRange = GetAimRange(level);
        MaxRotation = GetMaxRotation(level);

        return this;
    }

    public void Reset()
    {
        Damage = BaseDamage;
        Speed = BaseSpeed;
        Cooldown = BaseCooldown;
        Range = BaseRange;
        AimRange = BaseAimRange;
        MaxRotation = BaseMaxRotation;
    }
}
