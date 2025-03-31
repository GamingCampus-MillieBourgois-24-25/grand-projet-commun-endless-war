using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBehaviour : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] protected int maxHealth;
    [SerializeField] protected float flashDuration = 0.1f;
    [SerializeField] private Color flashColor = Color.red;

    protected int health;
    private Renderer objectRenderer;
    private Color originalColor;

    void Start()
    {
        health = maxHealth;
        objectRenderer = GetComponent<Renderer>();

        if (objectRenderer != null)
        {
            originalColor = objectRenderer.material.color;
        }

        OnHealthInitialized();
    }

    protected virtual void OnHealthInitialized() { }

    public virtual void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            DestroyEnemy();
        }
        else
        {
            StartCoroutine(FlashRed());
        }
    }

    private IEnumerator FlashRed()
    {
        if (objectRenderer != null)
        {
            objectRenderer.material.color = flashColor;
            yield return new WaitForSeconds(flashDuration);
            objectRenderer.material.color = originalColor;
        }
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }
}
