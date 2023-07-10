using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Define an enum for the item types
public enum ItemType
{
    ReverseLeftRight,
    MovingBlocks
}

public class ItemEffect : MonoBehaviour
{
    public ItemType itemType;
    public float ReverseLeftRightDuration = 5.0f;

    private void OnTriggerEnter(Collider other)
    {
        if (!GameManager.Instance.isRacing) return;
        
        switch (itemType)
        {
            case ItemType.MovingBlocks:
                break;
            
            case ItemType.ReverseLeftRight:
                if (other.gameObject.CompareTag("Player1"))
                {
                    // Perform reversing left and right control for player 2
                    VehicleControl player2Control = GameObject.FindWithTag("Player2").GetComponent<VehicleControl>();
                    GameManager.Instance.StartCoroutine(player2Control.ReverseControlForSeconds(ReverseLeftRightDuration, 2));
                    // This is a one-time item
                    gameObject.SetActive(false);
                }
                if (other.gameObject.CompareTag("Player2"))
                {
                    // Perform reversing left and right control for player 1
                    VehicleControl player1Control = GameObject.FindWithTag("Player1").GetComponent<VehicleControl>();
                    GameManager.Instance.StartCoroutine(player1Control.ReverseControlForSeconds(ReverseLeftRightDuration, 1));
                    // This is a one-time item
                    gameObject.SetActive(false);
                }
                break;
        }
    }
}
