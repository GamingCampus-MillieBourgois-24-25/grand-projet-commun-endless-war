using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectType
{
    Poison,
    Slow,
    Heal,
    Stun
}

[CreateAssetMenu(menuName = "StatusEffect/Effect", fileName = "NewStatusEffect")]
public class StatusEffect : ScriptableObject
{
    public EffectType effectType;
    public float effectValue;
    public float duration;
    public bool affectSelf;
}
