using System.Collections.Generic;
using UnityEngine;
using SoulGames.Utilities;

namespace SoulGames.EasyGridBuilderPro
{
    [CreateAssetMenu(menuName = "SoulGames/Easy Grid Builder Pro/BuildableEdgeObjectTypeSO", order = 0)] //Scriptable object asset path
    public class BuildableEdgeObjectTypeSO : ScriptableObject
    {
        public enum Dir //Create enums for 'Dir'
        {
            Down,
            Left,
            Up,
            Right,
        }

        [Space]
        [Tooltip("Name of the buildable object.")] [Rename("Unique Object Name")]
        public string objectName; //Name of the object
        [Tooltip("Provide a Buildable Object Type Category SO asset. \n(This is used in UI to categorize the icons.)")]
        public BuildableObjectTypeCategorySO buildableCategorySO; //Name of the object
        [Tooltip("Leave these empty for now. These will be used in future updates.)")] [TextArea(4,5)]
        public string objectDescription; //Name of the object
        [Tooltip("Leave these empty for now. These will be used in future updates.")] [TextArea(2,2)]
        public string objectToolTipDescription; //Name of the object
        [Tooltip("Sprite image of the object's icon. \n(This is used in UI to display the icons.)")]
        public Sprite objectIcon; //Name of the object
        [Space]
        [Tooltip("Buildable object prefab. This is the prefab that will be spawned. \n(If use multiple objects, a random one will be choose to spawn.)")]
        public Transform[] objectPrefab; //Final prefab's transform
        [Tooltip("Buildable object visual. This is used to display a temparary ghost visual before spawn the object. If this is empty, Prefab's first object will be used instead")]
        public Transform ghostPrefab; //Visual transform
        [Tooltip("A custome material to be used in temparary ghost object, when object is placeable. \n(If nested children objects exist, check upto 4 nested levels). If this is empty, 'Visual' objects defualt materials will be used.")]
        public Material placeableGhostMaterial; //Custom material to visual object
        [Tooltip("A custome material to be used in temparary ghost object, when object is not placeable. \n(If nested children objects exist, check upto 4 nested levels). If this is empty, 'placeableVisualMaterial' materials will be used.")]
        public Material notPlaceableGhostMaterial; //Custom material to visual object
        [Tooltip("If enabled you can set the layer of the spawnning object.)")]
        public bool setBuiltObjectLayer = false;
        [Tooltip("This layer will be set to the spawnned buildable objetct.")]
        public LayerMask builtObjectLayer;
        [Space]
        [Tooltip("If this is enabled, provided canvas grid will displayed under the object.")]
        public bool showGridBelowObject;
        [Tooltip("Drag and drop the provided Canvas Grid prefab.")]
        public Canvas objectGridCanvas;
        [Tooltip("Drag and drop a provided sprite to use as the grid visual.)")]
        public Sprite gridImageSprite;
        [Tooltip("Canvas grid image will take this color when it is placeable.)")]
        public Color gridImagePlaceableColor = new Color32(255, 255, 255, 255);
        [Tooltip("Canvas grid image will take this color when it is not placeable.)")]
        public Color gridImageNotPlaceableColor = new Color32(255, 255, 255, 255);
        [Space]
        [Tooltip("Enable hold and place objects on grid. \n(Can be used to hold click and paint objects on grid instead of click each time to spawn objects.) \n(Doesn't work with 'placeAndDeselect'. Only use one at a time.)")] [Rename("Hold Key To Place")]
        public bool holdToPlace; //enable holdClickToPlace
        [Tooltip("Enable deselect object automatically after placing it. \n(Doesn't work with 'holdToPlace'. Only use one at a time.)")] [Rename("Place & Auto Deselect")]
        public bool placeAndDeselect; //enable placeAndDeselect
        //[Tooltip("Enable deselect object automatically after placing it. \n(Doesn't work with 'holdToPlace'. Only use one at a time.)")] [Rename("Object Movable After Build")]
        //public bool objectMovable; //enable placeAndDeselect
        [Space]
        [Tooltip("Enable using build conditions for the object type. \n(Can add build conditions via script. Check documentation.)")]
        public bool enableBuildCondition; //enable use conditions
        [Tooltip("If 'Build Condition' is enabled, provide a Build Conditon SO asset. \n('Build Condition' must be enabled to use this.)")]
        public BuildConditionSO buildConditionSO; //use condition cost

        public static Vector2Int GetDirForwardVector(Dir dir)
        {
            switch (dir) //Swtich use 'dir'
            {
                default:
                case Dir.Down: return new Vector2Int(0, -1);
                case Dir.Left: return new Vector2Int(-1, 0);
                case Dir.Up: return new Vector2Int(0, +1);
                case Dir.Right: return new Vector2Int(+1, 0);
            }
        }

        public static Dir GetDir(Vector2Int from, Vector2Int to)
        {
            if (from.x < to.x)
            {
                return Dir.Right;
            }
            else
            {
                if (from.x > to.x)
                {
                    return Dir.Left;
                }
                else
                {
                    if (from.y < to.y)
                    {
                        return Dir.Up;
                    }
                    else
                    {
                        return Dir.Down;
                    }
                }
            }
        }

        public int GetRotationAngle(Dir dir) //This function return rotation an angle (Used to rotate object)
        {
            switch (dir) //Swtich use 'dir'
            {
                default:
                case Dir.Down:  return 0;       //If 'Down' do not rotate
                case Dir.Left:  return 270;     //If 'Left' rotate 90
                case Dir.Up:    return 180;     //If 'Up' rotate 180
                case Dir.Right: return 90;      //If 'Right' rotate 270
            }
        }

        public Vector2Int GetRotationOffset(Dir dir, float cellSize) //This function return an offset (Used to offset object)
        {
            Vector2Int sizeVector = CalculatePlacedObjectSize(cellSize);
            // switch (dir) //Swtich use 'dir'
            // {
            //     default:
            //     case Dir.Down:  return new Vector2Int(0, 0);                            //If 'Down' do not offset
            //     case Dir.Left:  return new Vector2Int(0, sizeVector.x);                 //If 'Left'
            //     case Dir.Up:    return new Vector2Int(sizeVector.x, sizeVector.y);      //If 'Up'
            //     case Dir.Right: return new Vector2Int(sizeVector.y, 0);                 //If 'Right'
            // }
            switch (dir) //Swtich use 'dir'
            {
                default:
                case Dir.Down:  return new Vector2Int(0, 0);                                //If 'Down' do not offset
                case Dir.Left:  return new Vector2Int(0, 0);                                //If 'Left'
                case Dir.Up:    return new Vector2Int(0, 0);                                //If 'Up'
                case Dir.Right: return new Vector2Int(0, 0);                                //If 'Right'
            }
        }

        public Vector2Int CalculatePlacedObjectSize(float cellSize)
        {
            int calculatedWidth = 0;
            int calculatedHeigth = 0;

            float newObjectScaleX = objectPrefab[0].GetComponent<BuildableEdgeObject>().GetObjectScale().x;
            float newObjectScaleZ = objectPrefab[0].GetComponent<BuildableEdgeObject>().GetObjectScale().y;

            calculatedWidth = (int)(Mathf.Ceil((float)newObjectScaleX / cellSize));
            calculatedHeigth = (int)(Mathf.Ceil((float)newObjectScaleZ / cellSize));
        
            return new Vector2Int(calculatedWidth, calculatedHeigth);
        }

        public List<Vector2Int> GetGridPositionList(Vector2Int offset, Dir dir, float cellSize) //This function get all the grid positions that the object use
        {
            Vector2Int sizeVector = CalculatePlacedObjectSize(cellSize);
            List<Vector2Int> gridPositionList = new List<Vector2Int>(); //Create a list to store vector2 grid posisions 

            switch (dir) //Swtich use 'dir'
            {
                default:
                case Dir.Down:
                    for (int x = 0; x < sizeVector.x; x++) //Cycles through the 'width'
                    {
                        for (int y = 0; y < sizeVector.y; y++) //Cycles through the 'height'
                        {
                            gridPositionList.Add(offset + new Vector2Int(x, y)); //Add positions to the 'gridPositionList' list
                        }
                    }
                    break;
                case Dir.Up:
                    for (int x = 0; x < sizeVector.x; x++) //Cycles through the 'width'
                    {
                        for (int y = 0; y < sizeVector.y; y++) //Cycles through the 'height'
                        {
                            gridPositionList.Add(offset - new Vector2Int(x, y)); //Add positions to the 'gridPositionList' list
                        }
                    }
                    break;
                case Dir.Left:
                    for (int x = 0; x < sizeVector.y; x++) //Cycles through the 'width'
                    {
                        for (int y = 0; y < sizeVector.x; y++) //Cycles through the 'height'
                        {
                            gridPositionList.Add(offset + new Vector2Int(x, y)); //Add positions to the 'gridPositionList' list
                        }
                    }
                    break;
                case Dir.Right:
                    for (int x = 0; x < sizeVector.y; x++) //Cycles through the 'width'
                    {
                        for (int y = 0; y < sizeVector.x; y++) //Cycles through the 'height'
                        {
                            gridPositionList.Add(offset - new Vector2Int(x, y)); //Add positions to the 'gridPositionList' list
                        }
                    }
                    break;
            }
            return gridPositionList; //Return the calculated 'gridPositionList' list
        }
    }
}