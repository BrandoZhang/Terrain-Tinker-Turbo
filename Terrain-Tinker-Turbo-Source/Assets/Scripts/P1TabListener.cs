using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class P1TabListener : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("P1 Track Libraries")] 
    public GameObject p1TileLibrary;
    public GameObject p1TrafficLibrary;
    public GameObject p1ObstaclesLibrary;
    
    [Header("P1 Tabs")]
    public Button p1Tile;
    public Button p1Traffic;
    public Button p1Obstacles;
    
    //Original color
    private ColorBlock original;
    
    void Awake()
    {
        //Player 1
        p1Tile.onClick.AddListener(P1TileOnClick);
        p1Traffic.onClick.AddListener(P1TrafficOnClick);
        p1Obstacles.onClick.AddListener(P1ObstaclesOnClick);

        ColorBlock colors = p1Tile.GetComponent<Button>().colors;
        original = colors;
    }
    // Start is called before the first frame update
    void Start()
    {
        //By default - start with Tiles
        P1TileOnClick();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
        void P1TileOnClick()
    {
        //Tile
        ColorBlock colors = p1Tile.GetComponent<Button>().colors;
        colors.selectedColor = Color.blue;
        colors.normalColor = Color.blue;
        p1Tile.GetComponent<Button>().colors = colors;
        
        //Traffic
        p1Traffic.GetComponent<Button>().colors = original;
        
        //Obstacles
        p1Obstacles.GetComponent<Button>().colors = original;
        
        //Show only Tile Libraries
        GameManager.Instance.SetGameObject(p1TileLibrary,true);
        GameManager.Instance.SetGameObject(p1TrafficLibrary,false);
        GameManager.Instance.SetGameObject(p1ObstaclesLibrary,false);
        
    }

    void P1TrafficOnClick()
    {
        //Traffic
        ColorBlock colors = p1Traffic.GetComponent<Button>().colors;
        colors.selectedColor = Color.blue;
        colors.normalColor = Color.blue;
        p1Traffic.GetComponent<Button>().colors = colors;
        
        //Tiles
        p1Tile.GetComponent<Button>().colors = original;
        
        //Obstacles
        p1Obstacles.GetComponent<Button>().colors = original;
        
        //Show only Traffic Libraries
        GameManager.Instance.SetGameObject(p1TileLibrary,false);
        GameManager.Instance.SetGameObject(p1TrafficLibrary,true);
        GameManager.Instance.SetGameObject(p1ObstaclesLibrary,false);
    }

    void P1ObstaclesOnClick()
    {
        //Obstacles
        ColorBlock colors = p1Obstacles.GetComponent<Button>().colors;
        colors.selectedColor = Color.blue;
        colors.normalColor = Color.blue;
        p1Obstacles.GetComponent<Button>().colors = colors;
        
        //Traffic
        p1Traffic.GetComponent<Button>().colors = original;
        
        //Tiles
        p1Tile.GetComponent<Button>().colors = original;
        
        //Show only Obstacles Libraries
        GameManager.Instance.SetGameObject(p1TileLibrary,false);
        GameManager.Instance.SetGameObject(p1TrafficLibrary,false);
        GameManager.Instance.SetGameObject(p1ObstaclesLibrary,true);
    }
}
