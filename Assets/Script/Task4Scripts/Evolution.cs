using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Evolution : MonoBehaviour
{
    public GameObject dragonPrefab;
    public Vector3 targetPosition;
    public int populationSize = 10;
    public int generations = 50;
    public float mutationRate = 0.01f;
    public float crossoverRate = 0.7f;

    private List<DragonController> dragons = new List<DragonController>();
    private List<NeuralNetwork> neuralNetworks = new List<NeuralNetwork>();

    void Start()
    {
        InitializePopulation();
    }

    void InitializePopulation()
    {
        for (int i = 0; i < populationSize; i++)
        {
            GameObject dragon = Instantiate(dragonPrefab, Vector3.zero, Quaternion.identity);
            NeuralNetwork nn = new NeuralNetwork(7, 10, 4); // 7 inputs, 10 hidden neurons, 4 outputs
            dragon.GetComponent<DragonController>().Initialize(nn, targetPosition);
            dragons.Add(dragon.GetComponent<DragonController>());
            neuralNetworks.Add(nn);
        }
    }

    void FixedUpdate()
    {
        if (generations > 0)
        {
            SimulateGeneration();
            EvolvePopulation();
            generations--;
        }
    }

    void SimulateGeneration()
    {
        foreach (DragonController dragon in dragons)
        {
            dragon.CalculateFitness();
        }
    }

    void EvolvePopulation()
    {
        neuralNetworks.Sort((x, y) => y.fitness.CompareTo(x.fitness));
        List<NeuralNetwork> newGeneration = new List<NeuralNetwork>();

        // Select the top-performing networks for the next generation
        for (int i = 0; i < populationSize / 2; i++)
        {
            newGeneration.Add(neuralNetworks[i]);
        }

        // Perform crossover and mutation to create the rest of the new generation
        while (newGeneration.Count < populationSize)
        {
            NeuralNetwork parent1 = neuralNetworks[Random.Range(0, populationSize / 2)];
            NeuralNetwork parent2 = neuralNetworks[Random.Range(0, populationSize / 2)];

            if (Random.Range(0f, 1f) < crossoverRate)
            {
                NeuralNetwork child = Crossover(parent1, parent2);
                Mutate(child);
                newGeneration.Add(child);
            }
            else
            {
                // In case crossover is not performed, directly mutate one of the parents
                NeuralNetwork child = new NeuralNetwork(parent1); // Copy constructor
                Mutate(child);
                newGeneration.Add(child);
            }
        }

        neuralNetworks = newGeneration;

        // Re-initialize dragons with new neural networks
        for (int i = 0; i < populationSize; i++)
        {
            dragons[i].Initialize(neuralNetworks[i], targetPosition);
            dragons[i].transform.position = Vector3.zero; // Reset position
            dragons[i].GetComponent<Rigidbody>().velocity = Vector3.zero; // Reset velocity
        }
    }

    NeuralNetwork Crossover(NeuralNetwork parent1, NeuralNetwork parent2)
    {
        NeuralNetwork child = new NeuralNetwork(7, 10, 4);

        for (int i = 0; i < parent1.weights.Length; i++)
        {
            child.weights[i] = Random.Range(0f, 1f) < 0.5f ? parent1.weights[i] : parent2.weights[i];
        }

        return child;
    }

    void Mutate(NeuralNetwork nn)
    {
        for (int i = 0; i < nn.weights.Length; i++)
        {
            if (Random.Range(0f, 1f) < mutationRate)
            {
                nn.weights[i] += Random.Range(-0.1f, 0.1f); // Mutate by a small random value
            }
        }
    }

}
