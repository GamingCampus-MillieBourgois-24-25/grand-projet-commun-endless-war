using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerHealthBehaviour))]
public class Health : Editor
{
    private int xpToAdd = 1;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        PlayerHealthBehaviour player = (PlayerHealthBehaviour)target;

        if (!Application.isPlaying)
        {
            EditorGUILayout.HelpBox("Ces boutons fonctionnent uniquement en mode Play.", MessageType.Info);
            return;
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Debug", EditorStyles.boldLabel);

        if (GUILayout.Button("Die"))
        {
            player.Die();
            Debug.Log("Mort déclenchée.");
        }
    }
}
