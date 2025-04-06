using UnityEngine;

[CreateAssetMenu(fileName = "NewAttackSkill", menuName = "Attack/Skill Settings")]
public class SkillSettings : ScriptableObject
{
    [Header("Skill Details")]
    public string skillName;
    public GameObject skillBehaviour;

    [Header("Attack Settings")]
    public AttackSettings attackSettings;
}
