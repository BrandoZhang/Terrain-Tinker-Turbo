using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            // Check which scene is currently loaded
            if (SceneManager.GetActiveScene().name == "TrackEditingScene")
            {
                // TODO: Save the track data
                
                // If it's the track editing scene, load the play scene
                SceneManager.LoadScene("PlayScene");
            }
            else
            {
                // If it's the play scene, load the track editing scene
                SceneManager.LoadScene("TrackEditingScene");
            }
        }
    }
}
