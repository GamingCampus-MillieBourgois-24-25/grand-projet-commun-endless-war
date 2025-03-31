using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField]
    private float attackRange = 2f;
    [SerializeField] private int damage = 5;
    [SerializeField] private float attackCooldown = 1f;

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
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();

        if (agent == null)
        {
            Debug.LogError("NavMeshAgent manquant sur " + gameObject.name);
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
        if (!isInRange)
        {
            agent.SetDestination(player.transform.position);
        }
    }

    private void CheckDistanceToPlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distanceToPlayer <= attackRange)
        {
            isInRange = true;
        }
        else
        {
            isInRange = false;
        }
    }

    private void UpdateTimer()
    {
        attackTimer += Time.deltaTime;
    }

    private void Attack()
    {
        if (isInRange && attackTimer >= attackCooldown)
        {
            if (player.TryGetComponent<HealthBehaviour>(out HealthBehaviour health))
            {
                health.TakeDamage(damage);
                attackTimer = 0f;
            }
        }
    }
}
