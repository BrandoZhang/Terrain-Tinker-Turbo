using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class P2TabListener : MonoBehaviour
{
    [Header("P2 Track Libraries")]
    public GameObject p2TileLibrary;
    public GameObject p2TrafficLibrary;
    public GameObject p2ObstaclesLibrary;
    
    [Header("P2 Tabs")]
    public Button p2Tile;
    public Button p2Traffic;
    public Button p2Obstacles;
    
    //Original color
    private ColorBlock original;
  
    void Awake()
    {
        //Player 2
        p2Tile.onClick.AddListener(P2TileOnClick);
        p2Traffic.onClick.AddListener(P2TrafficOnClick);
        p2Obstacles.onClick.AddListener(P2ObstaclesOnClick);      
        
        ColorBlock colors = p2Tile.GetComponent<Button>().colors;
        original = colors;
    }
    // Start is called before the first frame update
    void Start()
    {
        P2TileOnClick();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void P2TileOnClick()
    {
        //Tile
        ColorBlock colors = p2Tile.GetComponent<Button>().colors;
        colors.selectedColor = Color.red;
        colors.normalColor = Color.red;
        p2Tile.GetComponent<Button>().colors = colors;
        
        //Traffic
        p2Traffic.GetComponent<Button>().colors = original;
        
        //Obstacles
        p2Obstacles.GetComponent<Button>().colors = original;
        
        //Show only Tile Libraries
        GameManager.Instance.SetGameObject(p2TileLibrary,true);
        GameManager.Instance.SetGameObject(p2TrafficLibrary,false);
        GameManager.Instance.SetGameObject(p2ObstaclesLibrary,false);
        
    }
    
    void P2TrafficOnClick()
    {
        //Traffic
        ColorBlock colors = p2Traffic.GetComponent<Button>().colors;
        colors.selectedColor = Color.red;
        colors.normalColor = Color.red;
        p2Traffic.GetComponent<Button>().colors = colors;
        
        //Tiles
        p2Tile.GetComponent<Button>().colors = original;
        
        //Obstacles
        p2Obstacles.GetComponent<Button>().colors = original;
        
        //Show only Traffic Libraries
        GameManager.Instance.SetGameObject(p2TileLibrary,false);
        GameManager.Instance.SetGameObject(p2TrafficLibrary,true);
        GameManager.Instance.SetGameObject(p2ObstaclesLibrary,false);
    }
    
    void P2ObstaclesOnClick()
    {
        //Obstacles
        ColorBlock colors = p2Obstacles.GetComponent<Button>().colors;
        colors.selectedColor = Color.red;
        colors.normalColor = Color.red;
        p2Obstacles.GetComponent<Button>().colors = colors;
        
        //Traffic
        p2Traffic.GetComponent<Button>().colors = original;
        
        //Tiles
        p2Tile.GetComponent<Button>().colors = original;
        
        //Show only Obstacles Libraries
        GameManager.Instance.SetGameObject(p2TileLibrary,false);
        GameManager.Instance.SetGameObject(p2TrafficLibrary,false);
        GameManager.Instance.SetGameObject(p2ObstaclesLibrary,true);
    }
}
