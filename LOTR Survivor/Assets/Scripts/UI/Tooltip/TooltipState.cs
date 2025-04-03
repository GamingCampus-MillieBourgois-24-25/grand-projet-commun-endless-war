using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class TooltipEntry
{
    public string tooltipID;
    public bool seen;
}

[CreateAssetMenu(fileName = "TooltipState", menuName = "Game/TooltipState")]
public class TooltipState : ScriptableObject
{
    [SerializeField]
    private List<TooltipEntry> seenTooltips = new List<TooltipEntry>();

    public bool HasSeenTooltip(string tooltipID)
    {
        foreach (var entry in seenTooltips)
        {
            if (entry.tooltipID == tooltipID)
            {
                return entry.seen;
            }
        }
        return false;
    }

    public void MarkTooltipAsSeen(string tooltipID)
    {
        var entry = seenTooltips.Find(e => e.tooltipID == tooltipID);
        if (entry == null)
        {
            seenTooltips.Add(new TooltipEntry { tooltipID = tooltipID, seen = true });
        }
        else
        {
            entry.seen = true;
        }
    }

    public void ResetTooltips()
    {
        seenTooltips.Clear();
    }
}
