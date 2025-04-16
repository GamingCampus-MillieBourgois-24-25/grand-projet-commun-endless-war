using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuffType
{
    Heal,
    Damage,
    Speed,
    Cooldown,
    Range,
    ProjectileSpeed
}

[CreateAssetMenu(menuName = "Buff/Apply Buff Effect", fileName = "New Buff Effect")]
public class BuffEffect : ScriptableObject
{
    [Header("Buff Parameters")]
    public BuffType buffType;
    public float multiplier;
    public float duration;
}