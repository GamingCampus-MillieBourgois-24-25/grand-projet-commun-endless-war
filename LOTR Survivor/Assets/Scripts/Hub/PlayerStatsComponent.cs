using UnityEngine;

public class PlayerStatsComponent : MonoBehaviour
{
    [Header("Donn�es du personnage")]
    public PlayerStatsSO stats;

    void Start()
    {
        if (stats != null)
        {
            Debug.Log($"Le personnage {stats.characterName} est de race {stats.race} et classe {stats.classe}");
        }
        else
        {
            Debug.LogWarning("Aucun ScriptableObject de stats assign� !");
        }
    }

    // Exemple d�acc�s aux stats
    public int GetCurrentHP()
    {
        return stats != null ? stats.pointsDeVie : 0;
    }

    public void TakeDamage(int amount)
    {
        if (stats != null)
        {
            stats.pointsDeVie -= amount;
            Debug.Log($"Le personnage a maintenant {stats.pointsDeVie} PV");
        }
    }
}
