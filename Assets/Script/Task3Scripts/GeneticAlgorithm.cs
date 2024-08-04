using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticAlgorithm : MonoBehaviour
{

    public GameObject agentPrefab;
    public Transform[] spawnPoints;
    public int populationSize = 10;
    public int generations = 50;
    public float mutationRate = 0.01f;

    private List<GeneticAlgorithmParameters> population;
    private int currentGeneration;
    private float bestFitness;

    private void Start()
    {
        InitializePopulation();
        StartCoroutine(RunGenerations());
    }

    private void InitializePopulation()
    {
        population = new List<GeneticAlgorithmParameters>();
        for (int i = 0; i < populationSize; i++)
        {
            GeneticAlgorithmParameters agent = ScriptableObject.CreateInstance<GeneticAlgorithmParameters>();
            agent.healthThreshold = Random.Range(0, 100);
            agent.movementSpeed = Random.Range(1, 10);
            population.Add(agent);

            int spawnIndex = i % spawnPoints.Length;
            Instantiate(agentPrefab, spawnPoints[spawnIndex].position, spawnPoints[spawnIndex].rotation).GetComponent<FuzzyLogic>().movementSpeed = agent.movementSpeed;
        }
    }

    private IEnumerator RunGenerations()
    {
        for (currentGeneration = 0; currentGeneration < generations; currentGeneration++)
        {
            EvaluateFitness();
            Select();
            Crossover();
            Mutate();
            yield return new WaitForSeconds(1f);
        }
    }

    private void EvaluateFitness()
    {
        foreach (var agent in population)
        {
            // Replace this with actual fitness calculation
            agent.fitness = Random.Range(0, 100);
            if (agent.fitness > bestFitness)
            {
                bestFitness = agent.fitness;
            }
        }
    }

    private void Select()
    {
        List<GeneticAlgorithmParameters> newPopulation = new List<GeneticAlgorithmParameters>();
        float totalFitness = 0f;

        foreach (var agent in population)
        {
            totalFitness += agent.fitness;
        }

        for (int i = 0; i < populationSize; i++)
        {
            float randomPoint = Random.Range(0, totalFitness);
            float cumulativeFitness = 0f;

            foreach (var agent in population)
            {
                cumulativeFitness += agent.fitness;
                if (cumulativeFitness >= randomPoint)
                {
                    newPopulation.Add(agent);
                    break;
                }
            }
        }

        population = newPopulation;
    }

    private void Crossover()
    {
        List<GeneticAlgorithmParameters> newPopulation = new List<GeneticAlgorithmParameters>();

        for (int i = 0; i < populationSize / 2; i++)
        {
            int parent1Index = Random.Range(0, populationSize);
            int parent2Index = Random.Range(0, populationSize);

            GeneticAlgorithmParameters parent1 = population[parent1Index];
            GeneticAlgorithmParameters parent2 = population[parent2Index];

            GeneticAlgorithmParameters child1 = ScriptableObject.CreateInstance<GeneticAlgorithmParameters>();
            GeneticAlgorithmParameters child2 = ScriptableObject.CreateInstance<GeneticAlgorithmParameters>();

            child1.healthThreshold = parent1.healthThreshold;
            child1.movementSpeed = parent2.movementSpeed;

            child2.healthThreshold = parent2.healthThreshold;
            child2.movementSpeed = parent1.movementSpeed;

            newPopulation.Add(child1);
            newPopulation.Add(child2);
        }

        population = newPopulation;
    }

    private void Mutate()
    {
        foreach (var agent in population)
        {
            if (Random.value < mutationRate)
            {
                agent.healthThreshold = Random.Range(0, 100);
                agent.movementSpeed = Random.Range(1, 10);
            }
        }
    }
}
