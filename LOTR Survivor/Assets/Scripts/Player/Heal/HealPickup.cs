using UnityEngine;
using FMODUnity;

public class HealPickup : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject prefab;
    [SerializeField] private EventReference pickupEvent;

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
            picked = true;

            PlayerHealthBehaviour playerHealth = other.GetComponent<PlayerHealthBehaviour>();
            if (playerHealth != null)
            {
                playerHealth.Heal(healAmount);

                //if (!pickupEvent.IsNull)
                //    OneShotAudio.Play(pickupEvent, transform.position);

                if (animator != null)
                    animator.SetTrigger("Picked");
                else
                    Delete();
            }
        }
    }

    private void Delete()
    {
        Debug.Log("debug");
        if (ObjectPool.Instance != null)
            ObjectPool.Instance.Despawn(gameObject, prefab);
        else
            Destroy(gameObject);
    }
}