using UnityEngine;

public static class SkillFactory
{
    public static ActiveSkill CreateSkill(SkillSettings settings, Transform parent)
    {
        if (settings == null || settings.skillBehaviour == null)
        {
            Debug.LogWarning("SkillSettings or prefab is missing.");
            return null;
        }

        GameObject instance = Object.Instantiate(settings.skillBehaviour, parent);
        AttackBehaviour behaviour = instance.GetComponent<AttackBehaviour>();

        if (behaviour == null)
        {
            Debug.LogWarning($"Prefab {settings.name} does not contain an AttackBehaviour.");
            Object.Destroy(instance);
            return null;
        }

        behaviour.SetAttackSettings(settings.attackSettings);
        return new ActiveSkill(settings, behaviour);
    }
}
