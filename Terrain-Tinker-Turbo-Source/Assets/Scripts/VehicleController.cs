using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleController : MonoBehaviour
{
    public float speed;
    public float turnSpeed;
    private float _horizontalInput;
    private float _forwardInput;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Get player inputs
        _horizontalInput = Input.GetAxis("Horizontal");
        _forwardInput = Input.GetAxis("Vertical");
        
        // Move the vehicle forward
        transform.Translate(Vector3.forward * (Time.deltaTime * speed * _forwardInput));
        // Turn the vehicle
        transform.Rotate(Vector3.up, Time.deltaTime * turnSpeed * _horizontalInput);
    }
}
