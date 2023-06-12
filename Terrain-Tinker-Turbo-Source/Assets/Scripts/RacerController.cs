using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RacerController : MonoBehaviour
{
    public float accelerationForce = 100f;  // How fast the car accelerates
    public float turningForce = 5f;  // How fast the car turns

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        // Get the horizontal and vertical input (up/down and left/right)
        // Unity automatically maps these to the arrow keys and 'A' 'D' keys
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Debug.Log("Horizontal input: " + horizontalInput);
        Debug.Log("Vertical input: " + verticalInput);

        // Apply a force in the forward direction of the car, multiplied by our input and acceleration force
        rb.AddForce(transform.forward * verticalInput * accelerationForce);

        // Create a new vector3 for turning, and turn it based on our input and turning force
        Vector3 newRotation = new Vector3(0f, horizontalInput * turningForce, 0f);

        // Apply the turning. This is done by creating a new rotation and then applying it
        rb.rotation *= Quaternion.Euler(newRotation);
    }
}
