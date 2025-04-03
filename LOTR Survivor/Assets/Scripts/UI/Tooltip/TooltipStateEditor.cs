using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TooltipState))]
public class TooltipStateEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        TooltipState tooltipState = (TooltipState)target;

        if (GUILayout.Button("Reset Tooltips"))
        {
            tooltipState.ResetTooltips();
            Debug.Log("Tooltips have been reset.");
        }
    }
}
