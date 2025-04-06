using UnityEngine;
using System.Collections;
using Cinemachine;

public class PlayerAnimation : MonoBehaviour
{
    [Header("Animation References")]
    [SerializeField] private Animator playerAnimator;

    [Header("Visual & Effects")]
    [SerializeField] private float flashDuration = 0.1f;
    [SerializeField] private Color flashColor = Color.red;
    [SerializeField] private GameObject deathParticle;

    private PlayerHealthBehaviour playerHealth;

    private Renderer objectRenderer;
    private CinemachineImpulseSource impulseSource;
    private Color originalColor;

    private void Awake()
    {
        playerHealth = GetComponent<PlayerHealthBehaviour>();
        objectRenderer = GetComponentInChildren<Renderer>();
        impulseSource = GetComponent<CinemachineImpulseSource>();

        if (objectRenderer != null)
        {
            originalColor = objectRenderer.material.color;
        }

        if (playerHealth != null)
        {
            playerHealth.OnPlayerDeath += HandlePlayerDeath;
            playerHealth.OnPlayerDamaged += HandleDamageAnimations;
        }
    }

    private void Update()
    {
        if (playerHealth.Health > 0)
        {
            HandleMovementAnimations();
        }
    }

    private void HandleMovementAnimations()
    {
        
    }

    private void HandleDamageAnimations()
    {
        StartCoroutine(FlashFeedback());
        CameraShakeManager.instance.CameraShake(impulseSource);
    }

    private void HandlePlayerDeath()
    {
        StartCoroutine(PlayDeathAnimation());
    }

    private IEnumerator FlashFeedback()
    {
        if (objectRenderer != null)
        {
            objectRenderer.material.SetColor("_Color", flashColor);
            objectRenderer.material.SetFloat("_Damaged", 1f);
            yield return new WaitForSeconds(flashDuration);
            objectRenderer.material.SetFloat("_Damaged", 0f);
            objectRenderer.material.SetColor("_Color", originalColor);
        }
    }

    private IEnumerator PlayDeathAnimation()
    {
        yield return new WaitForSecondsRealtime(1f);
        playerAnimator.SetTrigger("Die");
    }

    private void OnDestroy()
    {
        if (playerHealth != null)
        {
            playerHealth.OnPlayerDeath -= HandlePlayerDeath;
            playerHealth.OnPlayerDamaged -= HandleDamageAnimations;
        }
    }

    private void Explode()
    {
        Instantiate(deathParticle, transform.position, Quaternion.identity);
        Destroy(gameObject);
        Debug.LogError("yuo explod");
    }
}
