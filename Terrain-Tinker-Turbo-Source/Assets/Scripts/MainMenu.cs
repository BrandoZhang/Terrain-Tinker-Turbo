using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public void PlayGame()
    {
        SceneManager.LoadScene("PlayScene");
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

    public void headBack()
    {
        int currIdx = SceneManager.GetActiveScene().buildIndex;
        int nextIdx = (currIdx <= 2) ? 0 : currIdx-1; 
        SceneManager.LoadScene(nextIdx);
    }

    public void headNext()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
