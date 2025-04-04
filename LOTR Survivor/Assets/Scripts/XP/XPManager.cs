using UnityEngine;

public class XPManager : MonoBehaviour
{
    public static XPManager Instance;

    public int currentXP = 0;
    public int xpPerPickup = 1;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void AddXP(int amount)
    {
        currentXP += amount;
        Debug.Log("XP Actuelle : " + currentXP);
        // Tu peux ici mettre à jour une jauge UI par exemple
    }
}
