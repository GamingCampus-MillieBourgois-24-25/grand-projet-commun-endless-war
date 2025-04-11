using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillLibrary : MonoBehaviour
{
    public static SkillLibrary Instance { get; private set; }

    [SerializeField]
    private SkillSettings[] skillSettings;

    private SkillSettings startingSkill;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public SkillSettings GetRandomSkill()
    {
        if (skillSettings != null && skillSettings.Length > 0)
        {
            int randomIndex = Random.Range(0, skillSettings.Length);
            return skillSettings[randomIndex];
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
        skillSettings = newSkillSettings;
    }
}
