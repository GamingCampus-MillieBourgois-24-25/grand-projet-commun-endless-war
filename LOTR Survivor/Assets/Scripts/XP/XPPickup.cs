using UnityEngine;

public class XPPickup : MonoBehaviour
{
    public int xpValue = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            XPEvents.PickXP(xpValue);
            Destroy(gameObject);
        }
    }
}
