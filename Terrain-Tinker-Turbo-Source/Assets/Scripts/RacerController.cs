using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
public class RacerController : MonoBehaviour
{
    public bool canMove = true;
    public int playerIndex;  // Player index (1 or 2)
    public float accelerationForce = 30f;  // How fast the car accelerates
    public float turningForce = 3f;  // How fast the car turns
    public Transform startTransform;  // The Transform component where the racer will reset to
    public float minHeightThreshold = 20f;  // It is considered fall out of the Track if y value is less than this
    private bool tutorial1Check = false;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void ResetToStart()
    {
        Debug.Log("ResetToStart called for player " + playerIndex);
        rb.velocity = Vector3.zero;  // Reset velocity
        rb.angularVelocity = Vector3.zero;  // Reset angular velocity
        transform.position = startTransform.position;  // Reset position
        transform.rotation = startTransform.rotation;  // Reset rotation
    }
    
    private void FixedUpdate()
    {
        // Freeze the racer until it can move
        if (!canMove) return;
        // Check if the vehicle's height is below a certain threshold
        if (transform.position.y < minHeightThreshold) 
        {
            ResetToStart();
        }
        // Get the horizontal and vertical input (up/down and left/right)
        // Use different axes based on the player index
        float horizontalInput = Input.GetAxis("Horizontal" + (playerIndex == 1 ? "_P1" : "_P2"));
        float verticalInput = Input.GetAxis("Vertical" + (playerIndex == 1 ? "_P1" : "_P2"));
        
        //Only for Tutorial1 and only when tutorial1Check is false
        if (!tutorial1Check && SceneManager.GetActiveScene().name == "Tutorial1")
        {
            if (Math.Abs(horizontalInput) > 0 || Math.Abs(verticalInput) > 0)
            {
                tutorial1Check = true; //so that we can stop checking this logic again
                GameManager.Instance.setT1KeyboardControls(false); //remove keyboard controls instructions
            }
        }

        Debug.Log("Player " + playerIndex + " horizontal input: " + horizontalInput);
        Debug.Log("Player " + playerIndex + " vertical input: " + verticalInput);

        // Apply a force in the forward direction of the car, multiplied by our input and acceleration force
        rb.AddForce(transform.forward * verticalInput * accelerationForce);

        // Create a new vector3 for turning, and turn it based on our input and turning force
        Vector3 newRotation = new Vector3(0f, horizontalInput * turningForce, 0f);

        // Apply the turning. This is done by creating a new rotation and then applying it
        rb.rotation *= Quaternion.Euler(newRotation);
    }
}
