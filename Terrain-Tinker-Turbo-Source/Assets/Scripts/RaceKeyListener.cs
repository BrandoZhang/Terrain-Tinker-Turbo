using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceKeyListener : MonoBehaviour
{
    private bool isActivated;

    private bool isMenuShown;

    private GameObject MenuCanvas;

    // Start is called before the first frame update
    void Start()
    {
        MenuCanvas = GameManager.Instance.getMenuCanvas();
    }

    // Update is called once per frame
    void Update()
    {
        bool isGameCountingDown = GameManager.Instance.getCountDownStatus();
        bool isRacing = GameManager.Instance.getRacingStatus();
        bool isGameOver = GameManager.Instance.getGameOverStatus();
        
        //Check if Player clicked BackToGame in menu before
        if (GameManager.Instance.getBackToGameStatus())
        {
            isMenuShown = false;
            GameManager.Instance.clearBackToGameStatus();
        }
        
        //Pressing 'X' to start race without completing tiles on grid
        if (!isActivated && !isGameCountingDown && !isRacing && Input.GetKeyDown(KeyCode.X))
        {
            isActivated = true;
            GameManager.Instance.StartRaceNow();

        }
        
        //Pressing 'M' to display menu
        if (!isMenuShown && !isGameCountingDown && !isGameOver && Input.GetKeyDown(KeyCode.M))
        {
            isMenuShown = true;
            GameManager.Instance.DisplayMenu(MenuCanvas);
            
            //Freeze vehicles
            GameManager.Instance.FreezeVehicles();
        }
        else if (isMenuShown && Input.GetKeyDown(KeyCode.M))
        {
            isMenuShown = false;
            GameManager.Instance.HideMenu(MenuCanvas);
            
            //Unfreeze vehicles
            GameManager.Instance.FreeVehicles();
        }
    }
}
