using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Define an enum for the traffic sign types
public enum TrafficSignType
{
    Stop,
    NoLeftTurn,
    NoRightTurn
}

public class StandingTrafficSign : MonoBehaviour
{
    
    public Vector3 StandingOffset;
    public TrafficSignType trafficSignType;

    public void PerformStartFunction(VehicleControl racer)
    {
        // Perform different functions based on the traffic sign type
        switch (trafficSignType)
        {
            case TrafficSignType.Stop:
                Debug.Log("Stop sign function performed!");
                racer.StartCoroutine(racer.FreezeCarForSeconds(5));
                break;
            case TrafficSignType.NoLeftTurn:
                //Debug.Log("No left turn sign function performed!");
                if (racer != null)
                {
                    racer.canTurnLeft = false;
                }
                break;
            case TrafficSignType.NoRightTurn:
                //Debug.Log("No right turn sign function performed!");
                if (racer != null)
                {
                    racer.canTurnRight = false;
                }
                break;
        }
    }

    public void PerformExitFunction(VehicleControl racer)
    {
        // Perform different functions based on the traffic sign type
        switch (trafficSignType)
        {
            case TrafficSignType.Stop:
                break;
            case TrafficSignType.NoLeftTurn:
                //Debug.Log("No left turn sign function performed!");
                if (racer != null)
                {
                    racer.canTurnLeft = true;
                }
                break;
            case TrafficSignType.NoRightTurn:
                //Debug.Log("No right turn sign function performed!");
                if (racer != null)
                {
                    racer.canTurnRight = true;
                }
                break;
        }
    }
}
