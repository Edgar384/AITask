using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticAlgorithm : MonoBehaviour
{
    public GameObject agentPrefab;
    public Transform[] spawnPoints;
    public int populationSize = 10;
    public int generations = 10;
    public float mutationRate = 0.1f;
    public ResultsOutput resultOutput; // Reference to the ResultOutput script

    private List<GameObject> population = new List<GameObject>();

    private void Start()
    {
        InitializePopulation();
        StartCoroutine(Evolve());
    }

    private void InitializePopulation()
    {
        for (int i = 0; i < populationSize; i++)
        {
            GameObject agent = Instantiate(agentPrefab, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity);
            FuzzyLogic fuzzyLogic = agent.GetComponent<FuzzyLogic>();
            fuzzyLogic.healthThreshold = Random.Range(20f, 80f); // Random initial health threshold
            population.Add(agent);
        }
    }

    private IEnumerator Evolve()
    {
        for (int generation = 0; generation < generations; generation++)
        {
            yield return new WaitForSeconds(10f); // Time for each generation to run

            List<GameObject> newPopulation = new List<GameObject>();

            population.Sort((a, b) => CalculateFitness(b).CompareTo(CalculateFitness(a)));

            for (int i = 0; i < populationSize / 2; i++)
            {
                GameObject parent1 = population[i];
                GameObject parent2 = population[populationSize - 1 - i];

                GameObject offspring1 = Instantiate(agentPrefab, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity);
                GameObject offspring2 = Instantiate(agentPrefab, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity);

                FuzzyLogic fuzzyLogic1 = offspring1.GetComponent<FuzzyLogic>();
                FuzzyLogic fuzzyLogic2 = offspring2.GetComponent<FuzzyLogic>();

                fuzzyLogic1.healthThreshold = (parent1.GetComponent<FuzzyLogic>().healthThreshold + parent2.GetComponent<FuzzyLogic>().healthThreshold) / 2;

                if (Random.value < mutationRate) Mutate(fuzzyLogic1);
                if (Random.value < mutationRate) Mutate(fuzzyLogic2);

                newPopulation.Add(offspring1);
                newPopulation.Add(offspring2);
            }

            foreach (GameObject agent in population)
            {
                Destroy(agent);
            }

            population = newPopulation;

            OnGenerationEnd(generation);
        }
    }

    private float CalculateFitness(GameObject agent)
    {
        Character character = agent.GetComponent<Character>();
        return character.health; // Fitness based on health
    }

    private void Mutate(FuzzyLogic fuzzyLogic)
    {
        fuzzyLogic.healthThreshold += Random.Range(-10f, 10f);
    }

    private void OnGenerationEnd(int generation)
    {
        if (resultOutput != null)
        {
            resultOutput.LogGenerationResults(generation, population);
        }
    }
}
