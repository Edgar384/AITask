using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuzzyLogic : MonoBehaviour
{
    public Transform enemy;
    public Transform nearestHealthPack;
    public float healthThreshold = 50f;
    public float movementSpeed = 5f;

    private Character character;

    private void Start()
    {
        character = GetComponent<Character>();
    }

    private void Update()
    {
        Evaluate();
    }

    public void Evaluate()
    {
        if (character.health < healthThreshold)
        {
            FindNearestHealthPack();
            if (nearestHealthPack != null)
            {
                if (Vector3.Distance(transform.position, nearestHealthPack.position) < Vector3.Distance(transform.position, enemy.position))
                {
                    MoveTowards(nearestHealthPack.position);
                }
                else
                {
                    MoveAwayFrom(enemy.position);
                }
            }
        }
        else
        {
            MoveTowards(enemy.position);
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
        transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * movementSpeed);
    }

    private void MoveAwayFrom(Vector3 target)
    {
        transform.position = Vector3.MoveTowards(transform.position, transform.position - (target - transform.position), Time.deltaTime * movementSpeed);
    }
}
