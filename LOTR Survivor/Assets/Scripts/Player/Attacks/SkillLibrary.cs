using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillLibrary : MonoBehaviour
{
    public static SkillLibrary Instance { get; private set; }

    [SerializeField]
    private SkillSettings[] attackSkillSettings;

    [SerializeField]
    private SkillSettings[] buffSkillSettings;

    [SerializeField]
    private SkillSettings[] startingSkills;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public SkillSettings GetRandomSkill()
    {
        if (attackSkillSettings != null && attackSkillSettings.Length > 0)
        {
            List<SkillSettings> availableSkills = new List<SkillSettings>();
            foreach (var skill in attackSkillSettings)
            {
                if (!skill.IsMaxLevel())
                {
                    availableSkills.Add(skill);
                }
            }

            if (availableSkills.Count > 0)
            {
                int randomIndex = Random.Range(0, availableSkills.Count);
                return availableSkills[randomIndex];
            }
            else
            {
                Debug.LogWarning("Toutes les compétences ont atteint leur niveau max.");
                return GetRandomBuffSkill();
            }
        }
        else
        {
            Debug.LogWarning("Skill settings array is empty or null.");
            return null;
        }
    }

    public SkillSettings GetRandomBuffSkill()
    {
        if (buffSkillSettings != null && buffSkillSettings.Length > 0)
        {
            int randomIndex = Random.Range(0, buffSkillSettings.Length);
            return buffSkillSettings[randomIndex];
        }
        else
        {
            Debug.LogWarning("Skill settings array is empty or null.");
            return null;
        }
    }

    public SkillSettings GetStartingSkill()
    {
        if (startingSkills != null && startingSkills.Length > 0)
        {
            List<SkillSettings> availableStartingSkills = new List<SkillSettings>();
            foreach (var skill in startingSkills)
            {
                if (!skill.IsMaxLevel())
                {
                    availableStartingSkills.Add(skill);
                }
            }

            if (availableStartingSkills.Count > 0)
            {
                int randomIndex = Random.Range(0, availableStartingSkills.Count);
                return availableStartingSkills[randomIndex];
            }
            else
            {
                Debug.LogWarning("Toutes les compétences de démarrage ont atteint leur niveau max.");
                return GetRandomBuffSkill();
            }
        }
        else
        {
            Debug.LogWarning("Skill settings array is empty or null.");
            return null;
        }
    }

    public void InitializeSkillSettings(SkillSettings[] newSkillSettings)
    {
        attackSkillSettings = newSkillSettings;
    }
}
