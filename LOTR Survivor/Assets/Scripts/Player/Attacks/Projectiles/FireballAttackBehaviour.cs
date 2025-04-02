using UnityEngine;

public class FireballAttackBehaviour : AttackBehaviour
{
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private float fireballSpeed = 10f;

    protected override void Attack()
    {
        GameObject nearestEnemy = FindNearestEnemy();

        if (nearestEnemy != null)
        {
            GameObject fireball = SpawnOrInstantiate(fireballPrefab, transform.position, Quaternion.identity);

            HomingProjectile fireballScript = fireball.GetComponent<HomingProjectile>();
            if (fireballScript != null)
            {
                fireballScript.Initialize(nearestEnemy, damage, fireballSpeed, fireballPrefab);
            }
        }
    }
}
