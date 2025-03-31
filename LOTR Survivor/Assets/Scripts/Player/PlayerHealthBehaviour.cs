using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthBehaviour : HealthBehaviour
{
    [SerializeField] HealthBarCanvas healthBarCanvas;

    protected override void OnHealthInitialized()
    {
        if (healthBarCanvas != null)
        {
            healthBarCanvas.UpdateUI(health, maxHealth);
        }
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        if (healthBarCanvas != null)
        {
            healthBarCanvas.UpdateUI(health, maxHealth);
        }
    }
}
