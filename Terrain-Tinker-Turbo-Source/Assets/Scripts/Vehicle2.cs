using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle2 : MonoBehaviour
{
    private float speed  = 10.0f;
    private float turnSpeed = 45.0f;
    
    public Rigidbody rb;
    private float moveVertical;
    private Vector3 newVelocity;


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
;
        float moveHorizontal = 0;

        if (Input.GetKey(KeyCode.I))
        {
            moveVertical = 1;
        }
        if (Input.GetKey(KeyCode.K))
        {
            moveVertical = -1;
        }
        if (Input.GetKey(KeyCode.J))
        { 
            transform.Rotate(Vector3.down * turnSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.L))
        { 
            transform.Rotate(Vector3.up * turnSpeed * Time.deltaTime);
        }
        newVelocity = new Vector3(transform.forward.x, 0.0f, transform.forward.z);
        rb.AddForce(newVelocity * speed * moveVertical);
        //rb.velocity = newVelocity * moveVertical * speed;
        
        //check if the car is airborne when driving
        /*if (!Physics.Raycast(transform.position, -Vector3.up, 0.5f))
        {
            //apply a downward force to the car
            rb.AddForce(-Vector3.up * 10);
        }*/


    }
    void FixedUpdate() 
    {
        newVelocity = new Vector3(transform.forward.x, 0.0f, transform.forward.z);
        rb.velocity = newVelocity * moveVertical * speed;
        
        
        //check if the car is airborne when driving
        
        if (!Physics.Raycast(transform.position, -Vector3.up, 0.5f))
        {
            //apply a downward force to the car
            rb.AddForce(-Vector3.up * 100);
        }
    }
}
