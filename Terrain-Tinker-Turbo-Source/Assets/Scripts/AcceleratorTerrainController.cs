using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcceleratorTerrainController : MonoBehaviour
{
    // The direction of acceleration. This should be set in the Unity editor.
    public Vector3 accelerationDirection;
    public float accelerationStrength;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    // This function is called when an vehicle enters the terrain's collider.
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Accelerator Triggered");
        // Check if the other object is a racer (player or other competitors).
        // Remember to add a "Racer" tag to player and other competitors.
        if (other.gameObject.CompareTag("Racer"))
        {
            // Get the Rigidbody component of the racer.
            Rigidbody racerRigidbody = other.gameObject.GetComponent<Rigidbody>();

            // Apply a force to the racer in the direction of acceleration.
            racerRigidbody.AddForce(accelerationDirection * accelerationStrength, ForceMode.Force);
        }
    }
}
