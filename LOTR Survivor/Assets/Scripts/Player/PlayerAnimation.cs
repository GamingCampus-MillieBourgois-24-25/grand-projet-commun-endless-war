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

    [SerializeField] private GameObject deathParticle;
    [SerializeField] private GameObject reviveParticle;
    [SerializeField] private GameObject healingParticle;
    [SerializeField] private float blinkSpeed = 0.1f;
    [SerializeField] private Transform mesh;

    [SerializeField] private float slowmoScale = 0.1f;
    [SerializeField] private float slowmoDuration = 1.5f;

    [SerializeField] private AudioClip[] damageClips;
    [SerializeField] private AudioClip heal;

    private PlayerHealthBehaviour playerHealth;
    private bool isReviving = false;

    private Renderer objectRenderer;
    private CinemachineImpulseSource impulseSource;
    private Material originalMaterial;
    private bool isInvulnerable;

    public float movement;

    private void Awake()
    {
        playerHealth = GetComponent<PlayerHealthBehaviour>();
        objectRenderer = GetComponentInChildren<Renderer>();
        impulseSource = GetComponent<CinemachineImpulseSource>();

        if (objectRenderer != null)
        {
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
            HealthEvents.OnHealing += HandleHealingAnimations;

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
            HealthEvents.OnHealing -= HandleHealingAnimations;

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
        if (movement > 0.1)
        {
            playerAnimator.SetBool("IsMoving", true);
        }
        else
        {
            playerAnimator.SetBool("IsMoving", true);
        }
    }

    private void HandleDamageAnimations(int amount)
    {
        if (damageClips != null && damageClips.Length > 0)
        {
            AudioClip damageClip = damageClips[UnityEngine.Random.Range(0, damageClips.Length)];
            VolumeManager.Instance.PlaySFX(damageClip, 0.8f);
        }

        StartCoroutine(FlashFeedback());
        if (CameraShakeManager.instance != null)
        {
            CameraShakeManager.instance.CameraShake(impulseSource);
        }
    }

    private void HandleHealingAnimations(int amount)
    {
        VolumeManager.Instance.PlaySFX(heal, 0.5f);
        Instantiate(healingParticle, transform.position, Quaternion.identity, transform);
    }

    private void HandlePlayerDeath()
    {
        StartCoroutine(PlayDeathAnimation());
        if (CameraShakeManager.instance != null)
        {
            CameraShakeManager.instance.CameraShake(impulseSource);
        }
    }

    private void HandlePlayerRevive(Transform player)
    {
        Debug.Log("rezdf");
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
        Time.timeScale = slowmoScale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        yield return new WaitForSecondsRealtime(slowmoDuration);

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        GetComponent<PlayerInput>().enabled = false;

        yield return new WaitForSecondsRealtime(1f);
        playerAnimator.SetTrigger("Die");
    }

    private IEnumerator PlayReviveAnimation()
    {
        isReviving = true;
        Vector3 position = transform.position;
        position.y = 0;
        Instantiate(reviveParticle, position, Quaternion.identity);

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
        ResetMesh();
        gameObject.SetActive(false);
        HealthEvents.GameOver();
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(Screen.width - 150, 10, 150, 30), "Time Scale: " + Time.timeScale.ToString("F2"));
    }

    private void ResetMesh()
    {
        if (mesh != null)
        {
            mesh.localRotation = Quaternion.identity;
            mesh.localPosition = new Vector3(0,0.5f,0);
        }
    }
}
