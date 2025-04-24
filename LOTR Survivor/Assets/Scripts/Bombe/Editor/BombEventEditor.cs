using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BombEvent))]
public class BombEventEditor : Editor
{
    private float radius = 5f;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        BombEvent bombEvent = (BombEvent)target;

        GUILayout.Space(10);
        radius = EditorGUILayout.FloatField("Rayon d'effet", radius);

        if(GUILayout.Button("Kill Enemies In Range (Around Player)"))
        {
            GameObject player = GameObject.FindWithTag("Player");
            if(player != null)
            {
                bombEvent.KillEnemiesInRange(player.transform.position, radius);
            }
            else
            {
                Debug.LogWarning("Aucun objet avec le tag 'Player' trouvé !");
            }
        }

        /*if(GUILayout.Button("KIll All Enemies"))
        {
            bombEvent.KillAllEnemies();
        }

        if(GUILayout.Button("Kill All Visible Enemies"))
        {
            bombEvent.KillAllVisibleEnemies();
        }*/
    }
}
