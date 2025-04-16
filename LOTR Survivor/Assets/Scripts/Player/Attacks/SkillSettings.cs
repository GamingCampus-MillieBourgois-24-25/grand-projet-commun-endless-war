using UnityEngine;

[CreateAssetMenu(fileName = "NewAttackSkill", menuName = "Attack/Skill Settings")]
public class SkillSettings : ScriptableObject
{
    [Header("Skill Details")]
    public string skillName;
    public Sprite skillSprite;
    public SkillType skillType;

    [TextArea(3, 10)]
    public string skillDescription;

    [Header("Logic Prefab")]
    public GameObject skillBehaviour;

    [Header("Attack Settings")]
    public AttackSettings attackSettings;
}

public enum SkillType
{
    Normal,
    Starting,
    Buff,
}