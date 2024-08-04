using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour
{
    public float healthRestoreAmount = 20f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Character player = other.GetComponent<Character>();
            if (player != null)
            {
                player.RestoreHealth(healthRestoreAmount);
                Destroy(gameObject);
            }
        }
    }
}
