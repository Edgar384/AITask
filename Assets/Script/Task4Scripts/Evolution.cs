using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Evolution : MonoBehaviour
{
    public int populationSize = 50;
    public int generations = 100;
    public float mutationRate = 0.01f;
    public DragonController dragonPrefab;
    public Transform target;

    private List<NeuralNetwork> population = new List<NeuralNetwork>();
    private NeuralNetwork bestNetwork;

    void Start()
    {
        InitializePopulation();
        StartCoroutine(Evolve());
    }

    void InitializePopulation()
    {
        for (int i = 0; i < populationSize; i++)
        {
            NeuralNetwork newNetwork = new NeuralNetwork(/* initialize with required parameters */);
            newNetwork.Initialize();
            population.Add(newNetwork);
        }
    }

    IEnumerator<Evolve>
    {
        for (int gen = 0; gen<generations; gen++)
        {
            foreach (var network in population)
            {
                // Instantiate a dragon and assign its network
                DragonController dragon = Instantiate(dragonPrefab);
    dragon.neuralNetwork = network;
                dragon.target = target;

                // Simulate for a fixed duration
                yield return new WaitForSeconds(5);

    // Calculate fitness based on the distance to the target
    float distanceToTarget = Vector3.Distance(dragon.transform.position, target.position);
    network.fitness = 1 / distanceToTarget; // Inverse of distance for fitness
                
                // Clean up
                Destroy(dragon.gameObject);
}

// Sort by fitness
population.Sort((x, y) => y.fitness.CompareTo(x.fitness));

// Save the best network
bestNetwork = population[0];

// Perform crossover and mutation to create a new population
List<NeuralNetwork> newPopulation = new List<NeuralNetwork>();

for (int i = 0; i < populationSize; i++)
{
    NeuralNetwork parent1 = population[Random.Range(0, populationSize / 2)];
    NeuralNetwork parent2 = population[Random.Range(0, populationSize / 2)];
    NeuralNetwork child = parent1.Crossover(parent2);
    child.Mutate(mutationRate);
    newPopulation.Add(child);
}

population = newPopulation;
        }
    }
}
