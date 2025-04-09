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
            Debug.LogWarning("Les donn�es de comp�tence ou le prefab sont manquants.");
            return;
        }

        var existingSkill = GetSkillByName(skillData.skillName);
        if (existingSkill != null)
        {
            Debug.Log($"Skill '{skillData.skillName}' d�j� existant, upgrade...");
            existingSkill.Behaviour.Upgrade();
            return;
        }

        GameObject skillInstance = Instantiate(skillData.skillBehaviour, transform);
        AttackBehaviour skillBehaviour = skillInstance.GetComponent<AttackBehaviour>();

        if (skillBehaviour != null)
        {
            skillBehaviour.SetAttackSettings(skillData.attackSettings);
            activeSkills.Add(new ActiveSkill(skillData, skillBehaviour));
            Debug.Log($"Skill '{skillData.skillName}' ajout� avec succ�s.");
        }
        else
        {
            Debug.LogWarning("Le prefab de comp�tence n'a pas de script AttackBehaviour.");
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
            Debug.Log($"Skill '{skillName}' upgrad� !");
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
            Debug.LogWarning("Aucune comp�tence trouv�e correspondant aux param�tres fournis.");
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

