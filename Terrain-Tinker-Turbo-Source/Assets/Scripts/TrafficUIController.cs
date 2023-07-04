using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TrafficUIController : MonoBehaviour
{
    [Header(("Public Traffic Rule Settings"))]
    public float stopSignDuration = 5.0f;
    
    [Header("Player 1 Traffic Sign Settings")]
    public TextMeshProUGUI player1EffectText;
    public Image player1StopSignImage;
    public Image player1NoLeftTurnImage;
    public Image player1NoRightTurnImage;
    
    [Header("Player 2 Traffic Sign Settings")]
    public TextMeshProUGUI player2EffectText;
    public Image player2StopSignImage;
    public Image player2NoLeftTurnImage;
    public Image player2NoRightTurnImage;

    private void Start()
    {
        HideSignPlayer1(TrafficSignType.Stop);
        HideSignPlayer2(TrafficSignType.Stop);
        HideSignPlayer1(TrafficSignType.NoLeftTurn);
        HideSignPlayer2(TrafficSignType.NoLeftTurn);
        HideSignPlayer1(TrafficSignType.NoRightTurn);
        HideSignPlayer2(TrafficSignType.NoRightTurn);
    }

    public void HideSignPlayer1(TrafficSignType signType)
    {
        player1EffectText.text = "";
        switch (signType)
        {
            case TrafficSignType.Stop:
                player1StopSignImage.enabled = false;
                break;
            case TrafficSignType.NoLeftTurn:
                player1NoLeftTurnImage.enabled = false;
                break;
            case TrafficSignType.NoRightTurn:
                player1NoRightTurnImage.enabled = false;
                break;
        }
    }
    
    public void HideSignPlayer2(TrafficSignType signType)
    {
        player2EffectText.text = "";
        switch (signType)
        {
            case TrafficSignType.Stop:
                player2StopSignImage.enabled = false;
                break;
            case TrafficSignType.NoLeftTurn:
                player2NoLeftTurnImage.enabled = false;
                break;
            case TrafficSignType.NoRightTurn:
                player2NoRightTurnImage.enabled = false;
                break;
        }
    }
    
    public void ShowSignPlayer1(TrafficSignType signType)
    {
        switch (signType)
        {
            case TrafficSignType.Stop:
                player1StopSignImage.enabled = true;
                StartCoroutine(
                    CountdownAndHideStopSign(stopSignDuration, player1EffectText, 1)
                    );
                break;
            case TrafficSignType.NoLeftTurn:
                player1NoLeftTurnImage.enabled = true;
                break;
            case TrafficSignType.NoRightTurn:
                player1NoRightTurnImage.enabled = true;
                break;
        }

    }
    
    public void ShowSignPlayer2(TrafficSignType signType)
    {
        switch (signType)
        {
            case TrafficSignType.Stop:
                player2StopSignImage.enabled = true;
                StartCoroutine(
                    CountdownAndHideStopSign(stopSignDuration, player2EffectText, 2)
                    );
                break;
            case TrafficSignType.NoLeftTurn:
                player2NoLeftTurnImage.enabled = true;
                break;
            case TrafficSignType.NoRightTurn:
                player2NoRightTurnImage.enabled = true;
                break;
        }

    }
    private IEnumerator CountdownAndHideStopSign(float duration, TextMeshProUGUI timerText, int playerNumber)
    {
        while (duration > 0)
        {
            timerText.text = "You must stop here for " + duration.ToString("0.0") + " sec.";
            duration -= Time.deltaTime;
            yield return null;
        }
        timerText.text = "";

        // Hide the sign after the countdown is finished
        switch (playerNumber)
        {
            case 1:
                HideSignPlayer1(TrafficSignType.Stop);
                break;
            case 2:
                HideSignPlayer2(TrafficSignType.Stop);
                break;
        }
    }
}
