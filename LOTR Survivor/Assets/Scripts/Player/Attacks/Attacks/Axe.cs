using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : Attack
{
    private Transform playerTransform;
    private float rotationAngle = 0f;
    private float initialDistance;

    public void Initialize(Transform newPlayer)
    {
        playerTransform = newPlayer;
        initialDistance = Vector3.Distance(transform.position, playerTransform.position);
        rotationAngle = 0;
    }

    void Update()
    {
        if (player != null)
        {
            UpdateAttack();
        }
        else
        {
            DestroyAttack();
        }
    }

    protected override void UpdateAttack()
    {
        float angleThisFrame = skillSettings.Speed * PlayerStatsMultiplier.projectileSpeedMultiplier * Time.deltaTime;
        rotationAngle += angleThisFrame;

        transform.RotateAround(playerTransform.position, Vector3.up, skillSettings.Speed * Time.deltaTime);

        Vector3 directionFromPlayer = (transform.position - playerTransform.position).normalized;
        transform.position = playerTransform.position + directionFromPlayer * initialDistance;

        if (rotationAngle >= skillSettings.MaxRotation * PlayerStatsMultiplier.projectileSpeedMultiplier)
        {
            DestroyAttack();
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            DealDamage(collider);
        }
    }

    private void DealDamage(Collider collider)
    {
        int finalDamage = Mathf.RoundToInt(skillSettings.Damage * PlayerStatsMultiplier.damageMultiplier);

        EnemyHealthBehaviour enemy = collider.GetComponent<EnemyHealthBehaviour>();
        if (enemy != null)
        {
            enemy.TakeDamage(finalDamage);
        }
    }
}
