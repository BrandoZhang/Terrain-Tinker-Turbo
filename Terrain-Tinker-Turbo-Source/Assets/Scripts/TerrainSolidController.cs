using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TerrainSolidController : MonoBehaviour
{
    // The plane the object is currently being dragged on
    Plane dragPlane;

    // The difference between where the mouse is on the drag plane and 
    // where the origin of the object is on the drag plane
    Vector3 offset;

    Camera myMainCamera;
    
    // Flag to check if the terrain piece is on the Track
    private bool isOnTrack = false;

    // Add a private reference to store a list of currently hovered TerrainPlaceholders
    private List<TerrainPlaceholderController> placeholderControllers = new List<TerrainPlaceholderController>();

    private List<string> terrainData;

    void Start() 
    {
        myMainCamera = Camera.main;
    }

    void OnMouseDown() 
    {
        Ray camRay = myMainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(camRay, out hit))
        {
            // Check if the GameObject hit by the ray or its parent has the "TerrainSolid" tag
            GameObject hitObject = hit.transform.gameObject;
            bool validTag = hitObject.CompareTag("TerrainSolid") || 
                          (hitObject.transform.parent != null && hitObject.transform.parent.CompareTag("TerrainSolid"));

            if (validTag)
            {
                GameObject objectToDrag = hitObject.CompareTag("TerrainSolid") ? hitObject : hitObject.transform.parent.gameObject;
                dragPlane = new Plane(myMainCamera.transform.forward, objectToDrag.transform.position);
        
                float planeDist;
                dragPlane.Raycast(camRay, out planeDist);
                offset = objectToDrag.transform.position - camRay.GetPoint(planeDist);
            }
        }
    }

    void OnMouseDrag() 
    {
        Ray camRay = myMainCamera.ScreenPointToRay(Input.mousePosition);

        float planeDist;
        if (dragPlane.Raycast(camRay, out planeDist))
        {
            GameObject objectToMove = transform.CompareTag("TerrainSolid") ? gameObject : transform.parent.gameObject;
            objectToMove.transform.position = camRay.GetPoint(planeDist) + offset;
        }

        // Check if the R key is being pressed
        if (Input.GetKeyDown(KeyCode.R))
        {
            // If it is, rotate the object 90 degrees on the Y axis
            GameObject objectToMove = transform.CompareTag("TerrainSolid") ? gameObject : transform.parent.gameObject;
            objectToMove.transform.Rotate(0, 90, 0);

            // If this is an accelerator terrain, rotate the acceleration direction as well
            AcceleratorTerrainController accelerator = GetComponent<AcceleratorTerrainController>();
            if (accelerator != null)
            {
                accelerator.accelerationDirection = Quaternion.Euler(0, 90, 0) * accelerator.accelerationDirection;
            }
        }
    }

    void OnMouseUp() 
    {
        if (isOnTrack && GameManager.Instance.CanPlaceBlock() && placeholderControllers.Count > 0)
        {
            // Find the closest TerrainPlaceholder
            TerrainPlaceholderController closestPlaceholderController = null;
            float minDistance = float.MaxValue;

            foreach (TerrainPlaceholderController placeholderController in placeholderControllers)
            {
                float distance = Vector3.Distance(transform.position, placeholderController.gameObject.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestPlaceholderController = placeholderController;
                }
            }

            if (closestPlaceholderController != null)
            {
                // Align with the closest TerrainPlaceholder
                GameObject objectToMove = transform.CompareTag("TerrainSolid") ? gameObject : transform.parent.gameObject;
                objectToMove.transform.position = closestPlaceholderController.gameObject.transform.position;

                // Set the closest TerrainPlaceholder to occupied
                closestPlaceholderController.setToOccupied();

                // Make the terrain piece a child of Track
                objectToMove.transform.parent = GameObject.Find("Track").transform;
                // Mark current player has placed a block and switch to the other player
                GameManager.Instance.BlockPlaced();
                GameManager.Instance.AddTerrainData(gameObject.name);
            }

            // Reset other TerrainPlaceholders
            foreach (TerrainPlaceholderController placeholderController in placeholderControllers)
            {
                if (placeholderController != closestPlaceholderController && !placeholderController.isOccupied)
                {
                    placeholderController.setToEmpty();
                }
            }

            // Clear the list of TerrainPlaceholders
            placeholderControllers.Clear();
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("TerrainPlaceholder"))
        {
            isOnTrack = true;
            TerrainPlaceholderController placeholderController = other.gameObject.GetComponent<TerrainPlaceholderController>();
            // Check if the TerrainPlaceholder is already occupied
            if (!placeholderController.isOccupied)
            {
                placeholderControllers.Add(placeholderController);
                placeholderController.setToSelected();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("TerrainPlaceholder"))
        {
            TerrainPlaceholderController placeholderController = other.gameObject.GetComponent<TerrainPlaceholderController>();
            if (placeholderControllers.Contains(placeholderController))
            {
                placeholderControllers.Remove(placeholderController);
                if (!placeholderController.isOccupied)
                {
                    placeholderController.setToEmpty();
                }
            }

            // If there are no more placeholders in the list, set isOnTrack to false
            if (placeholderControllers.Count == 0)
            {
                isOnTrack = false;
            }
        }
    }
}
