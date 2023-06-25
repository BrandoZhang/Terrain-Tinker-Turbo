using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[SerializeField]
public class StatManager
{
    public string winner;
    public string currentTime;
    public string gameVersion;
    public string currentScene;

    public StatManager(string winner, string sceneName)
    {
        currentTime = System.DateTime.Now.ToString("HH:mm dd MMMM, yyyy");
        gameVersion = "June 25th";
        currentScene = sceneName;
        this.winner = winner;
    }
}
