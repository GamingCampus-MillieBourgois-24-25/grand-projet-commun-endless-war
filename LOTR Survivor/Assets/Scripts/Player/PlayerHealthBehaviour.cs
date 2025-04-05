using System.Collections;
using Cinemachine;
using UnityEngine;

public class PlayerHealthBehaviour : MonoBehaviour, IHealth
{
    [Header("Health Parameters")]
    [SerializeField] private int maxHealth;
    [SerializeField] private int health;

    [Header("Flash Parameters")]
    [SerializeField] private float flashDuration = 0.1f;
    [SerializeField] private Color flashColor = Color.red;

    [Header("Invulnerability Settings")]
    [SerializeField] private float invulnerabilityDuration = 2f;
    [SerializeField] private float blinkSpeed = 0.2f;

    [Header("Visual & Effects")]
    [SerializeField] private HealthBarCanvas healthBarCanvas;
    [SerializeField] private GameObject deathParticle;

    private Renderer objectRenderer;
    private Color originalColor;
    private Animator animator;
    private CinemachineImpulseSource impulseSource;

    private bool isInvulnerable;
    private float invulnerabilityTimer;

    public int MaxHealth { get => maxHealth; set => maxHealth = value; }
    public int Health { get => health; set => health = value; }

    public float FlashDuration { get => flashDuration; set => flashDuration = value; }
    public Color FlashColor { get => flashColor; set => flashColor = value; }

    private void Start()
    {
        Health = MaxHealth;
        InitializeComponents();
        OnHealthInitialized();
    }

    private void InitializeComponents()
    {
        objectRenderer = GetComponentInChildren<Renderer>();
        animator = GetComponent<Animator>();
        impulseSource = GetComponent<CinemachineImpulseSource>();

        if (objectRenderer != null)
        {
            originalColor = objectRenderer.material.color;
        }
    }

    private void Update()
    {
        if (isInvulnerable)
        {
            HandleInvulnerability();
        }
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

        if (Mathf.PingPong(Time.time, 2 * blinkSpeed) > blinkSpeed)
        {
            objectRenderer.enabled = true;
        }
        else
        {
            objectRenderer.enabled = false;
        }
    }

    public void OnHealthInitialized()
    {
        healthBarCanvas?.UpdateUI(Health, MaxHealth);
    }

    public void TakeDamage(int damage)
    {
        if (isInvulnerable) return;

        Health -= damage;

        CameraShakeManager.instance.CameraShake(impulseSource);
        healthBarCanvas?.UpdateUI(Health, MaxHealth);

        if (Health <= 0)
        {
            PlayDeathAnimation();
        }
        else
        {
            StartCoroutine(FlashRed());
            StartInvulnerability();
        }
    }

    private void StartInvulnerability()
    {
        isInvulnerable = true;
        invulnerabilityTimer = invulnerabilityDuration;
    }

    private IEnumerator FlashRed()
    {
        objectRenderer.material.SetColor("_Color", flashColor);
        objectRenderer.material.SetFloat("_Damaged", 1f);
        yield return new WaitForSeconds(FlashDuration);
        objectRenderer.material.SetFloat("_Damaged", 0f);
        objectRenderer.material.SetColor("_Color", originalColor);
    }



    private void PlayDeathAnimation()
    {
        GamePauseManager.Instance.PauseGame();
        StartCoroutine(WaitAndPlayDeathAnimation());
    }

    private IEnumerator WaitAndPlayDeathAnimation()
    {
        yield return new WaitForSecondsRealtime(1f);
        animator.SetTrigger("Die");
    }

    private void Explode()
    {
        Instantiate(deathParticle, transform.position, Quaternion.identity);
        Destroy(gameObject);
        Debug.LogError("you explod");
    }
}
