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
    
    void Start() 
    {
        myMainCamera = Camera.main;
    }

    void OnMouseDown() 
    {
        dragPlane = new Plane(myMainCamera.transform.forward, transform.position);
        Ray camRay = myMainCamera.ScreenPointToRay(Input.mousePosition);
    
        float planeDist;
        dragPlane.Raycast(camRay, out planeDist);
        offset = transform.position - camRay.GetPoint(planeDist);
    }

    void OnMouseDrag() 
    {
        Ray camRay = myMainCamera.ScreenPointToRay(Input.mousePosition);

        float planeDist;
        dragPlane.Raycast(camRay, out planeDist);
        transform.position = camRay.GetPoint(planeDist) + offset;

        // Check if the R key is being pressed
        if (Input.GetKeyDown(KeyCode.R))
        {
            // If it is, rotate the object 90 degrees on the Y axis
            transform.Rotate(0, 90, 0);

            // If this is an accelerator terrain, rotate the acceleration direction as well
            AcceleratorTerrainController accelerator = GetComponent<AcceleratorTerrainController>();
            TrackPiece trackPiece = GetComponent<TrackPiece>();
            if (accelerator != null)
            {
                accelerator.accelerationDirection = Quaternion.Euler(0, 90, 0) * accelerator.accelerationDirection;
                trackPiece.accelerationDirection = accelerator.accelerationDirection;
            
                // Find the corresponding TrackPieceData and update its accelerationDirection
                foreach (TrackPieceData piece in TrackData.Instance.trackPieces)
                {
                    if (piece.persistentID == trackPiece.persistentID)
                    {
                        piece.accelerationDirection = trackPiece.accelerationDirection;
                        break;
                    }
                }
            }
        }
    }

    void OnMouseUp() 
    {
        if (isOnTrack && GameManager.Instance.CanPlaceBlock())
        {
            // Make the terrain piece a child of Track
            transform.parent = GameObject.Find("Track").transform;
            // Mark current player has placed a block and switch to the other player
            GameManager.Instance.BlockPlaced();  
        }
    }
    
    
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("TerrainSolidController: OnTriggerEnter()");
        // Check if the terrain piece has entered the Track
        if (other.gameObject.name == "Track")
        {
            isOnTrack = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Check if the terrain piece has left the Track
        if (other.gameObject.name == "Track")
        {
            isOnTrack = false;
        }
    }
}
