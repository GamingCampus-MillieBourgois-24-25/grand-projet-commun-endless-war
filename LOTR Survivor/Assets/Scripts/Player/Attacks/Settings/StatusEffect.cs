using UnityEngine;

public enum EffectType
{
    Poison,
    Slow,
    Heal,
    Stun,
    Burn,
    Bleed
}

[CreateAssetMenu(menuName = "StatusEffect/Effect", fileName = "NewStatusEffect")]
public class StatusEffect : ScriptableObject
{
    [Header("General")]
    public EffectType effectType;

    [Tooltip("Valeur de l'effet. Interprétation dépend du type (dps, %, etc.)")]
    public float effectValue;

    [Tooltip("Durée en secondes")]
    public float duration = 1f;

    [Tooltip("Appliquer à soi-même plutôt qu'à la cible ?")]
    public bool affectSelf = false;
}
