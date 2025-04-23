using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TooltipTestUI))]
public class TestSave : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        TooltipTestUI script = (TooltipTestUI)target;

        GUILayout.Space(10);
        EditorGUILayout.LabelField("Debug Controls", EditorStyles.boldLabel);

        if (GUILayout.Button("Save and Restart"))
        {
            script.SaveAndRestart();
        }

        if (GUILayout.Button("Reset Tooltips"))
        {
            script.ResetTooltips();
        }
    }
}
