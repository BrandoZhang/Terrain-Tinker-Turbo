using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

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
    public int limit = 3;  // Maximum number of track blocks each player can place

    [Header("Game Object")]
    public GameObject track;  // Reference to the Track GameObject
    private Rigidbody trackRigidbody;  // Reference to the Rigidbody on the Track GameObject
    public RacerController racer1;  // Reference to the first racer's controller
    public RacerController racer2;  // Reference to the second racer's controller

    public Camera mainCamera;
    public Camera player1Camera;
    public Camera player2Camera;

    private TextMeshProUGUI[] text;
    private RawImage img;
    private RawImage[] raceImg;
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
        
        //Aarti: Disable turn text 
        turnText.gameObject.SetActive(false);

        if (!SceneManager.GetActiveScene().name.Contains("Tutorial"))
        {
            raceImg = FindObjectsOfType<RawImage>();
            raceImg.FirstOrDefault(t => t.name == "Player1Turn").enabled = true;
            raceImg.FirstOrDefault(t => t.name == "Player2Turn").enabled = false;
            raceImg.FirstOrDefault(t => t.name == "RaceStart").enabled = false;
            
        }
        
        
        
        // Start in editing phase, configure the collider and rigidbody
        trackRigidbody.isKinematic = true;  // To fix the track (otherwise will fall due to gravity)
        trackRigidbody.useGravity = false;  // Insane :)
        
        //Check Tutorial
        text = FindObjectsOfType<TextMeshProUGUI>();
        if (SceneManager.GetActiveScene().name == "Tutorial1")
        {
             //Hide keyboard controls for now
             setT1KeyboardControls(false);
             StartCoroutine(Countdown());
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
    { if (currentPlayer == 1)
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
        
        //Aarti: Update turn image
        if(currentPlayer == 1)
        {
            SetImgEnabled("Player1Turn", true);
            SetImgEnabled("Player2Turn", false);
        }
        else
        {
            SetImgEnabled("Player1Turn", false);
            SetImgEnabled("Player2Turn", true);
        }
        
        // If all blocks have been placed, transition to racing phase
        if (player1BlockCount >= limit && player2BlockCount >= limit)
        {
            //TransitionToRacingPhase();
            SetImgEnabled("Player1Turn", false);
            SetImgEnabled("Player2Turn", false);
            SetImgEnabled("RaceStart", true);

            StartCoroutine(Countdown());
        }
    }

    void TransitionToRacingPhase()
    {
        
        // TODO: Add racing phase transition code here
        isRacing = true;
        phaseText.text = "Racing Phase";
        turnText.gameObject.SetActive(false);  // Clear for Racing Phase
        countdownText.gameObject.SetActive(false);
        countdownText.enabled = false;
        
        if (SceneManager.GetActiveScene().name == "Tutorial2")
        {
            TextMeshProUGUI[] text = FindObjectsOfType<TextMeshProUGUI>();
            text.FirstOrDefault(t => t.name == "Instruction").enabled = false;
            text.FirstOrDefault(t => t.name == "FinishLine").enabled = false;
            
            SetImgEnabled("DragnDrop", false);
        }
        
        if (SceneManager.GetActiveScene().name == "Tutorial3")
        {
            TextMeshProUGUI[] text = FindObjectsOfType<TextMeshProUGUI>();
            text.FirstOrDefault(t => t.name == "RotateInstruction").enabled = false;
            text.FirstOrDefault(t => t.name == "FinishLine").enabled = false;
            
            SetImgEnabled("RotateImg", false);
        }


        // Deactivate main camera and activate player cameras
        mainCamera.enabled = false;
        player1Camera.enabled = true;
        player2Camera.enabled = true;
        
        // Transition to racing phase, configure the collider and rigidbody
        trackRigidbody.isKinematic = true;  // To fix the track (otherwise will fall due to gravity)
        trackRigidbody.useGravity = false;  // Insane :)
        
        //Tutorial 1
        if (SceneManager.GetActiveScene().name == "Tutorial1")
        {
            //Remove "Driver Watch Your Front"
            text.FirstOrDefault(t => t.name == "Tutorial1Text").enabled = false;

            //Remove Counter Text
            countdownText.text = "";
            
            //Display Keyboard Control and Removes when either player hit keys
            setT1KeyboardControls(true);
        }
        
        SetImgEnabled("RaceStart", false);

        // Reset racers to starting points
        racer1.ResetToStart();
        racer2.ResetToStart();
    }

    public void SetImgEnabled(string imgName, bool val)
    {
        if (raceImg.FirstOrDefault(t => t.name == imgName) != null)
        {
            raceImg.FirstOrDefault(t => t.name == imgName).enabled = val;
        }
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

    public void mainCameraView()
    {
        mainCamera.enabled = true;
        player1Camera.enabled = false;
        player2Camera.enabled = false;
    }

    public void setT1KeyboardControls(bool status)
    {
        //Keyboard Control text
         text.FirstOrDefault(t => t.name == "P1P2Instruction").enabled = status;
         
         //Player-1 Control
         img = FindObjectsOfType<RawImage>().FirstOrDefault(t => t.name == "P1Control");
         img.enabled = status;
         
         //Player-2 Control
         img = FindObjectsOfType<RawImage>().FirstOrDefault(t => t.name == "P2Control");
         img.enabled = status;
    }

}
