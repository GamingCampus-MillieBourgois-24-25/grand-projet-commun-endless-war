using System;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerStats", menuName = "Player Stats")]
public class PlayerStatsSO : ScriptableObject
{
    [Header("Identité")]
    public string characterName;
    public Sprite imageCharacter;
    public GameObject characterPrefab;
    public Race race;
    public Classe classe;

    [Header("Stats de Base")]
    public int pointsDeVie;

    [Header("Attributs")]
    public SkillSettings[] attacksBase;

    [Header("Vitesse")]
    public float vitesseDeDeplacement;

}
