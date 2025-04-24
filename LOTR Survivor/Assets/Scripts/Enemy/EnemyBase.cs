using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyBase : MonoBehaviour
{
    [Header("Enemy Data")]
    [SerializeField] protected EnemySO enemyData;

    protected Transform player;
    protected NavMeshAgent agent;
    [SerializeField] protected Animator animator;

    protected bool isInRange = false;
    protected bool isAttacking = false;
    protected float attackTimer = 0f;
    public bool isStunned = false;

    protected virtual void OnEnable()
    {
        HealthEvents.OnReviveComplete += SetPlayer;
        HealthEvents.OnPlayerDeath += HandlePlayerDeath;
        isStunned = false;
    }

    protected virtual void OnDisable()
    {
        HealthEvents.OnReviveComplete -= SetPlayer;
        HealthEvents.OnPlayerDeath -= HandlePlayerDeath;
    }

    protected virtual void Start()
    {
        Initialize();
    }

    protected virtual void Update()
    {
        if (player == null || isStunned) return;

        attackTimer += Time.deltaTime;
        MoveTowardsPlayer();
        CheckDistanceToPlayer();

        if (CanAttack())
        {
            StartAttack();
        }
    }

    protected virtual void Initialize()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        agent = GetComponent<NavMeshAgent>();

        if (agent != null) agent.speed = enemyData.speed;
    }

    protected virtual void SetPlayer(Transform playerTransform)
    {
        player = playerTransform;
        if (agent != null) agent.isStopped = false;
    }

    protected virtual void HandlePlayerDeath()
    {
        isStunned = false;
        StopMoving();
    }

    protected virtual void MoveTowardsPlayer()
    {
        if (!isInRange && agent != null)
        {
            agent.SetDestination(player.position);
        }
    }

    protected virtual void StopMoving()
    {
        if (agent != null && agent.isActiveAndEnabled)
        {
            agent.isStopped = true;
            agent.SetDestination(transform.position);
        }
    }

    protected virtual void ResumeMoving()
    {
        if (agent != null && agent.isActiveAndEnabled)
        {
            agent.isStopped = false;
        }
    }

    protected virtual void CheckDistanceToPlayer()
    {
        float dist = Vector3.Distance(transform.position, player.position);
        isInRange = dist <= enemyData.aggroRange;

        if (isInRange) StopMoving();
        else ResumeMoving();
    }

    protected abstract bool CanAttack();
    protected abstract void StartAttack();
}
