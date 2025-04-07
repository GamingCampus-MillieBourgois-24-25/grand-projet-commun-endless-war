using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(XPManager))]
public class LevelUpEditor : Editor
{
    private int xpToAdd = 1;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        XPManager xPManager = (XPManager)target;

        if (!Application.isPlaying)
        {
            EditorGUILayout.HelpBox("Ces boutons fonctionnent uniquement en mode Play.", MessageType.Info);
            return;
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("XP Debug", EditorStyles.boldLabel);

        xpToAdd = EditorGUILayout.IntField("XP à ajouter :", xpToAdd);

        if (GUILayout.Button($"Ajouter {xpToAdd} XP"))
        {
            xPManager.AddXP(xpToAdd);
            Debug.Log($"{xpToAdd} XP ajoutés.");
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Level Up Debug", EditorStyles.boldLabel);

        if (GUILayout.Button("Level Up avec UI"))
        {
            xPManager.LevelUp();
            Debug.Log("Level Up déclenché (UI).");
        }

        if (GUILayout.Button("Force Level Up (sans UI)"))
        {
            xPManager.OnLevelUpBuffSelected();
            Debug.Log("Level Up forcé (sans UI).");
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Debug Infos", EditorStyles.boldLabel);

        if (GUILayout.Button("Afficher l'état interne"))
        {
            Debug.Log($"[XPManager DEBUG]\n" +
                      $"Niveau actuel : {GetPrivateField<int>(xPManager, "currentLevel")}\n" +
                      $"XP Actuel : {GetPrivateField<int>(xPManager, "currentXP")} / {GetPrivateField<int>(xPManager, "maxXP")}\n" +
                      $"XP en attente : {GetPrivateField<int>(xPManager, "pendingXp")}\n" +
                      $"LevelingUp en cours : {GetPrivateField<bool>(xPManager, "isLevelingUp")}");
        }
    }

    private T GetPrivateField<T>(object obj, string fieldName)
    {
        var field = obj.GetType().GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        return field != null ? (T)field.GetValue(obj) : default;
    }
}
