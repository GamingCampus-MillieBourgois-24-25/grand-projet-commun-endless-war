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
    [SerializeField] private float blinkSpeed = 0.2f;

    private Renderer objectRenderer;

    private bool isInvulnerable;
    private float invulnerabilityTimer;

    public event Action OnPlayerDeath;
    public event Action OnPlayerDamaged;

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
        objectRenderer = GetComponentInChildren<Renderer>();
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

    private void HandleInvulnerability()
    {
        invulnerabilityTimer -= Time.deltaTime;

        if (invulnerabilityTimer <= 0f)
        {
            isInvulnerable = false;
            objectRenderer.enabled = true;
            return;
        }

        objectRenderer.enabled = Mathf.PingPong(Time.time, 2 * blinkSpeed) > blinkSpeed;
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

    private void StartInvulnerability()
    {
        isInvulnerable = true;
        invulnerabilityTimer = invulnerabilityDuration;
    }

    private void Die()
    {
        OnPlayerDeath?.Invoke();
        GetComponent<PlayerInput>().enabled = false;
    }
}
