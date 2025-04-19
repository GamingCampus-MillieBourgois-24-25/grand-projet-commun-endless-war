using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSkill", menuName = "SkillTree/Skill")]
public class SkillSO : ScriptableObject
{
    public string skillName;
    public string skillText;
    public Sprite skillIcon;
    public int skillCost;
}
