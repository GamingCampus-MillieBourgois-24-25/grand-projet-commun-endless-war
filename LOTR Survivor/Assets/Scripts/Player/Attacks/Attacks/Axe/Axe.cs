using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : Attack
{
    private Transform player;
    private float rotationAngle = 0f;
    private float initialDistance;

    public void Initialize(Transform newPlayer)
    {
        player = newPlayer;
        initialDistance = Vector3.Distance(transform.position, player.position);
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
        float angleThisFrame = attackSettings.Speed * Time.deltaTime;
        rotationAngle += angleThisFrame;

        transform.RotateAround(player.position, Vector3.up, attackSettings.Speed * Time.deltaTime);

        Vector3 directionFromPlayer = (transform.position - player.position).normalized;
        transform.position = player.position + directionFromPlayer * initialDistance;

        if (rotationAngle >= attackSettings.MaxRotation)
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
        EnemyHealthBehaviour enemy = collider.GetComponent<EnemyHealthBehaviour>();
        if (enemy != null)
        {
            enemy.TakeDamage(attackSettings.Damage);
        }
    }
}
