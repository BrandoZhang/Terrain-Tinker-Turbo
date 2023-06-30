using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class WheelController : MonoBehaviour
{
    [SerializeField] private WheelCollider frontRight;
    [SerializeField] private WheelCollider frontLeft;
    [SerializeField] private WheelCollider backRight;
    [SerializeField] private WheelCollider backLeft;

    [SerializeField] private Transform frontRightTransform;
    [SerializeField] private Transform frontLeftTransform;
    [SerializeField] private Transform backRightTransform;
    [SerializeField] private Transform backLeftTransform;

    public bool canMove = true;
    public int playerIndex;  // Player index (1 or 2)
    public float acceleration = 3000f;
    public float breakingForce = 30f;
    public float maxTurnAngle = 50f;
    public Transform startTransform;  // The Transform component where the racer will reset to
    public float minHeightThreshold = 20f;  // It is considered fall out of the Track if y value is less than this
    private Rigidbody rb;
    private bool tutorial1Check = false;

    private float currentAcceleration = 0f;
    private float currentBreakingForce = 0f;
    private float currentTurnAngle = 0f;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, -4, 0);
    }

    
    // Update is called once per frame
    void FixedUpdate()
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
        
        // Get forward and back acceleration
        currentAcceleration = acceleration * verticalInput;
        
        // Brake if pressing Space
        if (Input.GetKey(KeyCode.Space))
        {
            currentBreakingForce = breakingForce;
        }
        else
        {
            currentBreakingForce = 0f;
        }

        // Apply acceleration to front wheels
        frontRight.motorTorque = currentAcceleration;
        frontLeft.motorTorque = currentAcceleration;
        backRight.motorTorque = currentAcceleration;
        backLeft.motorTorque = currentAcceleration;

        // Apply breaking force to all wheels
        frontRight.brakeTorque = currentBreakingForce;
        frontLeft.brakeTorque = currentBreakingForce;
        backRight.brakeTorque = currentBreakingForce;
        backLeft.brakeTorque = currentBreakingForce;
        
        // Steering
        currentTurnAngle = maxTurnAngle * horizontalInput;
        frontLeft.steerAngle = currentTurnAngle;
        frontRight.steerAngle = currentTurnAngle;
        
        // Update Wheel movement
        UpdateWheel(frontLeft, frontLeftTransform);
        UpdateWheel(frontRight, frontRightTransform);
        UpdateWheel(backLeft, backLeftTransform);
        UpdateWheel(backRight, backRightTransform);
        
    }
    public void ResetToStart()
    {
        Debug.Log("ResetToStart called for player " + playerIndex);
        rb.velocity = Vector3.zero;  // Reset velocity
        rb.angularVelocity = Vector3.zero;  // Reset angular velocity
        transform.position = startTransform.position;  // Reset position
        transform.rotation = startTransform.rotation;  // Reset rotation
    }
    void UpdateWheel(WheelCollider col, Transform trans)
    {
        Vector3 pos;
        Quaternion rotation;
        col.GetWorldPose(out pos, out rotation);

        trans.position = pos;
        trans.rotation = rotation;
    }
}
