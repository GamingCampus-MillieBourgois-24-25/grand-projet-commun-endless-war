using UnityEngine;
using FMODUnity;

public class GoldPickupMagnet : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject prefab;
    [SerializeField] private float magnetRadius = 15f;
    [SerializeField] private EventReference magnetSoundEvent;

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

            GoldMagnetEvents.Trigger(transform.position, magnetRadius);

            if (animator != null)
                animator.SetTrigger("Picked");
            else
                Delete();
        }
    }

    private void Delete()
    {
        if (ObjectPool.Instance != null)
            ObjectPool.Instance.Despawn(gameObject, prefab);
        else
            Destroy(gameObject);
    }
}
