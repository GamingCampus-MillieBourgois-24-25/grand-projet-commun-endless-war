using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    [Header("Enemy Data")]
    [SerializeField] private EnemySO enemyData;

    private Transform player;
    private NavMeshAgent agent;
    private float attackTimer;
    private bool isInRange = false;

    public bool isStunned = false;

    private void OnEnable()
    {
        HealthEvents.OnReviveComplete += SetPlayer;
        HealthEvents.OnPlayerDeath += HandlePlayerDeath;
        isStunned = false;
    }

    private void OnDisable()
    {
        HealthEvents.OnReviveComplete -= SetPlayer;

        if (player != null)
        {
            PlayerHealthBehaviour playerHealth = player.GetComponent<PlayerHealthBehaviour>();
            HealthEvents.OnPlayerDeath -= HandlePlayerDeath;
        }
    }

    void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        if (enemyData == null)
        {
            Debug.LogError("Enemy Data non assigné sur " + gameObject.name);
            return;
        }

        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();

        if (agent == null)
        {
            Debug.LogError("NavMeshAgent manquant sur " + gameObject.name);
            return;
        }

        agent.speed = enemyData.speed;

        PlayerHealthBehaviour playerHealth = player.GetComponent<PlayerHealthBehaviour>();
        HealthEvents.OnPlayerDeath += HandlePlayerDeath;
    }

    void Update()
    {
        UpdateTimer();
        if (player != null)
        {
            MoveTowardsPlayer();
            CheckDistanceToPlayer();
            Attack();
        }
    }

    private void SetPlayer(Transform playerTransform)
    {
        if (playerTransform == null)
        {
            Debug.LogWarning("SetPlayer called with a null transform.");
            return;
        }
        player = playerTransform;
        agent.isStopped = false;
    }

    private void MoveTowardsPlayer()
    {
        if (!isInRange && agent != null)
        {
            agent.SetDestination(player.position);
        }
    }

    private void CheckDistanceToPlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        bool wasInRange = isInRange;
        isInRange = distanceToPlayer <= enemyData.aggroRange;

        if (isInRange && !wasInRange)
        {
            StopMoving();
        }
        else if (!isInRange && wasInRange)
        {
            ResumeMoving();
        }
    }

    private void UpdateTimer()
    {
        attackTimer += Time.deltaTime;
    }

    private void Attack()
    {
        if (isInRange && attackTimer >= enemyData.attackCooldown && !isStunned)
        {
            if (player.TryGetComponent<IHealth>(out IHealth health))
            {
                health.TakeDamage(enemyData.attackPower);
                attackTimer = 0f;
            }
        }
    }

    private void StopMoving()
    {
        if (agent != null && agent.isActiveAndEnabled)
        {
            agent.isStopped = true;
            agent.SetDestination(transform.position);
        }
        else
        {
            Debug.LogWarning("NavMeshAgent is not active when trying to stop movement.");
        }
    }

    private void ResumeMoving()
    {
        if (agent != null && agent.isActiveAndEnabled)
        {
            agent.isStopped = false;
        }
    }

    private void HandlePlayerDeath()
    {
        StopMoving();
    }
}
