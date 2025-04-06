using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    [Header("Enemy Data")]
    [SerializeField] private EnemySO enemyData;

    private GameObject player;
    private NavMeshAgent agent;
    private float attackTimer;
    private bool isInRange = false;

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

        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();

        if (agent == null)
        {
            Debug.LogError("NavMeshAgent manquant sur " + gameObject.name);
            return;
        }

        agent.speed = enemyData.speed;

        PlayerHealthBehaviour playerHealth = player.GetComponent<PlayerHealthBehaviour>();
        if (playerHealth != null)
        {
            playerHealth.OnPlayerDeath += HandlePlayerDeath;
        }
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

    private void MoveTowardsPlayer()
    {
        if (!isInRange && agent != null)
        {
            agent.SetDestination(player.transform.position);
        }
    }

    private void CheckDistanceToPlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        isInRange = distanceToPlayer <= enemyData.aggroRange;
    }

    private void UpdateTimer()
    {
        attackTimer += Time.deltaTime;
    }

    private void Attack()
    {
        if (isInRange && attackTimer >= enemyData.attackCooldown)
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


    private void HandlePlayerDeath()
    {
        StopMoving();
    }

    void OnDestroy()
    {
        if (player != null)
        {
            PlayerHealthBehaviour playerHealth = player.GetComponent<PlayerHealthBehaviour>();
            if (playerHealth != null)
            {
                playerHealth.OnPlayerDeath -= HandlePlayerDeath;
            }
        }
    }

}
