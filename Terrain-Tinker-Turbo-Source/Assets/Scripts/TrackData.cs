using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This code was used for keeping track data during scene transition. However, as we deprecate the multi-scenes game
/// logic, this code is not used any more. This code is kept in case the game may have "save to archive" function in
/// the future.
/// This is a data structure used to store the state of a track piece. It includes the position, rotation, and type of
/// the track piece, but it could also include other properties like `accelerationDirection` and `accelerationStrength`
/// if you need to save and load this data as well.
///
/// Importantly, instances of `TrackPieceData` are created and managed by the `TrackData` script when saving and loading
/// the track layout.
/// </summary>
public class TrackData : MonoBehaviour
{
    // This list will hold the data for each track piece
    public List<TrackPieceData> trackPieces = new List<TrackPieceData>();
    // A static instance of TrackData that other scripts can access:
    public static TrackData Instance { get; private set; }

    // This method will be called to save the current state of the track
    public void SaveTrackData()
    {
        Debug.Log("SaveTrackData");
        // Clear the current track data
        trackPieces.Clear();

        // Prepare a counter for the persistent ID
        int idCounter = 0;
    
        // Loop through each child of this object (each track piece)
        foreach(Transform child in transform)
        {
            // Get the TrackPiece component from the child object
            TrackPiece trackPiece = child.GetComponent<TrackPiece>();

            // If the child object has a TrackPiece component
            if(trackPiece != null)
            {
                // Create a new TrackPieceData object with the current state of the track piece
                TrackPieceData pieceData = new TrackPieceData
                {
                    persistentID = "TrackPiece" + idCounter++, // Generate a new ID
                    position = child.position,
                    rotation = child.rotation,
                    type = trackPiece.type,
            
                    // Save the acceleration properties
                    accelerationDirection = trackPiece.accelerationDirection,
                    accelerationStrength = trackPiece.accelerationStrength
                };
    
                // Add the TrackPieceData object to the list
                trackPieces.Add(pieceData);
            }
        }
    }

    // This method will be called to load the track to current state
    public void LoadTrackData()
    {
        Debug.Log("LoadTrackData");

        // Clear all children of this object (each track piece)
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // Loop through the stored track data
        foreach (TrackPieceData pieceData in trackPieces)
        {
            // Instantiate a new track piece based on the type
            GameObject trackPieceObj = Instantiate(Resources.Load(pieceData.type), pieceData.position, pieceData.rotation) as GameObject;

            // Set the parent of the new track piece to be this object
            trackPieceObj.transform.SetParent(transform);

            // Store the persistent ID in the GameObject's name
            trackPieceObj.name = pieceData.persistentID;
    
            // Get the TrackPiece component from the instantiated object
            TrackPiece trackPiece = trackPieceObj.GetComponent<TrackPiece>();

            // If the instantiated object has a TrackPiece component
            if(trackPiece != null)
            {
                // Load the acceleration properties
                trackPiece.accelerationDirection = pieceData.accelerationDirection;
                trackPiece.accelerationStrength = pieceData.accelerationStrength;
            }

            // Get the AcceleratorTerrainController component from the instantiated object
            AcceleratorTerrainController accelerator = trackPieceObj.GetComponent<AcceleratorTerrainController>();

            // If the instantiated object has an AcceleratorTerrainController component
            if(accelerator != null)
            {
                // Load the acceleration properties
                accelerator.accelerationDirection = pieceData.accelerationDirection;
                accelerator.accelerationStrength = pieceData.accelerationStrength;
            }
        }
    }

}

// This class will hold the data for a single track piece
[System.Serializable]
public class TrackPieceData
{
    public string persistentID;
    public Vector3 position;
    public Quaternion rotation;
    public string type;
    // Acceleration properties are currently only available when type=="AcceleratorM54x54A00"
    public Vector3 accelerationDirection;
    public float accelerationStrength;
}