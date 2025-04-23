using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthBehaviour : MonoBehaviour
{
    [Header("Enemy Data")]
    [SerializeField] public EnemySO enemyData;

    [Header("XP Pickup")]
    [SerializeField] public GameObject xpPrefab;

    [Header("Health Pickup")]
    [SerializeField] public GameObject healthPrefab;

    [Header("XP Magnet Pickup")]
    [SerializeField] private GameObject xpMagnetPrefab;
    [SerializeField, Range(0f, 1f)] private float xpMagnetDropChance = 0.1f;

    [Header("Visual & Effects")]
    [SerializeField] private Material flashMaterial;
    [SerializeField] private float flashDuration = 0.1f;
    [SerializeField] private Renderer mesh;

    [SerializeField] private GameObject deathEffectSlash;
    [SerializeField] private GameObject deathEffectMagic;

    public int health;
    private Material originalMaterial;

    private static int killCounter = 0;
    private static int killsForHealthPickup = 30;

    public int MaxHealth { get => enemyData != null ? (int)enemyData.maxHealth : 100; set => enemyData.maxHealth = value; }
    public int Health { get => health; set => health = value; }
    public float FlashDuration { get => enemyData != null ? enemyData.flashDuration : 0.1f; set => enemyData.flashDuration = value; }
    public Color FlashColor { get => enemyData != null ? enemyData.flashColor : Color.red; set => enemyData.flashColor = value; }

    private void Awake()
    {
        if (enemyData == null)
        {
            Debug.LogError("Enemy Data non assigné sur " + gameObject.name);
            return;
        }

        if (mesh != null)
        {
            originalMaterial = mesh.material;
        }
    }

    void Start()
    {
        OnHealthInitialized();
    }

    private void OnEnable()
    {
        if (mesh != null)
        {
            mesh.material = originalMaterial;

        }
        OnHealthInitialized();
    }

    public void OnHealthInitialized() 
    { 
        health = MaxHealth;
    }

    public void TakeDamage(int damage, DamageType type = DamageType.Magic)
    {
        Health -= damage;

        if (Health <= 0)
        {
            PlayDeathVFX(type);
            DestroyEnemy();
        }
        else
        {
            StartCoroutine(FlashRed());
        }
    }

    private IEnumerator FlashRed()
    {
        if (mesh != null)
        {
            mesh.material = flashMaterial;
            yield return new WaitForSeconds(flashDuration);
            mesh.material = originalMaterial;
        }
    }

    private void PlayDeathVFX(DamageType type)
    {
        if (deathEffectSlash == null || deathEffectMagic == null)
            return;
        if (type == DamageType.Magic)
        {
            Instantiate(deathEffectMagic, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
        }
        else
        {
            Instantiate(deathEffectSlash, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
        }
    }
    private void DestroyEnemy()
    {
        killCounter++;

        if (xpPrefab != null && ObjectPool.Instance != null)
        {
            ObjectPool.Instance.Spawn(xpPrefab, transform.position, Quaternion.identity);
        }

        if (killCounter % killsForHealthPickup == 0 && healthPrefab != null)
        {
            Instantiate(healthPrefab, transform.position, Quaternion.identity);
        }

        if (xpMagnetPrefab != null && Random.value < xpMagnetDropChance)
        {
            Instantiate(xpMagnetPrefab, transform.position, Quaternion.identity);
        }

        if (ObjectPool.Instance != null)
        {
            ObjectPool.Instance.Despawn(gameObject, enemyData.prefab);
        }
        else
        {
            Debug.Log("ObjectPool Instance is not present in the scene!");
            Destroy(gameObject);
        }
    }
}
