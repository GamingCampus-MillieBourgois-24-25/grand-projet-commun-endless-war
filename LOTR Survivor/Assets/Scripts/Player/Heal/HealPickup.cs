using UnityEngine;

public class HealPickup : MonoBehaviour
{
    public int healAmount = 20;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealthBehaviour playerHealth = other.GetComponent<PlayerHealthBehaviour>();
            if (playerHealth != null)
            {
                playerHealth.Heal(healAmount);
                Destroy(gameObject);
            }
        }
    }
}