using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillHolderBehaviour : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    private SkillSettings _skillSettings;

    public void UpdateData(SkillSettings skillSettings)
    {
        _skillSettings = skillSettings;
        text.text = skillSettings.skillName;
    }
}
