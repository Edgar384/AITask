using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public float patrolSpeed = 3.5f;
    public float chaseSpeed = 5f;
    public float sightRange = 15f;
    public float sightAngle = 45f;
    public float hearingRange = 10f;
    public float attackRange = 2f;
    public float damageAmount = 10f;
    public float health = 100f;
    public Transform[] patrolPoints;

    private NavMeshAgent agent;
    private int currentPatrolIndex;
    private Transform player;
    private Vector3 lastKnownPosition;
    public bool isChasing;
    public bool isSearching;
    public bool isAttacking;
    private bool isDead;
    public EnemyStats stats;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        if (stats != null)
        {
            sightRange = stats.sightRange;
            hearingRange = stats.hearingRange;
        }

        currentPatrolIndex = 0;
        Patrol();
    }

    void Update()
    {
        if (isDead) return;

        CheckSight();
        CheckHearing();

        if (isAttacking)
        {
            AttackPlayer();
        }
        else if (isChasing)
        {
            ChasePlayer();
        }
        else if (isSearching)
        {
            SearchPlayer();
        }
        else
        {
            Patrol();
        }
    }

    public void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        agent.speed = patrolSpeed;
        if (agent.remainingDistance < 0.5f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        }
    }

    public void ChasePlayer()
    {
        agent.speed = chaseSpeed;
        agent.SetDestination(player.position);
    }

    public void SearchPlayer()
    {
        agent.speed = patrolSpeed;
        agent.SetDestination(lastKnownPosition);

        if (agent.remainingDistance < 0.5f)
        {
            isSearching = false;
            Patrol();
        }
    }

    public void AttackPlayer()
    {
        if (Vector3.Distance(transform.position, player.position) > attackRange)
        {
            isAttacking = false;
            isChasing = true;
            return;
        }
        Character character = player.GetComponent<Character>();
        if (character != null)
        {
            character.TakeDamage(10f); // Adjust the damage value as needed
        }
        // Add attack logic here (e.g., reduce player health)
        Debug.Log("Enemy is attacking the player!");
    }

    public void Die()
    {
        isDead = true;
        agent.isStopped = true;
        Debug.Log("Enemy has died!");
        Destroy(gameObject, 2f); // Delay to allow death animation to play
    }

    public void CheckSight()
    {
        if (Vector3.Distance(transform.position, player.position) > sightRange) return;

        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float angleBetweenEnemyAndPlayer = Vector3.Angle(transform.forward, directionToPlayer);

        if (angleBetweenEnemyAndPlayer < sightAngle / 2f)
        {
            if (!Physics.Linecast(transform.position, player.position))
            {
                isChasing = true;
                isSearching = false;
                lastKnownPosition = player.position;

                if (Vector3.Distance(transform.position, player.position) <= attackRange)
                {
                    isAttacking = true;
                    isChasing = false;
                }

                AlertOthers();
            }
        }
    }

    public void CheckHearing()
    {
        if (Vector3.Distance(transform.position, player.position) <= hearingRange)
        {
            isSearching = true;
            isChasing = false;
            lastKnownPosition = player.position;

            AlertOthers();
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }

    public void AlertOthers()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, hearingRange);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                hitCollider.GetComponent<EnemyController>().OnAlerted(player.position);
            }
        }
    }

    public void OnAlerted(Vector3 position)
    {
        isSearching = true;
        isChasing = false;
        lastKnownPosition = position;
    }

    void OnDrawGizmos()
    {
        // Sight range
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        // Hearing range
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, hearingRange);

        // Sight cone
        Gizmos.color = Color.red;
        Vector3 forward = transform.forward * sightRange;
        Vector3 leftBoundary = Quaternion.Euler(0, -sightAngle / 2, 0) * forward;
        Vector3 rightBoundary = Quaternion.Euler(0, sightAngle / 2, 0) * forward;
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Character character = collision.gameObject.GetComponent<Character>();
            if (character != null)
            {
                character.TakeDamage(damageAmount);
            }
        }
        // Check if the collision is with a projectile
        Projectile projectile = collision.gameObject.GetComponent<Projectile>();
        if (projectile != null)
        {
            TakeDamage(projectile.damage);
        }
    }
}
