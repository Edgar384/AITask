using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class DragonController : MonoBehaviour
{
    public Rigidbody rb;
    public NeuralNetwork neuralNetwork;
    public float speed = 10f;
    public float rotationSpeed = 5f;
    public Transform target;

    void Start()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }

        if (neuralNetwork == null)
        {
            neuralNetwork = new NeuralNetwork(9, 6, 4);
            
        }
    }

    void FixedUpdate()
    {
        if (neuralNetwork == null || rb == null)
        {
            Debug.LogError("NeuralNetwork or Rigidbody not initialized!");
            return;
        }

        // Get the inputs to the neural network
        Vector3 dragonPosition = transform.position;
        Vector3 targetPosition = target.position;
        Vector3 velocity = rb.velocity;

        float[] inputs = new float[]
        {
            dragonPosition.x, dragonPosition.y, dragonPosition.z,
            targetPosition.x, targetPosition.y, targetPosition.z,
            velocity.x, velocity.y, velocity.z
        };

        // Get the outputs from the neural network
        float[] outputs = neuralNetwork.FeedForward(inputs);

        // Assume the outputs are in the range of -1 to 1, and we map them to forces and torques
        float leftWingFlap = Mathf.Clamp(outputs[0], -1f, 1f) * speed;
        float rightWingFlap = Mathf.Clamp(outputs[1], -1f, 1f) * speed;
        float tailHorizontal = Mathf.Clamp(outputs[2], -1f, 1f) * rotationSpeed;
        float tailVertical = Mathf.Clamp(outputs[3], -1f, 1f) * rotationSpeed;

        // Apply the forces to control the dragon's flight
        Vector3 liftForce = Vector3.up * (leftWingFlap + rightWingFlap);
        rb.AddForce(liftForce);

        // Apply torque to control yaw and pitch
        Vector3 torque = new Vector3(-tailVertical, tailHorizontal, 0f);
        rb.AddTorque(torque);

        // Apply forward movement to the dragon
        Vector3 forwardForce = transform.forward * Mathf.Abs(leftWingFlap + rightWingFlap) * 0.5f;
        rb.AddForce(forwardForce);
    }
}
