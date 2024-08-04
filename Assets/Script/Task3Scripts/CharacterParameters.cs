using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GeneticAlgorithm/Parameters")]
public class GeneticAlgorithmParameters : ScriptableObject
{
    public float healthThreshold;
    public float movementSpeed;
    public float fitness;
}
