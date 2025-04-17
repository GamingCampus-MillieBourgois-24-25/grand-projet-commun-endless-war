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

    [Tooltip("Valeur de l'effet. Interpr�tation d�pend du type (dps, %, etc.)")]
    public float effectValue;

    [Tooltip("Dur�e en secondes")]
    public float duration = 1f;

    [Tooltip("Appliquer � soi-m�me plut�t qu'� la cible ?")]
    public bool affectSelf = false;
}
