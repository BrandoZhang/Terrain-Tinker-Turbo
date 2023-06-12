using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public TrackData trackData;
    public GameObject track;  // Reference to the Track GameObject
    private Collider trackCollider;  // Reference to the Collider on the Track GameObject
    private Rigidbody trackRigidbody;  // Reference to the Rigidbody on the Track GameObject

    // Start is called before the first frame update
    void Start()
    {
        // Find the Track GameObject
        track = GameObject.Find("Track");
        
        // Get the Collider from the Track GameObject
        trackCollider = track.GetComponent<Collider>();
        
        // Get the Rigidbody from the Track GameObject
        trackRigidbody = track.GetComponent<Rigidbody>();

        // Check which scene is currently loaded
        if (SceneManager.GetActiveScene().name == "PlayScene")
        {
            // Load the track data
            TrackData.Instance.LoadTrackData();
            
            // Disable the collider on the Track GameObject
            trackCollider.enabled = false;

            // Disable the Rigidbody on the Track GameObject
            trackRigidbody.isKinematic = true;

            // Disable gravity for the Track GameObject
            trackRigidbody.useGravity = false;
        }
        else if (SceneManager.GetActiveScene().name == "TrackEditingScene" && TrackData.Instance.trackPieces.Count > 0)
        {
            // Clear all children of Track
            ClearTrack();

            // Load the track data
            TrackData.Instance.LoadTrackData();
            
            // Enable the collider on the Track GameObject
            trackCollider.enabled = true;

            // Enable the Rigidbody on the Track GameObject
            trackRigidbody.isKinematic = true;

            // Disable gravity for the Track GameObject
            trackRigidbody.useGravity = false;
        }
    }

    // Clear all children of Track
    private void ClearTrack()
    {
        foreach(Transform child in track.transform)
        {
            Destroy(child.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            // Check which scene is currently loaded
            if (SceneManager.GetActiveScene().name == "TrackEditingScene")
            {
                // Save the track data
                TrackData.Instance.SaveTrackData();
            }

            // Switch scenes
            if (SceneManager.GetActiveScene().name == "PlayScene")
            {
                SceneManager.LoadScene("TrackEditingScene");
            }
            else if (SceneManager.GetActiveScene().name == "TrackEditingScene")
            {
                SceneManager.LoadScene("PlayScene");
            }
        }
    }
}
