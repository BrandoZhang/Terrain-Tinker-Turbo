using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishBlock : MonoBehaviour
{
    private bool isRaceOver = false;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player1"))
        {
            GameManager.Instance.Player1Finished();
            isRaceOver = true;
        }
        else if (other.CompareTag("Player2"))
        {
            GameManager.Instance.Player2Finished();
            isRaceOver = true;
        }
        
        //Tutorial1: Once racing ends, return back to original view
        if (SceneManager.GetActiveScene().name == "Tutorial1" ||
            SceneManager.GetActiveScene().name == "Tutorial2" ||
            SceneManager.GetActiveScene().name == "Tutorial3")
        {
            GameManager.Instance.mainCameraView();
        }
        
        //PlayScene2: Once racing ends, show menu to restart or return to menu
        if (SceneManager.GetActiveScene().name == "PlayScene2" && isRaceOver)
        {
            GameManager.Instance.showEndGameOptions();
            
            //Reset
            isRaceOver = false;
        }
        
    }
    
}
