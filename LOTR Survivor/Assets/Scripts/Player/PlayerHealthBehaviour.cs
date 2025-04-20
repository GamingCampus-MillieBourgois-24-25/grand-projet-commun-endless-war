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

    [SerializeField] private float regenRate = 0;
    [SerializeField] private float regenTime = 40f;

    private bool isDead = false;

    private bool isInvulnerable;
    private float invulnerabilityTimer;
    private bool canRegen = false;
    private float regenTimer;

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
        maxHealth = Mathf.RoundToInt(maxHealth* (1 + PlayerStatsManager.Instance.HealthBoost / 100));
        Health = MaxHealth;
        regenRate = PlayerStatsManager.Instance.RegenBoost;
        if (regenRate > 0)
        {
            canRegen = true;
        }
    }

    private void Update()
    {
        if (isInvulnerable)
            HandleInvulnerability();
        if (canRegen)
            HandleRegen();
    }

    private void HandleRegen()
    {
        regenTimer += Time.deltaTime;
        if (regenTimer > regenTime)
        {
            Heal(Mathf.RoundToInt((regenRate/100) * maxHealth));
            regenTimer = 0;
        }
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
        TakeDamage(damage, false);
    }

    public void TakeDamage(int damage, bool ignoreInvulnerability)
    {
        if ((isInvulnerable && !ignoreInvulnerability) || isDead) return;

        Health -= damage;

        if (Health <= 0)
        {
            Die();
        }
        else
        {
            HealthEvents.PlayerDamagedEvent(damage);
            if (!ignoreInvulnerability)
                StartInvulnerability();
        }
    }


    public void Die()
    {
        isDead = true;
        LevelUpManager.Instance.enabled = false;
        HealthEvents.PlayerDeathEvent();
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
        LevelUpManager.Instance.enabled = true;
        Heal(Mathf.RoundToInt(maxHealth * amount));
        HealthEvents.Revive(transform);
        StartInvulnerability(true);
    }
}