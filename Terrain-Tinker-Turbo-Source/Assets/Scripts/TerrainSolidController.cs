using System;
using System.Collections;
using System.Collections.Generic;
using TouchScript.Behaviors;
using UnityEngine;
using TouchScript.Gestures;
using TouchScript.Gestures.TransformGestures;

public class TerrainSolidController : MonoBehaviour
{
    private Vector3 originalPosition;
    private List<TerrainPlaceholderController> collidedPlaceholders = new List<TerrainPlaceholderController>();
    
    // Flag to check if the terrain piece is on the Track
    private bool isOnTrack = false;
    // Flag to indicate whether the object is currently being clicked on
    private bool isClicked = false;

    // Add a private reference to store a list of currently hovered TerrainPlaceholders
    private List<TerrainPlaceholderController> placeholderControllers = new List<TerrainPlaceholderController>();

    private List<string> terrainData;

    void Start() 
    {
        originalPosition = transform.position;
    }

    private void OnEnable()
    {
        GetComponent<PressGesture>().Pressed += PressedHandler;
        GetComponent<ReleaseGesture>().Released += ReleasedHandler;
        GetComponent<TransformGesture>().TransformCompleted += TransformCompletedHandler;
    }

    private void OnDisable()
    {
        GetComponent<PressGesture>().Pressed -= PressedHandler;
        GetComponent<ReleaseGesture>().Released -= ReleasedHandler;
        GetComponent<TransformGesture>().TransformCompleted -= TransformCompletedHandler;
    }

    private void PressedHandler(object sender, EventArgs e)
    {
        // Set the flag to true when the object is clicked on
        isClicked = true;
    }

    private void ReleasedHandler(object sender, EventArgs e)
    {
        // Set the flag to false when the object is released
        isClicked = false;
    }

    void Update()
    {
        // Check if the R key is being pressed, but only if the object is currently being clicked on
        if (isClicked && Input.GetKeyDown(KeyCode.R))
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

    private void TransformCompletedHandler(object sender, EventArgs e)
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
            
            // After aligning the block to the placeholder, disable its Transformer and TransformGesture component.
            Transformer transformer = GetComponent<Transformer>();
            TransformGesture transformGesture = GetComponent<TransformGesture>();
            if (transformer != null)
            {
                transformer.enabled = false;
            }
            if (transformGesture != null)
            {
                transformGesture.enabled = false;
            }
        }
        else
        {
            // If the block is not on track, reset its position to the original position.
            transform.position = originalPosition;
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
