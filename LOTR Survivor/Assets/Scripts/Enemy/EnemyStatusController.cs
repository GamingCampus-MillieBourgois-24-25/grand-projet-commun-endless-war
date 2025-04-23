using System.Collections;
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

    [SerializeField] private GameObject burnParticle;
    [SerializeField] private GameObject bleedParticle;
    [SerializeField] private GameObject poisonParticle;
    [SerializeField] private GameObject slowParticle;
    [SerializeField] private GameObject stunParticle;

    private GameObject currentBurnParticle;
    private GameObject currentBleedParticle;
    private GameObject currentPoisonParticle;
    private GameObject currentSlowParticle;
    private GameObject currentStunParticle;

    [SerializeField] private Transform stunEffectAnchor;

    private void Awake()
    {
        enemyBehaviour = GetComponent<EnemyBehaviour>();
        healthBehaviour = GetComponent<EnemyHealthBehaviour>();
        baseSpeed = GetComponent<UnityEngine.AI.NavMeshAgent>().speed;
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        if (poisonRoutine != null) StopCoroutine(poisonRoutine);
        if (burnRoutine != null) StopCoroutine(burnRoutine);
        if (bleedRoutine != null) StopCoroutine(bleedRoutine);
        if (slowRoutine != null) StopCoroutine(slowRoutine);
        if (stunRoutine != null) StopCoroutine(stunRoutine);

        poisonRoutine = burnRoutine = bleedRoutine = slowRoutine = stunRoutine = null;

        ReturnParticle(ref currentPoisonParticle, poisonParticle);
        ReturnParticle(ref currentBurnParticle, burnParticle);
        ReturnParticle(ref currentBleedParticle, bleedParticle);
        ReturnParticle(ref currentSlowParticle, slowParticle);
        ReturnParticle(ref currentStunParticle, stunParticle);

        var agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.speed = baseSpeed;
        agent.isStopped = false;
    }


    public void ApplyPoison(float damagePerSecond, float duration)
    {
        if (poisonRoutine != null) StopCoroutine(poisonRoutine);
        poisonRoutine = StartCoroutine(Poison(damagePerSecond, duration));
    }

    private IEnumerator Poison(float dps, float duration)
    {
        if (poisonParticle != null && currentPoisonParticle == null)
        {
            currentPoisonParticle = SpawnParticle(poisonParticle);
            currentPoisonParticle.transform.SetParent(transform);
        }

        float timer = 0f;
        while (timer < duration)
        {
            healthBehaviour.TakeDamage(Mathf.RoundToInt(dps));
            yield return new WaitForSeconds(0.7f);
            timer += 0.7f;
        }

        if (currentPoisonParticle != null)
        {
            ObjectPool.Instance.Despawn(currentPoisonParticle, poisonParticle);
            currentPoisonParticle = null;
        }
    }

    public void ApplySlow(float slowPercent, float duration)
    {
        if (slowRoutine != null) StopCoroutine(slowRoutine);
        slowRoutine = StartCoroutine(Slow(slowPercent, duration));
    }

    private IEnumerator Slow(float percent, float duration)
    {
        if (slowParticle != null && currentSlowParticle == null)
        {
            currentSlowParticle = SpawnParticle(slowParticle);
            currentSlowParticle.transform.SetParent(transform);
        }

        var agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        float originalSpeed = agent.speed;
        agent.speed = baseSpeed * (1f - percent);

        yield return new WaitForSeconds(duration);

        agent.speed = originalSpeed;

        if (currentSlowParticle != null)
        {
            ObjectPool.Instance.Despawn(currentSlowParticle, slowParticle);
            currentSlowParticle = null;
        }
    }

    public void ApplyStun(float duration)
    {
        if (stunRoutine != null) StopCoroutine(stunRoutine);
        stunRoutine = StartCoroutine(Stun(duration));
    }

    private IEnumerator Stun(float duration)
    {
        if (stunParticle != null && currentStunParticle == null && stunEffectAnchor!=null)
        {
            currentStunParticle = ObjectPool.Instance.Spawn(stunParticle, stunEffectAnchor.position, Quaternion.Euler(-90f, 0f, 0f));
            currentStunParticle.transform.SetParent(transform);
        }

        enemyBehaviour.isStunned = true;
        var agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.isStopped = true;

        yield return new WaitForSeconds(duration);

        agent.isStopped = false;
        enemyBehaviour.isStunned = false;

        if (currentStunParticle != null)
        {
            ObjectPool.Instance.Despawn(currentStunParticle, stunParticle);
            currentStunParticle = null;
        }
    }

    public void ApplyBurn(float dps, float duration)
    {
        Debug.Log("burning");
        if (burnRoutine != null) StopCoroutine(burnRoutine);
        burnRoutine = StartCoroutine(Burn(dps, duration));
        Debug.Log("really burning");
    }

    private IEnumerator Burn(float dps, float duration)
    {
        if (burnParticle != null && currentBurnParticle == null)
        {
            currentBurnParticle = SpawnParticle(burnParticle);
            currentBurnParticle.transform.SetParent(transform);
        }

        Debug.Log("absolute burning");

        float timer = 0f;
        int damageTick = Mathf.RoundToInt(dps);
        while (timer < duration)
        {
            healthBehaviour.TakeDamage(damageTick);
            yield return new WaitForSeconds(1f);
            timer += 1f;
        }

        if (currentBurnParticle != null)
        {
            ObjectPool.Instance.Despawn(currentBurnParticle, burnParticle);
            currentBurnParticle = null;
        }
    }

    public void ApplyBleed(float dps, float duration)
    {
        if (bleedRoutine != null) StopCoroutine(bleedRoutine);
        bleedRoutine = StartCoroutine(Bleed(dps, duration));
    }

    private IEnumerator Bleed(float dps, float duration)
    {
        if (bleedParticle != null && currentBleedParticle == null)
        {
            currentBleedParticle = SpawnParticle(bleedParticle);
            currentBleedParticle.transform.SetParent(transform);
        }

        float timer = 0f;
        int damageTick = Mathf.RoundToInt(dps);
        while (timer < duration)
        {
            healthBehaviour.TakeDamage(damageTick);
            yield return new WaitForSeconds(0.5f);
            timer += 0.5f;
        }

        if (currentBleedParticle != null)
        {
            ObjectPool.Instance.Despawn(currentBleedParticle, bleedParticle);
            currentBleedParticle = null;
        }
    }

    private GameObject SpawnParticle(GameObject prefab)
    {
        return ObjectPool.Instance.Spawn(prefab, transform.position, Quaternion.identity);
    }

    private void ReturnParticle(ref GameObject instance, GameObject prefab)
    {
        if (instance != null)
        {
            ObjectPool.Instance.Despawn(instance, prefab);
            instance = null;
        }
    }

}
