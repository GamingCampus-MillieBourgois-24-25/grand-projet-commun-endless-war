using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatusController : MonoBehaviour, IPoisonable, ISlowable, IStunnable, IBurnable, IBleedable
{
    private EnemyBehaviour enemyBehaviour;
    private EnemyHealthBehaviour healthBehaviour;
    private float baseSpeed;

    private Coroutine poisonRoutine;
    private Coroutine slowRoutine;
    private Coroutine stunRoutine;
    private Coroutine burnRoutine;
    private Coroutine bleedRoutine;

    private void Awake()
    {
        enemyBehaviour = GetComponent<EnemyBehaviour>();
        healthBehaviour = GetComponent<EnemyHealthBehaviour>();
        baseSpeed = GetComponent<UnityEngine.AI.NavMeshAgent>().speed;
    }

    public void ApplyPoison(float damagePerSecond, float duration)
    {
        if (poisonRoutine != null) StopCoroutine(poisonRoutine);
        poisonRoutine = StartCoroutine(Poison(damagePerSecond, duration));
    }

    private IEnumerator Poison(float dps, float duration)
    {
        float timer = 0f;
        while (timer < duration)
        {
            healthBehaviour.TakeDamage(Mathf.RoundToInt(dps));
            yield return new WaitForSeconds(0.7f);
            timer += 0.7f;
        }
    }

    public void ApplySlow(float slowPercent, float duration)
    {
        if (slowRoutine != null) StopCoroutine(slowRoutine);
        slowRoutine = StartCoroutine(Slow(slowPercent, duration));
    }

    private IEnumerator Slow(float percent, float duration)
    {
        var agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        float originalSpeed = agent.speed;
        agent.speed = baseSpeed * (1f - percent);
        yield return new WaitForSeconds(duration);
        agent.speed = originalSpeed;
    }

    public void ApplyStun(float duration)
    {
        if (stunRoutine != null) StopCoroutine(stunRoutine);
        stunRoutine = StartCoroutine(Stun(duration));
    }

    private IEnumerator Stun(float duration)
    {
        var agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.isStopped = true;
        yield return new WaitForSeconds(duration);
        agent.isStopped = false;
    }

    public void ApplyBurn(float duration)
    {
        if (burnRoutine != null) StopCoroutine(burnRoutine);
        burnRoutine = StartCoroutine(Burn(duration));
    }

    private IEnumerator Burn(float duration)
    {
        float timer = 0f;
        float damageTick = 5f;
        while (timer < duration)
        {
            healthBehaviour.TakeDamage(Mathf.RoundToInt(damageTick));
            yield return new WaitForSeconds(1f);
            timer += 1f;
        }
    }

    public void ApplyBleed(float duration)
    {
        if (bleedRoutine != null) StopCoroutine(bleedRoutine);
        bleedRoutine = StartCoroutine(Bleed(duration));
    }

    private IEnumerator Bleed(float duration)
    {
        float timer = 0f;
        float damageTick = 3f;
        while (timer < duration)
        {
            healthBehaviour.TakeDamage(Mathf.RoundToInt(damageTick));
            yield return new WaitForSeconds(0.5f);
            timer += 0.5f;
        }
    }
}
