using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcceleratorTerrainController : MonoBehaviour
{
    // The direction of acceleration. This should be set in the Unity editor.
    public Vector3 accelerationDirection;
    public float accelerationStrength;
    
    // This function is called when an vehicle enters the terrain's collider.
    private void OnTriggerStay(Collider other)
    {
        Debug.Log("Accelerator Triggered");
        // Check if the other object is a racer (player1 or player2).
        // Remember to add a "Player1" and "Player2" tag to respective players.
        if (other.gameObject.CompareTag("Player1") || other.gameObject.CompareTag("Player2"))
        {
            Debug.Log("Accelerator: Detect the other part is player.");
            // Get the Rigidbody component of the racer.
            Rigidbody racerRigidbody = other.GetComponentInParent<Rigidbody>();

            // Apply a force to the racer in the direction of acceleration.
            racerRigidbody.AddForce(accelerationDirection * accelerationStrength, ForceMode.Force);
        }
    }
}
