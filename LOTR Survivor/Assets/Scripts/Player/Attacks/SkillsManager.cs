using System.Collections.Generic;
using UnityEngine;

public class SkillsManager : MonoBehaviour
{
    [SerializeField] private PlayerHealthBehaviour healthBehaviour;

    [Header("Skills Setup")]
    [SerializeField] private SkillSettings[] startingSkills;

    private readonly List<ActiveSkill> activeSkills = new();

    private void Awake()
    {
        HealthEvents.OnPlayerDeath += DisableAllSkills;
        HealthEvents.OnRevive += EnableAllSkills;
    }

    private void Start()
    {
        foreach (var skillData in startingSkills)
        {
            AddSkill(skillData);
        }
    }

    public void AddSkill(SkillSettings skillData)
    {
        if (!IsValidSkill(skillData))
        {
            Debug.LogWarning("Les données de compétence ou le prefab sont manquants.");
            return;
        }

        var existingSkill = GetSkillByName(skillData.skillName);
        if (existingSkill != null)
        {
            Debug.Log($"Skill '{skillData.skillName}' déjà existant, upgrade...");
            existingSkill.Behaviour.Upgrade();
            return;
        }

        GameObject skillInstance = Instantiate(skillData.skillBehaviour, transform);
        AttackBehaviour skillBehaviour = skillInstance.GetComponent<AttackBehaviour>();

        if (skillBehaviour != null)
        {
            skillBehaviour.SetAttackSettings(skillData.attackSettings);
            activeSkills.Add(new ActiveSkill(skillData, skillBehaviour));
            Debug.Log($"Skill '{skillData.skillName}' ajouté avec succès.");
        }
        else
        {
            Debug.LogWarning("Le prefab de compétence n'a pas de script AttackBehaviour.");
        }
    }

    public ActiveSkill GetSkillByName(string skillName)
    {
        return activeSkills.Find(skill => skill.SkillSettings.skillName == skillName);
    }

    public bool HasSkill(string skillName)
    {
        return GetSkillByName(skillName) != null;
    }

    public void UpgradeSkill(string skillName)
    {
        var skill = GetSkillByName(skillName);
        if (skill != null)
        {
            skill.Behaviour.Upgrade();
            Debug.Log($"Skill '{skillName}' upgradé !");
        }
    }

    public void RemoveSkillBySettings(SkillSettings settings)
    {
        var skill = activeSkills.Find(s => s.SkillSettings == settings);
        if (skill != null)
        {
            activeSkills.Remove(skill);
            skill.Destroy();
        }
        else
        {
            Debug.LogWarning("Aucune compétence trouvée correspondant aux paramètres fournis.");
        }
    }

    public void DisableAllSkills()
    {
        foreach (var skill in activeSkills)
        {
            if (skill?.Behaviour != null)
                skill.Disable();
        }
    }

    public void EnableAllSkills(Transform player)
    {
        foreach (var skill in activeSkills)
        {
            if (skill?.Behaviour != null)
                skill.Enable();
        }
    }

    private bool IsValidSkill(SkillSettings skillData)
    {
        return skillData != null && skillData.skillBehaviour != null;
    }
}


public class ActiveSkill
{
    public SkillSettings SkillSettings { get; }
    public AttackBehaviour Behaviour { get; }

    public ActiveSkill(SkillSettings settings, AttackBehaviour behaviour)
    {
        SkillSettings = settings;
        Behaviour = behaviour;
    }

    public void Enable() => Behaviour.enabled = true;
    public void Disable() => Behaviour.enabled = false;
    public void Destroy() => Object.Destroy(Behaviour.gameObject);
}

