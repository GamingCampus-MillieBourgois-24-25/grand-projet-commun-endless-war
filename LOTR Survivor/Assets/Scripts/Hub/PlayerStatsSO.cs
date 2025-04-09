using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerStats", menuName = "Player Stats")]
public class PlayerStatsSO : ScriptableObject
{
    [Header("Identité")]
    public string characterName;
    public Sprite imageCharacter;
    public Race race;
    public Classe classe;

    [Header("Stats de Base")]
    public int niveau;
    public int experience;
    public int pointsDeVie;
    public int pointsDeMana;

    [Header("Attributs")]
    public int force;
    public int agilite;
    public int intelligence;
    public int defense;

    [Header("Vitesse")]
    public float vitesseDeDeplacement;
    public float vitesseDAttaque;

    [Header("Ressources")]
    public int golds;
}
