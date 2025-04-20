    using UnityEngine;

public static class StatusEffectUtils
{
    public static void Apply(StatusEffect effect, GameObject target, GameObject self)
    {
        GameObject targetToAffect = effect.affectSelf ? self : target;
        if (targetToAffect == null || !targetToAffect.activeSelf)
        {
            Debug.LogWarning($"Target {targetToAffect?.name} is either null or inactive. Aborting status effect.");
            return;
        }

        if (!effect.affectSelf)
        {
            var enemyHealth = targetToAffect.GetComponent<EnemyHealthBehaviour>();
            if (enemyHealth != null && enemyHealth.enemyData.statusImmunities.Contains(effect.effectType))
            {
                Debug.Log($"{targetToAffect.name} est immunisé contre {effect.effectType}");
                return;
            }
        }
        
        switch (effect.effectType)
        {
            case EffectType.Heal:
                ApplyHeal(targetToAffect, effect.effectValue);
                break;

            case EffectType.Poison:
                targetToAffect.GetComponent<IPoisonable>()?.ApplyPoison(effect.effectValue, effect.duration);
                break;

            case EffectType.Burn:
                targetToAffect.GetComponent<IBurnable>()?.ApplyBurn(effect.effectValue, effect.duration);
                break;

            case EffectType.Bleed:
                targetToAffect.GetComponent<IBleedable>()?.ApplyBleed(effect.effectValue, effect.duration);
                break;

            case EffectType.Slow:
                targetToAffect.GetComponent<ISlowable>()?.ApplySlow(effect.effectValue, effect.duration);
                break;

            case EffectType.Stun:
                targetToAffect.GetComponent<IStunnable>()?.ApplyStun(effect.duration);
                break;
        }
    }

    private static void ApplyHeal(GameObject target, float amount)
    {
        var health = target.GetComponent<PlayerHealthBehaviour>();
        if (health != null)
        {
            if (amount >= 0)
                health.Heal((int)amount);
            else
                ApplyDamage(target, -amount);
        }
    }

    private static void ApplyDamage(GameObject target, float amount)
    {
        var health = target.GetComponent<PlayerHealthBehaviour>();
        if (health != null)
        {
            int intDamage = Mathf.CeilToInt(amount);
            if (intDamage > 0)
                health.TakeDamage(intDamage, true);
        }
    }
}
