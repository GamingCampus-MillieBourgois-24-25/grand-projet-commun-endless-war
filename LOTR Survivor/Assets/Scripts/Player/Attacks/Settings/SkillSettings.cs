using FMODUnity;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAttackSkill", menuName = "Attack/Skill Settings")]
public class SkillSettings : ScriptableObject
{
    public int CurrentLevel = 1;

    [Header("Skill Details")]
    public string skillName;
    public Sprite skillSprite;
    public SkillType skillType;

    [TextArea(3, 10)]
    public string skillDescription;

    [Header("Logic Prefab")]
    public GameObject skillBehaviour;

    [Header("Base Stats")]
    public int BaseDamage = 10;
    public float BaseSpeed = 10f;
    public float BaseCooldown = 1f;
    public float BaseRange = 10f;
    public float Scale = 1f;
    public float BaseAimRange = 5f;
    public float BaseMaxRotation = 360f;
    public float RotationOffset = 0f;
    public float WideRange = 10f;

    public StatusEffect[] attackEffects;
    public StatusEffect[] statusEffects;
    public BuffEffect[] buffEffects;

    [Header("Skill Type")]
    public AttackType[] attackTypes;
    public DamageType damageType;

    [Tooltip("Number of Projectiles")]
    public int NumberOfAttacks = 10;

    [Tooltip("Cooldown Between Attacks")]
    public float CooldownBetweenAttacks = 5f;

    [Header("VFX / SFX")]
    public GameObject prefab;
    public GameObject hitPrefab;
    public AudioClip spawnEvent;
    public AudioClip hitEvent;

    [Header("Upgrade Multipliers")]
    public float DamageUpgrade = 1f;
    public float SpeedUpgrade = 1f;
    public float CooldownUpgrade = 1f;
    public float RangeUpgrade = 1f;
    public float AimRangeUpgrade = 1f;
    public float MaxRotationUpgrade = 1f;

    [Header("Upgrade Limits")]
    public int MaxDamage = 999;
    public float MaxSpeed = 100f;
    public float MinCooldown = 1f;
    public float MaxRange = 50f;
    public float MaxAimRange = 30f;
    public float MaxMaxRotation = 360f;

    [Header("Leveling")]
    public int MaxLevel = 10;

    public int Damage;
    public float Speed;
    public float Cooldown;
    public float Range;
    public float AimRange;
    public float MaxRotation;

    public bool acquired = false;
    public SkillSettings Upgrade()
    {
        if (CurrentLevel < MaxLevel)
        {
            CurrentLevel++;
        }

        Damage = Mathf.Min(Mathf.RoundToInt(BaseDamage * Mathf.Pow(DamageUpgrade, CurrentLevel - 1)), MaxDamage);
        Speed = Mathf.Min(BaseSpeed * Mathf.Pow(SpeedUpgrade, CurrentLevel - 1), MaxSpeed);
        Cooldown = Mathf.Max(BaseCooldown * Mathf.Pow(CooldownUpgrade, CurrentLevel - 1), MinCooldown);
        Range = Mathf.Min(BaseRange * Mathf.Pow(RangeUpgrade, CurrentLevel - 1), MaxRange);
        AimRange = Mathf.Min(BaseAimRange * Mathf.Pow(AimRangeUpgrade, CurrentLevel - 1), MaxAimRange);
        MaxRotation = Mathf.Min(BaseMaxRotation * Mathf.Pow(MaxRotationUpgrade, CurrentLevel - 1), MaxMaxRotation);

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
        CurrentLevel = 1;
        acquired = false;
    }

    public bool IsMaxLevel()
    {
        return CurrentLevel >= MaxLevel;
    }
}

public enum SkillType
{
    Normal,
    Starting,
    Buff,
}

public enum AttackType
{
    Slash,
    Projectile,
    Buff
}

public enum DamageType
{
    Magic,
    NoMagic
}
