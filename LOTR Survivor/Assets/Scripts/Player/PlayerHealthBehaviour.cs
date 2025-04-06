using System;
using System.Collections;
using Cinemachine;
using UnityEngine;

public class PlayerHealthBehaviour : MonoBehaviour, IHealth
{
    [Header("Health Parameters")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int health;

    [Header("Invulnerability")]
    [SerializeField] private float invulnerabilityDuration = 2f;

    private bool isInvulnerable;
    private float invulnerabilityTimer;

    public event Action OnPlayerDeath;
    public event Action OnPlayerDamaged;

    public event Action OnInvulnerabilityStart;
    public event Action OnInvulnerabilityEnd;

    public int MaxHealth { get => maxHealth; set => maxHealth = Mathf.Max(1, value); }

    public int Health
    {
        get => health;
        set
        {
            health = Mathf.Clamp(value, 0, MaxHealth);
            HealthEvents.RaiseHealthChanged(health, MaxHealth); 
        }
    }

    private void Awake()
    {

    }

    private void Start()
    {
        Health = MaxHealth;
    }

    private void Update()
    {
        if (isInvulnerable)
            HandleInvulnerability();
    }

    private void StartInvulnerability()
    {
        isInvulnerable = true;
        invulnerabilityTimer = invulnerabilityDuration;
        OnInvulnerabilityStart?.Invoke();
    }

    private void HandleInvulnerability()
    {
        invulnerabilityTimer -= Time.deltaTime;

        if (invulnerabilityTimer <= 0f)
        {
            isInvulnerable = false;
            OnInvulnerabilityEnd?.Invoke();
        }
    }

    public void TakeDamage(int damage)
    {
        if (isInvulnerable) return;

        Health -= damage;

        if (Health <= 0)
        {
            Die();
        }
        else
        {
            OnPlayerDamaged?.Invoke();
            StartInvulnerability();
        }
    }

    private void Die()
    {
        StartCoroutine(SlowmoThenDeath());
    }

    private IEnumerator SlowmoThenDeath()
    {
        Time.timeScale = 0.1f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale; 

        yield return new WaitForSecondsRealtime(1.5f);

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        OnPlayerDeath?.Invoke();

        GetComponent<PlayerInput>().enabled = false;
    }

}
