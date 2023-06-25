using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class StatManager
{
    public string winner;

    public StatManager(string winner)
    {
        this.winner = winner;
    }
}
