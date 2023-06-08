using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
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
