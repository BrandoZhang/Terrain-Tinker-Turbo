using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private bool gameOver;

    [Header("Game State")]
    public bool isRacing = false;  // State variable to indicate in which phase the game is
    public TextMeshProUGUI phaseText;  // UI that indicates in which phase the game is
    public TextMeshProUGUI turnText;  // UI that indicates who's turn (only valid in editing phase)
    public TextMeshProUGUI winText;  // UI that will display when game ends
    public TextMeshProUGUI countdownText;
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
    public GameObject player1TrackLibrary;  // Reference to the player 1's TrackLibrary
    public GameObject player2TrackLibrary;  // Reference to the player 2's TrackLibrary

    public Camera mainCamera;
    public Camera player1Camera;
    public Camera player2Camera;
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

        // Initially deactivate player cameras and activate main camera
        mainCamera.enabled = true;
        player1Camera.enabled = false;
        player2Camera.enabled = false;
        
        // Adjust player camera viewports for split screen
        player1Camera.rect = new Rect(0, 0, 0.5f, 1);
        player2Camera.rect = new Rect(0.5f, 0, 0.5f, 1);
        
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
        
        player2TrackLibrary.SetActive(false);  // Start editing with player 1
    }

    public void SwitchTrackLibrary()
    {
        if (currentPlayer == 1)
        {
            player1TrackLibrary.SetActive(true);
            player2TrackLibrary.SetActive(false);
        }
        else
        {
            player1TrackLibrary.SetActive(false);
            player2TrackLibrary.SetActive(true);
        }
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
        SwitchTrackLibrary();
        // Update the turnText field with the remaining blocks
        int remainingBlocks = currentPlayer == 1 ? limit - player1BlockCount : limit - player2BlockCount;
        turnText.text = "Player " + currentPlayer + "'s Turn - " + remainingBlocks + " blocks left";

        // If all blocks have been placed, transition to racing phase
        if (player1BlockCount >= limit && player2BlockCount >= limit)
        {
            player1TrackLibrary.SetActive(false);  // Hide the TrackLibrary when finish editing
            //TransitionToRacingPhase();
            StartCoroutine(Countdown());
        }
    }

    void TransitionToRacingPhase()
    {
        // TODO: Add racing phase transition code here
        isRacing = true;
        phaseText.text = "Racing Phase";
        turnText.text = "";  // Clear for Racing Phase
        
        // Deactivate main camera and activate player cameras
        mainCamera.enabled = false;
        player1Camera.enabled = true;
        player2Camera.enabled = true;
        
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

    IEnumerator Countdown()
    {
        // Disable player controls
        racer1.canMove = false;
        racer2.canMove = false;
        
        // Countdown from 5 to 0
        for (int i = 5; i >= 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSeconds(1);
        }
        // Enable player controls
        racer1.canMove = true;
        racer2.canMove = true;
        
        // Start the racing Phase
        TransitionToRacingPhase();
    }
}
