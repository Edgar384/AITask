using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class NeuralNetwork
{
    private int inputNodes;
    private int hiddenNodes;
    private int outputNodes;
    private float[,] weightsInputHidden;
    private float[,] weightsHiddenOutput;

    public NeuralNetwork(int inputNodes, int hiddenNodes, int outputNodes)
    {
        this.inputNodes = inputNodes;
        this.hiddenNodes = hiddenNodes;
        this.outputNodes = outputNodes;

        weightsInputHidden = new float[inputNodes, hiddenNodes];
        weightsHiddenOutput = new float[hiddenNodes, outputNodes];

        InitializeWeights();
    }

    private void InitializeWeights()
    {
        // Initialize weights with random values
        for (int i = 0; i < inputNodes; i++)
        {
            for (int j = 0; j < hiddenNodes; j++)
            {
                weightsInputHidden[i, j] = UnityEngine.Random.Range(-1f, 1f);
            }
        }

        for (int i = 0; i < hiddenNodes; i++)
        {
            for (int j = 0; j < outputNodes; j++)
            {
                weightsHiddenOutput[i, j] = UnityEngine.Random.Range(-1f, 1f);
            }
        }
    }

    private float Sigmoid(float x)
    {
        return 1f / (1f + Mathf.Exp(-x));
    }

    public float[] FeedForward(float[] inputs)
    {
        // Hidden layer
        float[] hidden = new float[hiddenNodes];
        for (int i = 0; i < hiddenNodes; i++)
        {
            float sum = 0f;
            for (int j = 0; j < inputNodes; j++)
            {
                sum += inputs[j] * weightsInputHidden[j, i];
            }
            hidden[i] = Sigmoid(sum);
        }

        // Output layer
        float[] outputs = new float[outputNodes];
        for (int i = 0; i < outputNodes; i++)
        {
            float sum = 0f;
            for (int j = 0; j < hiddenNodes; j++)
            {
                sum += hidden[j] * weightsHiddenOutput[j, i];
            }
            outputs[i] = Sigmoid(sum);
        }

        return outputs;
    }

    // Mutation function
    public void Mutate(float mutationRate)
    {
        for (int i = 0; i < inputNodes; i++)
        {
            for (int j = 0; j < hiddenNodes; j++)
            {
                if (UnityEngine.Random.Range(0f, 1f) < mutationRate)
                {
                    weightsInputHidden[i, j] += UnityEngine.Random.Range(-0.5f, 0.5f);
                }
            }
        }

        for (int i = 0; i < hiddenNodes; i++)
        {
            for (int j = 0; j < outputNodes; j++)
            {
                if (UnityEngine.Random.Range(0f, 1f) < mutationRate)
                {
                    weightsHiddenOutput[i, j] += UnityEngine.Random.Range(-0.5f, 0.5f);
                }
            }
        }
    }

    // Crossover function
    public NeuralNetwork Crossover(NeuralNetwork partner)
    {
        NeuralNetwork child = new NeuralNetwork(inputNodes, hiddenNodes, outputNodes);

        for (int i = 0; i < inputNodes; i++)
        {
            for (int j = 0; j < hiddenNodes; j++)
            {
                child.weightsInputHidden[i, j] = UnityEngine.Random.Range(0f, 1f) < 0.5f ? weightsInputHidden[i, j] : partner.weightsInputHidden[i, j];
            }
        }

        for (int i = 0; i < hiddenNodes; i++)
        {
            for (int j = 0; j < outputNodes; j++)
            {
                child.weightsHiddenOutput[i, j] = UnityEngine.Random.Range(0f, 1f) < 0.5f ? weightsHiddenOutput[i, j] : partner.weightsHiddenOutput[i, j];
            }
        }

        return child;
    }
}