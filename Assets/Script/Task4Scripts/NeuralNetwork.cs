using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuralNetwork
{
    public float fitness; // Fitness value for the evolutionary algorithm
    private int[] layers; // Layers of the neural network
    private float[][] neurons; // Neurons in each layer
    public float[][][] weights; // Weights between the layers (made public for easier access in copy constructor)

    // Standard constructor for creating a neural network
    public NeuralNetwork(int[] layers)
    {
        this.layers = layers;
        InitNeurons();
        InitWeights();
    }

    // Copy constructor for creating a copy of an existing neural network
    public NeuralNetwork(NeuralNetwork copyNetwork)
    {
        this.layers = (int[])copyNetwork.layers.Clone();
        InitNeurons(); // Initialize neurons based on layers
        InitWeights(); // Initialize weights with same structure

        // Copy the weights from the existing network
        for (int i = 0; i < weights.Length; i++)
        {
            for (int j = 0; j < weights[i].Length; j++)
            {
                for (int k = 0; k < weights[i][j].Length; k++)
                {
                    weights[i][j][k] = copyNetwork.weights[i][j][k];
                }
            }
        }
    }

    // Initialize neurons array
    private void InitNeurons()
    {
        neurons = new float[layers.Length][];
        for (int i = 0; i < layers.Length; i++)
        {
            neurons[i] = new float[layers[i]];
        }
    }

    // Initialize weights array
    private void InitWeights()
    {
        weights = new float[layers.Length - 1][][];
        for (int i = 0; i < weights.Length; i++)
        {
            weights[i] = new float[layers[i]][];
            for (int j = 0; j < weights[i].Length; j++)
            {
                weights[i][j] = new float[layers[i + 1]];

                // Randomize weights
                for (int k = 0; k < weights[i][j].Length; k++)
                {
                    weights[i][j][k] = UnityEngine.Random.Range(-0.5f, 0.5f);
                }
            }
        }
    }

    // Feedforward function
    public float[] FeedForward(float[] inputs)
    {
        if (inputs.Length != neurons[0].Length)
        {
            Debug.LogError("Input size does not match the neural network input layer size.");
            return new float[0]; // Return empty array if mismatch
        }

        // Set input neurons
        for (int i = 0; i < inputs.Length; i++)
        {
            neurons[0][i] = inputs[i];
        }

        // Feedforward through each layer
        for (int i = 1; i < layers.Length; i++)
        {
            for (int j = 0; j < neurons[i].Length; j++)
            {
                float value = 0f;
                for (int k = 0; k < neurons[i - 1].Length; k++)
                {
                    value += neurons[i - 1][k] * weights[i - 1][k][j];
                }
                neurons[i][j] = Mathf.Tan(value); // Activation function
            }
        }

        return neurons[neurons.Length - 1]; // Return output layer
    }

    // Mutate weights
    public void Mutate(float mutationRate)
    {
        for (int i = 0; i < weights.Length; i++)
        {
            for (int j = 0; j < weights[i].Length; j++)
            {
                for (int k = 0; k < weights[i][j].Length; k++)
                {
                    float randomNumber = UnityEngine.Random.Range(0f, 100f);
                    if (randomNumber < mutationRate)
                    {
                        weights[i][j][k] = UnityEngine.Random.Range(-0.5f, 0.5f);
                    }
                }
            }
        }
    }
}