using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnAttackBehaviour : MonoBehaviour
{
    [SerializeField] SkillSettings attackSettings;

    private void Start()
    {
        attackSettings.Reset();
    }

    private void OnEnable()
    {
        HealthEvents.OnReviveComplete += Attack;
    }

    private void OnDisable()
    {
        HealthEvents.OnReviveComplete -= Attack;
    }

    private void Attack(Transform player)
    {
        GameObject particle = Instantiate(attackSettings.prefab, player.position, Quaternion.identity);
        particle.transform.localScale = Vector3.one * attackSettings.Scale;

        StartCoroutine(DealDamage(player));
    }

    IEnumerator DealDamage(Transform player)
    {
        yield return new WaitForSeconds(attackSettings.Cooldown);
        Collider[] hitEnemies = Physics.OverlapSphere(player.position, attackSettings.Range, LayerMask.GetMask("Enemy"));

        for (int i = 0; i < hitEnemies.Length; i++)
        {
            int randIndex = Random.Range(i, hitEnemies.Length);
            Collider temp = hitEnemies[i];
            hitEnemies[i] = hitEnemies[randIndex];
            hitEnemies[randIndex] = temp;
        }

        foreach (Collider enemy in hitEnemies)
        {
            yield return new WaitForSeconds(0.02f);
            Instantiate(attackSettings.hitPrefab, enemy.transform.position, Quaternion.identity);
            EnemyHealthBehaviour health = enemy.GetComponent<EnemyHealthBehaviour>();
            if (health != null)
            {
                health.TakeDamage(attackSettings.Damage, attackSettings.damageType);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackSettings != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackSettings.Range);
        }
    }
}
