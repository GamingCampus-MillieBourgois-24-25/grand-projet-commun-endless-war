using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TooltipStateViewer))]
public class TooltipStateViewerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        TooltipStateViewer tooltipStateViewer = (TooltipStateViewer)target;

        if (GUILayout.Button("Reset Tooltips"))
        {
            tooltipStateViewer.ResetTooltips();
        }
    }
}
