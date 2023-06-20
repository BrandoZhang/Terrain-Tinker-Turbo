using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private float currentAcceleration = 0f;
    private float currentBreakingForce = 0f;
    private float currentTurnAngle = 0f;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        float horizontalInput = Input.GetAxis("Horizontal" + (playerIndex == 1 ? "_P1" : "_P2"));
        float verticalInput = Input.GetAxis("Vertical" + (playerIndex == 1 ? "_P1" : "_P2"));
        
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

    void UpdateWheel(WheelCollider col, Transform trans)
    {
        Vector3 pos;
        Quaternion rotation;
        col.GetWorldPose(out pos, out rotation);

        trans.position = pos;
        trans.rotation = rotation;
    }
}
