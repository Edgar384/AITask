using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuzzyLogic : MonoBehaviour
{
    public Transform enemy;
    public Transform healthPack;
    public float healthThreshold = 50f;
    public Transform nearestHealthPack;

    private Character character;

    private void Start()
    {
        character = GetComponent<Character>();
    }

    private void Update()
    {
        if (character.health < healthThreshold)
        {
            if (Vector3.Distance(transform.position, healthPack.position) < Vector3.Distance(transform.position, enemy.position))
            {
                MoveTowards(healthPack.position);
            }
            else
            {
                MoveAwayFrom(enemy.position);
            }
        }
        else
        {
            MoveTowards(enemy.position);
        }
    }
    public void Evaluate()
    {
        if (character.health <= healthThreshold)
        {
            FindNearestHealthPack();
            if (nearestHealthPack != null)
            {
                // Move towards the nearest health pack
                MoveTowards(nearestHealthPack.position);
            }
        }
        else
        {
            // Normal behavior, e.g., patrolling or engaging enemies
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
    }
    private void MoveTowards(Vector3 target)
    {
        // Movement logic towards the target
        transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * 5f);
    }

    private void MoveAwayFrom(Vector3 target)
    {
        // Movement logic away from the target
        transform.position = Vector3.MoveTowards(transform.position, transform.position - (target - transform.position), Time.deltaTime * 5f);
    }
}
