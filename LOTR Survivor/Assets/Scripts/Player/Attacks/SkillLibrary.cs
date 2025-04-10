using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillLibrary : MonoBehaviour
{
    [SerializeField]
    static SkillSettings[] skillSettings;
    static SkillSettings startingSkill;

    public static SkillSettings GetRandomSkill()
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

    public static SkillSettings GetStartingSkill()
    {
        return startingSkill;
    }

    public static void InitializeSkillSettings(SkillSettings[] newSkillSettings)
    {
        skillSettings = newSkillSettings;
    }
}
