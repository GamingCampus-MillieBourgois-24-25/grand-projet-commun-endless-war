using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : Attack
{
    private Transform player;
    private float rotationAngle = 0f;
    private float maxRotation = 360f;
    private float initialDistance;

    public void Initialize(int damage, float speed, float rotation, Transform player, GameObject prefab)
    {
        rotationAngle = 0f;
        base.Initialize(damage, speed, prefab);
        this.maxRotation = rotation;
        this.player = player;
        initialDistance = Vector3.Distance(transform.position, player.position);
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
        float angleThisFrame = speed * Time.deltaTime;
        rotationAngle += angleThisFrame;

        transform.RotateAround(player.position, Vector3.up, speed * Time.deltaTime);

        Vector3 directionFromPlayer = (transform.position - player.position).normalized;
        transform.position = player.position + directionFromPlayer * initialDistance;

        if (rotationAngle >= maxRotation)
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
            enemy.TakeDamage(damage);
        }
    }
}
