using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuzzyLogic : MonoBehaviour
{
    public Transform enemy;
    public Transform healthPack;
    public float healthThreshold = 50f;

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
