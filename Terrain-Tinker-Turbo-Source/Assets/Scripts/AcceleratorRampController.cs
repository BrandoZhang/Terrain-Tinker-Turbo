using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcceleratorRampController : MonoBehaviour
{
    // The direction of acceleration. This should be set in the Unity editor.
    public Vector3 accelerationDirection;
    public float accelerationStrength;
    
    // This function is called when an vehicle enters the terrain's collider.
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player1") || other.gameObject.CompareTag("Player2"))
        {
            Debug.Log("Accelerator: Detect the other part is player.");
            // Get the Rigidbody component of the racer.
            Rigidbody racerRigidbody = other.GetComponentInParent<Rigidbody>();

            // Convert acceleration direction from local space to world space
            Vector3 worldSpaceAcceleration = transform.TransformDirection(accelerationDirection);

            // Apply a force to the racer in the direction of acceleration.
            racerRigidbody.AddForce(worldSpaceAcceleration * accelerationStrength, ForceMode.Force);
        }
    }
}
