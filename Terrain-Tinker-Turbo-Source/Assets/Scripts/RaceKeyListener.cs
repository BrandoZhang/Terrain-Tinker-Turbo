using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceKeyListener : MonoBehaviour
{
    private bool isActivated;

    private bool isMenuShown;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bool isGameCountingDown = GameManager.Instance.getCountDownStatus();
        
        //Pressing 'X' to start race without completing tiles on grid
        if (!isActivated && Input.GetKeyDown(KeyCode.X))
        {
            isActivated = true;
            GameManager.Instance.StartRaceNow();

        }
        
        //Pressing 'M' to display menu
        if (!isMenuShown && !isGameCountingDown && Input.GetKeyDown(KeyCode.M))
        {
            isMenuShown = true;
            GameManager.Instance.DisplayMenu();
            
            //Freeze vehicles
            GameManager.Instance.FreezeVehicles();
        }
        else if (isMenuShown && Input.GetKeyDown(KeyCode.M))
        {
            isMenuShown = false;
            GameManager.Instance.HideMenu();
            
            //Unfreeze vehicles
            GameManager.Instance.FreeVehicles();
        }
    }
}
