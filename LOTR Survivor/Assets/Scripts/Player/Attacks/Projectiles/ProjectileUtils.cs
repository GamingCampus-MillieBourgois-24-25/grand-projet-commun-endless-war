using UnityEngine;

public static class ProjectileUtils
{
    public static GameObject FindNearestEnemy(Vector3 position, float range, LayerMask enemyLayer)
    {
        Collider[] enemiesInRange = Physics.OverlapSphere(position, range, enemyLayer);

        if (enemiesInRange.Length == 0)
            return null;

        Collider nearestEnemy = enemiesInRange[0];
        float shortestDistance = Vector3.Distance(position, nearestEnemy.transform.position);

        foreach (Collider enemy in enemiesInRange)
        {
            float distance = Vector3.Distance(position, enemy.transform.position);
            if (distance < shortestDistance)
            {
                nearestEnemy = enemy;
                shortestDistance = distance;
            }
        }

        return nearestEnemy.gameObject;
    }
}
