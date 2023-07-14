using System;
using UnityEngine;

namespace SoulGames.EasyGridBuilderPro
{
    public class GridXZ<TGridObjectXZ> //This class is not derrived by Monobehavior
    {
        public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged; //Define event, This event is called when grid array cell(object)'s value is changed
        public class OnGridObjectChangedEventArgs : EventArgs //Event class
        {
            public int x; //Event parameters
            public int z; //Event parameters
        }

        public event EventHandler<OnEdgeObjectChangedEventArgs> OnEdgeObjectChanged; //Define event, This event is called when grid array cell(object)'s value is changed
        public class OnEdgeObjectChangedEventArgs : EventArgs //Event class
        {
            public int x; //Event parameters
            public int z; //Event parameters
        }

        private int width; //Grid width
        private int length; //Grid height
        private float cellSize; //Grid cell size
        private Vector3 originPosition; //GridXZ origin position
        private TGridObjectXZ[,] gridArray; //Grid deffined as a multidementional array (Of type TGridObjectXZ)
        private Transform nodeParent; //Parent transform to textmesh objects

        private bool showRuntimeGridText; //Toggle grid text debug view
        private Color32 gridTextColor; //Runtime grid text color
        private float gridTextSizeMultiplier; //Runtime grid text size multiplier
        private bool showCellValueText; //Show runtime grid text cell value
        private string gridTextPrefix; //Runtime grid text prefix text
        private string gridTextSuffix; //Runtime grid text suffix text
        private Vector3 gridTextLocalOffset; //Runtime grid text position offset

        private bool showRuntimeNodeGrid; //Toggle grid node view
        private GameObject[] node; //Visual presentation for grid nodes
        private float gridNodeMarginPercentage; //Grid node Margin relative to the cellsize 
        private Vector3 gridNodeLocalOffset; //Grid node position offset

        public GridXZ(int width, int length, float cellSize, Vector3 originPosition, Func<GridXZ<TGridObjectXZ>, int, int, TGridObjectXZ> createGridObject, bool showRuntimeNodeGrid, bool showRuntimeGridText, Color32 gridTextColor, float gridTextSizeMultiplier, bool showCellValueText, string gridTextPrefix, string gridTextSuffix, Vector3 gridTextLocalOffset, Transform nodeParent, GameObject[] node, float gridNodeMarginPercentage, Vector3 gridNodeLocalOffset) //Constructor
        {
            this.width = width;
            this.length = length;
            this.cellSize = cellSize;
            this.originPosition = originPosition;
            this.nodeParent = nodeParent;

            this.showRuntimeGridText = showRuntimeGridText;
            this.gridTextColor = gridTextColor;
            this.gridTextSizeMultiplier = gridTextSizeMultiplier;
            this.showCellValueText = showCellValueText;
            this.gridTextPrefix = gridTextPrefix;
            this.gridTextSuffix = gridTextSuffix;
            this.gridTextLocalOffset = gridTextLocalOffset;

            this.showRuntimeNodeGrid = showRuntimeNodeGrid;
            this.node = node;
            this.gridNodeMarginPercentage = gridNodeMarginPercentage;
            this.gridNodeLocalOffset = gridNodeLocalOffset;

            gridArray = new TGridObjectXZ[width, length]; //Grid array declaration

            for (int x = 0; x < gridArray.GetLength(0); x++) //Cycle through the grid 1st dimention
            {
                for (int z = 0; z < gridArray.GetLength(1); z++) //Cycle through the grid 2nd dimention
                {
                    gridArray[x, z] = createGridObject(this, x, z); //Create and assign grid objects to the grid array
                }
            }

            if (showRuntimeNodeGrid) //If show grid runtime nodes
            {
                var nodeObjectParent = new GameObject("Node Grid");
                nodeObjectParent.transform.parent = nodeParent;

                for (int x = 0; x < gridArray.GetLength(0); x++) //Cycle through the grid 1st dimention
                {
                    for (int z = 0; z < gridArray.GetLength(1); z++) //Cycle through the grid 2nd dimention
                    {
                        var nodeObject = GameObject.Instantiate(node[UnityEngine.Random.Range (0, node.Length)], GetWorldPosition(x, z) + new Vector3(cellSize, 0, cellSize) * .5f, Quaternion.identity);
                        nodeObject.transform.parent = nodeObjectParent.transform;

                        float paddedNodeSize = (cellSize/100) * gridNodeMarginPercentage;
                        nodeObject.transform.localScale = new Vector3(paddedNodeSize, nodeObject.transform.localScale.y , paddedNodeSize);
                        nodeObject.transform.position = new Vector3(nodeObject.transform.position.x + gridNodeLocalOffset.x, nodeObject.transform.position.y + gridNodeLocalOffset.y, nodeObject.transform.position.z + gridNodeLocalOffset.z);
                    }
                }
            }

            if (showRuntimeGridText) //If show grid runtime text
            {
                var textObjectParent = new GameObject("Text Grid");
                textObjectParent.transform.parent = nodeParent;

                TextMesh[,] debugTextArray = new TextMesh[width, length]; //TextMesh deffined as a multidementional array
                for (int x = 0; x < gridArray.GetLength(0); x++) //Cycle through the grid 1st dimention
                {
                    for (int z = 0; z < gridArray.GetLength(1); z++) //Cycle through the grid 2nd dimention
                    {
                        string toString = "";
                        if (showCellValueText)  toString = gridArray[x, z]?.ToString();
                        debugTextArray[x, z] = CreateWorldText(gridTextPrefix + "\n" + toString + gridTextSuffix, null, GetWorldPosition(x, z) + new Vector3(cellSize, 0, cellSize) * .5f, Mathf.RoundToInt((1.5f * cellSize) * gridTextSizeMultiplier), gridTextColor, TextAnchor.MiddleCenter, TextAlignment.Center); //Calling creat world text function to instantiate textmesh objects inside the grid and assign them to newly created debugTextArray
                        //debugTextArray[x, z].transform.localScale = Vector3.one * .13f;
                        debugTextArray[x, z].transform.position = new Vector3(debugTextArray[x, z].transform.position.x + gridTextLocalOffset.x, debugTextArray[x, z].transform.position.y + gridTextLocalOffset.y, debugTextArray[x, z].transform.position.z + gridTextLocalOffset.z);
                        debugTextArray[x, z].transform.eulerAngles = new Vector3(90, 0, 0); //Rotate instantiated text objects 90 in 'x' axis
                        debugTextArray[x, z].transform.parent = textObjectParent.transform;
                    }
                }

                OnGridObjectChanged += (object sender, OnGridObjectChangedEventArgs eventArgs) => //Subscribe to the event 'OnGridObjectChanged'
                {
                    debugTextArray[eventArgs.x, eventArgs.z].text = gridArray[eventArgs.x, eventArgs.z]?.ToString(); //Update the debug text array
                };
            }
        }

        public int GetWidth() //This function return the grid width
        {
            return width;
        }

        public int GetLength() //This function return the grid height
        {
            return length;
        }

        public float GetCellSize() //This function return the grid cell size
        {
            return cellSize;
        }

        public Vector3 GetWorldPosition(int x, int z) //This function calculate and return cell world position
        {
            return new Vector3(x, 0, z) * cellSize + originPosition;
        }

        public void GetXZ(Vector3 worldPosition, out int x, out int z) //This function return calculated 'x' and 'y' positions
        {
            x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
            z = Mathf.FloorToInt((worldPosition - originPosition).z / cellSize);
        }

        public void SetGridObjectXZ(int x, int z, TGridObjectXZ value) //This function set the values to the specific grid cells
        {
            if (x >= 0 && z >= 0 && x < width && z < length) //Check whether the 'x' and 'y' values are in the range
            {
                gridArray[x, z] = value; //Assign passed value to the specific grid cell in the grid array
                TriggerGridObjectChanged(x, z); //Call the function 'TriggerGridObjectChanged' and pass the cell
            }
        }

        public void TriggerGridObjectChanged(int x, int z) //This function call the event 'OnGridObjectChanged'
        {
            OnGridObjectChanged?.Invoke(this, new OnGridObjectChangedEventArgs { x = x, z = z }); //Call the event and pass parameters 'int x' and 'int z'
        }

        public void TriggerEdgeObjectChanged(int x, int z) //This function call the event 'OnGridObjectChanged'
        {
            OnEdgeObjectChanged?.Invoke(this, new OnEdgeObjectChangedEventArgs { x = x, z = z }); //Call the event and pass parameters 'int x' and 'int z'
        }


        public void SetGridObjectXZ(Vector3 worldPosition, TGridObjectXZ value) //This function grab 'world position' and 'value' from the caller and do the following
        {
            GetXZ(worldPosition, out int x, out int z); //Call 'GetXZ' function and grab calculated values (x&z)
            SetGridObjectXZ(x, z, value); //Call the upper 'SetGridObjectXZ' function and pass all the parameters
        }

        public TGridObjectXZ GetGridObjectXZ(int x, int z) //This function get the values from the specific grid cells
        {
            if (x >= 0 && z >= 0 && x < width && z < length) //Check whether the 'x' and 'y' values are in the range
            {
                return gridArray[x, z]; //If true return values of the specific grid cell
            }
            else
            {
                return default(TGridObjectXZ); //Else return default value (null)
            }
        }

        public TGridObjectXZ GetGridObjectXZ(Vector3 worldPosition) //This function grab 'world position' from the caller and do the following
        {
            int x, z; //Innitialize variables
            GetXZ(worldPosition, out x, out z); //Call 'GetXZ' function and grab calculated values (x&z)
            return GetGridObjectXZ(x, z); //Call the upper 'GetGridObjectXZ' function and return all the parameters
        }

        public Vector2Int ValidateGridPosition(Vector2Int gridPosition) //This function take 'gridPosition' clamp it to be in fixed grid value
        {
            return new Vector2Int
            (
                Mathf.Clamp(gridPosition.x, 0, width - 1),
                Mathf.Clamp(gridPosition.y, 0, length - 1)
            );
        }

        public bool IsValidGridPosition(Vector2Int gridPosition) //This function take 'gridPosition' and check wheather it's inside the fixed grid and return a bool
        {
            int x = gridPosition.x;
            int z = gridPosition.y;

            if (x >= 0 && z >= 0 && x < width && z < length)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsValidGridPositionWithMargin(Vector2Int gridPosition) //This function take 'gridPosition' and check wheather it's inside the fixed grid with entered Margin values and return a bool
        {
            Vector2Int Margin = new Vector2Int(2, 2);
            int x = gridPosition.x;
            int z = gridPosition.y;

            if (x >= Margin.x && z >= Margin.y && x < width - Margin.x && z < length - Margin.y)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #region Extras
        public static TextMesh CreateWorldText(string text, Transform parent = null, Vector3 localPosition = default(Vector3), int fontSize = 40, Color? color = null, TextAnchor textAnchor = TextAnchor.UpperLeft, TextAlignment textAlignment = TextAlignment.Left, int sortingOrder = 5000)
        {
            if (color == null) color = Color.green;
            return CreateWorldText(parent, text, localPosition, fontSize, (Color)color, textAnchor, textAlignment, sortingOrder);
        }
            
        public static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder)
        {
            GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
            Transform transform = gameObject.transform;
            transform.SetParent(parent, false);
            transform.localPosition = localPosition;
            TextMesh textMesh = gameObject.GetComponent<TextMesh>();
            textMesh.anchor = textAnchor;
            textMesh.alignment = textAlignment;
            textMesh.text = text;
            textMesh.fontSize = fontSize;
            textMesh.color = color;
            textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
            return textMesh;
        }
        #endregion
    }
}