using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }
}
