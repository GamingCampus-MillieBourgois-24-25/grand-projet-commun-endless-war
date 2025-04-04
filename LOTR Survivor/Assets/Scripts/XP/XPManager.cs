using UnityEngine;

public class XPManager : MonoBehaviour
{
    public static XPManager Instance;

    [Header("Parameters")]
    [SerializeField] XPBarCanvas xPBarCanvas;
    [SerializeField] private int maxXP = 10;

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

        currentXP = Mathf.Clamp(currentXP, 0, maxXP);

        xPBarCanvas.UpdateXP(currentXP);

        Debug.Log("XP Actuelle : " + currentXP);
        // Tu peux ici mettre à jour une jauge UI par exemple
    }
}
