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
    private SkillSettings startingSkill;

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
            int randomIndex = Random.Range(0, attackSkillSettings.Length);
            return attackSkillSettings[randomIndex];
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
        return startingSkill;
    }

    public void InitializeSkillSettings(SkillSettings[] newSkillSettings)
    {
        attackSkillSettings = newSkillSettings;
    }
}
