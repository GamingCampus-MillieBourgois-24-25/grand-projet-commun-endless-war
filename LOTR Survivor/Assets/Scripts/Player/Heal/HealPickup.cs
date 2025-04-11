using UnityEngine;

public class HealPickup : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject prefab;
    [SerializeField] private AudioClip pickupClip;
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
                TryPlayPickupSound();

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

    private void TryPlayPickupSound()
    {
        if (pickupClip != null)
        {
            float sfxVolume = VolumeManager.Instance.GetSFXVolume();
            OneShotAudio.PlayClip(pickupClip, transform.position, sfxVolume);
        }
    }

    private void Delete()
    {
        if (ObjectPool.Instance != null)
        {
            ObjectPool.Instance.Despawn(gameObject, prefab);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
