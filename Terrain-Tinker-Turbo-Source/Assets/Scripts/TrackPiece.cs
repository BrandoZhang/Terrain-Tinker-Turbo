using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This code was used for keeping track piece data during scene transition. However, as we deprecate the multi-scenes
/// game logic, this code is not used any more. This code is kept in case the game may have "save to archive" function
/// in the future.
/// This is a component that will attach to track piece GameObjects in game scene (e.g., TerrainFlat,
/// TerrainPlaceholder, etc.).
/// It's used to hold information about the track piece, such as its type, acceleration direction, and acceleration
/// strength.
/// </summary>
public class TrackPiece : MonoBehaviour
{
    public string type;

    public string persistentID;
    // Properties of accelerator
    public Vector3 accelerationDirection;
    public float accelerationStrength;
}
