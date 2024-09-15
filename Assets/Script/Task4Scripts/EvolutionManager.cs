using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EvolutionManager : MonoBehaviour
{
    public int populationSize = 10;
    public float mutationRate = 0.05f;
    public int generations = 50;

    private List<NeuralNetwork> population;
    public GameObject dragonPrefab;
    public Transform target;

    void Start()
    {
        StartCoroutine(Evolve());
    }

    private IEnumerator Evolve()
    {
        // Initialize population
        population = new List<NeuralNetwork>();

        for (int i = 0; i < populationSize; i++)
        {
            // Initialize neural network for each dragon in the population
            int[] layers = new int[] { 7, 10, 10, 4 }; // Input and output size should match the DragonController's network
            NeuralNetwork network = new NeuralNetwork(layers);
            population.Add(network);
        }

        // Loop through generations
        for (int gen = 0; gen < generations; gen++)
        {
            // Instantiate dragons and assign networks
            List<DragonController> dragonControllers = new List<DragonController>();

            for (int i = 0; i < population.Count; i++)
            {
                GameObject dragon = Instantiate(dragonPrefab);
                DragonController controller = dragon.GetComponent<DragonController>();
                controller.network = population[i]; // Assign neural network
                controller.target = target; // Set the target

                dragonControllers.Add(controller);
            }

            // Let the dragons fly for a while (duration of simulation)
            yield return new WaitForSeconds(10f); // Simulate for 10 seconds

            // Evaluate fitness for each dragon
            for (int i = 0; i < dragonControllers.Count; i++)
            {
                DragonController dragon = dragonControllers[i];
                NeuralNetwork network = dragon.network;

                // Fitness can be distance to target, smoothness of flight, etc.
                float distanceToTarget = Vector3.Distance(dragon.transform.position, target.position);
                network.fitness = 1f / distanceToTarget; // Example fitness function
            }

            // Breed new population based on fitness
            population = BreedNewPopulation();

            // Destroy all the dragons from this generation
            foreach (var dragon in dragonControllers)
            {
                Destroy(dragon.gameObject);
            }
        }
    }

    private List<NeuralNetwork> BreedNewPopulation()
    {
        List<NeuralNetwork> newPopulation = new List<NeuralNetwork>();

        // Sort by fitness (descending)
        population.Sort((a, b) => b.fitness.CompareTo(a.fitness));

        for (int i = 0; i < populationSize; i++)
        {
            NeuralNetwork parentA = population[i];
            NeuralNetwork offspring = new NeuralNetwork(parentA); // Clone parentA
            offspring.Mutate(mutationRate); // Mutate the offspring

            newPopulation.Add(offspring);
        }

        return newPopulation;
    }
}
