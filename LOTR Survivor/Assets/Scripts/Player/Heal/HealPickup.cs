using UnityEngine;

public class HealPickup : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject prefab;
    public int healAmount = 20;
    private bool picked = false;

    private void OnEnable()
    {
        picked = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !picked)
        {
            PlayerHealthBehaviour playerHealth = other.GetComponent<PlayerHealthBehaviour>();
            if (playerHealth != null)
            {
                picked = true;
                playerHealth.Heal(healAmount);

                if (animator != null)
                {
                    animator.SetTrigger("Picked");
                }
                else
                {
                    Delete();
                }
            }
        }
    }

    private void Delete()
    {
        if (ObjectPool.Instance != null)
        {
            ObjectPool.Instance.Despawn(gameObject, prefab);
        }
    }
}