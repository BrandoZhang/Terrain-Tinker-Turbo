using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceKeyListener : MonoBehaviour
{
    private bool isActivated;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //Pressing 'S' to start race without completing tiles on grid
        if (!isActivated && Input.GetKeyDown(KeyCode.S))
        {
            isActivated = true;
            GameManager.Instance.StartRaceNow();
            GameManager.Instance.StartEarly();
            

        }   
    }
}
