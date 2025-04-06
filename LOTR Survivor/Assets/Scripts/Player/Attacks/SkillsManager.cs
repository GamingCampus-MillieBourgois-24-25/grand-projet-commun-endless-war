using System.Collections.Generic;
using UnityEngine;

public class SkillsManager : MonoBehaviour
{
    [SerializeField] PlayerHealthBehaviour healthBehaviour;

    [Header("Skills Setup")]
    [SerializeField] private SkillSettings[] startingSkills;

    private List<ActiveSkill> activeSkills = new List<ActiveSkill>();

    private void Awake()
    {
        if (healthBehaviour != null)
        {
            healthBehaviour.OnPlayerDeath += DisableAllSkills;
        }
    }

    void Start()
    {
        foreach (var skillData in startingSkills)
        {
            AddSkill(skillData);
        }
    }

    public void AddSkill(SkillSettings skillData)
    {
        if (skillData != null && skillData.skillBehaviour != null)
        {
            GameObject skillInstance = Instantiate(skillData.skillBehaviour, transform);

            AttackBehaviour skillBehaviour = skillInstance.GetComponent<AttackBehaviour>();
            if (skillBehaviour != null)
            {
                activeSkills.Add(new ActiveSkill(skillData, skillBehaviour));
                skillBehaviour.SetAttackSettings(skillData.attackSettings);
            }
            else
            {
                Debug.LogWarning("Le prefab de compétence n'a pas de script AttackBehaviour.");
            }
        }
        else
        {
            Debug.LogWarning("Les données de compétence ou le prefab sont manquants.");
        }
    }

    public void RemoveSkillBySettings(SkillSettings skillSettings)
    {
        ActiveSkill skillToRemove = activeSkills.Find(skill => skill.skillSettings == skillSettings);

        if (skillToRemove != null)
        {
            activeSkills.Remove(skillToRemove);
            Destroy(skillToRemove.attackBehaviour.gameObject);
        }
        else
        {
            Debug.LogWarning("Aucune compétence trouvée avec ces SkillSettings.");
        }
    }

    public void DisableAllSkills()
    {
        foreach (var skill in activeSkills)
        {
            skill.attackBehaviour.enabled = false;
        }
    }

    public void EnableAllSkills()
    {
        foreach (var skill in activeSkills)
        {
            skill.attackBehaviour.enabled = true;
        }
    }
}

public class ActiveSkill
{
    public SkillSettings skillSettings;
    public AttackBehaviour attackBehaviour;

    public ActiveSkill(SkillSettings skillSettings, AttackBehaviour attackBehaviour)
    {
        this.skillSettings = skillSettings;
        this.attackBehaviour = attackBehaviour;
    }
}
