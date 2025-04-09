using UnityEngine;

public class PlayerUpgradeManager : MonoBehaviour
{
    [Header("R�f�rence des stats")]
    public PlayerStatsSO playerStats;

    [Header("Co�t de chaque am�lioration")]
    public int healthUpgradeCost = 100;
    public int damageUpgradeCost = 150;
    public int speedUpgradeCost = 120;

    public void UpgradeHealth()
    {
        if (playerStats.golds >= healthUpgradeCost)
        {
            playerStats.golds -= healthUpgradeCost;
            playerStats.pointsDeVie += 10;
            Debug.Log("PV augment�s !");
        }
        else
        {
            Debug.Log("Pas assez de gold pour am�liorer les PV.");
        }
    }

    public void UpgradeForce()
    {
        if (playerStats.golds >= damageUpgradeCost)
        {
            playerStats.golds -= damageUpgradeCost;
            playerStats.force += 2;
            Debug.Log("Force augment�e !");
        }
        else
        {
            Debug.Log("Pas assez de gold pour am�liorer la Force.");
        }
    }

    public void UpgradeVitesse()
    {
        if (playerStats.golds >= speedUpgradeCost)
        {
            playerStats.golds -= speedUpgradeCost;
            playerStats.vitesseDeDeplacement += 0.2f;
            Debug.Log("Vitesse augment�e !");
        }
        else
        {
            Debug.Log("Pas assez de gold pour am�liorer la Vitesse.");
        }
    }
}
