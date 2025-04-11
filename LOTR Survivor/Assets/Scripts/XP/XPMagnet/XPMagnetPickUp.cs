using UnityEngine;

public class XPPickupMagnetPickup : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject prefab;
    [SerializeField] private float magnetRadius = 15f;
    [SerializeField] private AudioClip magnetSound;

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

            XPMagnetEvents.Trigger(transform.position, magnetRadius);

            if (magnetSound != null)
            {
                float sfxVolume = VolumeManager.Instance.GetSFXVolume();
                OneShotAudio.PlayClip(magnetSound, transform.position, sfxVolume);
            }

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

    private void Delete()
    {
        if (ObjectPool.Instance != null)
        {
            ObjectPool.Instance.Despawn(gameObject, prefab);
        }
    }
}
