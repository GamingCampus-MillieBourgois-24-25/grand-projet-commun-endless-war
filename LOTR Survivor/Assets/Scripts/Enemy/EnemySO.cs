using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemy", menuName = "Enemies/New Enemy")]
public class EnemySO : ScriptableObject
{
    [Header("Health")]
    [Tooltip("Points de vie maximum de l'ennemi.")]
    public int maxHealth = 100;

    [Header("Attack")]
    [Tooltip("Puissance d'attaque de l'ennemi.")]
    public int attackPower = 10;

    [Tooltip("Temps d'attente entre deux attaques.")]
    public float attackCooldown = 1.5f;

    [Header("Movement")]
    [Tooltip("Vitesse de déplacement de l'ennemi.")]
    public float speed = 3f;

    [Header("Behavior")]
    [Tooltip("Rayon d'agressivité. Si le joueur entre dans cette zone, l'ennemi commence à attaquer.")]
    public float aggroRange = 10f;

    [Tooltip("Préfabriqué de l'ennemi.")]
    public GameObject prefab;

    [Tooltip("Durée du flash lorsque l'ennemi prend des dégâts.")]
    [Min(0f)]
    public float flashDuration = 0.1f;

    [Tooltip("Couleur de flash lorsqu'il prend des dégâts.")]
    public Color flashColor = Color.red;

    [Tooltip("Description de l'ennemi pour référence dans l'inspecteur.")]
    [TextArea]
    public string description;
}
