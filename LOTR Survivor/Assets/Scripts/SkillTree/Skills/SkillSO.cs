using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSkill", menuName = "SkillTree/Skill")]
public class SkillSO : ScriptableObject
{
    public SkillNameType skillNameType;
    public float value;
    public string skillText;
    public Sprite skillIcon;
    public int skillCost;
    public Color skillColor = Color.white;
}

public enum SkillNameType
{
    Health, // done
    Damage,
    ShotSpeed,
    Rate,
    XP, // done
    Range,
    Speed, // done
    Regen, // done
    Skill
}