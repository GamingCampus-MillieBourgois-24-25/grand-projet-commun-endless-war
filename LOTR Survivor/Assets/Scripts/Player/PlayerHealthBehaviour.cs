using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthBehaviour : MonoBehaviour, IHealth
{
    [Header("Parameters")]
    [SerializeField] protected int maxHealth;
    [SerializeField] protected float flashDuration = 0.1f;
    [SerializeField] private Color flashColor = Color.red;
    [SerializeField] HealthBarCanvas healthBarCanvas;

    private Renderer objectRenderer;
    private Color originalColor;

    public int MaxHealth { get => maxHealth; set => maxHealth = value; }
    public int Health { get; set; }
    public float FlashDuration { get => flashDuration; set => flashDuration = value; }
    public Color FlashColor { get => flashColor; set => flashColor = value; }

    void Start()
    {
        Health = MaxHealth;
        objectRenderer = GetComponent<Renderer>();

        if (objectRenderer != null)
        {
            originalColor = objectRenderer.material.color;
        }

        OnHealthInitialized();
    }

    public void OnHealthInitialized()
    {
        if (healthBarCanvas != null)
        {
            healthBarCanvas.UpdateUI(Health, MaxHealth);
        }
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;

        if (Health <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            StartCoroutine(FlashRed());

            if (healthBarCanvas != null)
            {
                healthBarCanvas.UpdateUI(Health, MaxHealth);
            }
        }
    }

    private IEnumerator FlashRed()
    {
        if (objectRenderer != null)
        {
            objectRenderer.material.color = flashColor;
            yield return new WaitForSeconds(FlashDuration);
            objectRenderer.material.color = originalColor;
        }
    }

    public void Heal(int amount)
    {
        Health += amount;
        Health = Mathf.Clamp(Health, 0, MaxHealth);

        Debug.Log("Le joueur s'est heal.");

        if (healthBarCanvas != null)
        {
            healthBarCanvas.UpdateUI(Health, MaxHealth);
        }
    }
}