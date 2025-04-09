using UnityEngine;

public class PlayerUpgradeManager : MonoBehaviour
{
    [Header("Référence des stats")]
    public PlayerStatsSO playerStats;

    [Header("Coût de chaque amélioration")]
    public int healthUpgradeCost = 100;
    public int damageUpgradeCost = 150;
    public int speedUpgradeCost = 120;

    public void UpgradeHealth()
    {
        if (playerStats.golds >= healthUpgradeCost)
        {
            playerStats.golds -= healthUpgradeCost;
            playerStats.pointsDeVie += 10;
            Debug.Log("PV augmentés !");
        }
        else
        {
            Debug.Log("Pas assez de gold pour améliorer les PV.");
        }
    }

    public void UpgradeForce()
    {
        if (playerStats.golds >= damageUpgradeCost)
        {
            playerStats.golds -= damageUpgradeCost;
            playerStats.force += 2;
            Debug.Log("Force augmentée !");
        }
        else
        {
            Debug.Log("Pas assez de gold pour améliorer la Force.");
        }
    }

    public void UpgradeVitesse()
    {
        if (playerStats.golds >= speedUpgradeCost)
        {
            playerStats.golds -= speedUpgradeCost;
            playerStats.vitesseDeDeplacement += 0.2f;
            Debug.Log("Vitesse augmentée !");
        }
        else
        {
            Debug.Log("Pas assez de gold pour améliorer la Vitesse.");
        }
    }
}
