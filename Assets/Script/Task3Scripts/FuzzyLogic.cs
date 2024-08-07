using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TextCore.Text;

public class FuzzyLogic : MonoBehaviour
{
    public float healthThreshold = 50f;
    public Transform nearestHealthPack;
    private Transform nearestEnemy;

    public Character character;
    private NavMeshAgent navMeshAgent;

    private void Start()
    {
        character = GetComponent<Character>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        if (character == null || navMeshAgent == null)
        {
            Debug.LogError("Character or NavMeshAgent component not found on this GameObject!");
        }

        // Find enemies and health packs at the start
        FindNearestHealthPack();
        FindNearestEnemy();
    }

    private void Update()
    {
        // Find the nearest health pack and enemy each frame
        FindNearestHealthPack();
        FindNearestEnemy();

        if (character.health < healthThreshold)
        {
            if (nearestHealthPack != null && nearestEnemy != null)
            {
                if (Vector3.Distance(transform.position, nearestHealthPack.position) < Vector3.Distance(transform.position, nearestEnemy.position))
                {
                    MoveTowards(nearestHealthPack.position);
                }
                else
                {
                    MoveAwayFrom(nearestEnemy.position);
                }
            }
        }
        else
        {
            if (nearestEnemy != null)
            {
                MoveTowards(nearestEnemy.position);
            }
        }
    }

    public void Evaluate()
    {
        if (character.health <= healthThreshold)
        {
            FindNearestHealthPack();
            if (nearestHealthPack != null)
            {
                MoveTowards(nearestHealthPack.position);
            }
        }
    }

    private void FindNearestHealthPack()
    {
        HealthPack[] healthPacks = FindObjectsOfType<HealthPack>();
        float shortestDistance = Mathf.Infinity;
        Transform closestHealthPack = null;

        foreach (HealthPack healthPack in healthPacks)
        {
            float distance = Vector3.Distance(transform.position, healthPack.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                closestHealthPack = healthPack.transform;
            }
        }

        nearestHealthPack = closestHealthPack;

        if (nearestHealthPack == null)
        {
            Debug.LogWarning("No health packs found in the scene!");
        }
    }

    private void FindNearestEnemy()
    {
        EnemyController[] enemies = FindObjectsOfType<EnemyController>();
        float shortestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (EnemyController enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                closestEnemy = enemy.transform;
            }
        }

        nearestEnemy = closestEnemy;

        if (nearestEnemy == null)
        {
            Debug.LogWarning("No enemies found in the scene!");
        }
    }

    private void MoveTowards(Vector3 target)
    {
        if (navMeshAgent != null)
        {
            navMeshAgent.SetDestination(target);
        }
    }

    private void MoveAwayFrom(Vector3 target)
    {
        if (navMeshAgent != null)
        {
            Vector3 direction = transform.position - target;
            navMeshAgent.SetDestination(transform.position + direction);
        }
    }
}

