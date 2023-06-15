using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainPlaceholderController : MonoBehaviour
{
    // Whether this placeholder contains a track block
    public bool isOccupied = false;
    // Reference to the renderer
    private Renderer _renderer;

    private float _highlightedAlpha = 0.2f;
    private float _emptyAlpha = 0.4f;
    private float _occupiedAlpha = 0f;

    // Reference to the GameManager
    private GameManager gameManager;

    /// <summary>
    /// Mark the current placeholder as occupied.
    /// </summary>
    public void setToOccupied()
    {
        if (isOccupied) return;
        
        Color color = _renderer.material.color;
        color.a = _occupiedAlpha;
        _renderer.material.color = color;
        isOccupied = true;
    }

    /// <summary>
    /// Mark the current placeholder as selected (with a track block hovered above it)
    /// </summary>
    public void setToSelected()
    {
        if (isOccupied) return;
        
        Color color = _renderer.material.color;
        color.a = _highlightedAlpha;
        _renderer.material.color = color;
    }

    /// <summary>
    /// Mark the current placeholder as empty (not occupied by a track block)
    /// </summary>
    public void setToEmpty()
    {
        Color color = _renderer.material.color;
        color.a = _emptyAlpha;
        _renderer.material.color = color;
        isOccupied = false;
    }
    void Start()
    {
        _renderer = GetComponent<Renderer>();
        // Find the GameManager object in the scene
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("TerrainPlaceholderController: GameManager not found in scene.");
        }
    }

    void Update()
    {
        // If the GameManager's isRacing property is true, set this object to inactive
        if (gameManager != null && gameManager.isRacing)
        {
            gameObject.SetActive(false);
        }
        else
        {
            // Otherwise, set it to active
            gameObject.SetActive(true);
        }
    }
}
