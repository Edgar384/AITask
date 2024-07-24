using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentSpawner : MonoBehaviour
{
    public GameObject agentPrefab;
    public Transform[] spawnPositions;
    public bool useGeneticAlgorithm;

    private void Start()
    {
        foreach (var spawnPosition in spawnPositions)
        {
            var agent = Instantiate(agentPrefab, spawnPosition.position, Quaternion.identity);
            agent.GetComponent<GeneticAlgorithmSetup>().enabled = useGeneticAlgorithm;
        }
    }
}
