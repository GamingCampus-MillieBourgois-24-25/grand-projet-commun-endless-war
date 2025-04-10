using UnityEngine;

public class XPPickup : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject prefab;
    public int xpValue = 1;

    private bool picked = false;
    private bool isBeingMagnetized = false;
    private Transform target;

    private void OnEnable()
    {
        picked = false;
        isBeingMagnetized = false;
        target = null;
        XPMagnetEvents.OnMagnetTriggered += OnMagnetTriggered;
    }

    private void OnDisable()
    {
        XPMagnetEvents.OnMagnetTriggered -= OnMagnetTriggered;
    }

    private void Update()
    {
        if (isBeingMagnetized && target != null && !picked)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, 10f * Time.deltaTime);
        }
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

    private void OnMagnetTriggered(Vector3 center, float radius)
    {
        if (Vector3.Distance(transform.position, center) <= radius)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                target = player.transform;
                isBeingMagnetized = true;
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
