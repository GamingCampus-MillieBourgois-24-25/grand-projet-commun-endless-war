using UnityEngine;
using System.Collections;
using Cinemachine;

public class PlayerAnimation : MonoBehaviour
{
    [Header("Animation References")]
    [SerializeField] private Animator playerAnimator;

    [Header("Visual & Effects")]
    [SerializeField] private Material flashMaterial;
    [SerializeField] private float flashDuration = 0.1f;
    [SerializeField] private Color flashColor = Color.red;
    [SerializeField] private GameObject deathParticle;
    [SerializeField] private GameObject reviveParticle;
    [SerializeField] private float blinkSpeed = 0.1f;

    private PlayerHealthBehaviour playerHealth;
    private bool isReviving = false;

    private Renderer objectRenderer;
    private CinemachineImpulseSource impulseSource;
    private Material originalMaterial;
    private Color originalColor;
    private bool isInvulnerable;

    private void Awake()
    {
        playerHealth = GetComponent<PlayerHealthBehaviour>();
        objectRenderer = GetComponentInChildren<Renderer>();
        impulseSource = GetComponent<CinemachineImpulseSource>();

        if (objectRenderer != null)
        {
            originalColor = objectRenderer.material.color;
            originalMaterial = objectRenderer.material;
        }
    }

    private void OnEnable()
    {
        if (playerHealth != null)
        {
            HealthEvents.OnPlayerDeath += HandlePlayerDeath;
            HealthEvents.OnPlayerDamaged += HandleDamageAnimations;
            HealthEvents.OnRevive += HandlePlayerRevive;

            playerHealth.OnInvulnerabilityStart += OnInvulnerabilityStart;
            playerHealth.OnInvulnerabilityEnd += OnInvulnerabilityEnd;
        }
    }

    private void OnDisable()
    {
        if (playerHealth != null)
        {
            HealthEvents.OnPlayerDeath -= HandlePlayerDeath;
            HealthEvents.OnPlayerDamaged -= HandleDamageAnimations;
            HealthEvents.OnRevive -= HandlePlayerRevive;

            playerHealth.OnInvulnerabilityStart -= OnInvulnerabilityStart;
            playerHealth.OnInvulnerabilityEnd -= OnInvulnerabilityEnd;
        }
    }

    private void OnInvulnerabilityStart()
    {
        isInvulnerable = true;
    }

    private void OnInvulnerabilityEnd()
    {
        isInvulnerable = false;
        if (objectRenderer != null)
            objectRenderer.enabled = true;
    }

    private void Update()
    {
        if (playerHealth.Health > 0)
        {
            HandleMovementAnimations();
        }

        if (isInvulnerable && objectRenderer != null && !isReviving)
        {
            objectRenderer.enabled = Mathf.PingPong(Time.time, 2 * blinkSpeed) > blinkSpeed;
        }
    }

    private void HandleMovementAnimations()
    {
        
    }

    private void HandleDamageAnimations(int amount)
    {
        StartCoroutine(FlashFeedback());
        if (CameraShakeManager.instance != null)
        {
            CameraShakeManager.instance.CameraShake(impulseSource);
        }
    }

    private void HandlePlayerDeath()
    {
        StartCoroutine(PlayDeathAnimation());
    }

    private void HandlePlayerRevive(Transform player)
    {
        StartCoroutine(PlayReviveAnimation());
    }

    private IEnumerator FlashFeedback()
    {
        if (objectRenderer != null)
        {
            objectRenderer.material = flashMaterial;
            yield return new WaitForSeconds(flashDuration);
            objectRenderer.material = originalMaterial;
        }
    }

    private IEnumerator PlayDeathAnimation()
    {
        yield return new WaitForSecondsRealtime(1f);
        playerAnimator.SetTrigger("Die");
    }

    private IEnumerator PlayReviveAnimation()
    {
        isReviving = true;
        Instantiate(reviveParticle, transform.position, Quaternion.identity);
        playerAnimator.SetTrigger("Revive");
        yield return new WaitForSecondsRealtime(2f);
        HealthEvents.ReviveFinished(transform);
        GetComponent<PlayerInput>().enabled = true;
        XPManager.Instance.CheckLevelUP();
        isReviving = false;
    }

    private void Explode()
    {
        Instantiate(deathParticle, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
        HealthEvents.GameOver();
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(Screen.width - 150, 10, 150, 30), "Time Scale: " + Time.timeScale.ToString("F2"));
    }
}
