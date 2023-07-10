using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TouchScript.Gestures;
using TouchScript.Gestures.TransformGestures;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class DraggableTrafficSign : MonoBehaviour
{
    public GameObject trafficSign3DPrefab;  // Reference to the 3D Prefab
    private Vector3 originalPosition;
    private List<TrafficSignHolder> collidedHolders = new List<TrafficSignHolder>();
    
    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position;
    }

    private void OnEnable()
    {
        GetComponent<TransformGesture>().TransformCompleted += TransformCompletedHandler;
    }

    private void OnDisable()
    {
        GetComponent<TransformGesture>().TransformCompleted -= TransformCompletedHandler;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Debug.Log("DraggableTrafficSign: OnTriggerEnter!");
        TrafficSignHolder holder = other.GetComponent<TrafficSignHolder>();
        if (holder != null && holder.CanHoldSign())
        {
            collidedHolders.Add(holder);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Debug.Log("DraggableTrafficSign: OnTriggerExit!");
        TrafficSignHolder holder = other.GetComponent<TrafficSignHolder>();
        if (holder != null)
        {
            collidedHolders.Remove(holder);
        }
    }

    private void TransformCompletedHandler(object sender, EventArgs e)
    {
        // Debug.Log("Transform Completed!");
        if (collidedHolders.Count > 0)
        {
            // Find the closest holder
            TrafficSignHolder closestHolder = null;
            float minDistance = float.MaxValue;
            foreach (TrafficSignHolder holder in collidedHolders)
            {
                float distance = Vector3.Distance(transform.position, holder.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestHolder = holder;
                }
            }

            // Instantiate the 3D prefab at the position of the holder
            Vector3 offset = trafficSign3DPrefab.GetComponent<StandingTrafficSign>().StandingOffset;
            GameObject trafficSign3D = Instantiate(trafficSign3DPrefab, 
                                             closestHolder.transform.position + offset, 
                                                    Quaternion.identity);
            trafficSign3D.transform.parent = closestHolder.transform;
            // Configures the real component that performs traffic rule functions 
            closestHolder.sign = trafficSign3D.GetComponent<StandingTrafficSign>();  
            GameManager.Instance.BlockPlaced();
            // Debug.Log("Created traffic sign 3D prefab at location: " + trafficSign3D.transform.position);
            
            // Destroy current gameObject
            // Destroy(gameObject);
            transform.parent = closestHolder.transform;
            transform.GetComponent<Collider>().enabled = false;
        }
        else
        {
            // No valid holder found, reset to original position
            transform.position = originalPosition;
        }

        // Clear the list of collided holders
        collidedHolders.Clear();
    }

    private void Update()
    {
        if (GameManager.Instance.isRacing)
        {
            Destroy(gameObject);
        }
    }
}
