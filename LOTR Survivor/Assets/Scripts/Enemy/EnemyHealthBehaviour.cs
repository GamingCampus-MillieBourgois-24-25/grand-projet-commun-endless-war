using System;
using System.Collections;
using UnityEngine;

public class EnemyHealthBehaviour : MonoBehaviour, IHealth
{
    [Header("Enemy Data")]
    [SerializeField] public EnemySO enemyData;

    private int health;
    private Renderer objectRenderer;
    private Color originalColor;
    private bool isDead;

    public static event Action<EnemyHealthBehaviour> OnEnemyDied;

    public int MaxHealth
    {
        get => (int)enemyData.maxHealth;
        set { }
    }
    public int Health
    {
        get => health;
        set => health = Mathf.Clamp(value, 0, MaxHealth);
    }

    private void Awake()
    {
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer != null)
            originalColor = objectRenderer.material.color;
    }

    private void Start()
    {
        if (enemyData == null)
        {
            Debug.LogError("Enemy Data non assigné sur " + gameObject.name);
            return;
        }

        Health = MaxHealth;
    }

    public void Initialize(EnemySO enemySO)
    {
        enemyData = enemySO;
        Health = MaxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        Health -= damage;

        if (Health <= 0)
        {
            isDead = true;
            DestroyEnemy();
        }
        else
        {
            StartCoroutine(FlashFeedback());
        }
    }

    private IEnumerator FlashFeedback()
    {
        if (objectRenderer != null)
        {
            objectRenderer.material.color = enemyData.flashColor;
            yield return new WaitForSeconds(enemyData.flashDuration);
            objectRenderer.material.color = originalColor;
        }
    }

    private void DestroyEnemy()
    {
        OnEnemyDied?.Invoke(this);

        if (ObjectPool.Instance != null)
            ObjectPool.Instance.Despawn(gameObject, enemyData.prefab);
        else
            Destroy(gameObject);
    }
}
