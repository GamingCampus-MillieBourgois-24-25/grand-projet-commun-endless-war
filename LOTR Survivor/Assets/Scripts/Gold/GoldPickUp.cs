using UnityEngine;

public class GoldPickup : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject prefab;
    [SerializeField] private SphereCollider sphereCollider;
    public int goldValue = 1;

    private bool picked = false;
    private bool isBeingMagnetized = false;
    private Transform target;

    private void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();
    }

    private void OnEnable()
    {
        picked = false;
        isBeingMagnetized = false;
        target = null;
        GoldMagnetEvents.OnMagnetTriggered += OnMagnetTriggered;
    }

    private void OnDisable()
    {
        GoldMagnetEvents.OnMagnetTriggered -= OnMagnetTriggered;
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
            MoneyManager.Instance.AddGold(goldValue); // Ajout et sauvegarde

            if (animator != null)
                animator.SetTrigger("Picked");
            else
                Delete();
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
            ObjectPool.Instance.Despawn(gameObject, prefab);
    }
}
