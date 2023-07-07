using System;
using UnityEngine;

namespace SoulGames.EasyGridBuilderPro
{
    public class GridXY<TGridObjectXY> //This class is not derrived by Monobehavior
    {
        public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged; //Define event, This event is called when grid array cell(object)'s value is changed
        public class OnGridObjectChangedEventArgs : EventArgs //Event class
        {
            public int x; //Event parameters
            public int y; //Event parameters
        }
        
        public event EventHandler<OnEdgeObjectChangedEventArgs> OnEdgeObjectChanged; //Define event, This event is called when grid array cell(object)'s value is changed
        public class OnEdgeObjectChangedEventArgs : EventArgs //Event class
        {
            public int x; //Event parameters
            public int y; //Event parameters
        }

        private int width; //Grid width
        private int length; //Grid height
        private float cellSize; //Grid cell size
        private Vector3 originPosition; //Grid origin position
        private TGridObjectXY[,] gridArray; //Grid deffined as a multidementional array (Of type TGridObjectXY)
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

        public GridXY(int width, int length, float cellSize, Vector3 originPosition, Func<GridXY<TGridObjectXY>, int, int, TGridObjectXY> createGridObject, bool showRuntimeNodeGrid, bool showRuntimeGridText, Color32 gridTextColor, float gridTextSizeMultiplier, bool showCellValueText, string gridTextPrefix, string gridTextSuffix, Vector3 gridTextLocalOffset, Transform nodeParent, GameObject[] node, float gridNodeMarginPercentage, Vector3 gridNodeLocalOffset) //Constructor
        {
            this.width = width; //Assign width
            this.length = length; //Assign height
            this.cellSize = cellSize; //Assign cell size
            this.originPosition = originPosition; //Assign origin position
            this.nodeParent = nodeParent; //Assign nodeParent

            this.showRuntimeGridText = showRuntimeGridText; //Assign showRuntimeGridText
            this.gridTextColor = gridTextColor; //Assign runtimeGridTextColor
            this.gridTextSizeMultiplier = gridTextSizeMultiplier; //Assign runtimeGridTextSizeMultiplier
            this.showCellValueText = showCellValueText; //Assign showCellValueText
            this.gridTextPrefix = gridTextPrefix; //Assign runtimeGridTextPrefix
            this.gridTextSuffix = gridTextSuffix; //Assign runtimeGridTextSuffix
            this.gridTextLocalOffset = gridTextLocalOffset; //Assign runtimeGridTextOffset
            
            this.showRuntimeNodeGrid = showRuntimeNodeGrid; //Assign showRuntimeNodeGrid
            this.node = node; //Assign node
            this.gridNodeMarginPercentage = gridNodeMarginPercentage; //Assign gridNodeMarginPercentage
            this.gridNodeLocalOffset = gridNodeLocalOffset; //Assign gridNodeLocalOffset

            gridArray = new TGridObjectXY[width, length]; //Grid array declaration

            for (int x = 0; x < gridArray.GetLength(0); x++) //Cycle through the grid 1st dimention
            {
                for (int y = 0; y < gridArray.GetLength(1); y++) //Cycle through the grid 2nd dimention
                {
                    gridArray[x, y] = createGridObject(this, x, y); //Create and assign grid objects to the grid array
                }
            }

            if (showRuntimeNodeGrid) //If show grid runtime nodes
            {
                var nodeObjectParent = new GameObject("Node Grid");
                nodeObjectParent.transform.parent = nodeParent;

                for (int x = 0; x < gridArray.GetLength(0); x++) //Cycle through the grid 1st dimention
                {
                    for (int y = 0; y < gridArray.GetLength(1); y++) //Cycle through the grid 2nd dimention
                    {
                        var nodeObject = GameObject.Instantiate(node[UnityEngine.Random.Range (0, node.Length)], GetWorldPosition(x, y) + new Vector3(cellSize, 0, cellSize) * .5f, Quaternion.identity);
                        nodeObject.transform.parent = nodeObjectParent.transform;

                        float paddedNodeSize = (cellSize/100) * gridNodeMarginPercentage;
                        nodeObject.transform.localScale = new Vector3(paddedNodeSize, nodeObject.transform.localScale.y , paddedNodeSize);
                        nodeObject.transform.position = new Vector3(nodeObject.transform.position.x + gridNodeLocalOffset.x, (nodeObject.transform.position.y + cellSize / 2) + gridNodeLocalOffset.y, (nodeObject.transform.position.z - cellSize / 2) + gridNodeLocalOffset.z);
                        nodeObject.transform.eulerAngles = new Vector3(90, 0, 0); //Rotate instantiated node objects 90 in 'x' axis
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
                    for (int y = 0; y < gridArray.GetLength(1); y++) //Cycle through the grid 2nd dimention
                    {
                        string toString = "";
                        if (showCellValueText)  toString = gridArray[x, y]?.ToString();
                        debugTextArray[x, y] = CreateWorldText(gridTextPrefix + "\n" + toString + gridTextSuffix, null, GetWorldPosition(x, y) + new Vector3(cellSize, 0, cellSize) * .5f, Mathf.RoundToInt((1.5f * cellSize) * gridTextSizeMultiplier), gridTextColor, TextAnchor.MiddleCenter, TextAlignment.Center); //Calling creat world text function to instantiate textmesh objects inside the grid and assign them to newly created debugTextArray
                        //debugTextArray[x, z].transform.localScale = Vector3.one * .13f;
                        debugTextArray[x, y].transform.position = new Vector3(debugTextArray[x, y].transform.position.x + gridTextLocalOffset.x, (debugTextArray[x, y].transform.position.y + cellSize / 2) + gridTextLocalOffset.y, (debugTextArray[x, y].transform.position.z - cellSize / 2) + gridTextLocalOffset.z);
                        debugTextArray[x, y].transform.eulerAngles = new Vector3(0, 0, 0); //Rotate instantiated text objects 90 in 'x' axis
                        debugTextArray[x, y].transform.parent = textObjectParent.transform;
                    }
                }

                OnGridObjectChanged += (object sender, OnGridObjectChangedEventArgs eventArgs) => //Subscribe to the event 'OnGridObjectChanged'
                {
                    debugTextArray[eventArgs.x, eventArgs.y].text = gridArray[eventArgs.x, eventArgs.y]?.ToString(); //Update the debug text array
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

        public Vector3 GetWorldPosition(int x, int y) //This function calculate and return cell world position
        {
            return new Vector3(x, y, 0) * cellSize + originPosition;
        }

        public void GetXY(Vector3 worldPosition, out int x, out int y) //This function return calculated 'x' and 'y' positions
        {
            x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
            y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
        }

        public void SetGridObjectXY(int x, int y, TGridObjectXY value) //This function set the values to the specific grid cells
        {
            if (x >= 0 && y >= 0 && x < width && y < length) //Check whether the 'x' and 'y' values are in the range
            {
                gridArray[x, y] = value; //Assign passed value to the specific grid cell in the grid array
                TriggerGridObjectChanged(x, y); //Call the function 'TriggerGridObjectChanged' and pass the cell
            }
        }

        public void TriggerGridObjectChanged(int x, int y) //This function call the event 'OnGridObjectChanged'
        {
            OnGridObjectChanged?.Invoke(this, new OnGridObjectChangedEventArgs { x = x, y = y }); //Call the event and pass parameters 'int x' and 'int z'
        }

        public void TriggerEdgeObjectChanged(int x, int y) //This function call the event 'OnGridObjectChanged'
        {
            OnEdgeObjectChanged?.Invoke(this, new OnEdgeObjectChangedEventArgs { x = x, y = y }); //Call the event and pass parameters 'int x' and 'int z'
        }

        public void SetGridObjectXY(Vector3 worldPosition, TGridObjectXY value) //This function grab 'world position' and 'value' from the caller and do the following
        {
            GetXY(worldPosition, out int x, out int y); //Call 'GetXZ' function and grab calculated values (x&z)
            SetGridObjectXY(x, y, value); //Call the upper 'SetGridObjectXZ' function and pass all the parameters
        }

        public TGridObjectXY GetGridObjectXY(int x, int y) //This function get the values from the specific grid cells
        {
            if (x >= 0 && y >= 0 && x < width && y < length) //Check whether the 'x' and 'y' values are in the range
            {
                return gridArray[x, y]; //If true return values of the specific grid cell
            }
            else
            {
                return default(TGridObjectXY); //Else return default value (null)
            }
        }

        public TGridObjectXY GetGridObjectXY(Vector3 worldPosition) //This function grab 'world position' from the caller and do the following
        {
            int x, y; //Innitialize variables
            GetXY(worldPosition, out x, out y); //Call 'GetXZ' function and grab calculated values (x&z)
            return GetGridObjectXY(x, y); //Call the upper 'GetGridObjectXZ' function and return all the parameters
        }

        public Vector2Int ValidateGridPosition(Vector2Int gridPosition) //This function take 'gridPosition' clamp it to be in fixed grid value
        {
            return new Vector2Int
            (
                Mathf.Clamp(gridPosition.x, width - 1, 0),
                Mathf.Clamp(gridPosition.y, length - 1, 0)
            );
        }

        public bool IsValidGridPosition(Vector2Int gridPosition) //This function take 'gridPosition' and check wheather it's inside the fixed grid and return a bool
        {
            int x = gridPosition.x;
            int y = gridPosition.y;

            if (x >= 0 && y >= 0 && x < width && y < length)
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
            int y = gridPosition.y;

            if (x >= Margin.x && y >= Margin.y && x < width - Margin.x && y < length - Margin.y)
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
            if (color == null) color = Color.white;
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
