using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyBase : MonoBehaviour
{
    [Header("Enemy Data")]
    [SerializeField] protected EnemySO enemyData;

    protected Transform player;
    protected NavMeshAgent agent;
    [SerializeField] protected Animator animator;
    [SerializeField] protected float rotationSpeed = 1.0f;

    protected bool isInRange = false;
    protected bool isAttacking = false;
    protected float attackTimer = 0f;
    public bool isStunned = false;

    public bool followPlayer = true;

    protected virtual void OnEnable()
    {
        HealthEvents.OnReviveComplete += SetPlayer;
        HealthEvents.OnPlayerDeath += HandlePlayerDeath;

        if (animator != null)
        {
            animator.speed = 1f;
            animator.Play("Idle");
        }

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

        if (!isAttacking)
        {
            MoveTowardsPlayer();
        }

        CheckDistanceToPlayer();

        if (isInRange)
        {
            RotateTowardsPlayer();
        }

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
        followPlayer = true;
    }

    protected virtual void HandlePlayerDeath()
    {
        StopMoving();
        followPlayer = false;
    }

    protected virtual void MoveTowardsPlayer()
    {
        if (!isInRange && agent != null)
        {
            agent.SetDestination(player.position);
        }
    }

    protected virtual void RotateTowardsPlayer()
    {
        if (player == null) return;

        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
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
        if (agent != null && agent.isActiveAndEnabled && followPlayer)
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

    public void Stun()
    {
        animator.Play("Idle");
        isStunned = true;
        agent.isStopped = true;
        isAttacking = false;
    }

    public void UnStun()
    {
        isStunned = false;
        ResumeMoving();
    }

    protected abstract bool CanAttack();
    protected abstract void StartAttack();
}
