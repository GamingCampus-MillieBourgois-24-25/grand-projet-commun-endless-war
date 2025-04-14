using UnityEngine;

public static class StatusEffectUtils
{
    public static void Apply(StatusEffect effect, GameObject target, GameObject self)
    {
        GameObject targetToAffect = effect.affectSelf ? self : target;

        if (targetToAffect == null) return;

        switch (effect.effectType)
        {
            case EffectType.Heal:
                var health = targetToAffect.GetComponent<PlayerHealthBehaviour>();
                if (health != null)
                    health.Heal((int)effect.effectValue);
                break;

            //case EffectType.Poison:
            //    var poisonable = targetToAffect.GetComponent<IPoisonable>();
            //    poisonable?.ApplyPoison(effect.effectValue, effect.duration);
            //    break;

            //case EffectType.Slow:
            //    var slowable = targetToAffect.GetComponent<ISlowable>();
            //    slowable?.ApplySlow(effect.effectValue, effect.duration);
            //    break;

            //case EffectType.Stun:
            //    var stunnable = targetToAffect.GetComponent<IStunnable>();
            //    stunnable?.ApplyStun(effect.duration);
            //    break;
        }
    }
}
