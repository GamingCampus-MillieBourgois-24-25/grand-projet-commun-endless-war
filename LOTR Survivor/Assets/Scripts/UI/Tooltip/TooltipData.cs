using UnityEngine;

[CreateAssetMenu(fileName = "New Tooltip", menuName = "Tooltip/TooltipData")]
public class TooltipData : ScriptableObject
{
    public int tooltipID;
    public string title;
    [TextArea] public string content;
}
