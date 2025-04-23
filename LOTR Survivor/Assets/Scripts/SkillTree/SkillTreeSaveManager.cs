using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class SkillTreeSaveManager
{
    private string saveFilePath;

    public SkillTreeSaveManager()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "skillTreeSave.json");
    }

    public void SaveSkillTree(SkillSlot[] skillSlots)
    {
        SkillTreeSaveData saveData = new SkillTreeSaveData();

        foreach (SkillSlot slot in skillSlots)
        {
            SkillSlotData data = new SkillSlotData
            {
                skillName = slot.skillSO.name,
                skillSlotState = slot.skillSlotState
            };
            saveData.skillSlotsData.Add(data);
        }

        string json = JsonConvert.SerializeObject(saveData, Formatting.Indented);
        File.WriteAllText(saveFilePath, json);
    }

    public void LoadSkillTree(SkillSlot[] skillSlots)
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            SkillTreeSaveData saveData = JsonConvert.DeserializeObject<SkillTreeSaveData>(json);

            foreach (SkillSlot slot in skillSlots)
            {
                SkillSlotData savedData = saveData.skillSlotsData.Find(x => x.skillName == slot.skillSO.name);
                if (savedData != null)
                {
                    slot.skillSlotState = savedData.skillSlotState;
                    slot.UpdateUI();
                    slot.UpdateLinks();
                }
            }
        }
    }
}

[System.Serializable]
public class SkillTreeSaveData
{
    public List<SkillSlotData> skillSlotsData = new List<SkillSlotData>();
}

[System.Serializable]
public class SkillSlotData
{
    public string skillName;
    public SkillSlotState skillSlotState;
}
