using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using System;

public class SkillHolderBehaviour : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    public SkillSettings _skillSettings;

    [SerializeField] Image selectImage;

    public static event Action<SkillHolderBehaviour> OnSkillSelected;

    private void Start()
    {
        Unselect();
    }

    public void UpdateData(SkillSettings skillSettings)
    {
        _skillSettings = skillSettings;
        text.text = skillSettings.skillName;
    }

    public void Select()
    {
        selectImage.enabled = true;
        OnSkillSelected?.Invoke(this);
    }

    public void Unselect()
    {
        selectImage.enabled = false;
    }
}
