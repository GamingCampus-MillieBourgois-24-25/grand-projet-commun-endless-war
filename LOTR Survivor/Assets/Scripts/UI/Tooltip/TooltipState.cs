using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TooltipEntry
{
    public int tooltipID;
    public bool seen;
}

[CreateAssetMenu(fileName = "TooltipState", menuName = "Game/TooltipState")]
public class TooltipState : ScriptableObject
{
    [SerializeField]
    private List<TooltipEntry> tooltipEntries = new List<TooltipEntry>();

    private Dictionary<int, bool> seenTooltips = new Dictionary<int, bool>();

    private void OnEnable()
    {
        seenTooltips.Clear();
        foreach (var entry in tooltipEntries)
        {
            seenTooltips[entry.tooltipID] = entry.seen;
        }
    }

    public bool HasSeenTooltip(int tooltipID)
    {
        return seenTooltips.ContainsKey(tooltipID) && seenTooltips[tooltipID];
    }

    public void MarkTooltipAsSeen(int tooltipID)
    {
        if (!seenTooltips.ContainsKey(tooltipID))
        {
            seenTooltips.Add(tooltipID, true);
        }
        else
        {
            seenTooltips[tooltipID] = true;
        }

        UpdateTooltipList();
    }

    public void ResetTooltips()
    {
        seenTooltips.Clear();

        foreach (var entry in tooltipEntries)
        {
            entry.seen = false;
        }

        UpdateTooltipList();
    }

    private void UpdateTooltipList()
    {
        tooltipEntries.Clear();
        foreach (var kvp in seenTooltips)
        {
            tooltipEntries.Add(new TooltipEntry { tooltipID = kvp.Key, seen = kvp.Value });
        }
    }
}
