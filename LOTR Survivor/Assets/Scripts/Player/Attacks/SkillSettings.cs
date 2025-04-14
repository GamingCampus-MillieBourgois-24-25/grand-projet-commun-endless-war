using UnityEngine;

[CreateAssetMenu(fileName = "NewAttackSkill", menuName = "Attack/Skill Settings")]
public class SkillSettings : ScriptableObject
{
    [Header("Skill Details")]
    public string skillName;
    public Sprite skillSprite;
    public SkillType skillType;
    public string skillDescription;

    [Header("Logic Prefab")]
    public GameObject skillBehaviour;

    [Header("Attack Settings")]
    public AttackSettings attackSettings;

    [Header("Heal/Buff Skill")]
    public float healAmount;
}

public enum SkillType
{
    Attack,
    Starting,
    Buff
}