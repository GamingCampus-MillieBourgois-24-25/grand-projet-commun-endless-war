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
    [Tooltip("Vitesse de d�placement de l'ennemi.")]
    public float speed = 3f;

    [Header("Behavior")]
    [Tooltip("Rayon d'agressivit�. Si le joueur entre dans cette zone, l'ennemi commence � attaquer.")]
    public float aggroRange = 10f;

    [Tooltip("Pr�fabriqu� de l'ennemi.")]
    public GameObject prefab;

    [Tooltip("Dur�e du flash lorsque l'ennemi prend des d�g�ts.")]
    [Min(0f)]
    public float flashDuration = 0.1f;

    [Tooltip("Couleur de flash lorsqu'il prend des d�g�ts.")]
    public Color flashColor = Color.red;

    [Tooltip("Description de l'ennemi pour r�f�rence dans l'inspecteur.")]
    [TextArea]
    public string description;
}
