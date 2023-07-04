using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    private GameObject MenuCanvas;

    void Start()
    {
        MenuCanvas = GameManager.Instance.getMenuCanvas();
    }
    
    // Start is called before the first frame update
    public void PlayGame()
    {
        SceneManager.LoadScene("PlayScene2");
    }

    public void launchTutorial1()
    {
        SceneManager.LoadScene("Tutorial1");
    }

    public void launchTutorial2()
    {
        SceneManager.LoadScene("Tutorial2");
    }

    public void launchTutorial3()
    {
        SceneManager.LoadScene("Tutorial3");
    }
    
    public void launchTutorial4()
    {
        SceneManager.LoadScene("Tutorial4");
    }

    public void headBack()
    {
        int currIdx = SceneManager.GetActiveScene().buildIndex;
        int nextIdx = (currIdx <= 2) ? 0 : currIdx-1; 
        SceneManager.LoadScene(nextIdx);
    }

    public void headNext()
    {
        int currIdx = SceneManager.GetActiveScene().buildIndex;
        int nextIdx = (currIdx >= 5) ? 0 : currIdx + 1;
        SceneManager.LoadScene(nextIdx);
    }

    public void launchMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Restart()
    {
        int currIdx = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currIdx);
    }
    
    public void BackToGame()
    {
        GameManager.Instance.HideMenu(MenuCanvas);
        
        //To inform KeyListener.cs for Menu interaction
        GameManager.Instance.setBackToGameStatus();
        
        //Unfreeze vehicles
        GameManager.Instance.FreeVehicles();
    }
    
    public void ShowGameControl()
    {
        GameManager.Instance.DisplayMenu(GameManager.Instance.getHelpCanvas());
        
        //freeze vehicles
        GameManager.Instance.FreezeVehicles();
    }
    
    public void HideGameControl()
    {
        GameManager.Instance.HideMenu(GameManager.Instance.getHelpCanvas());
        
        //To inform KeyListener.cs for Menu interaction
        GameManager.Instance.setBackToGameStatus();
        
        //Unfreeze vehicles
        GameManager.Instance.FreeVehicles();
    }
}
