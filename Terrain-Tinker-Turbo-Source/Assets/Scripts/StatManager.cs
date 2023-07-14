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
    public string terrains;
    public string p1Reset;
    public string p2Reset;

    public StatManager(string winner, string sceneName, string terrains, string p1Re, string p2Re)
    {
        currentTime = System.DateTime.Now.ToString("HH:mm dd MMMM, yyyy");
        gameVersion = "Beta";
        currentScene = sceneName;
        this.winner = winner;
        this.terrains = terrains;
        p1Reset = p1Re;
        p2Reset = p2Re;
    }
}
