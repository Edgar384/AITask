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

    private void Start()
    {
        fuzzyLogic = GetComponent<FuzzyLogic>();
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);
        fuzzyLogic.Evaluate();
    }

    public void RestoreHealth(float amount)
    {
        health += amount;
        health = Mathf.Clamp(health, 0, maxHealth);
    }
}
