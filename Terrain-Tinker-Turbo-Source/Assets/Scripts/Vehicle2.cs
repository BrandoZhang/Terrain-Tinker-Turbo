using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle2 : MonoBehaviour
{
    private float speed  = 15.0f;
    private float turnSpeed = 45.0f;

    //public string forwardKey;
    //public string backwardKey;
    //public string leftKey;
    //public string rightKey;
    public Rigidbody rb;
    private float moveVertical;
    private Vector3 newVelocity;
    Vector3 movement;
    Vector3 rotationalTorque;

    private float horizontalInput;
    private float forwardInput;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    
    void Update()
    {
        //horizontalInput = Input.GetAxis("Horizontal");
        //forwardInput = Input.GetAxis("Vertical");
        // Move the vehicle forward
        //transform.Translate(Vector3.forward * Time.deltaTime * speed * forwardInput);
        //transform.Rotate(Vector3.up, turnSpeed * horizontalInput * Time.deltaTime);
        //transform.Translate(Vector3.right * Time.deltaTime * turnSpeed * horizontalInput);
        float moveHorizontal = 0;
        //float moveVertical = 0;

        if (Input.GetKey(KeyCode.I))
        {
            //Debug.Log("Moving Player");
            moveVertical = 1;
        }
        if (Input.GetKey(KeyCode.K))
        {
            moveVertical = -1;
        }
        if (Input.GetKey(KeyCode.J))
        { 
            transform.Rotate(Vector3.down * turnSpeed * Time.deltaTime);
            //moveHorizontal = -1; 
        }
        if (Input.GetKey(KeyCode.L))
        { 
            transform.Rotate(Vector3.up * turnSpeed * Time.deltaTime);
            //moveHorizontal = 1; 
        }

        movement = new Vector3(0.0f, 0.0f, moveVertical);
        rotationalTorque = new Vector3(0.0f, moveHorizontal, 0.0f);
        //movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;



    }
    void FixedUpdate() 
    {
        //Debug.Log(movement * speed);
        //rb.AddTorque(rotationalTorque * turnSpeed);
        newVelocity = new Vector3(transform.forward.x, 0.0f, transform.forward.z);
        rb.velocity = newVelocity * moveVertical * speed;
        //rb.AddForce(transform.forward * moveVertical * speed);
        
    }
}
