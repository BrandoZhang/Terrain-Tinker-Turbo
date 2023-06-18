using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishBlock : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player1"))
        {
            GameManager.Instance.Player1Finished();
        }
        else if (other.CompareTag("Player2"))
        {
            GameManager.Instance.Player2Finished();
        }
        
        //Tutorial1: Once racing ends, return back to original view
        if (SceneManager.GetActiveScene().name == "Tutorial1" ||
            SceneManager.GetActiveScene().name == "Tutorial2" ||
            SceneManager.GetActiveScene().name == "Tutorial3")
        {
            GameManager.Instance.mainCameraView();
        }
    }
    
}
