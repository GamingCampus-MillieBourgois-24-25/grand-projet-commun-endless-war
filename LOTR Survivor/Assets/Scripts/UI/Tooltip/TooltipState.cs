using System.Collections.Generic;

[System.Serializable]
public class TooltipEntry
{
    public int tooltipID;
    public bool seen;
}

public class TooltipState
{
    private static TooltipState _instance;
    public static TooltipState Instance
    {
        get
        {
            _instance ??= new TooltipState();
            return _instance;
        }
    }

    private List<TooltipEntry> tooltipEntries = new();
    private Dictionary<int, bool> seenTooltips = new();

    private TooltipState() { }

    public void Initialize(List<TooltipEntry> initialEntries)
    {
        tooltipEntries = new List<TooltipEntry>(initialEntries);
        seenTooltips.Clear();
        foreach (var entry in tooltipEntries)
        {
            seenTooltips[entry.tooltipID] = entry.seen;
        }
    }

    public bool HasSeenTooltip(int tooltipID)
    {
        return seenTooltips.TryGetValue(tooltipID, out bool seen) && seen;
    }

    public void MarkTooltipAsSeen(int tooltipID)
    {
        seenTooltips[tooltipID] = true;
        UpdateTooltipList();

        SaveLoadManager.SaveTooltipState(ToList());
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

    public List<TooltipEntry> ToList()
    {   
        return new List<TooltipEntry>(tooltipEntries);
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
