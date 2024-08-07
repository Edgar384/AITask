using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class NeuralNetwork
{

    public float[] weights;
    public float fitness;

    public NeuralNetwork(int inputSize, int hiddenSize, int outputSize)
    {
        // Initialize the neural network with random weights
        weights = new float[(inputSize * hiddenSize) + (hiddenSize * outputSize)];
        for (int i = 0; i < weights.Length; i++)
        {
            weights[i] = UnityEngine.Random.Range(-1f, 1f);
        }
        fitness = 0f;
    }

    public NeuralNetwork(NeuralNetwork copy)
    {
        weights = new float[copy.weights.Length];
        copy.weights.CopyTo(weights, 0);
        fitness = copy.fitness;
    }

    public float[] FeedForward(float[] inputs)
    {
        // Feedforward logic here
        // Assuming a single hidden layer for simplicity
        int inputSize = 7;
        int hiddenSize = 10;
        int outputSize = 4;

        float[] hidden = new float[hiddenSize];
        float[] output = new float[outputSize];

        // Input to hidden
        for (int i = 0; i < hiddenSize; i++)
        {
            hidden[i] = 0;
            for (int j = 0; j < inputSize; j++)
            {
                hidden[i] += inputs[j] * weights[j + i * inputSize];
            }
            hidden[i] = MathF.Tanh(hidden[i]);
        }

        // Hidden to output
        for (int i = 0; i < outputSize; i++)
        {
            output[i] = 0;
            for (int j = 0; j < hiddenSize; j++)
            {
                output[i] += hidden[j] * weights[inputSize * hiddenSize + j + i * hiddenSize];
            }
            output[i] = MathF.Tanh(output[i]);
        }

        return output;
    }
}
