using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class GeneticAlgorithm : MonoBehaviour
{
    public int populationSize = 10;
    public List<FuzzyLogic> population;
    public float mutationRate = 0.01f;
    public int generations = 10;

    private void Start()
    {
        InitializePopulation();
        //for (int i = 0; i < generations; i++)
        //{
        //    EvaluateFitness();
        //    Select();
        //    Crossover();
        //    Mutate();
        //}
        ShowResults();
    }



    private void InitializePopulation()
    {
        population = new List<FuzzyLogic>();
        for (int i = 0; i < populationSize; i++)
        {
            FuzzyLogic individual = Instantiate(Resources.Load<FuzzyLogic>("FuzzyLogicPrefab"));
            population.Add(individual);
        }
    }

    private void EvaluateFitness()
    {
        foreach (var individual in population)
        {
            individual.character.health = Random.Range(0, individual.character.maxHealth);
            individual.Evaluate();
        }
    }

    private void Select()
    {
        population.Sort((a, b) => a.character.health.CompareTo(b.character.health));
        population = population.GetRange(0, populationSize / 2);
    }

    private void Crossover()
    {
        int originalSize = population.Count;
        for (int i = 0; i < originalSize; i += 2)
        {
            FuzzyLogic parent1 = population[i];
            FuzzyLogic parent2 = population[i + 1];

            FuzzyLogic child1 = Instantiate(parent1);
            FuzzyLogic child2 = Instantiate(parent2);

            float temp = child1.healthThreshold;
            child1.healthThreshold = child2.healthThreshold;
            child2.healthThreshold = temp;

            population.Add(child1);
            population.Add(child2);
        }
    }

    private void Mutate()
    {
        foreach (var individual in population)
        {
            if (Random.value < mutationRate)
            {
                individual.healthThreshold += Random.Range(-5f, 5f);
            }
        }
    }

    private void ShowResults()
    {
        foreach (var individual in population)
        {
            Debug.Log("Health Threshold: " + individual.healthThreshold);
        }
    }
}

