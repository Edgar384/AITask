using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour
{
    public float healthAmount = 25f;

    void OnTriggerEnter(Collider other)
    {
        Character character = other.GetComponent<Character>();
        if (character != null)
        {
            character.RestoreHealth(healthAmount);
            Destroy(gameObject);
        }
    }
}
