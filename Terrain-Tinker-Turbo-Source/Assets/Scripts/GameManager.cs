using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private bool gameOver;

    [Header("Game State")]
    public bool isRacing = false;  // State variable to indicate in which phase the game is
    public TextMeshProUGUI phaseText;  // UI that indicates in which phase the game is
    public TextMeshProUGUI turnText;  // UI that indicates who's turn (only valid in editing phase)
    public TextMeshProUGUI winText;  // UI that will display when game ends
    private int currentPlayer = 1;  // Start with player 1
    private int player1BlockCount = 0;  // Number of track blocks placed by player 1
    private int player2BlockCount = 0;  // Number of track blocks placed by player 2
    private int limit = 3;  // Maximum number of track blocks each player can place

    [Header("Game Object")]
    public GameObject track;  // Reference to the Track GameObject
    private Collider trackCollider;  // Reference to the Collider on the Track GameObject
    private Rigidbody trackRigidbody;  // Reference to the Rigidbody on the Track GameObject
    public RacerController racer1;  // Reference to the first racer's controller
    public RacerController racer2;  // Reference to the second racer's controller

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        // Find the Track GameObject
        track = GameObject.Find("Track");
        
        // Get the Collider from the Track GameObject
        trackCollider = track.GetComponent<Collider>();
        
        // Get the Rigidbody from the Track GameObject
        trackRigidbody = track.GetComponent<Rigidbody>();
        
        // Clear the win text at the start
        winText.text = "";  
        // Start with editing phase
        phaseText.text = "Track Editing Phase";  
        // Initialize the turnText field with the remaining blocks
        int remainingBlocks = currentPlayer == 1 ? limit - player1BlockCount : limit - player2BlockCount;
        turnText.text = "Player " + currentPlayer + "'s Turn - " + remainingBlocks + " blocks left";
        gameOver = false;
        
        // Start in editing phase, configure the collider and rigidbody
        trackCollider.enabled = true;  // For drag-and-drop function
        trackRigidbody.isKinematic = true;  // To fix the track (otherwise will fall due to gravity)
        trackRigidbody.useGravity = false;  // Insane :)
    }

    public bool CanPlaceBlock()
    {
        if (currentPlayer == 1 && player1BlockCount < limit)
        {
            return true;
        }
        else if (currentPlayer == 2 && player2BlockCount < limit)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void BlockPlaced()
    {
        if (currentPlayer == 1)
        {
            player1BlockCount++;
        }
        else if (currentPlayer == 2)
        {
            player2BlockCount++;
        }

        // Switch the current player
        currentPlayer = 3 - currentPlayer;  // If currentPlayer was 1, it becomes 2 and vice versa
        // Update the turnText field with the remaining blocks
        int remainingBlocks = currentPlayer == 1 ? limit - player1BlockCount : limit - player2BlockCount;
        turnText.text = "Player " + currentPlayer + "'s Turn - " + remainingBlocks + " blocks left";

        // If all blocks have been placed, transition to racing phase
        if (player1BlockCount >= limit && player2BlockCount >= limit)
        {
            TransitionToRacingPhase();
        }
    }

    void TransitionToRacingPhase()
    {
        // TODO: Add racing phase transition code here
        isRacing = true;
        phaseText.text = "Racing Phase";
        turnText.text = "";  // Clear for Racing Phase
        
        // Transition to racing phase, configure the collider and rigidbody
        trackCollider.enabled = false;  // Otherwise the racers can drive through placeholders
        trackRigidbody.isKinematic = true;  // To fix the track (otherwise will fall due to gravity)
        trackRigidbody.useGravity = false;  // Insane :)
        
        // Reset racers to starting points
        racer1.ResetToStart();
        racer2.ResetToStart();
    }
    
    public void Player1Finished()
    {
        if (!gameOver)
        {
            Debug.Log("Player 1 Wins!");
            winText.text = "Player 1 Wins!";
            gameOver = true;
        }
    }

    public void Player2Finished()
    {
        if (!gameOver)
        {
            Debug.Log("Player 2 Wins!");
            winText.text = "Player 2 Wins!";
            gameOver = true;
        }
    }
}
