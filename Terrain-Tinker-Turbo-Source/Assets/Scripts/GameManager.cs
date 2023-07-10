using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Proyecto26;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;
using Newtonsoft.Json;


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
    // TODO: Does not separate the counting of traffic signs yet. Need to revise the logic of counting, limiting, and UI.
    private int player1TrafficSignCount = 0;  // Number of traffic signs placed by player 1
    private int player2TrafficSignCount = 0;  // Number of traffic signs placed by player 2
    public int limit = 3;  // Maximum number of track blocks each player can place
    [Header("Game Object")]
    public GameObject track;  // Reference to the Track GameObject
    private Rigidbody trackRigidbody;  // Reference to the Rigidbody on the Track GameObject
    public VehicleControl racer1;  // Reference to the first racer's controller
    // public RacerController racer2;  // Reference to the second racer's controller
    public VehicleControl racer2;  // Reference to the second racer's controller
    public GameObject player1TrackLibrary;  // Reference to the player 1's TrackLibrary
    public GameObject player2TrackLibrary;  // Reference to the player 2's TrackLibrary
    public GameObject player1TrafficSignLibrary;  // Reference to the player 1's TrafficSignLibrary
    public GameObject player2TrafficSignLibrary;  // Reference to the player 2's TrafficSignLibrary
    public GameObject player1DeactivePlane;  // Reference to the player 1's Grey Plane
    public GameObject player2DeactivePlane;  // Reference to the player 2's Grey Plane
    
    public List<TerrData> terrainData = new List<TerrData>();
    public int player1Reset = 0;
    public int player2Reset = 0;
    public List<List<float>> player1ResetPos = new List<List<float>>();
    public List<List<float>> player2ResetPos = new List<List<float>>();
    public List<float> player1Speed = new List<float>();
    public List<float> player2Speed = new List<float>();

    [Header("UI Settings")]
    public Camera mainCamera;
    public Camera player1Camera;
    public Camera player2Camera;

    private TextMeshProUGUI[] text;
    private RawImage img;
    private RawImage[] raceImg;

    public GameObject MenuCanvas;
    public GameObject GameOverCanvas;
    public GameObject HelpCanvas;
    private bool isCountDown;
    private bool isBackToGameClicked;
    
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
             SetImgEnabled("RaceStart", false);
             SetTextEnabled("Tutorial1Text", true); // Disable instruction in tutorial 1
             player1DeactivePlane.SetActive(false);
             //StartCoroutine(Countdown());
        }
        else
        {
            SetImgEnabled("Player1Turn", true);
            SetImgEnabled("Player2Turn", false);
            SetImgEnabled("RaceStart", false);
            player2DeactivePlane.SetActive(true);
        }
        
        SetTextEnabled("TrackLibraryText", false);
        
        //Disable movement of both Player
        racer1.canMove = false;
        racer2.canMove = false;

        /*if (SceneManager.GetActiveScene().name == "PlayScene2")
        {
            //For end race options
            SetTextEnabled("RestartButtonGO", false);
            SetTextEnabled("MenuButtonGO", false);
        }*/
        
        // TODO: Duplicate TrackLibrary in script instead of Unity Editor
        //player2TrackLibrary.SetActive(false);
        //player2TrafficSignLibrary.SetActive(false);

        HideMenu(MenuCanvas);
        HideMenu(GameOverCanvas);
        HideMenu(HelpCanvas);
    }

    public void SwitchTrackLibrary()
    {
        tracklibraryText.text = "Player " + currentPlayer + "'s Track Library";
        if (currentPlayer == 1)
        {
            //player1TrackLibrary.SetActive(true);
            //player1TrafficSignLibrary.SetActive(true);
            //player2TrackLibrary.SetActive(false);
            //player2TrafficSignLibrary.SetActive(false);
            player1DeactivePlane.SetActive(false);
            player2DeactivePlane.SetActive(true);
        }
        else
        {
            //player1TrackLibrary.SetActive(false);
            //player1TrafficSignLibrary.SetActive(false);
            //player2TrackLibrary.SetActive(true);
            //player2TrafficSignLibrary.SetActive(true);
            player1DeactivePlane.SetActive(true);
            player2DeactivePlane.SetActive(false);
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
            player1TrafficSignLibrary.SetActive(false);  // Hide the TrafficSignLibrary when finish editing
            player2TrackLibrary.SetActive(false); 
            player2TrafficSignLibrary.SetActive(false);  
            player1DeactivePlane.SetActive(false);  
            player2DeactivePlane.SetActive(false);  
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
        isCountDown = false;
        isRacing = true;
        phaseText.text = ""; //Kenny - Decluttering scene, player should know it is race phase
        turnText.gameObject.SetActive(false);  // Clear for Racing Phase
        countdownText.gameObject.SetActive(false);
        countdownText.enabled = false;

        
        
        if (SceneManager.GetActiveScene().name == "PlayScene2")
        {
            SetTextEnabled("MessInstruction", false);
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
            raceImg.FirstOrDefault(t => t.name == imgName).gameObject.SetActive(val);
            //raceImg.FirstOrDefault(t => t.name == imgName).enabled = val;
        }
    }
    
    public void SetTextEnabled(string textName, bool val)
    {
        if (text.FirstOrDefault(t => t.name == textName) != null)
        {
            text.FirstOrDefault(t => t.name == textName).gameObject.SetActive(val);
            //text.FirstOrDefault(t => t.name == textName).enabled = val;
        }
    }
    
    public void Player1Finished()
    {
        if (!gameOver)
        {
            Debug.Log("Player 1 Wins!");
            winText.text = "Player 1 Wins!";
            gameOver = true;
            PostToDatabase("Player1");
            Debug.Log("Player1 wins RECORDED");
        }
        
        //Freeze position after reaching finish line
        // racer1.canMove = false;
    }

    public void Player2Finished()
    {
        if (!gameOver)
        {
            Debug.Log("Player 2 Wins!");
            winText.text = "Player 2 Wins!";
            gameOver = true;
            PostToDatabase("Player2");
            Debug.Log("Player2 wins RECORDED");
        }
        
        //Freeze position after reaching finish line
        // racer2.canMove = false;
    }

    IEnumerator Countdown()
    {
        // Disable player controls
        FreezeVehicles();
        
        SetImgEnabled("RaceStart", true);
        
        if (SceneManager.GetActiveScene().name == "Tutorial4")
        {
            SetTextEnabled("Player2Path", false);
            SetTextEnabled("Player1Path", false);
        }
        
        if (SceneManager.GetActiveScene().name == "Tutorial1")
        {
            SetTextEnabled("Tutorial1Text", false);
            SetImgEnabled("PlayerInfoImg", false);
        }
        
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
        
        if (SceneManager.GetActiveScene().name == "Tutorial4")
        {
            SetTextEnabled("MessInstruction", false);
        }

        isCountDown = true;
        
        // Countdown from 5 to 0
        for (int i = 5; i >= 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSeconds(1);
        }
        
        // Enable player controls
        FreeVehicles();

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

    /*public void showEndGameOptions()
    {
        SetTextEnabled("RestartButtonGO", true);
        SetTextEnabled("MenuButtonGO", true);        
    }*/

    public void StartRaceNow()
    {
        player1TrackLibrary.SetActive(false);
        player1TrafficSignLibrary.SetActive(false);
        player2TrackLibrary.SetActive(false);
        player2TrafficSignLibrary.SetActive(false);
        player1DeactivePlane.SetActive(false);
        player2DeactivePlane.SetActive(false);
        tracklibraryText.text = "";
 
        SetImgEnabled("Player1Turn", false);
        SetImgEnabled("Player2Turn", false);

        StartCoroutine(Countdown());     
    }

    public bool getGameOverStatus()
    {
        return gameOver;
    }
    
    private void PostToDatabase(string winnerPlayer)
    {
        string convertedTerrainData = JsonConvert.SerializeObject(terrainData);

        Player1Stats p1Reset = new Player1Stats {ResetCount = player1Reset, ResetSpeed = player1Speed, Position = player1ResetPos};
        Player2Stats p2Reset = new Player2Stats
            { ResetCount = player2Reset, ResetSpeed = player2Speed, Position = player2ResetPos };
        string convertedP1 = JsonConvert.SerializeObject(p1Reset);
        string convertedP2 = JsonConvert.SerializeObject(p2Reset);
        StatManager winner = new StatManager(winnerPlayer, getCurrScene(), convertedTerrainData, convertedP1, convertedP2);
        RestClient.Post("https://ttt-analytics-8ee9b-default-rtdb.firebaseio.com/Beta.json", winner);
    }

    private string getCurrScene()
    {
        Scene currScene = SceneManager.GetActiveScene();
        string currSceneName = currScene.name;
        return currSceneName;
    }

    public void AddTerrainData(string terrainName, Vector3 position, Quaternion rotation)
    {
        List<float> posInfo = new List<float> { position.x, position.y, position.z };
        List<float> rotInfo = new List<float> { rotation.x, rotation.y, rotation.z };
        terrainData.Add(new TerrData{Terrain = terrainName, Position = posInfo, Rotation = rotInfo, PlayerNum = 3- currentPlayer});
    }
    

    public void HideMenu(GameObject obj)
    {
        obj.GetComponent<Canvas>().enabled = false;
    }

    public void DisplayMenu(GameObject obj)
    {
        obj.GetComponent<Canvas>().enabled = true;
    }

    public bool getCountDownStatus()
    {
        return isCountDown;
    }

    public void FreeVehicles()
    {
        racer1.canMove = true;
        racer2.canMove = true;
    }

    public void FreezeVehicles()
    {
        racer1.canMove = false;
        racer2.canMove = false;
    }

    public GameObject getMenuCanvas()
    {
        return MenuCanvas;
    }

    public GameObject getGameOverCanvas()
    {
        return GameOverCanvas;
    }
    
    public GameObject getHelpCanvas()
    {
        return HelpCanvas;
    }
    public void setBackToGameStatus()
    {
        isBackToGameClicked = true;
    }

    public void clearBackToGameStatus()
    {
        isBackToGameClicked = false;
    }

    public bool getBackToGameStatus()
    {
        return isBackToGameClicked;
    }

    public bool getRacingStatus()
    {
        return isRacing;
    }
    
    public class TerrData
    {
        public string Terrain;
        public List<float> Position;
        public List<float> Rotation;
        public int PlayerNum;
    }

    public class Player1Stats
    {
        public int ResetCount;
        public List<float> ResetSpeed;
        public List<List<float>> Position;
    }

    public class Player2Stats
    {
        public int ResetCount;
        public List<float> ResetSpeed;
        public List<List<float>> Position;
    }

    public void ResetStats(string playerName, Vector3 position, float speed)
    {
        List<float> pos = new List<float> { position.x, position.y, position.z };
        if (playerName == "Player1-RallyCar")
        {
            player1Reset++;
            player1ResetPos.Add(pos);
            player1Speed.Add(speed);
        }
        else
        {
            player2Reset++;
            player2ResetPos.Add(pos);
            player2Speed.Add(speed);
        }
    }

    public void SetGameObject(GameObject obj, bool flag)
    {
        obj.SetActive(flag);
    }
}
