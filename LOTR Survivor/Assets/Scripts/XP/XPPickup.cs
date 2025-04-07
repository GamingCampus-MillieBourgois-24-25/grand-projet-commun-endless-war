using UnityEngine;

public class XPPickup : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject prefab;
    public int xpValue = 1;
    private bool picked = false;

    public void OnEnable()
    {
        picked = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !picked)
        {
            picked = true;
            XPEvents.PickXP(xpValue);
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
