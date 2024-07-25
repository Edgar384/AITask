using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class Character : MonoBehaviour
{
    public float health = 100f;
    public float maxHealth = 100f;
    public FuzzyLogic fuzzyLogic;

    void Start()
    {
        fuzzyLogic = GetComponent<FuzzyLogic>();
        
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);
       
        fuzzyLogic.Evaluate(); // Re-evaluate fuzzy logic rules after taking damage
        Debug.Log("Player took damage. Current health: " + health);

        if (health <= 0)
        {
            Die();
        }
    }

    public void RestoreHealth(float amount)
    {
        health += amount;
        health = Mathf.Clamp(health, 0, maxHealth);
       
    }

  
    void Die()
    {
        Debug.Log("Player has died!");
        // Add any death handling logic here, such as game over screen
    }
}
