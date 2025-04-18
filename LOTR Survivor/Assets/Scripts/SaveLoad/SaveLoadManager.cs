using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveLoadManager : MonoBehaviour
{
    private static string SaveFilePath => Path.Combine(Application.persistentDataPath, "tooltip_state.json");
    
    public static void SaveTooltipState(List<TooltipEntry> tooltipEntries)
    {
        string json = JsonUtility.ToJson(new TooltipDataWrapper(tooltipEntries), true);
        File.WriteAllText(SaveFilePath, json);
        Debug.Log($"Tooltip state save to {SaveFilePath}");
    }

    public static void LoadTooltipState()
    {
        if(File.Exists(SaveFilePath))
        {
            string json = File.ReadAllText(SaveFilePath);
            TooltipDataWrapper wrapper = JsonUtility.FromJson<TooltipDataWrapper>(json);
            TooltipState.Instance.Initialize(wrapper.entries);
            Debug.Log("Tooltip state loaded.");
        }
        else
        {
            Debug.Log("No tooltip state save file found");
        }
    }

    [System.Serializable]
    private class TooltipDataWrapper
    {
        public List<TooltipEntry> entries;

        public TooltipDataWrapper(List<TooltipEntry> entries)
        {
            this.entries = entries;
        }
    }
}
