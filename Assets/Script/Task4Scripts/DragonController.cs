using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonController : MonoBehaviour
{
    private NeuralNetwork neuralNetwork;
    public Rigidbody rb;
    public Vector3 targetPosition;

    public float fitness;
    public float initialDistance;

    public void Initialize(NeuralNetwork nn, Vector3 target)
    {
        neuralNetwork = nn;
        targetPosition = target;
        rb = GetComponent<Rigidbody>();
        fitness = 0f;
        initialDistance = Vector3.Distance(transform.position, targetPosition);
    }

    void FixedUpdate()
    {
        float[] inputs = new float[]
        {
            transform.position.x, transform.position.y, transform.position.z,
            targetPosition.x, targetPosition.y, targetPosition.z,
            rb.velocity.magnitude
        };

        float[] outputs = neuralNetwork.FeedForward(inputs);

        Vector3 wingForce = new Vector3(outputs[0] - outputs[1], outputs[2], outputs[3]);
        rb.AddForce(wingForce);

        CalculateFitness();
    }

    public void CalculateFitness()
    {
        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);
        fitness = (initialDistance - distanceToTarget) / initialDistance; // Normalize to 0-1 range
        neuralNetwork.fitness = fitness;
    }
}
