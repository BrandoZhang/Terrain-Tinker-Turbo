using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Networking;
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
    public TextMeshProUGUI tracklibraryText;  // UI that indicates the ownership of current track library
    public TextMeshProUGUI countdownText;
    private int currentPlayer = 1;  // Start with player 1
    private int player1BlockCount = 0;  // Number of track blocks placed by player 1
    private int player2BlockCount = 0;  // Number of track blocks placed by player 2
    public int limit = 3;  // Maximum number of track blocks each player can place

    [Header("Game Object")]
    public GameObject track;  // Reference to the Track GameObject
    private Rigidbody trackRigidbody;  // Reference to the Rigidbody on the Track GameObject
    public RacerController racer1;  // Reference to the first racer's controller
    // public RacerController racer2;  // Reference to the second racer's controller
    public WheelController racer2;  // Reference to the second racer's controller
    public GameObject player1TrackLibrary;  // Reference to the player 1's TrackLibrary
    public GameObject player2TrackLibrary;  // Reference to the player 2's TrackLibrary

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
        phaseText.text = "Put Block in Grid";  
        // Initialize the turnText field with the remaining blocks
        int remainingBlocks = currentPlayer == 1 ? limit - player1BlockCount : limit - player2BlockCount;
        turnText.text = "Player " + currentPlayer + "'s Turn - " + remainingBlocks + " blocks left";
        gameOver = false;
        tracklibraryText.text = "Player " + currentPlayer + "'s Track Library";
        
        //Aarti: Disable turn text 
        turnText.gameObject.SetActive(false);
        
        // Start in editing phase, configure the collider and rigidbody
        trackRigidbody.isKinematic = true;  // To fix the track (otherwise will fall due to gravity)
        trackRigidbody.useGravity = false;  // Insane :)
        
        //Check Tutorial
        text = FindObjectsOfType<TextMeshProUGUI>();
        raceImg = FindObjectsOfType<RawImage>();
        if (SceneManager.GetActiveScene().name == "Tutorial1")
        {
             //Hide keyboard controls for now
             setT1KeyboardControls(false);
             SetTextEnabled("TrackLibraryText", false);
             StartCoroutine(Countdown());
        }
        else
        {
            SetImgEnabled("Player1Turn", true);
            SetImgEnabled("Player2Turn", false);
            SetImgEnabled("RaceStart", false);
        }
        
        SetTextEnabled("Tutorial1Text", false); // Disable instruction in tutorial 1

        if (SceneManager.GetActiveScene().name == "PlayScene2")
        {
            SetTextEnabled("RestartButton", false);
            SetTextEnabled("MenuButton", false);
        }
        // TODO: Duplicate TrackLibrary in script instead of Unity Editor
        player2TrackLibrary.SetActive(false);
    }

    public void SwitchTrackLibrary()
    {
        tracklibraryText.text = "Player " + currentPlayer + "'s Track Library";
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
        
        //Aarti: Update turn image
        if(currentPlayer == 1)
        {
            SetImgEnabled("Player1Turn", true);
            SetImgEnabled("Player2Turn", false);
        }
        else if(currentPlayer == 2)
        {
            SetImgEnabled("Player1Turn", false);
            SetImgEnabled("Player2Turn", true);
        }
        
        // If all blocks have been placed, transition to racing phase
        if (player1BlockCount >= limit && player2BlockCount >= limit)
        {
            player1TrackLibrary.SetActive(false);  // Hide the TrackLibrary when finish editing
            tracklibraryText.text = "";
            //TransitionToRacingPhase();
            SetImgEnabled("Player1Turn", false);
            SetImgEnabled("Player2Turn", false);
            
            StartCoroutine(Countdown());
        }
    }

    void TransitionToRacingPhase()
    {
        // TODO: Add racing phase transition code here
        isRacing = true;
        phaseText.text = ""; //Kenny - Decluttering scene, player should know it is race phase
        turnText.gameObject.SetActive(false);  // Clear for Racing Phase
        countdownText.gameObject.SetActive(false);
        countdownText.enabled = false;
        
        if (SceneManager.GetActiveScene().name == "Tutorial2")
        {
            SetTextEnabled("Instruction", false);
            SetTextEnabled("FinishLine", false);
            
            SetImgEnabled("DragnDrop", false);
        }
        
        if (SceneManager.GetActiveScene().name == "Tutorial3")
        {
            SetTextEnabled("RotateInstruction", false);
            SetTextEnabled("FinishLine", false);
            
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
            SetTextEnabled("Tutorial1Text", false);

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
    
    public void SetTextEnabled(string textName, bool val)
    {
        if (text.FirstOrDefault(t => t.name == textName) != null)
        {
            text.FirstOrDefault(t => t.name == textName).enabled = val;
        }
    }
    
    public void Player1Finished()
    {
        if (!gameOver)
        {
            Debug.Log("Player 1 Wins!");
            winText.text = "Player 1 Wins!";
            gameOver = true;
            StartCoroutine(UpdateFirebaseData("WinRoundCount/Player1"));
        }
        
        //Freeze position after reaching finish line
        racer1.canMove = false;
    }

    public void Player2Finished()
    {
        if (!gameOver)
        {
            Debug.Log("Player 2 Wins!");
            winText.text = "Player 2 Wins!";
            gameOver = true;
            StartCoroutine(UpdateFirebaseData("WinRoundCount/Player2"));
        }
        
        //Freeze position after reaching finish line
        racer2.canMove = false;
    }

    IEnumerator Countdown()
    {
        // Disable player controls
        racer1.canMove = false;
        racer2.canMove = false;
        
        SetImgEnabled("RaceStart", true);
        
        // Countdown from 5 to 0
        for (int i = 5; i >= 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSeconds(1);
        }
        // Enable player controls
        racer1.canMove = true;
        racer2.canMove = true;

        // Clear countdown text
        countdownText.text = "";
        
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
        SetTextEnabled("P1P2Instruction", status);

        //Player-1 Control
        SetImgEnabled("P1Control", status);

        //Player-2 Control
         SetImgEnabled("P2Control", status);
    }

    public void showEndGameOptions()
    {
        SetTextEnabled("RestartButton", true);
        SetTextEnabled("MenuButton", true);        
    }
    private IEnumerator UpdateFirebaseData(string player)
    {
        string url = "https://cs526-acf9d-default-rtdb.firebaseio.com/" + player + ".json";
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || 
            www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(www.error);
        }
        else
        {
            int finishCount = int.Parse(www.downloadHandler.text) + 1;

            UnityWebRequest wwwPut = UnityWebRequest.Put(url, finishCount.ToString());
            yield return wwwPut.SendWebRequest();

            if (wwwPut.result == UnityWebRequest.Result.ConnectionError || 
                wwwPut.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(wwwPut.error);
            }
            else
            {
                Debug.Log("Successfully updated Firebase data");
            }
        }
    }


}
