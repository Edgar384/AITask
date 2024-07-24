using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticAlgorithmSetup : MonoBehaviour
{
    public GameObject agentPrefab;
    public Transform[] spawnPositions;
    public bool enableGeneticUpdate;

    private void Start()
    {
        foreach (var spawnPosition in spawnPositions)
        {
            var agent = Instantiate(agentPrefab, spawnPosition.position, Quaternion.identity);
            if (enableGeneticUpdate)
            {
                agent.AddComponent<GeneticAlgorithm>();
            }
        }
    }
}
