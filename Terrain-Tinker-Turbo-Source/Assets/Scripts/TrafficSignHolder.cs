using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficSignHolder : MonoBehaviour
{
    public StandingTrafficSign sign;  // The traffic sign on this block
    public TrafficUIController trafficUIController;  // Reference to the TrafficUIController

    // Start is called before the first frame update
    void Start()
    {
        trafficUIController = GameObject.Find("TrafficUIController").GetComponent<TrafficUIController>();
    }

    public bool CanHoldSign()
    {
        // this block can hold a sign if it doesn't have one
        return sign == null;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Functions only work in racing phase.
        if (!GameManager.Instance.isRacing) return;

        Debug.Log("TrafficSignHolder: OnTriggerEnter");
        // Return if there's no traffic sign on this block
        if (sign == null) return;
        // Note: Only `Stop` sign uses `OnTriggerEnter`, other signs use `OnTriggerStay`.
        if (sign.trafficSignType != TrafficSignType.Stop) return;
        // Perform traffic sign function on players
        if (other.gameObject.CompareTag("Player1"))
        {
            trafficUIController.ShowSignPlayer1(sign.trafficSignType);
            VehicleControl racer = other.gameObject.GetComponentInParent<VehicleControl>();
            sign.PerformStartFunction(racer);
        }
        else if (other.gameObject.CompareTag("Player2"))
        {
            trafficUIController.ShowSignPlayer2(sign.trafficSignType);
            VehicleControl racer = other.gameObject.GetComponentInParent<VehicleControl>();
            sign.PerformStartFunction(racer);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Functions only work in racing phase.
        if (!GameManager.Instance.isRacing) return;

        Debug.Log("TrafficSignHolder: OnTriggerStay");
        // Return if there's no traffic sign on this block
        if (sign == null) return;
        // Perform traffic sign function on players
        if (other.gameObject.CompareTag("Player1"))
        {
            trafficUIController.ShowSignPlayer1(sign.trafficSignType);
            VehicleControl racer = other.GetComponentInParent<VehicleControl>();
            sign.PerformStartFunction(racer);
        }
        else if (other.gameObject.CompareTag("Player2"))
        {
            trafficUIController.ShowSignPlayer2(sign.trafficSignType);
            VehicleControl racer = other.GetComponentInParent<VehicleControl>();
            sign.PerformStartFunction(racer);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Functions only work in racing phase.
        if (!GameManager.Instance.isRacing) return;

        Debug.Log("TrafficSignHolder: OnTriggerStay");
        // Return if there's no traffic sign on this block
        if (sign == null) return;
        // Temp fix, if `Stop` really work, the following line is not needed.
        if (sign.trafficSignType == TrafficSignType.Stop) return;
        // Hide traffic sign for players
        if (other.gameObject.CompareTag("Player1"))
        {
            trafficUIController.HideSignPlayer1(sign.trafficSignType);
            VehicleControl racer = other.GetComponentInParent<VehicleControl>();
            sign.PerformExitFunction(racer);
        }
        else if (other.gameObject.CompareTag("Player2"))
        {
            trafficUIController.HideSignPlayer2(sign.trafficSignType);
            VehicleControl racer = other.GetComponentInParent<VehicleControl>();
            sign.PerformExitFunction(racer);
        }
    }
}
