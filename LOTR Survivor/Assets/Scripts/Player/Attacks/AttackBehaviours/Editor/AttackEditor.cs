using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AttackBehaviour), true)]
public class Attack : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        AttackBehaviour attack = (AttackBehaviour)target;

        if (!Application.isPlaying)
        {
            EditorGUILayout.HelpBox("Ces boutons fonctionnent uniquement en mode Play.", MessageType.Info);
            return;
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Debug", EditorStyles.boldLabel);

        if (GUILayout.Button("Upgrade"))
        {
            attack.Upgrade();
            Debug.Log("Attaque améliorée.");
        }
    }
}
