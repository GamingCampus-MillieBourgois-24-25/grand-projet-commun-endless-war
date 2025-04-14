using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthBehaviour : MonoBehaviour, IHealth
{
    [Header("Health Parameters")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int health;

    [Header("Invulnerability")]
    [SerializeField] private float invulnerabilityDuration = 2f;

    [SerializeField] private float slowmoScale = 0.1f;
    [SerializeField] private float slowmoDuration = 1.5f;

    private bool isDead = false;

    private bool isInvulnerable;
    private float invulnerabilityTimer;

    public event Action OnInvulnerabilityStart;
    public event Action OnInvulnerabilityEnd;

    public int MaxHealth { get => maxHealth;
        set {
            int current = MaxHealth;
            maxHealth = Mathf.Max(1, value);
            HealthEvents.RaiseHealthChanged(health, MaxHealth);
            Health += maxHealth - current;
        }
    }

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

    private void OnEnable()
    {
        XPEvents.OnLevelComplete += HandleLevelComplete;
    }

    private void OnDisable()
    {
        XPEvents.OnLevelComplete -= HandleLevelComplete;
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

    private void HandleLevelComplete()
    {
        StartInvulnerability();
    }

    private void StartInvulnerability(bool revive = false)
    {
        isInvulnerable = true;
        invulnerabilityTimer = invulnerabilityDuration;
        if (revive)
        {
            invulnerabilityTimer *= 4f;
        }
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
        if (isInvulnerable || isDead) return;

        Health -= damage;

        if (Health <= 0)
        {
            Die();
        }
        else
        {
            HealthEvents.PlayerDamagedEvent(damage);
            StartInvulnerability();
        }
    }

    public void TakeDamageNoInvincibility(int damage)
    {
        if (isInvulnerable || isDead) return;

        Health -= damage;

        if (Health <= 0)
        {
            Die();
        }
        else
        {
            HealthEvents.PlayerDamagedEvent(damage);
        }
    }

    public void Die()
    {
        isDead = true;
        StartCoroutine(SlowmoThenDeath());
    }

    private IEnumerator SlowmoThenDeath()
    {
        Time.timeScale = slowmoScale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        yield return new WaitForSecondsRealtime(slowmoDuration);

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        HealthEvents.PlayerDeathEvent();

        GetComponent<PlayerInput>().enabled = false;
    }

    public void Heal(int amount)
    {
        if (isDead) return;
        Health += amount;
        Health = Mathf.Clamp(Health, 0, MaxHealth);
    }

    public void Revive(float amount)
    {
        isDead = false;
        Heal(Mathf.RoundToInt(maxHealth * amount));
        HealthEvents.Revive(transform);
        StartInvulnerability(true);
    }
}