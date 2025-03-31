using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : MonoBehaviour
{
    private int damage;
    private float speed;
    private Transform player;
    private float rotationAngle = 0f;

    public void Initialize(int damage, float speed, Transform player)
    {
        this.damage = damage;
        this.speed = speed;
        this.player = player;
    }

    void Update()
    {
        RotateAxe();
    }

    private void RotateAxe()
    {
        float angleThisFrame = speed * Time.deltaTime;
        rotationAngle += angleThisFrame;

        transform.RotateAround(player.position, Vector3.up, speed * Time.deltaTime);

        if (rotationAngle >= 360f)
        {
            DestroyAxe();
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
        HealthBehaviour enemy = collider.GetComponent<HealthBehaviour>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
    }

    private void DestroyAxe()
    {
        Destroy(gameObject);
    }
}
