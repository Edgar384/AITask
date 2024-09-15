using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonController : MonoBehaviour
{
    public NeuralNetwork network; // Neural network that controls the dragon

    private Rigidbody rb; // Assuming the dragon has a Rigidbody for physics-based movement
    public Transform target; // The target the dragon should fly towards

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Example: initializing the network with 7 inputs (3 for position, 3 for velocity, 1 for altitude)
        // and 4 outputs (2 for wing strength, 2 for tail control)
        int[] layers = new int[] { 7, 10, 10, 4 };
        network = new NeuralNetwork(layers);
    }

    void FixedUpdate()
    {
        FlyTowardsTarget(); // Call the method to control flight every physics frame
    }

    public void FlyTowardsTarget()
    {
        if (network == null || target == null)
        {
            Debug.LogError("Neural Network or target is missing.");
            return;
        }

        // Inputs to the neural network: position (3), velocity (3), altitude difference (1)
        float[] inputs = new float[7];
        inputs[0] = transform.position.x;
        inputs[1] = transform.position.y;
        inputs[2] = transform.position.z;
        inputs[3] = rb.velocity.x;
        inputs[4] = rb.velocity.y;
        inputs[5] = rb.velocity.z;
        inputs[6] = target.position.y - transform.position.y; // Altitude difference

        // Get the output from the neural network (control for wings and tail)
        float[] output = network.FeedForward(inputs);

        // Use the output to control the dragon's movement
        ControlDragon(output);
    }

    private void ControlDragon(float[] output)
    {
        if (output.Length < 4)
        {
            Debug.LogError("Neural Network output does not match expected size.");
            return;
        }

        // Left and right wing control (output[0] and output[1])
        float leftWingStrength = output[0];
        float rightWingStrength = output[1];

        // Tail control (output[2] and output[3])
        float tailHorizontal = output[2];
        float tailVertical = output[3];

        // Example of how to apply the forces to the dragon for flying
        Vector3 wingForce = new Vector3(0, leftWingStrength + rightWingStrength, 0); // Adjust upward force
        rb.AddForce(wingForce);

        // Tail control for direction (yaw and pitch control)
        transform.Rotate(tailVertical, tailHorizontal, 0);
    }
}
