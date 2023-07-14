using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMechanics : MonoBehaviour
{
    private void OnTriggerStay(Collider other)  
    {
        if (other.gameObject.CompareTag("Flip"))
        {
            if (other.gameObject.name == "Collision")   // Check if the collider belongs to the vehicle
            {
                VehicleControl racer = other.transform.root.GetComponent<VehicleControl>(); // Get the VehicleControl from the root of the collider
                if (racer != null)
                {
                    racer.controlFlipped = true;
                }
            }
            
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("control unflipped");
        if (other.gameObject.name == "Collision")   // Check if the collider belongs to the vehicle
        {
            VehicleControl racer = other.transform.root.GetComponent<VehicleControl>(); // Get the VehicleControl from the root of the collider
            if (racer != null)
            {
                racer.controlFlipped = false;
            }
        }
    }
    /*void OnTriggerEnter(Collider other)
   {
       Debug.Log("control flipped");
       VehicleControl vehicleController = other.GetComponent<VehicleControl>();
       if (vehicleController != null && !vehicleController.controlFlipped)
       {
           vehicleController.FlipControls();
       }
   }

   void OnTriggerExit(Collider other)
   {
       Debug.Log("control unflipped");
       VehicleControl vehicleController = other.GetComponent<VehicleControl>();
       if (vehicleController != null && vehicleController.controlFlipped)
       {
           vehicleController.FlipControls();
       }
   }*/
}
