using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyCombat : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform player;
    private Animator animator;

    [Header("Ranges")]
    public float aggroRange = 5f;
    public float stoppingDistance = 2f;

    [Header("Patrol")]
    public float patrolRadius = 6f;
    public float patrolWaitTime = 2f;

    [Header("Attack")]
    public float attackCooldown = 1.5f;

    private float lastAttackTime;
    private bool isPatrolling;

    public float enemyDamage = 10f;

    private bool isRooted;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        agent.stoppingDistance = stoppingDistance;
        StartCoroutine(PatrolRoutine());
    }

    void Update()
    {
        if (isRooted) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= aggroRange)
        {
            StopAllCoroutines();
            isPatrolling = false;
            ChasePlayer(distanceToPlayer);
        }
        else
        {
            if (!isPatrolling)
                StartCoroutine(PatrolRoutine());
        }
    }


  void ChasePlayer(float distance)
    {
        if (isRooted) return;

        if (distance > stoppingDistance)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
        }
        else
        {
            agent.isStopped = true;
            FaceTarget();
            TryAttack();
        }
    }

    void TryAttack()
    {
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;

            agent.isStopped = true;
            agent.velocity = Vector3.zero;

            animator.SetTrigger("Attack");
        }
    }

    void FaceTarget()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0f;

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                lookRotation,
                Time.deltaTime * 10f
            );
        }
    }

    IEnumerator PatrolRoutine()
    {
        isPatrolling = true;

        while (true)
        {
            Vector3 randomPoint = GetRandomNavMeshPoint(patrolRadius);
            agent.isStopped = false;
            agent.SetDestination(randomPoint);

            yield return new WaitUntil(() =>
                !agent.pathPending && agent.remainingDistance <= 0.5f
            );

            yield return new WaitForSeconds(patrolWaitTime);
        }
    }

    Vector3 GetRandomNavMeshPoint(float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;

        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, radius, NavMesh.AllAreas);
        return hit.position;
    }

    public void DealDamageToPlayer()
    {
        if (player == null) return;

        Stats playerStats = player.GetComponent<Stats>();
        if (playerStats == null) return;

        playerStats.TakeDamage(player.gameObject, enemyDamage);
    }

    public void SetRooted(bool value)
    {
        isRooted = value;

        if (agent == null) return;

        if (isRooted)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
        }
        else
        {
            agent.isStopped = false;
        }
    }

}