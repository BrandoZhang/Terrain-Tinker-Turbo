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

    public void PerformFunction()
    {
        // Perform different functions based on the traffic sign type
        switch (trafficSignType)
        {
            case TrafficSignType.Stop:
                Debug.Log("Stop sign function performed!");
                // TODO: Add stop sign function implementation here
                break;
            case TrafficSignType.NoLeftTurn:
                Debug.Log("No left turn sign function performed!");
                // TODO: Add no left turn sign function implementation here
                break;
            case TrafficSignType.NoRightTurn:
                Debug.Log("No right turn sign function performed!");
                // TODO: Add no right turn sign function implementation here
                break;
        }
    }
}
