using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using SoulGames.Utilities;

namespace SoulGames.EasyGridBuilderPro
{
    [ExecuteAlways]                                                                     //This script execute in editor and play mode
    public class EasyGridBuilderPro : MonoBehaviour
    {
        public static EasyGridBuilderPro Instance { get; private set; }                 //Create a static instance of this script

        public event EventHandler OnSelectedBuildableChanged;                           //Event declaration
        public event EventHandler OnObjectPlaced;                                       //Event declaration
        public event EventHandler OnActiveGridLevelChanged;                             //Event declaration //Pro Feature
        public event EventHandler OnGridModeChange;                                     //Event declaration

        public event OnBuildableEdgeObjectFlipDelegate OnBuildableEdgeObjectFlip;
        public delegate void OnBuildableEdgeObjectFlipDelegate(float edgeRotation);

        public event OnBuildConditionCheckCallerBuildableGridObjectDelegate OnBuildConditionCheckCallerBuildableGridObject;
        public delegate void OnBuildConditionCheckCallerBuildableGridObjectDelegate(BuildableGridObjectTypeSO buildableGridObjectTypeSO);
        public event OnBuildConditionCompleteCallerBuildableGridObjectDelegate OnBuildConditionCompleteCallerBuildableGridObject;
        public delegate void OnBuildConditionCompleteCallerBuildableGridObjectDelegate(BuildableGridObjectTypeSO buildableGridObjectTypeSO);
        public event OnBuildConditionCheckCallerBuildableEdgeObjectDelegate OnBuildConditionCheckCallerBuildableEdgeObject;
        public delegate void OnBuildConditionCheckCallerBuildableEdgeObjectDelegate(BuildableEdgeObjectTypeSO buildableEdgeObjectTypeSO);
        public event OnBuildConditionCompleteCallerBuildableEdgebjectDelegate OnBuildConditionCompleteCallerBuildableEdgeObject;
        public delegate void OnBuildConditionCompleteCallerBuildableEdgebjectDelegate(BuildableEdgeObjectTypeSO buildableEdgeObjectTypeSO);
        public event OnBuildConditionCheckCallerBuildableFreeObjectDelegate OnBuildConditionCheckCallerBuildableFreeObject;
        public delegate void OnBuildConditionCheckCallerBuildableFreeObjectDelegate(BuildableFreeObjectTypeSO buildableFreeObjectTypeSO);
        public event OnBuildConditionCompleteCallerBuildableFreeObjectDelegate OnBuildConditionCompleteCallerBuildableFreeObject;
        public delegate void OnBuildConditionCompleteCallerBuildableFreeObjectDelegate(BuildableFreeObjectTypeSO buildableFreeObjectTypeSO);

        public event OnBuildableGridObjectTypeSOListChangeDelegate OnBuildableGridObjectTypeSOListChange;
        public delegate void OnBuildableGridObjectTypeSOListChangeDelegate();
        public event OnBuildableEdgeObjectTypeSOListChangeDelegate OnBuildableEdgeObjectTypeSOListChange;
        public delegate void OnBuildableEdgeObjectTypeSOListChangeDelegate();
        public event OnBuildableFreeObjectTypeSOListChangeDelegate OnBuildableFreeObjectTypeSOListChange;    
        public delegate void OnBuildableFreeObjectTypeSOListChangeDelegate();

        [HideInInspector]public GridEditorMode gridEditorMode = GridEditorMode.None;    //Stores grid editor mode enum value
        private GridMode gridMode = GridMode.None;                                      //Stores grid mode enum value
        private BuildableGridObjectTypeSO buildableGridObjectTypeSO;                    //Currently selected 'BuildableGridObjectTypeSO'
        private BuildableGridObjectTypeSO.Dir dir;                                      //Current direction(down by default)
        private BuildableFreeObjectTypeSO buildableFreeObjectTypeSO;                    //Currently selected 'BuildableFreeObjectTypeSO' //Pro Feature
        private float buildableFreeObjectRotation;
        private List<Transform> builtBuildableFreeObjectList;                           //Store built BuildableFreeObjects for saving and loading
        private BuildableEdgeObjectTypeSO buildableEdgeObjectTypeSO;                    //Currently selected 'BuildableGridObjectTypeSO'
        private BuildableEdgeObjectTypeSO.Dir edgeDir;                                  //Current direction(down by default)
        private float edgeRotation;
        private Vector3 localMousePosition;
        private BuildableObjectType currentBuildableObjectType;
        private BoxCollider colliderObject;                                             //Store box collider
        private int selectedIndex = 1;                                                  //Currently selected 'BuildableGridObjectTypeSO' index value
        private bool isBuildableDestroyActive;                                          //Simple bool to check wheather if destroy active
        private bool isBuildableBuildActive;                                            //Simple bool to check wheather if build active
        private GameObject canvas;                                                      //Store Canvas Object
        private List<GridXZ<GridObjectXZ>> gridXZList;                                  //Grid XZ list declaration //Pro Feature
        private List<GridXY<GridObjectXY>> gridXYList;                                  //Grid XY list declaration //Pro Feature
        private GridXZ<GridObjectXZ> gridXZ;                                            //Grid XZ declaration (Currently Active)
        private GridXY<GridObjectXY> gridXY;                                            //Grid XY declaration (Currently Active)
        private List<Vector3> gridOriginXZList;                                         
        private List<Vector3> gridOriginXYList;
        private int currentActiveGridIndex = 0;
        private bool useBuildModeActivationKey = false;                                 
        private bool useDestructionModeActivationKey = false;
        private bool buildablePlacementKeyHolding = false;
        private bool ghostRotateLeftKeyHolding = false;
        private bool ghostRotateRightKeyHolding = false;
        private bool buildableAreaBlockerHit = false;

        private int gridObjectListCount;
        private int edgeObjectListCount;
        private int freeObjectListCount;

        [Space]
        [Tooltip("List of buildable grid objects. Take 'Buildable Grid Object Type SO' assets")]
        [SerializeField]private List<BuildableGridObjectTypeSO> buildableGridObjectTypeSOList = null;
        [Tooltip("List of buildable edge objects. Take 'Buildable Edge Object Type SO' assets")]
        [SerializeField]private List<BuildableEdgeObjectTypeSO> buildableEdgeObjectTypeSOList = null; //Pro Feature
        [Tooltip("List of buildable free objects. Take 'Buildable Free Object Type SO' assets")]
        [SerializeField]private List<BuildableFreeObjectTypeSO> buildableFreeObjectTypeSOList = null; //Pro Feature
        [Tooltip("Currently using grid axis. \n(XZ = Horizontal, More useful in 3D) \n(XY = Vertical, More useful in 2D)")]
        [SerializeField]public GridAxis gridAxis = GridAxis.XZ;
        [Tooltip("Width of the grid. \n(How many cells in the 1st axis)")] [Min(0)]
        [SerializeField]private int gridWidth = 10;
        [Tooltip("Length of the grid. \n(How many cells in the 2nd axis)")] [Min(0)]
        [SerializeField]private int gridLength = 10;
        [Tooltip("Cell size of the grid. \n(CellSize 1 = Unit 1)")] [Min(0)]
        [SerializeField]private float cellSize = 10f;
        [Tooltip("Set XZ grid origin position in world space.")]
        [SerializeField]private Vector3 gridOriginXZ = new Vector3(0,0,0);
        [Tooltip("Set XY grid origin position in world space.")]
        [SerializeField]private Vector3 gridOriginXY = new Vector3(0,0,0);
        [Tooltip("Use attached game object's origin as grid orgin. If this is active 'Grid Origin' is ignored.")]
        [SerializeField]private bool useHolderPositionAsOrigin = true;

        [SerializeField]public bool showVerticalGridData = false;

        [Space]
        [Tooltip("How many grids should be created vertically")] [Min(1)]
        [SerializeField]private int verticalGridsCount = 1;
        [Tooltip("Space between each vertical grid")] [Min(0)]
        [SerializeField]private float gridHeight = 2.5f; //Pro Feature
        [Tooltip("If enabled, Provided input will be used to swtich between vertical grids")]
        [SerializeField]private bool changeHeightWithInput = true; //Pro Feature
        [Tooltip("If enabled, Vertical grid will choose automatically using the mouse position. If 'Change Height With Input' is also enabled, this will be ignored")]
        [SerializeField]public bool autoDetectHeight = false; //Pro Feature
        [Tooltip("Layer Mask that use to check 'Auto Detect Height'.")] [Rename("Auto Detect Height Layer")]
        [SerializeField]public LayerMask autoDetectHeightLayerMask;
        
        [SerializeField]public bool showBuildableDistanceData = false;

        [Space]
        [Tooltip("Enable using buildable distance settings. \n(Make sure to set Min Max distances and the Object)")]
        [SerializeField]public bool useBuildableDistance = false;
        [Tooltip("Provide the object transform that should be used to check buildable distance. \n(If an object is not provided Camera's transform will be used)")]
        [SerializeField]private Transform distanceCheckObject;
        [Tooltip("Set the minimum distance that should be from 'distanceCheckObject'. \n(Set to 0 if you don't need a minimum distance check)")]
        [SerializeField]private float distanceMin;
        [Tooltip("Set the maximum distance that should be from 'distanceCheckObject'.")]
        [SerializeField]private float distanceMax;

        [SerializeField]public bool showGridObjectCollisionData = false;

        [Space]
        [Tooltip("Layer Mask that is used as grid surface. Simply set this to 'Grid Surface' Layer")] [Rename("Grid Object Colliding Layer")]
        [SerializeField]public LayerMask mouseColliderLayerMask;
        [Tooltip("Layer Mask that is used to place Buildable Free Objects. Simply check all the layers that Buildable Free Objects can be built on top of")] [Rename("Free Object Colliding Layer")]
        [SerializeField]public LayerMask freeObjectCollidingLayerMask;
        [Tooltip("Set the Created collider's size. \n(1 = collider size equal to the grid size) \n(If using value = 1, when mouse point is not on the grid, ghost object will snap to the center of the grid. To prevent this keep the collider larger than the grid. If you have multiple grids close to another, use a lower size)")] [Range(0, 10)]
        [SerializeField]private float colliderSizeMultiplier = 5;
        [Tooltip("If this is disabled, When vertical grid is changed grid Collider will also change it's position according to the current active vertical grid.")]
        [SerializeField]private bool lockColliderOnHeightChange = false; //Pro feature

        [SerializeField]public bool showCanvasGridData = false;

        [Space]
        [Tooltip("Displays a canvas based grid in editor and play mode.")] [Rename("Show Editor&Runtime Canvas Grid")]
        [SerializeField]public bool showEditorAndRuntimeCanvasGrid = false;
        [Tooltip("Add provided 'Grid Canvas' prefab. \n(Must be provided)")]
        [SerializeField]public Canvas gridCanvasPrefab = null;
        [Tooltip("Add a provided sprite to use as the grid visual")]
        [SerializeField]public Sprite gridImageSprite = null;
        [Tooltip("Color of the grid when it is displaying")]
        [SerializeField]private Color showColor = new Color32(255, 255, 255, 255);
        [Tooltip("Color of the grid when it is not displaying.")]
        [SerializeField]private Color hideColor = new Color32(255, 255, 255, 0);
        [Tooltip("Color transition speed when grid change it's mode to show and hide.")] [Min(0f)]
        [SerializeField]private float colorTransitionSpeed = 20f;
        [Tooltip("Show grid in Default Mode \n(Visible even when not using the grid)")]
        [SerializeField]private bool showOnDefaultMode = true;
        [Tooltip("Show grid in Build Mode \n(Visible grid when placing objects)")]
        [SerializeField]private bool showOnBuildMode = true;
        [Tooltip("Show grid in Destruct Mode \n(Visible when destroying objects)")]
        [SerializeField]private bool showOnDestructMode = true;
        [Tooltip("Show grid in Selection Mode \n(Visible when selecting objects)")]
        [SerializeField]private bool showOnSelectedMode = true;
        [Tooltip("Color of the grid images.")]
        private bool showOnMoveMode = true;
        [Tooltip("If this is disabled, When vertical grid is changed Canvas grid view will also change it's position according to the current active vertical grid.")]
        [SerializeField]private bool lockCanvasGridOnHeightChange = false; //Pro feature

        [SerializeField]public bool showDebugGridData = false;

        [Space]
        [Tooltip("Display a debug grid in the editor and play mode. \n(Make sure to enable gizmos in editor and play mode)")] [Rename("Show Editor&Runtime Debug Grid")]
        [SerializeField]public bool showEditorAndRuntimeDebugGrid = true;
        [Tooltip("Color of the editor debug grid's lines.")] [Rename("Grid Lines Color")]
        [SerializeField]private Color editorGridLineColor = new Color32(255, 255, 255, 255);
        [Tooltip("If this is disabled, When vertical grid is changed Debug grid view will also change it's position according to the current active vertical grid.")]
        [SerializeField]private bool lockDebugGridOnHeightChange = false; //Pro feature

        [SerializeField]public bool showNodeGridData = false;

        [Space]
        [Tooltip("Display a node based grid in runtime mode. \n(Make sure to add 'Grid Node Prefab')")]
        [SerializeField]public bool showRuntimeNodeGrid = false;
        [Tooltip("Add prefab to spawn in each node cell. If multiple objects are provided, each cell will spawn a random object. \n(At least one must be provided)")]
        [SerializeField]private GameObject[] gridNodePrefab = null;
        [Tooltip("Node object's size as a percentage. \n(If cell size is 10 & 'Grid Node Size Percentage' is 20, Node object size will be = 2)")] [Rename("Grid Node Size Percentage")] [Range(0, 100)] 
        [SerializeField]private float gridNodeMarginPercentage = 95;
        [Tooltip("Each node object's local position offset.")]
        [SerializeField]private Vector3 gridNodeLocalOffset = new Vector3(0,0,0);

        [SerializeField]public bool showTextGridData = false;

        [Space]
        [Tooltip("Displays a text based grid in runtime mode, Each grid cell will display it's grid position as a text. Use this with larger cells. Smaller grid cells can have a blurred or invisible text.")] [Rename("Show Runtime Text Grid")]
        [SerializeField]public bool showRuntimeGridText = false;
        [Tooltip("Color of the grid text.")]
        [SerializeField]private Color gridTextColor = new Color32(0, 0, 0, 255);
        [Tooltip("Grid text size as a multiplier. \n(2 = Double sized text) \n(0.5 = Half sized text)")] [Min(0)]
        [SerializeField]private float gridTextSizeMultiplier = 1;
        [Tooltip("Grid text displays each cell's value. \n(First cell will display = '0,0')")]
        [SerializeField]private bool showCellValueText = true;
        [Tooltip("Grid text displays a prefix text before Cell Value.")]
        [SerializeField]private string gridTextPrefix = null;
        [Tooltip("Grid text displays a suffix text after Cell Value.")]
        [SerializeField]private string gridTextSuffix = null;
        [Tooltip("Each text object's local position offset.")]
        [SerializeField]private Vector3 gridTextLocalOffset = new Vector3(0,0,0);

        [SerializeField]public bool showSaveAndLoadData = false;

        [Space]
        [Tooltip("Activate save and load features.")] [Rename("Enable Save & Load")]
        [SerializeField]public bool enableSaveAndLoad = true;
        [Tooltip("Provide save file a unique name. \n(This name must be unique for each gridObject in all scenes. Grids with same ID will override saved data.)")]
        [SerializeField]public string uniqueSaveName = "EasyGridBuilder_SaveData_";
        [Tooltip("Provide save file location.")][ReadOnly]
        [SerializeField]public string saveLocation = "/EasyGridBuilder Pro/LocalSaves/";
        
        [Space]
        [Tooltip("Displays various console debug commands. \n(Need to enable at least one of below commands)")] [Rename("Show Console Text")]
        [SerializeField]public bool showConsoleText = false;
        
        [SerializeField]public bool showConsoleData = false;

        [Space]
        [Tooltip("Displays a debug command when placing objects.")]
        [SerializeField]private bool objectPlacement = false;
        [Tooltip("Displays a debug command when destroying objects.")]
        [SerializeField]private bool objectDestruction = false;
        [Tooltip("Displays a debug command when selecting grid objects.")]
        [SerializeField]public bool objectSelected = false;
        //[Tooltip("Displays a debug command when selecting grid objects.")]
        //[SerializeField]public bool objectMoving = false;
        [Tooltip("Displays a debug command when changing grid levels")]
        [SerializeField]public bool gridLevelChange = false; //Pro Feature
        [Tooltip("Displays a debug command when grid is saved or load")]
        [SerializeField]private bool saveAndLoad = false;
        [Space]
        [Tooltip("Enables provided unity events below.")]
        [SerializeField]public bool enableUnityEvents = false;

        [SerializeField]public bool showBaseEvent = false;
        
        [Space]
        public UnityEvent OnSelectedBuildableChangedUnityEvent;
        public UnityEvent OnGridCellChangedUnityEvent;
        public UnityEvent OnActiveGridLevelChangedUnityEvent; //Pro Feature

        [SerializeField]public bool showObjectInteractEvents = false;

        [Space]
        public UnityEvent OnObjectPlacedUnityEvent;
        public UnityEvent OnObjectRemovedUnityEvent;
        public UnityEvent OnObjectSelectedUnityEvent;
        public UnityEvent OnObjectDeselectedUnityEvent;
        //public UnityEvent OnObjectStartMovingUnityEvent;
        //public UnityEvent OnObjectStoppedMovingUnityEvent;

        public class GridObjectXZ
        {
            private GridXZ<GridObjectXZ> gridXZ;
            private int x;
            private int y;
            public BuildableGridObject buildableGridObject;
            public BuildableEdgeObject downBuildableEdgeObject;
            public BuildableEdgeObject leftBuildableEdgeObject;
            public BuildableEdgeObject upBuildableEdgeObject;
            public BuildableEdgeObject rightBuildableEdgeObject;

            public GridObjectXZ(GridXZ<GridObjectXZ> grid, int x, int y)
            {
                this.gridXZ = grid;
                this.x = x;
                this.y = y;
                buildableGridObject = null;
                downBuildableEdgeObject = null;
                leftBuildableEdgeObject = null;
                upBuildableEdgeObject = null;
                rightBuildableEdgeObject = null;
            }

            public override string ToString()
            {
                return x + ", " + y + "\n" + buildableGridObject;
            }

            public void TriggerGridObjectChanged()
            {
                gridXZ.TriggerGridObjectChanged(x, y);
            }

            public void TriggerEdgeObjectChanged()
            {
                gridXZ.TriggerEdgeObjectChanged(x, y);
            }

            public void SetPlacedObject(BuildableGridObject buildableGridObject)
            {
                this.buildableGridObject = buildableGridObject;
                TriggerGridObjectChanged();
            }

            public void SetPlacedDownEdgeObject(BuildableEdgeObject buildableEdgeObject)
            {
                this.downBuildableEdgeObject = buildableEdgeObject;
                TriggerEdgeObjectChanged();
            }
            public void SetPlacedLeftEdgeObject(BuildableEdgeObject buildableEdgeObject)
            {
                this.leftBuildableEdgeObject = buildableEdgeObject;
                TriggerEdgeObjectChanged();
            }
            public void SetPlacedUpEdgeObject(BuildableEdgeObject buildableEdgeObject)
            {
                this.upBuildableEdgeObject = buildableEdgeObject;
                TriggerEdgeObjectChanged();
            }
            public void SetPlacedRightEdgeObject(BuildableEdgeObject buildableEdgeObject)
            {
                this.rightBuildableEdgeObject = buildableEdgeObject;
                TriggerEdgeObjectChanged();
            }

            public void ClearPlacedObject()
            {
                buildableGridObject = null;
                TriggerGridObjectChanged();
            }

            public void ClearPlacedDownEdgeObject()
            {
                downBuildableEdgeObject = null;
                TriggerEdgeObjectChanged();
            }
            public void ClearPlacedLeftEdgeObject()
            {
                leftBuildableEdgeObject = null;
                TriggerEdgeObjectChanged();
            }
            public void ClearPlacedUpEdgeObject()
            {
                upBuildableEdgeObject = null;
                TriggerEdgeObjectChanged();
            }
            public void ClearPlacedRightEdgeObject()
            {
                rightBuildableEdgeObject = null;
                TriggerEdgeObjectChanged();
            }

            public BuildableGridObject GetPlacedObject()
            {
                return buildableGridObject;
            }
            
            public BuildableEdgeObject GetPlacedDownEdgeObject()
            {
                return downBuildableEdgeObject;
            }
            public BuildableEdgeObject GetPlacedLeftEdgeObject()
            {
                return leftBuildableEdgeObject;
            }
            public BuildableEdgeObject GetPlacedUpEdgeObject()
            {
                return upBuildableEdgeObject;
            }
            public BuildableEdgeObject GetPlacedRightEdgeObject()
            {
                return rightBuildableEdgeObject;
            }

            public bool CanBuild()
            {
                return buildableGridObject == null;
            }

            public bool CanBuildEdgeObjectDown()
            {
                return downBuildableEdgeObject == null;
            }
            public bool CanBuildEdgeObjectLeft()
            {
                return leftBuildableEdgeObject == null;
            }
            public bool CanBuildEdgeObjectUp()
            {
                return upBuildableEdgeObject == null;
            }
            public bool CanBuildEdgeObjectRight()
            {
                return rightBuildableEdgeObject == null;
            }
        }

        public class GridObjectXY
        {
            private GridXY<GridObjectXY> gridXY;
            private int x;
            private int y;
            public BuildableGridObject buildableGridObject;
            public BuildableEdgeObject downBuildableEdgeObject;
            public BuildableEdgeObject leftBuildableEdgeObject;
            public BuildableEdgeObject upBuildableEdgeObject;
            public BuildableEdgeObject rightBuildableEdgeObject;

            public GridObjectXY(GridXY<GridObjectXY> grid, int x, int y)
            {
                this.gridXY = grid;
                this.x = x;
                this.y = y;
                buildableGridObject = null;
                downBuildableEdgeObject = null;
                leftBuildableEdgeObject = null;
                upBuildableEdgeObject = null;
                rightBuildableEdgeObject = null;
            }

            public override string ToString()
            {
                return x + ", " + y + "\n" + buildableGridObject;
            }

            public void TriggerGridObjectChanged()
            {
                gridXY.TriggerGridObjectChanged(x, y);
            }
            
            public void TriggerEdgeObjectChanged()
            {
                gridXY.TriggerEdgeObjectChanged(x, y);
            }

            public void SetPlacedObject(BuildableGridObject buildableGridObject)
            {
                this.buildableGridObject = buildableGridObject;
                TriggerGridObjectChanged();
            }

            public void SetPlacedDownEdgeObject(BuildableEdgeObject buildableEdgeObject)
            {
                this.downBuildableEdgeObject = buildableEdgeObject;
                TriggerEdgeObjectChanged();
            }
            public void SetPlacedLeftEdgeObject(BuildableEdgeObject buildableEdgeObject)
            {
                this.leftBuildableEdgeObject = buildableEdgeObject;
                TriggerEdgeObjectChanged();
            }
            public void SetPlacedUpEdgeObject(BuildableEdgeObject buildableEdgeObject)
            {
                this.upBuildableEdgeObject = buildableEdgeObject;
                TriggerEdgeObjectChanged();
            }
            public void SetPlacedRightEdgeObject(BuildableEdgeObject buildableEdgeObject)
            {
                this.rightBuildableEdgeObject = buildableEdgeObject;
                TriggerEdgeObjectChanged();
            }

            public void ClearPlacedObject()
            {
                buildableGridObject = null;
                TriggerGridObjectChanged();
            }
            
            public void ClearPlacedDownEdgeObject()
            {
                downBuildableEdgeObject = null;
                TriggerEdgeObjectChanged();
            }
            public void ClearPlacedLeftEdgeObject()
            {
                leftBuildableEdgeObject = null;
                TriggerEdgeObjectChanged();
            }
            public void ClearPlacedUpEdgeObject()
            {
                upBuildableEdgeObject = null;
                TriggerEdgeObjectChanged();
            }
            public void ClearPlacedRightEdgeObject()
            {
                rightBuildableEdgeObject = null;
                TriggerEdgeObjectChanged();
            }

            public BuildableGridObject GetPlacedObject()
            {
                return buildableGridObject;
            }
            
            public BuildableEdgeObject GetPlacedDownEdgeObject()
            {
                return downBuildableEdgeObject;
            }
            public BuildableEdgeObject GetPlacedLeftEdgeObject()
            {
                return leftBuildableEdgeObject;
            }
            public BuildableEdgeObject GetPlacedUpEdgeObject()
            {
                return upBuildableEdgeObject;
            }
            public BuildableEdgeObject GetPlacedRightEdgeObject()
            {
                return rightBuildableEdgeObject;
            }

            public bool CanBuild()
            {
                return buildableGridObject == null;
            }
            
            public bool CanBuildEdgeObjectDown()
            {
                return downBuildableEdgeObject == null;
            }
            public bool CanBuildEdgeObjectLeft()
            {
                return leftBuildableEdgeObject == null;
            }
            public bool CanBuildEdgeObjectUp()
            {
                return upBuildableEdgeObject == null;
            }
            public bool CanBuildEdgeObjectRight()
            {
                return rightBuildableEdgeObject == null;
            }
        }

        private void Awake()
        {
            if (gridEditorMode == GridEditorMode.None) return;
            Instance = this;

            if (useHolderPositionAsOrigin)
            {
                if (gridEditorMode == GridEditorMode.GridLite)
                {
                    gridOriginXZ = new Vector3(((cellSize * gridWidth / 2)* -1) + transform.position.x, transform.position.y, ((cellSize * gridLength / 2)* -1) + transform.position.z);
                    gridOriginXY = new Vector3(((cellSize * gridWidth / 2)* -1) + transform.position.x, ((cellSize * gridLength / 2)* -1) + transform.position.y, transform.position.z);
                }
                else if (gridEditorMode == GridEditorMode.GridPro)
                {
                    gridOriginXZList = new List<Vector3>();
                    for (int i = 0; i < verticalGridsCount; i++)
                    {
                        Vector3 tempGridOriginXZ = new Vector3(((cellSize * gridWidth / 2)* -1) + transform.position.x, transform.position.y + (gridHeight * i), ((cellSize * gridLength / 2)* -1) + transform.position.z);
                        gridOriginXZList.Add(tempGridOriginXZ);
                    }
                    gridOriginXZ = gridOriginXZList[0];
                    
                    gridOriginXYList = new List<Vector3>();
                    for (int x = 0; x < verticalGridsCount; x++)
                    {
                        Vector3 tempGridOriginXY = new Vector3(((cellSize * gridWidth / 2)* -1) + transform.position.x, ((cellSize * gridLength / 2)* -1) + transform.position.y, transform.position.z - (gridHeight * x)); //Get holder objects origin for gridXY's origin
                        gridOriginXYList.Add(tempGridOriginXY);
                    }
                    gridOriginXY = gridOriginXYList[0];
                }
            }
            else
            {
                if (gridEditorMode == GridEditorMode.GridPro)
                {
                    gridOriginXZList = new List<Vector3>();
                    for (int i = 0; i < verticalGridsCount; i++)
                    {
                        Vector3 tempGridOriginXZ = new Vector3(gridOriginXZ.x, gridOriginXZ.y + (gridHeight * i), gridOriginXZ.z);
                        gridOriginXZList.Add(tempGridOriginXZ);
                    }
                    gridOriginXZ = gridOriginXZList[0];
                    
                    gridOriginXYList = new List<Vector3>();
                    for (int x = 0; x < verticalGridsCount; x++)
                    {
                        Vector3 tempGridOriginXY = new Vector3(gridOriginXY.x, gridOriginXY.y,  gridOriginXY.z - (gridHeight * x));
                        gridOriginXYList.Add(tempGridOriginXY);
                    }
                    gridOriginXY = gridOriginXYList[0];
                }
            }

            if (gridAxis == GridAxis.XZ)
            {
                if (Application.isPlaying) 
                {
                    if (gridEditorMode == GridEditorMode.GridLite)
                    {
                        gridXZ = new GridXZ<GridObjectXZ>(gridWidth, gridLength, cellSize, gridOriginXZ, (GridXZ<GridObjectXZ> g, int x, int y) => new GridObjectXZ(g, x, y), showRuntimeNodeGrid, showRuntimeGridText, gridTextColor, gridTextSizeMultiplier, showCellValueText, gridTextPrefix, gridTextSuffix, gridTextLocalOffset, this.transform, gridNodePrefab, gridNodeMarginPercentage, gridNodeLocalOffset);
                    }
                    else if (gridEditorMode == GridEditorMode.GridPro)
                    {
                        gridXZList = new List<GridXZ<GridObjectXZ>>();
                        for (int i = 0; i < verticalGridsCount; i++)
                        {
                            GridXZ<GridObjectXZ> tempGridXZ = new GridXZ<GridObjectXZ>(gridWidth, gridLength, cellSize, gridOriginXZList[i], (GridXZ<GridObjectXZ> g, int x, int y) => new GridObjectXZ(g, x, y), showRuntimeNodeGrid, showRuntimeGridText, gridTextColor, gridTextSizeMultiplier, showCellValueText, gridTextPrefix, gridTextSuffix, gridTextLocalOffset, this.transform, gridNodePrefab, gridNodeMarginPercentage, gridNodeLocalOffset);
                            gridXZList.Add(tempGridXZ);
                        }
                        gridXZ = gridXZList[0];
                    }
                }
                if (!transform.gameObject.GetComponent<BoxCollider>())
                {
                    colliderObject = transform.gameObject.AddComponent<BoxCollider>();
                    colliderObject.size = new Vector3((cellSize * gridWidth) * colliderSizeMultiplier, 0, (cellSize * gridLength) * colliderSizeMultiplier);
                }
            }
            else
            {
                if (Application.isPlaying)
                {
                    if (gridEditorMode == GridEditorMode.GridLite)
                    {
                        gridXY = new GridXY<GridObjectXY>(gridWidth, gridLength, cellSize, gridOriginXY, (GridXY<GridObjectXY> g, int x, int y) => new GridObjectXY(g, x, y), showRuntimeNodeGrid, showRuntimeGridText, gridTextColor, gridTextSizeMultiplier, showCellValueText, gridTextPrefix, gridTextSuffix, gridTextLocalOffset, this.transform, gridNodePrefab, gridNodeMarginPercentage, gridNodeLocalOffset);
                    }
                    else if (gridEditorMode == GridEditorMode.GridPro)
                    {
                        gridXYList = new List<GridXY<GridObjectXY>>();
                        for (int i = 0; i < verticalGridsCount; i++)
                        {
                            GridXY<GridObjectXY> tempGridXY = new GridXY<GridObjectXY>(gridWidth, gridLength, cellSize, gridOriginXYList[i], (GridXY<GridObjectXY> g, int x, int y) => new GridObjectXY(g, x, y), showRuntimeNodeGrid, showRuntimeGridText, gridTextColor, gridTextSizeMultiplier, showCellValueText, gridTextPrefix, gridTextSuffix, gridTextLocalOffset, this.transform, gridNodePrefab, gridNodeMarginPercentage, gridNodeLocalOffset);
                            gridXYList.Add(tempGridXY);
                        }
                        gridXY = gridXYList[0];
                    }
                }
                if (!transform.gameObject.GetComponent<BoxCollider>())
                {
                    colliderObject = transform.gameObject.AddComponent<BoxCollider>();
                    colliderObject.size = new Vector3((cellSize * gridWidth) * colliderSizeMultiplier,  (cellSize * gridLength) * colliderSizeMultiplier, 0);
                }
            }

            buildableGridObjectTypeSO = null;
            buildableEdgeObjectTypeSO = null;
            buildableFreeObjectTypeSO = null;
            builtBuildableFreeObjectList = new List<Transform>();

            gridObjectListCount = buildableGridObjectTypeSOList.Count;
            edgeObjectListCount = buildableEdgeObjectTypeSOList.Count;
            freeObjectListCount = buildableFreeObjectTypeSOList.Count;
        }

        private void Start()
        {
            if (Application.isPlaying)
            {
                GridObjectSelector.OnObjectSelect += OnObjectSelect;
                GridObjectSelector.OnObjectDeselect += OnObjectDeselect;
                GridObjectMover.OnObjectStartMoving += OnObjectStartMoving;
                GridObjectMover.OnObjectStoppedMoving += OnObjectStoppedMoving;
                GridObjectGhost.OnBuildableObjectAreaBlockerEnter += OnBuildableObjectAreaBlockerEnter;
                GridObjectGhost.OnBuildableObjectAreaBlockerExit += OnBuildableObjectAreaBlockerExit;
                FreeObjectGhost.OnBuildableObjectAreaBlockerEnter += OnBuildableObjectAreaBlockerEnter;
                FreeObjectGhost.OnBuildableObjectAreaBlockerExit += OnBuildableObjectAreaBlockerExit;
            }

            if (GetGridAxis() == GridAxis.XY)
            {
                buildableEdgeObjectTypeSOList.Clear();
            }
        }

        private void OnDestroy()
        {
            GridObjectSelector.OnObjectSelect -= OnObjectSelect;
            GridObjectSelector.OnObjectDeselect -= OnObjectDeselect;
            GridObjectMover.OnObjectStartMoving -= OnObjectStartMoving;
            GridObjectMover.OnObjectStoppedMoving -= OnObjectStoppedMoving;
            GridObjectGhost.OnBuildableObjectAreaBlockerEnter -= OnBuildableObjectAreaBlockerEnter;
            GridObjectGhost.OnBuildableObjectAreaBlockerExit -= OnBuildableObjectAreaBlockerExit;
            FreeObjectGhost.OnBuildableObjectAreaBlockerEnter -= OnBuildableObjectAreaBlockerEnter;
            FreeObjectGhost.OnBuildableObjectAreaBlockerExit -= OnBuildableObjectAreaBlockerExit;
        }

        private void OnObjectSelect(EasyGridBuilderPro ownSystem, GameObject selectedObject)
        {
            if (ownSystem == this)
            {
                if (enableUnityEvents) OnObjectSelectedUnityEvent?.Invoke();
            }
        }

        private void OnObjectDeselect(EasyGridBuilderPro ownSystem, GameObject selectedObject)
        {
            if (ownSystem == this)
            {
                if (enableUnityEvents) OnObjectDeselectedUnityEvent?.Invoke();
            }
        }

        private void OnBuildableObjectAreaBlockerEnter()
        {
            buildableAreaBlockerHit = true;
        }
        
        private void OnBuildableObjectAreaBlockerExit()
        {
            buildableAreaBlockerHit = false;
        }

        private void Update()
        {
            if (gridEditorMode == GridEditorMode.None) return;

            HandleGridOrigin();
            HandleGridCollider();
            HandleVisualCanvasGrid();

            if (Application.isPlaying)
            {
                localMousePosition = GetMouseWorldPosition();
                HandleAutoGridHeightDetection();
                HandleBuildableTypeSOChangeEvents();

                if (buildablePlacementKeyHolding)
                {
                    if (buildableGridObjectTypeSO != null)
                    {
                        if (buildableGridObjectTypeSO.holdToPlace && !buildableGridObjectTypeSO.placeAndDeselect)
                        {
                            TriggerBuildablePlacement();
                        }
                    }
                    else if (buildableEdgeObjectTypeSO != null)
                    {
                        if (buildableEdgeObjectTypeSO.holdToPlace && !buildableEdgeObjectTypeSO.placeAndDeselect)
                        {
                            TriggerBuildablePlacement();
                        }
                    }
                    else if (buildableFreeObjectTypeSO != null)
                    {
                        if (buildableFreeObjectTypeSO.holdToPlace && !buildableFreeObjectTypeSO.placeAndDeselect)
                        {
                            TriggerBuildablePlacement();
                        }
                    }
                }
                if (ghostRotateLeftKeyHolding)
                {
                    if (buildableFreeObjectTypeSO != null) buildableFreeObjectRotation -= Time.deltaTime * 90f;
                }
                if (ghostRotateRightKeyHolding)
                {
                    if (buildableFreeObjectTypeSO != null) buildableFreeObjectRotation += Time.deltaTime * 90f;
                }
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
        //                                                                                               UPDATE FUNCTIONS                                                                                           //
        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//

        private void HandleGridOrigin()
        {
            if (useHolderPositionAsOrigin)
            {
                if (!Application.isPlaying)
                {
                    gridOriginXZ = new Vector3(((cellSize * gridWidth / 2)* -1) + transform.position.x, transform.position.y, ((cellSize * gridLength / 2)* -1) + transform.position.z);
                    gridOriginXY = new Vector3(((cellSize * gridWidth / 2)* -1) + transform.position.x, ((cellSize * gridLength / 2)* -1) + transform.position.y, transform.position.z); 
                }
            }
        }

        private void HandleGridCollider()
        {
            if(!Application.isPlaying)
            {
                if (gridAxis == GridAxis.XZ)
                {
                    if (!transform.gameObject.GetComponent<BoxCollider>())
                    {
                        colliderObject = transform.gameObject.AddComponent<BoxCollider>();
                    }
                    else
                    {
                        if (colliderObject == null) colliderObject = transform.gameObject.GetComponent<BoxCollider>();
                        colliderObject.center = new Vector3(((cellSize * gridWidth / 2)) + gridOriginXZ.x - transform.position.x, gridOriginXZ.y - transform.position.y, ((cellSize * gridLength / 2)) + gridOriginXZ.z - transform.position.z);
                        colliderObject.size = new Vector3((cellSize * gridWidth) * colliderSizeMultiplier, 0, (cellSize * gridLength) * colliderSizeMultiplier);
                    } 
                }
                else
                {
                    if (!transform.gameObject.GetComponent<BoxCollider>())
                    {
                        colliderObject = transform.gameObject.AddComponent<BoxCollider>();
                    }
                    else
                    {
                        if (colliderObject == null) colliderObject = transform.gameObject.GetComponent<BoxCollider>();
                        colliderObject.center = new Vector3(((cellSize * gridWidth / 2)) + gridOriginXY.x - transform.position.x, ((cellSize * gridLength / 2)) + gridOriginXY.y - transform.position.y, gridOriginXY.z - transform.position.z);
                        colliderObject.size = new Vector3((cellSize * gridWidth) * colliderSizeMultiplier,  (cellSize * gridLength) * colliderSizeMultiplier, 0);
                    }
                }
            }
            else
            {
                if (colliderObject == null)
                {
                    colliderObject = transform.gameObject.GetComponent<BoxCollider>();
                }
                else
                {
                    if (gridAxis == GridAxis.XZ)
                    {
                        if (lockColliderOnHeightChange && gridEditorMode == GridEditorMode.GridPro)
                        {
                            
                            colliderObject.center = new Vector3(((cellSize * gridWidth / 2)) + gridOriginXZList[0].x - transform.position.x, gridOriginXZList[0].y - transform.position.y, ((cellSize * gridLength / 2)) + gridOriginXZList[0].z - transform.position.z);
                        }
                        else
                        {
                            colliderObject.center = new Vector3(((cellSize * gridWidth / 2)) + gridOriginXZ.x - transform.position.x, gridOriginXZ.y - transform.position.y, ((cellSize * gridLength / 2)) + gridOriginXZ.z - transform.position.z);
                        }
                        
                    }
                    else
                    {
                        if (lockColliderOnHeightChange && gridEditorMode == GridEditorMode.GridPro)
                        {
                            colliderObject.center = new Vector3(((cellSize * gridWidth / 2)) + gridOriginXYList[0].x - transform.position.x, ((cellSize * gridLength / 2)) + gridOriginXYList[0].y - transform.position.y, gridOriginXYList[0].z - transform.position.z);
                        }
                        else
                        {
                            colliderObject.center = new Vector3(((cellSize * gridWidth / 2)) + gridOriginXY.x - transform.position.x, ((cellSize * gridLength / 2)) + gridOriginXY.y - transform.position.y, gridOriginXY.z - transform.position.z);
                        }
                    }
                }
            }
        }

        private void HandleVisualCanvasGrid()
        {
            if (gridAxis == GridAxis.XZ)
            {
                if (showEditorAndRuntimeCanvasGrid)
                {
                    if (!canvas && gridCanvasPrefab)
                    {
                        if (!this.transform.Find("Grid Canvas(Clone)"))
                        {
                            canvas = Instantiate(gridCanvasPrefab.gameObject, Vector3.zero, Quaternion.identity);
                            canvas.transform.SetParent(this.transform, false);
                        }
                        else
                        {
                            canvas = this.transform.Find("Grid Canvas(Clone)").gameObject;
                        }
                    }
                }
                if (!showEditorAndRuntimeCanvasGrid)
                {
                    if (transform.childCount  != 0)
                    {
                        for (int i = 0; i < transform.childCount; i++)
                        {
                            var child = transform.GetChild(i);
                            if (child.name == "Grid Canvas(Clone)") DestroyImmediate(child.gameObject);
                        }
                    }
                }

                if (canvas)
                {
                    Vector2 widthAndHeight = new Vector2(gridWidth*cellSize, gridLength*cellSize);
                    Transform gridTexture = canvas.transform.GetChild(0);
                    Image gridImage = gridTexture.GetComponent<Image>();

                    canvas.transform.eulerAngles = new Vector3(90, 0, 0);
                    if (Application.isPlaying && lockCanvasGridOnHeightChange && gridEditorMode == GridEditorMode.GridPro)
                    {
                        canvas.transform.position = gridOriginXZList[0];
                    }
                    else
                    {
                        canvas.transform.position = gridOriginXZ;
                    }
                    canvas.GetComponent<RectTransform>().sizeDelta = widthAndHeight;
                    gridTexture.GetComponent<RectTransform>().sizeDelta = widthAndHeight;
                    
                    gridImage.sprite = gridImageSprite;
                    gridImage.type = Image.Type.Tiled;
                    gridImage.pixelsPerUnitMultiplier = 100 / cellSize;

                    if (Application.isPlaying)
                    {
                        switch (GetGridMode())
                        {
                            case GridMode.None:
                                if (showOnDefaultMode) { if (gridImage.color != showColor) CanvasAlphaTransition(true, gridImage); }
                                else { if (gridImage.color != hideColor) CanvasAlphaTransition(false, gridImage); }
                            break;
                            case GridMode.Build:
                                if (showOnBuildMode) { if (gridImage.color != showColor) CanvasAlphaTransition(true, gridImage); }
                                else { if (gridImage.color != hideColor) CanvasAlphaTransition(false, gridImage); }
                            break;
                            case GridMode.Destruct:
                                if (showOnDestructMode) { if (gridImage.color != showColor) CanvasAlphaTransition(true, gridImage); }
                                else { if (gridImage.color != hideColor) CanvasAlphaTransition(false, gridImage); }
                            break;
                            case GridMode.Selected:
                                if (showOnSelectedMode) { if (gridImage.color != showColor) CanvasAlphaTransition(true, gridImage); }
                                else { if (gridImage.color != hideColor) CanvasAlphaTransition(false, gridImage); }
                            break;
                            case GridMode.Moving:
                                if (showOnMoveMode) { if (gridImage.color != showColor) CanvasAlphaTransition(true, gridImage); }
                                else { if (gridImage.color != hideColor) CanvasAlphaTransition(false, gridImage); }
                            break;
                        }
                    }
                    else
                    {
                        gridImage.color = showColor;
                    }
                }
            }
            else
            {
                if (showEditorAndRuntimeCanvasGrid)
                {
                    if (!canvas && gridCanvasPrefab)
                    {
                        if (!this.transform.Find("Grid Canvas(Clone)"))
                        {
                            canvas = Instantiate(gridCanvasPrefab.gameObject, Vector3.zero, Quaternion.identity);
                            canvas.transform.SetParent(this.transform, false);
                        }
                        else
                        {
                            canvas = this.transform.Find("Grid Canvas(Clone)").gameObject;
                        }
                    }
                }
                if (!showEditorAndRuntimeCanvasGrid)
                {
                    if (transform.childCount  != 0)
                    {
                        for (int i = 0; i < transform.childCount; i++)
                        {
                            var child = transform.GetChild(i);
                            if (child.name == "Grid Canvas(Clone)") DestroyImmediate(child.gameObject);
                        }
                    }
                }

                if (canvas)
                {
                    Vector2 widthAndHeight = new Vector2(gridWidth*cellSize, gridLength*cellSize);
                    Transform gridTexture = canvas.transform.GetChild(0);
                    Image gridImage = gridTexture.GetComponent<Image>();

                    canvas.transform.eulerAngles = new Vector3(0, 0, 0);
                    if (Application.isPlaying && lockCanvasGridOnHeightChange && gridEditorMode == GridEditorMode.GridPro)
                    {
                        canvas.transform.position = gridOriginXYList[0];
                    }
                    else
                    {
                        canvas.transform.position = gridOriginXY;
                    }
                    canvas.GetComponent<RectTransform>().sizeDelta = widthAndHeight;
                    gridTexture.GetComponent<RectTransform>().sizeDelta = widthAndHeight;
                    
                    gridImage.sprite = gridImageSprite;
                    gridImage.type = Image.Type.Tiled;
                    gridImage.pixelsPerUnitMultiplier = 100 / cellSize;

                    if (Application.isPlaying)
                    {
                        switch (GetGridMode())
                        {
                            case GridMode.None:
                                if (showOnDefaultMode) { if (gridImage.color != showColor) CanvasAlphaTransition(true, gridImage); }
                                else { if (gridImage.color != hideColor) CanvasAlphaTransition(false, gridImage); }
                            break;
                            case GridMode.Build:
                                if (showOnBuildMode) { if (gridImage.color != showColor) CanvasAlphaTransition(true, gridImage); }
                                else { if (gridImage.color != hideColor) CanvasAlphaTransition(false, gridImage); }
                            break;
                            case GridMode.Destruct:
                                if (showOnDestructMode) { if (gridImage.color != showColor) CanvasAlphaTransition(true, gridImage); }
                                else { if (gridImage.color != hideColor) CanvasAlphaTransition(false, gridImage); }
                            break;
                            case GridMode.Selected:
                                if (showOnSelectedMode) { if (gridImage.color != showColor) CanvasAlphaTransition(true, gridImage); }
                                else { if (gridImage.color != hideColor) CanvasAlphaTransition(false, gridImage); }
                            break;
                            case GridMode.Moving:
                                if (showOnMoveMode) { if (gridImage.color != showColor) CanvasAlphaTransition(true, gridImage); }
                                else { if (gridImage.color != hideColor) CanvasAlphaTransition(false, gridImage); }
                            break;
                        }
                    }
                    else
                    {
                        gridImage.color = showColor;
                    }
                }
            }
        }

        private void CanvasAlphaTransition(bool _showAlpha, Image gridImage)
        {
            if (_showAlpha)
            {
                gridImage.color = Color.Lerp(gridImage.color, showColor, colorTransitionSpeed * Time.deltaTime);
            }
            else
            {
                gridImage.color = Color.Lerp(gridImage.color, hideColor, colorTransitionSpeed * Time.deltaTime);
            }
        }

        private void HandleAutoGridHeightDetection()
        {
            if (autoDetectHeight && !changeHeightWithInput)
            {
                if (gridEditorMode == GridEditorMode.GridPro)
                {
                    if (GetGridMode() != GridMode.None)
                    {
                        if (gridAxis == GridAxis.XZ)
                        {
                            int nextGridIndex = Mathf.Clamp(Mathf.RoundToInt((AutoDetectHeightMousePosition().y - gridOriginXZList[0].y) / gridHeight), 0, gridXZList.Count - 1);
                            gridXZ = gridXZList[nextGridIndex];
                            OnActiveGridLevelChanged?.Invoke(this, EventArgs.Empty);
                            if (enableUnityEvents) OnActiveGridLevelChangedUnityEvent?.Invoke();
                            if (showConsoleText && gridLevelChange) Debug.Log("Grid XZ " + "<color=green>Grid Level Changed! Current Grid Level :</color> " + (nextGridIndex + 1));
                            gridOriginXZ = gridOriginXZList[nextGridIndex];
                            currentActiveGridIndex = nextGridIndex;
                        }
                        else
                        {
                            int nextGridIndex = Mathf.Clamp(Mathf.RoundToInt(((AutoDetectHeightMousePosition().z - gridOriginXYList[0].z) * -1) / gridHeight), 0, gridXYList.Count - 1);
                            gridXY = gridXYList[nextGridIndex];
                            
                            OnActiveGridLevelChanged?.Invoke(this, EventArgs.Empty);
                            if (enableUnityEvents) OnActiveGridLevelChangedUnityEvent?.Invoke();
                            if (showConsoleText && gridLevelChange) Debug.Log("Grid XY " + "<color=green>Grid Level Changed! Current Grid Level :</color> " + (nextGridIndex + 1));
                            gridOriginXY = gridOriginXYList[nextGridIndex];
                            currentActiveGridIndex = nextGridIndex;
                        }
                    }
                }
            }
        }

        private void HandleBuildableTypeSOChangeEvents()
        {
            if (gridObjectListCount != buildableGridObjectTypeSOList.Count)
            {
                OnBuildableGridObjectTypeSOListChange?.Invoke();
                gridObjectListCount = buildableGridObjectTypeSOList.Count;
            }
            if (edgeObjectListCount != buildableEdgeObjectTypeSOList.Count)
            {
                OnBuildableEdgeObjectTypeSOListChange?.Invoke();
                edgeObjectListCount = buildableEdgeObjectTypeSOList.Count;
            }
            if (freeObjectListCount != buildableFreeObjectTypeSOList.Count)
            {
                OnBuildableFreeObjectTypeSOListChange?.Invoke();
                freeObjectListCount = buildableFreeObjectTypeSOList.Count;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
        //                                                                                            INPUT HANDLE FUNCTION                                                                                         //
        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//

        public void SetInputGridModeVariables(bool useBuildModeActivationKey, bool useDestructionModeActivationKey, bool useSelectionModeActivationKey)
        {
            this.useBuildModeActivationKey = useBuildModeActivationKey;
            this.useDestructionModeActivationKey = useDestructionModeActivationKey;
        }

        public void SetGridModeReset()
        {
            DeselectObjectType();
        }

        public void TriggerGridHeightChangeManually()
        {
            if (changeHeightWithInput)
            {
                if (gridEditorMode == GridEditorMode.GridPro)
                {
                    if (gridAxis == GridAxis.XZ)
                    {
                        int nextGridIndex = (gridXZList.IndexOf(gridXZ) + 1) % gridXZList.Count;
                        gridXZ = gridXZList[nextGridIndex];
                        OnActiveGridLevelChanged?.Invoke(this, EventArgs.Empty);
                        if (enableUnityEvents) OnActiveGridLevelChangedUnityEvent?.Invoke();
                        if (showConsoleText && gridLevelChange) Debug.Log("Grid XZ " + "<color=green>Grid Level Changed! Current Grid Level :</color> " + (nextGridIndex + 1));
                        gridOriginXZ = gridOriginXZList[nextGridIndex];
                        currentActiveGridIndex = nextGridIndex;
                    }
                    else
                    {
                        int nextGridIndex = (gridXYList.IndexOf(gridXY) + 1) % gridXYList.Count;
                        gridXY = gridXYList[nextGridIndex];
                        OnActiveGridLevelChanged?.Invoke(this, EventArgs.Empty);
                        if (enableUnityEvents) OnActiveGridLevelChangedUnityEvent?.Invoke();
                        if (showConsoleText && gridLevelChange) Debug.Log("Grid XY " + "<color=green>Grid Level Changed! Current Grid Level :</color> " + (nextGridIndex + 1));
                        gridOriginXY = gridOriginXYList[nextGridIndex];
                        currentActiveGridIndex = nextGridIndex;
                    }
                }
            }
        }

        public void TriggerGridHeightChangeUI(Vector2 value)
        {
            if (changeHeightWithInput)
            {
                if (gridEditorMode == GridEditorMode.GridPro)
                {
                    if (value.y > 0f)
                    {
                        if (gridAxis == GridAxis.XZ)
                        {
                            int nextGridIndex = (gridXZList.IndexOf(gridXZ) + 1);
                            nextGridIndex = Mathf.Clamp(nextGridIndex, 0, gridXZList.Count - 1);
                            gridXZ = gridXZList[nextGridIndex];
                            OnActiveGridLevelChanged?.Invoke(this, EventArgs.Empty);
                            if (enableUnityEvents) OnActiveGridLevelChangedUnityEvent?.Invoke();
                            if (showConsoleText && gridLevelChange) Debug.Log("Grid XZ " + "<color=green>Grid Level Changed! Current Grid Level :</color> " + (nextGridIndex + 1));
                            gridOriginXZ = gridOriginXZList[nextGridIndex];
                            currentActiveGridIndex = nextGridIndex;
                        }
                        else
                        {
                            int nextGridIndex = (gridXYList.IndexOf(gridXY) + 1);
                            nextGridIndex = Mathf.Clamp(nextGridIndex, 0, gridXYList.Count - 1);
                            gridXY = gridXYList[nextGridIndex];
                            OnActiveGridLevelChanged?.Invoke(this, EventArgs.Empty);
                            if (enableUnityEvents) OnActiveGridLevelChangedUnityEvent?.Invoke();
                            if (showConsoleText && gridLevelChange) Debug.Log("Grid XY " + "<color=green>Grid Level Changed! Current Grid Level :</color> " + (nextGridIndex + 1));
                            gridOriginXY = gridOriginXYList[nextGridIndex];
                            currentActiveGridIndex = nextGridIndex;
                        }
                    }
                    else if (value.y < 0f )
                    {
                        if (gridAxis == GridAxis.XZ)
                        {
                            int nextGridIndex = (gridXZList.IndexOf(gridXZ) - 1);
                            nextGridIndex = Mathf.Clamp(nextGridIndex, 0, gridXZList.Count - 1);
                            gridXZ = gridXZList[nextGridIndex];
                            OnActiveGridLevelChanged?.Invoke(this, EventArgs.Empty);
                            if (enableUnityEvents) OnActiveGridLevelChangedUnityEvent?.Invoke();
                            if (showConsoleText && gridLevelChange) Debug.Log("Grid XZ " + "<color=green>Grid Level Changed! Current Grid Level :</color> " + (nextGridIndex + 1));
                            gridOriginXZ = gridOriginXZList[nextGridIndex];
                            currentActiveGridIndex = nextGridIndex;
                        }
                        else
                        {
                            int nextGridIndex = (gridXYList.IndexOf(gridXY) - 1);
                            nextGridIndex = Mathf.Clamp(nextGridIndex, 0, gridXYList.Count - 1);
                            gridXY = gridXYList[nextGridIndex];
                            OnActiveGridLevelChanged?.Invoke(this, EventArgs.Empty);
                            if (enableUnityEvents) OnActiveGridLevelChangedUnityEvent?.Invoke();
                            if (showConsoleText && gridLevelChange) Debug.Log("Grid XY " + "<color=green>Grid Level Changed! Current Grid Level :</color> " + (nextGridIndex + 1));
                            gridOriginXY = gridOriginXYList[nextGridIndex];
                            currentActiveGridIndex = nextGridIndex;
                        }
                    }
                }
            }   
        }

        public void SetGridModeBuilding()
        {
            if (useBuildModeActivationKey)
            {
                if (GetGridMode() != GridMode.Build)
                {
                    isBuildableBuildActive = true;
                    SetGridMode(GridMode.Build);
                }
                else
                {
                    isBuildableBuildActive = false;
                    SetGridMode(GridMode.None);
                }
            }
        }

        public void TriggerBuildablePlacement() //This function handles 'buildableGridObjectTypeSO' object placement
        {
            buildablePlacementKeyHolding = true;
            if (!MultiGridManager.Instance.onGrid || MultiGridManager.Instance.activeGridSystem != this) return;

            if (useBuildModeActivationKey == false)
            {
                isBuildableBuildActive = true;
            }

            if (currentBuildableObjectType == BuildableObjectType.DefaultObject)
            {
                if (buildableGridObjectTypeSO != null)
                {
                    if (GetGridMode() == GridMode.None || GetGridMode() == GridMode.Build)
                    {
                        if (buildableGridObjectTypeSO.holdToPlace && !buildableGridObjectTypeSO.placeAndDeselect)
                        {
                            if (isBuildableBuildActive && buildableGridObjectTypeSO != null && !IsPointerOverUI())
                            {
                                Vector3 mousePosition = GetMouseWorldPosition(); //Call function 'GetMouseWorldPosition' and assign value to 'mousePosition'
                                if (useBuildableDistance)
                                {
                                    if (!distanceCheckObject) distanceCheckObject = Camera.main.transform;
                                    if (Vector3.Distance(distanceCheckObject.position, mousePosition) > distanceMin && Vector3.Distance(distanceCheckObject.position, mousePosition) < distanceMax)
                                    {
                                        CallObjectPlacementGridObject(mousePosition);
                                    }
                                    else
                                    {
                                        if (showConsoleText && objectPlacement) Debug.Log("Grid XZ " + "<color=orange>Cannot Build Here!</color> " + "Not inside the provided Min/Max range. Distance : " + Vector3.Distance(distanceCheckObject.position, mousePosition));
                                    }
                                }
                                else
                                {
                                    CallObjectPlacementGridObject(mousePosition);
                                }
                            }
                        }
                        else
                        {
                            if (isBuildableBuildActive && buildableGridObjectTypeSO != null && !IsPointerOverUI())
                            {
                                Vector3 mousePosition = GetMouseWorldPosition(); //Call function 'GetMouseWorldPosition' and assign value to 'mousePosition'
                                if (useBuildableDistance)
                                {
                                    if (!distanceCheckObject) distanceCheckObject = Camera.main.transform;
                                    if (Vector3.Distance(distanceCheckObject.position, mousePosition) > distanceMin && Vector3.Distance(distanceCheckObject.position, mousePosition) < distanceMax)
                                    {
                                        CallObjectPlacementGridObject(mousePosition);
                                        if (buildableGridObjectTypeSO.placeAndDeselect && !buildableGridObjectTypeSO.holdToPlace) DeselectObjectType(); //Call function 'DeselectObjectType'
                                    }
                                    else
                                    {
                                        if (showConsoleText && objectPlacement) Debug.Log("Grid XZ " + "<color=orange>Cannot Build Here!</color> " + "Not inside the provided Min/Max range. Distance : " + Vector3.Distance(distanceCheckObject.position, mousePosition));
                                    }
                                }
                                else
                                {
                                    CallObjectPlacementGridObject(mousePosition);
                                    if (buildableGridObjectTypeSO.placeAndDeselect) DeselectObjectType(); //Call function 'DeselectObjectType'
                                }
                            }
                        }
                    }
                } 
            }
            else if (currentBuildableObjectType == BuildableObjectType.EdgeObject)
            {
                if (buildableEdgeObjectTypeSO != null)
                {
                    if (GetGridMode() == GridMode.None || GetGridMode() == GridMode.Build)
                    {
                        if (buildableEdgeObjectTypeSO.holdToPlace && !buildableEdgeObjectTypeSO.placeAndDeselect)
                        {
                            if (isBuildableBuildActive && buildableEdgeObjectTypeSO != null && !IsPointerOverUI())
                            {
                                Vector3 mousePosition = GetMouseWorldPosition(); //Call function 'GetMouseWorldPosition' and assign value to 'mousePosition'
                                if (useBuildableDistance)
                                {
                                    if (!distanceCheckObject) distanceCheckObject = Camera.main.transform;
                                    if (Vector3.Distance(distanceCheckObject.position, mousePosition) > distanceMin && Vector3.Distance(distanceCheckObject.position, mousePosition) < distanceMax)
                                    {
                                        CallObjectPlacementEdgeObject(mousePosition);
                                    }
                                    else
                                    {
                                        if (showConsoleText && objectPlacement) Debug.Log("Grid XZ " + "<color=orange>Cannot Build Here!</color> " + "Not inside the provided Min/Max range. Distance : " + Vector3.Distance(distanceCheckObject.position, mousePosition));
                                    }
                                }
                                else
                                {
                                    CallObjectPlacementEdgeObject(mousePosition);
                                }
                            }
                        }
                        else
                        {
                            if (isBuildableBuildActive && buildableEdgeObjectTypeSO != null && !IsPointerOverUI())
                            {
                                Vector3 mousePosition = GetMouseWorldPosition(); //Call function 'GetMouseWorldPosition' and assign value to 'mousePosition'
                                if (useBuildableDistance)
                                {
                                    if (!distanceCheckObject) distanceCheckObject = Camera.main.transform;
                                    if (Vector3.Distance(distanceCheckObject.position, mousePosition) > distanceMin && Vector3.Distance(distanceCheckObject.position, mousePosition) < distanceMax)
                                    {
                                        CallObjectPlacementEdgeObject(mousePosition);
                                        if (buildableEdgeObjectTypeSO.placeAndDeselect && !buildableEdgeObjectTypeSO.holdToPlace) DeselectObjectType(); //Call function 'DeselectObjectType'
                                    }
                                    else
                                    {
                                        if (showConsoleText && objectPlacement) Debug.Log("Grid XZ " + "<color=orange>Cannot Build Here!</color> " + "Not inside the provided Min/Max range. Distance : " + Vector3.Distance(distanceCheckObject.position, mousePosition));
                                    }
                                }
                                else
                                {
                                    CallObjectPlacementEdgeObject(mousePosition);
                                    if (buildableEdgeObjectTypeSO.placeAndDeselect) DeselectObjectType(); //Call function 'DeselectObjectType'
                                }
                            }
                        }
                    }
                } 
            }
            else if (currentBuildableObjectType == BuildableObjectType.FreeObject)
            {
                if (buildableFreeObjectTypeSO != null)
                {
                    if (GetGridMode() == GridMode.None || GetGridMode() == GridMode.Build)
                    {
                        if (buildableFreeObjectTypeSO.holdToPlace && !buildableFreeObjectTypeSO.placeAndDeselect)
                        {
                            if (isBuildableBuildActive && buildableFreeObjectTypeSO != null && !IsPointerOverUI())
                            {
                                Vector3 mousePosition = GetMouseWorldPosition();
                                if (useBuildableDistance)
                                {
                                    if (!distanceCheckObject) distanceCheckObject = Camera.main.transform;
                                    if (Vector3.Distance(distanceCheckObject.position, mousePosition) > distanceMin && Vector3.Distance(distanceCheckObject.position, mousePosition) < distanceMax)
                                    {
                                        CallObjectPlacementFreeObject(mousePosition);
                                    }
                                    else
                                    {
                                        if (showConsoleText && objectPlacement) Debug.Log("Grid XZ " + "<color=orange>Cannot Build Here!</color> " + "Not inside the provided Min/Max range. Distance : " + Vector3.Distance(distanceCheckObject.position, mousePosition));
                                    }
                                }
                                else
                                {
                                    CallObjectPlacementFreeObject(mousePosition);
                                }
                            }
                        }
                        else
                        {
                            if (isBuildableBuildActive && buildableFreeObjectTypeSO != null && !IsPointerOverUI())
                            {
                                Vector3 mousePosition = GetMouseWorldPosition();
                                if (useBuildableDistance)
                                {
                                    if (!distanceCheckObject) distanceCheckObject = Camera.main.transform;
                                    if (Vector3.Distance(distanceCheckObject.position, mousePosition) > distanceMin && Vector3.Distance(distanceCheckObject.position, mousePosition) < distanceMax)
                                    {
                                        CallObjectPlacementFreeObject(mousePosition);
                                        if (buildableFreeObjectTypeSO.placeAndDeselect && !buildableFreeObjectTypeSO.holdToPlace) DeselectObjectType();
                                    }
                                    else
                                    {
                                        if (showConsoleText && objectPlacement) Debug.Log("Grid XZ " + "<color=orange>Cannot Build Here!</color> " + "Not inside the provided Min/Max range. Distance : " + Vector3.Distance(distanceCheckObject.position, mousePosition));
                                    }
                                }
                                else
                                {
                                    CallObjectPlacementFreeObject(mousePosition);
                                    if (buildableFreeObjectTypeSO.placeAndDeselect) DeselectObjectType();
                                }
                            }
                        }
                    }
                } 
            }
            
            if (useBuildModeActivationKey == false)
            {
                isBuildableBuildActive = false;
            }
        }

        public void TriggerBuildablePlacementCancelled()
        {
            buildablePlacementKeyHolding = false;
        }

        public void TriggerBuildableListScroll(Vector2 value)
        {
            if (useBuildModeActivationKey == false)
            {
                isBuildableBuildActive = true;
            }

            if (isBuildableBuildActive)
            {
                if (GetGridMode() == GridMode.None || GetGridMode() == GridMode.Build)
                {
                    if (gridEditorMode == GridEditorMode.GridLite)
                    {
                        if (value.y > 0f)
                        {
                            if (buildableGridObjectTypeSOList.Count > 0)
                            {
                                if (selectedIndex < buildableGridObjectTypeSOList.Count) { selectedIndex = selectedIndex + 1; }

                                selectedIndex = Mathf.Clamp(selectedIndex, 1, buildableGridObjectTypeSOList.Count);
                                buildableGridObjectTypeSO = buildableGridObjectTypeSOList[selectedIndex - 1];
                                RefreshselectedIndexType();
                                isBuildableDestroyActive = false;
                                if (GetGridMode() != GridMode.Build) SetGridMode(GridMode.Build);

                                if (enableUnityEvents) OnSelectedBuildableChangedUnityEvent?.Invoke();
                            }
                        }
                        else if (value.y < 0f )
                        {
                            if (buildableGridObjectTypeSOList.Count > 0)
                            {
                                if (selectedIndex != 1) { selectedIndex = selectedIndex - 1; }
                                
                                selectedIndex = Mathf.Clamp(selectedIndex, 1, buildableGridObjectTypeSOList.Count);
                                buildableGridObjectTypeSO = buildableGridObjectTypeSOList[selectedIndex - 1];
                                RefreshselectedIndexType();
                                isBuildableDestroyActive = false;
                                if (GetGridMode() != GridMode.Build) SetGridMode(GridMode.Build);

                                if (enableUnityEvents) OnSelectedBuildableChangedUnityEvent?.Invoke();
                            }
                        }
                    }
                    else if (gridEditorMode == GridEditorMode.GridPro)
                    {
                        if (value.y > 0f)
                        {
                            int totalCount = buildableGridObjectTypeSOList.Count + buildableEdgeObjectTypeSOList.Count + buildableFreeObjectTypeSOList.Count;
                            if (totalCount > 0)
                            {
                                if (selectedIndex < totalCount) { selectedIndex = selectedIndex + 1; }

                                selectedIndex = Mathf.Clamp(selectedIndex, 1, totalCount);
                                if (selectedIndex <= buildableGridObjectTypeSOList.Count)
                                {
                                    buildableGridObjectTypeSO = buildableGridObjectTypeSOList[selectedIndex - 1];
                                    currentBuildableObjectType = BuildableObjectType.DefaultObject;
                                    buildableFreeObjectTypeSO = null;
                                    buildableEdgeObjectTypeSO = null;
                                }
                                else if (selectedIndex <= buildableGridObjectTypeSOList.Count + buildableEdgeObjectTypeSOList.Count && selectedIndex > buildableGridObjectTypeSOList.Count)
                                {
                                    buildableEdgeObjectTypeSO = buildableEdgeObjectTypeSOList[selectedIndex - buildableGridObjectTypeSOList.Count - 1];
                                    currentBuildableObjectType = BuildableObjectType.EdgeObject;
                                    buildableGridObjectTypeSO = null;
                                    buildableFreeObjectTypeSO = null;
                                }
                                else
                                {
                                    buildableFreeObjectTypeSO = buildableFreeObjectTypeSOList[selectedIndex - buildableGridObjectTypeSOList.Count - buildableEdgeObjectTypeSOList.Count - 1 ];
                                    currentBuildableObjectType = BuildableObjectType.FreeObject;
                                    buildableGridObjectTypeSO = null;
                                    buildableEdgeObjectTypeSO = null;
                                }
                                RefreshselectedIndexType();
                                isBuildableDestroyActive = false;
                                if (GetGridMode() != GridMode.Build) SetGridMode(GridMode.Build);

                                if (enableUnityEvents) OnSelectedBuildableChangedUnityEvent?.Invoke();
                            }
                        }
                        else if (value.y < 0f )
                        {
                            int totalCount = buildableGridObjectTypeSOList.Count + buildableEdgeObjectTypeSOList.Count + buildableFreeObjectTypeSOList.Count;
                            if (totalCount > 0)
                            {
                                if (selectedIndex != 1) { selectedIndex = selectedIndex - 1; }
                                
                                selectedIndex = Mathf.Clamp(selectedIndex, 1, totalCount);
                                if (selectedIndex <= buildableGridObjectTypeSOList.Count)
                                {
                                    buildableGridObjectTypeSO = buildableGridObjectTypeSOList[selectedIndex - 1];
                                    currentBuildableObjectType = BuildableObjectType.DefaultObject;
                                    buildableEdgeObjectTypeSO = null;
                                    buildableFreeObjectTypeSO = null;
                                }
                                else if (selectedIndex <= buildableGridObjectTypeSOList.Count + buildableEdgeObjectTypeSOList.Count && selectedIndex > buildableGridObjectTypeSOList.Count)
                                {
                                    buildableEdgeObjectTypeSO = buildableEdgeObjectTypeSOList[selectedIndex - buildableGridObjectTypeSOList.Count - 1];
                                    currentBuildableObjectType = BuildableObjectType.EdgeObject;
                                    buildableGridObjectTypeSO = null;
                                    buildableFreeObjectTypeSO = null;
                                }
                                else
                                {
                                    buildableFreeObjectTypeSO = buildableFreeObjectTypeSOList[selectedIndex - buildableGridObjectTypeSOList.Count - buildableEdgeObjectTypeSOList.Count - 1 ];
                                    currentBuildableObjectType = BuildableObjectType.FreeObject;
                                    buildableGridObjectTypeSO = null;
                                    buildableEdgeObjectTypeSO = null;
                                }
                                RefreshselectedIndexType();
                                isBuildableDestroyActive = false;
                                if (GetGridMode() != GridMode.Build) SetGridMode(GridMode.Build);

                                if (enableUnityEvents) OnSelectedBuildableChangedUnityEvent?.Invoke();
                            }
                        }
                    }
                }
            }

            if (useBuildModeActivationKey == false)
            {
                isBuildableBuildActive = false;
            }
        }

        public void TriggerBuildableListUI(string buttonName)
        {
            if (useBuildModeActivationKey == false)
            {
                isBuildableBuildActive = true;
            }

            if (isBuildableBuildActive)
            {
                if (GetGridMode() == GridMode.None || GetGridMode() == GridMode.Build)
                {
                    if (gridEditorMode == GridEditorMode.GridLite)
                    {
                        foreach (BuildableGridObjectTypeSO item in buildableGridObjectTypeSOList)
                        {
                            if (item.objectName == buttonName)
                            {
                                buildableGridObjectTypeSO = item;
                                RefreshselectedIndexType();
                                isBuildableDestroyActive = false;
                                if (GetGridMode() != GridMode.Build) SetGridMode(GridMode.Build);

                                if (enableUnityEvents) OnSelectedBuildableChangedUnityEvent?.Invoke();
                                break;
                            }
                        }
                    }
                    else if (gridEditorMode == GridEditorMode.GridPro)
                    {
                        foreach (BuildableGridObjectTypeSO itemGrid in buildableGridObjectTypeSOList)
                        {
                            if (itemGrid.objectName == buttonName)
                            {
                                buildableGridObjectTypeSO = itemGrid;
                                currentBuildableObjectType = BuildableObjectType.DefaultObject;
                                buildableEdgeObjectTypeSO = null;
                                buildableFreeObjectTypeSO = null;
                                RefreshselectedIndexType();
                                isBuildableDestroyActive = false;
                                if (GetGridMode() != GridMode.Build) SetGridMode(GridMode.Build);

                                if (enableUnityEvents) OnSelectedBuildableChangedUnityEvent?.Invoke();
                                break;
                            }
                        }
                        foreach (BuildableEdgeObjectTypeSO itemEdge in buildableEdgeObjectTypeSOList)
                        {
                            if (itemEdge.objectName == buttonName)
                            {
                                buildableEdgeObjectTypeSO = itemEdge;
                                currentBuildableObjectType = BuildableObjectType.EdgeObject;
                                buildableGridObjectTypeSO = null;
                                buildableFreeObjectTypeSO = null;
                                RefreshselectedIndexType();
                                isBuildableDestroyActive = false;
                                if (GetGridMode() != GridMode.Build) SetGridMode(GridMode.Build);

                                if (enableUnityEvents) OnSelectedBuildableChangedUnityEvent?.Invoke();
                                break;
                            }
                        }
                        foreach (BuildableFreeObjectTypeSO itemFree in buildableFreeObjectTypeSOList)
                        {
                            if (itemFree.objectName == buttonName)
                            {
                                buildableFreeObjectTypeSO = itemFree;
                                currentBuildableObjectType = BuildableObjectType.FreeObject;
                                buildableGridObjectTypeSO = null;
                                buildableEdgeObjectTypeSO = null;
                                RefreshselectedIndexType();
                                isBuildableDestroyActive = false;
                                if (GetGridMode() != GridMode.Build) SetGridMode(GridMode.Build);

                                if (enableUnityEvents) OnSelectedBuildableChangedUnityEvent?.Invoke();
                                break;
                            }
                        }
                    }
                }
            }

            if (useBuildModeActivationKey == false)
            {
                isBuildableBuildActive = false;
            }
        }

        public void TriggerGhostRotateLeft()
        {
            ghostRotateLeftKeyHolding = true;
            dir = BuildableGridObjectTypeSO.GetNextDirLeft(dir);
            if (edgeRotation == 0)
            {
                edgeRotation = 180;
            }
            else
            {
                edgeRotation = 0;
            }
            OnBuildableEdgeObjectFlip?.Invoke(edgeRotation);
        }

        public void TriggerGhostRotateRight()
        {
            ghostRotateRightKeyHolding = true;
            dir = BuildableGridObjectTypeSO.GetNextDirRight(dir);
            if (edgeRotation == 0)
            {
                edgeRotation = 180;
            }
            else
            {
                edgeRotation = 0;
            }
            OnBuildableEdgeObjectFlip?.Invoke(edgeRotation);
        }

        public void TriggerGhostRotateLeftCancelled()
        {
            ghostRotateLeftKeyHolding = false;
        }

        public void TriggerGhostRotateRightCancelled()
        {
            ghostRotateRightKeyHolding = false;
        }

        public void SetGridModeDestruction()
        {
            if (useDestructionModeActivationKey)
            {
                if (GetGridMode() != GridMode.Destruct)
                {
                    buildableGridObjectTypeSO = null;
                    buildableEdgeObjectTypeSO = null;
                    buildableFreeObjectTypeSO = null;
                    isBuildableDestroyActive = true;
                    SetGridMode(GridMode.Destruct);
                    RefreshselectedIndexType();
                }
                else
                {
                    buildableGridObjectTypeSO = null;
                    buildableEdgeObjectTypeSO = null;
                    buildableFreeObjectTypeSO = null;
                    isBuildableDestroyActive = false;
                    SetGridMode(GridMode.None);
                    RefreshselectedIndexType();
                }
            }
        }

        public void TriggerBuildableDestroy() //This function handle destroying spawned objects
        {
            if (useDestructionModeActivationKey == false)
            {
                buildableGridObjectTypeSO = null;
                buildableEdgeObjectTypeSO = null;
                buildableFreeObjectTypeSO = null;
                isBuildableDestroyActive = true;
                if (GetGridMode() != GridMode.Destruct) SetGridMode(GridMode.Destruct);
                RefreshselectedIndexType();
            }

            if (GetGridMode() == GridMode.None || GetGridMode() == GridMode.Destruct)
            {
                if (isBuildableDestroyActive && !IsPointerOverUI())
                {
                    if (gridAxis == GridAxis.XZ)
                    {
                        BuildableGridObject buildableGridObject;
                        gridXZ.GetXZ(localMousePosition, out int x, out int z);

                        Vector3 mousePosition2 = GetPlacedObjectMouseWorldPosition(); //Newly Implemented (Experimental)
                        gridXZ.GetXZ(mousePosition2, out int c, out int v); //Newly Implemented (Experimental)

                        if (IsValidGridPositionXZ(new Vector2Int(x, z)))
                        {
                            buildableGridObject = gridXZ.GetGridObjectXZ(localMousePosition).GetPlacedObject();
                        }
                        else if(IsValidGridPositionXZ(new Vector2Int(c, v))) //Newly Implemented (Experimental)
                        {
                            buildableGridObject = gridXZ.GetGridObjectXZ(mousePosition2).GetPlacedObject();
                        }
                        else
                        {
                            buildableGridObject = null;
                        }

                        if (buildableGridObject != null)
                        {
                            buildableGridObject.DestroySelf();
                            if (enableUnityEvents) OnObjectRemovedUnityEvent?.Invoke();
                            if (showConsoleText && objectDestruction) Debug.Log("Grid XZ " + "<color=Red>Object Destroyed :</color> " + buildableGridObject);

                            List<Vector2Int> gridPositionList = buildableGridObject.GetGridPositionList();
                            foreach (Vector2Int gridPosition in gridPositionList)
                            {
                                gridXZ.GetGridObjectXZ(gridPosition.x, gridPosition.y).ClearPlacedObject();
                                if (enableUnityEvents) OnGridCellChangedUnityEvent?.Invoke();
                            }
                        }
                    }
                    else
                    {
                        Vector3 mousePosition = GetMouseWorldPosition();
                        BuildableGridObject buildableGridObject;
                        gridXY.GetXY(mousePosition, out int x, out int z);

                        Vector3 mousePosition2 = GetPlacedObjectMouseWorldPosition(); //Newly Implemented (Experimental)
                        gridXY.GetXY(mousePosition2, out int c, out int v); //Newly Implemented (Experimental)

                        if (IsValidGridPositionXY(new Vector2Int(x, z)))
                        {
                            buildableGridObject = gridXY.GetGridObjectXY(mousePosition).GetPlacedObject();
                        }
                        else if(IsValidGridPositionXY(new Vector2Int(c, v))) //Newly Implemented (Experimental)
                        {
                            buildableGridObject = gridXY.GetGridObjectXY(mousePosition2).GetPlacedObject();
                        }
                        else
                        {
                            buildableGridObject = null;
                        }

                        if (buildableGridObject != null)
                        {
                            buildableGridObject.DestroySelf();
                            if (enableUnityEvents) OnObjectRemovedUnityEvent?.Invoke();
                            if (showConsoleText && objectDestruction) Debug.Log("Grid XY " + "<color=Red>Object Destroyed :</color> " + buildableGridObject);

                            List<Vector2Int> gridPositionList = buildableGridObject.GetGridPositionList();
                            foreach (Vector2Int gridPosition in gridPositionList)
                            {
                                gridXY.GetGridObjectXY(gridPosition.x, gridPosition.y).ClearPlacedObject();
                                if (enableUnityEvents) OnGridCellChangedUnityEvent?.Invoke();
                            }
                        }
                    }
                    DestroyBuildableEdgeObject();
                    DestroyBuildableFreeObject();
                }
            }

            if (useDestructionModeActivationKey == false)
            {
                isBuildableDestroyActive = false;
                if (GetGridMode() != GridMode.None) SetGridMode(GridMode.None);
            }
        }

        private void DestroyBuildableEdgeObject() //This function handle destroying spawned objects
        {
            if (gridAxis == GridAxis.XZ)
            {
                BuildableEdgeObject buildableEdgeObject = GetPlacedEdgeObjectMouseWorldPosition();

                if (buildableEdgeObject != null)
                {
                    buildableEdgeObject.DestroySelf();
                    if (enableUnityEvents) OnObjectRemovedUnityEvent?.Invoke();
                    if (showConsoleText && objectDestruction) Debug.Log("Grid XZ " + "<color=Red>Object Destroyed :</color> " + buildableEdgeObject);

                    List<Vector2Int> gridPositionList = buildableEdgeObject.GetGridPositionList();
                    switch (buildableEdgeObject.GetEdgeObjectDir())
                    {
                        case BuildableEdgeObjectTypeSO.Dir.Down:
                            foreach (Vector2Int gridPosition in gridPositionList)
                            {
                                gridXZ.GetGridObjectXZ(gridPosition.x, gridPosition.y).ClearPlacedDownEdgeObject();
                                if (enableUnityEvents) OnGridCellChangedUnityEvent?.Invoke();
                            }
                        break;
                        case BuildableEdgeObjectTypeSO.Dir.Left:
                            foreach (Vector2Int gridPosition in gridPositionList)
                            {
                                gridXZ.GetGridObjectXZ(gridPosition.x, gridPosition.y).ClearPlacedLeftEdgeObject();
                                if (enableUnityEvents) OnGridCellChangedUnityEvent?.Invoke();
                            }
                        break;
                        case BuildableEdgeObjectTypeSO.Dir.Up:
                            foreach (Vector2Int gridPosition in gridPositionList)
                            {
                                gridXZ.GetGridObjectXZ(gridPosition.x, gridPosition.y).ClearPlacedUpEdgeObject();
                                if (enableUnityEvents) OnGridCellChangedUnityEvent?.Invoke();
                            }
                        break;
                        case BuildableEdgeObjectTypeSO.Dir.Right:
                            foreach (Vector2Int gridPosition in gridPositionList)
                            {
                                gridXZ.GetGridObjectXZ(gridPosition.x, gridPosition.y).ClearPlacedRightEdgeObject();
                                if (enableUnityEvents) OnGridCellChangedUnityEvent?.Invoke();
                            }
                        break;
                    }
                }
            }
            else
            {

            }
        }

        private void DestroyBuildableFreeObject()
        {
            BuildableFreeObject buildableFreeObject = null;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, freeObjectCollidingLayerMask))
            {
                if (raycastHit.collider.gameObject.transform.root.GetComponent<BuildableFreeObject>())
                buildableFreeObject = raycastHit.collider.gameObject.transform.root.GetComponent<BuildableFreeObject>();
            }

            if (buildableFreeObject != null) //If 'placedObject' not empty
            {
                buildableFreeObject.DestroySelf(); //Call function 'DestroySelf' 'placedObject' that will destroy the object
                if (enableUnityEvents) OnObjectRemovedUnityEvent?.Invoke(); //Invoke 'OnGridCellChangeUnityEvent' event
                if (showConsoleText && objectDestruction) Debug.Log("Grid XZ " + "<color=Red>Object Destroyed :</color> " + buildableFreeObject);

                foreach (Transform buildableFreeObjectTransform in builtBuildableFreeObjectList) //Cycle through the list
                {
                    if (buildableFreeObjectTransform == buildableFreeObject.transform)
                    {
                        builtBuildableFreeObjectList.Remove(buildableFreeObjectTransform);
                        break;
                    }
                }
            }
        }

        public void TriggerGridSave()
        {
            if (enableSaveAndLoad)
            {
                GridSave();
            }
        }

        public void TriggerGridLoad()
        {
            if (enableSaveAndLoad)
            {
                GridLoad();
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
        //                                                                                              SUPPORT FUNCTIONS                                                                                           //
        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//

        private void CallObjectPlacementGridObject(Vector3 mousePosition)
        {
            if (gridAxis == GridAxis.XZ)
            {
                gridXZ.GetXZ(mousePosition, out int x, out int z);
                Vector2Int buildableGridObjectOrigin = new Vector2Int(x, z);

                if (TryPlaceGridObjectXZ(buildableGridObjectOrigin, buildableGridObjectTypeSO, dir, false, out BuildableGridObject buildableGridObject))
                {
                    if (showConsoleText && objectPlacement) Debug.Log("Grid XZ " + "<color=green>Object Placed :</color> " + buildableGridObject);
                }
                else
                {
                    if (IsValidGridPositionXZ(new Vector2Int(x, z)))
                    {
                        if (showConsoleText && objectPlacement) Debug.Log("Grid XZ " + "<color=orange>Cannot Build Here!</color> " + "Grid Position: " + x +","+ z);
                    }
                    else
                    {
                        if (showConsoleText && objectPlacement) Debug.Log("Grid XZ " + "<color=orange>Cannot Build Here!</color> " + "Out of the Grid");
                    }
                }
            }
            else
            {
                gridXY.GetXY(mousePosition, out int x, out int y);
                Vector2Int buildableGridObjectOrigin = new Vector2Int(x, y);

                if (TryPlaceGridObjectXY(buildableGridObjectOrigin, buildableGridObjectTypeSO, dir, false, out BuildableGridObject buildableGridObject))
                {
                    if (showConsoleText && objectPlacement) Debug.Log("Grid XY " + "<color=green>Object Placed :</color> " + buildableGridObject);
                }
                else
                {
                    if (IsValidGridPositionXY(new Vector2Int(x, y)))
                    {
                        if (showConsoleText && objectPlacement) Debug.Log("Grid XY " + "<color=orange>Cannot Build Here!</color> " + "Grid Position: " + x +","+ y);
                    }
                    else
                    {
                        if (showConsoleText && objectPlacement) Debug.Log("Grid XY " + "<color=orange>Cannot Build Here!</color> " + "Out of the Grid");
                    }
                }
            }
        }

        private void CallObjectPlacementEdgeObject(Vector3 mousePosition)
        {
            if (gridAxis == GridAxis.XZ)
            {
                gridXZ.GetXZ(mousePosition, out int x, out int z);
                Vector2Int buildableEdgeObjectOrigin = new Vector2Int(x, z);

                if (TryPlaceEdgeObjectXZ(buildableEdgeObjectOrigin, buildableEdgeObjectTypeSO, CalcualteEdgeObjectDir(localMousePosition), edgeRotation, localMousePosition, false, out BuildableEdgeObject buildableEdgeObject))
                {
                    if (showConsoleText && objectPlacement) Debug.Log("Grid XZ " + "<color=green>Object Placed :</color> " + buildableEdgeObject);
                }
                else
                {
                    if (IsValidGridPositionXZ(new Vector2Int(x, z)))
                    {
                        if (showConsoleText && objectPlacement) Debug.Log("Grid XZ " + "<color=orange>Cannot Build Here!</color> " + "Grid Position: " + x +","+ z);
                    }
                    else
                    {
                        if (showConsoleText && objectPlacement) Debug.Log("Grid XZ " + "<color=orange>Cannot Build Here!</color> " + "Out of the Grid");
                    }
                }
            }
            else
            {
                gridXY.GetXY(mousePosition, out int x, out int y);
                Vector2Int buildableEdgeObjectOrigin = new Vector2Int(x, y);

                if (TryPlaceEdgeObjectXY(buildableEdgeObjectOrigin, buildableEdgeObjectTypeSO, CalcualteEdgeObjectDir(localMousePosition), edgeRotation, localMousePosition, false, out BuildableEdgeObject buildableEdgeObject))
                {
                    if (showConsoleText && objectPlacement) Debug.Log("Grid XY " + "<color=green>Object Placed :</color> " + buildableEdgeObject);
                }
                else
                {
                    if (IsValidGridPositionXY(new Vector2Int(x, y)))
                    {
                        if (showConsoleText && objectPlacement) Debug.Log("Grid XY " + "<color=orange>Cannot Build Here!</color> " + "Grid Position: " + x +","+ y);
                    }
                    else
                    {
                        if (showConsoleText && objectPlacement) Debug.Log("Grid XY " + "<color=orange>Cannot Build Here!</color> " + "Out of the Grid");
                    }
                }
            }
        }
        
        private void CallObjectPlacementFreeObject(Vector3 mousePosition)
        {
            if (gridAxis == GridAxis.XZ)
            {
                Vector3 placedObjectWorldPosition = BuildableFreeObjectCollidingMousePosition();
                if (TryPlaceFreeObjectXZ(buildableFreeObjectTypeSO, placedObjectWorldPosition, buildableFreeObjectRotation, false, out BuildableFreeObject buildableFreeObject))
                {
                    if (showConsoleText && objectPlacement) Debug.Log("Grid XZ " + "<color=green>Object Placed :</color> " + buildableFreeObject);
                }
                else
                {
                    if (showConsoleText && objectPlacement) Debug.Log("Grid XZ " + "<color=orange>Cannot Build Here!</color> " + "Out of the Grid");
                }
            }
            else
            {
                Vector3 placedObjectWorldPosition = BuildableFreeObjectCollidingMousePosition();
                if (TryPlaceFreeObjectXY(buildableFreeObjectTypeSO, placedObjectWorldPosition, buildableFreeObjectRotation, false, out BuildableFreeObject buildableFreeObject))
                {
                    if (showConsoleText && objectPlacement) Debug.Log("Grid XY " + "<color=green>Object Placed :</color> " + buildableFreeObject);
                }
                else
                {
                    if (showConsoleText && objectPlacement) Debug.Log("Grid XY " + "<color=orange>Cannot Build Here!</color> " + "Out of the Grid");
                }
            }
        }

        public bool TryPlaceGridObjectXZ(Vector2Int placedObjectOrigin, BuildableGridObjectTypeSO buildableGridObjectTypeSO, BuildableGridObjectTypeSO.Dir dir, bool isCallFromLoad, out BuildableGridObject buildableGridObject)
        {
            return TryPlaceGridObjectXZ(gridXZ, placedObjectOrigin, buildableGridObjectTypeSO, dir, isCallFromLoad, out buildableGridObject);
        }

        public bool TryPlaceGridObjectXZ(GridXZ<GridObjectXZ> passedGridXZ, Vector2Int placedObjectOrigin, BuildableGridObjectTypeSO buildableGridObjectTypeSO, BuildableGridObjectTypeSO.Dir dir, bool isCallFromLoad, out BuildableGridObject buildableGridObject) //This function handles object placement and return a bool
        {
            List<Vector2Int> gridPositionList = buildableGridObjectTypeSO.GetGridPositionList(placedObjectOrigin, dir, passedGridXZ.GetCellSize()); //Grab 'GetGridPositionList' from 'buildableGridObjectTypeSO' and cache it to a new list 'gridPositionList'
            bool canBuild = true; //Set 'canBuild' to true

            foreach (Vector2Int gridPosition in gridPositionList) //Loop through created 'gridPositionList'
            {
                bool isValidPosition = passedGridXZ.IsValidGridPosition(gridPosition); //Call 'IsValidGridPosition' function in 'grid' and assign return value in 'isValidPosition'

                if (!isValidPosition) //If not a valid position
                {
                    //Debug.Log("Not a valid build position");
                    canBuild = false; //Set 'canBuild' to false
                    break;
                }
                if (!passedGridXZ.GetGridObjectXZ(gridPosition.x, gridPosition.y).CanBuild()) //Call 'CanBuild' function and check if it returns false
                {
                    //Debug.Log("Not a valid build position");
                    canBuild = false; //Set 'canBuild' to false
                    break;
                }
                if (!isCallFromLoad)
                {
                    if (buildableGridObjectTypeSO.enableBuildCondition)
                    {
                        if (!GetBuildConditionBuildableGridObject(buildableGridObjectTypeSO))
                        {
                            //Debug.Log("Conditions not met");
                            canBuild = false; //Set 'canBuild' to false
                            break;
                        }
                    }

                    // Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    // if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f))
                    // {
                    //     if (raycastHit.collider.GetComponent<BuildableObjectAreaBlocker>())
                    //     {
                    //         //Debug.Log("Conditions not met");
                    //         canBuild = false; //Set 'canBuild' to false
                    //         break;
                    //     }
                    // }

                    if (buildableAreaBlockerHit)
                    {
                        //Debug.Log("Conditions not met");
                        canBuild = false; //Set 'canBuild' to false
                        break;
                    }
                }
            }

            if (canBuild) //If 'canBuild' is true
            {
                Vector2Int rotationOffset = buildableGridObjectTypeSO.GetRotationOffset(dir, passedGridXZ.GetCellSize()); //Call 'GetRotationOffset' function in 'buildableGridObjectTypeSO' and assign value to 'rotationOffset'
                Vector3 mousePosition = GetMouseWorldPosition(); //Call function 'GetMouseWorldPosition' and cache it in 'mousePosition'
                Vector3 placedObjectWorldPositionOld = passedGridXZ.GetWorldPosition(placedObjectOrigin.x, placedObjectOrigin.y) + new Vector3(rotationOffset.x, transform.localPosition.y, rotationOffset.y) * passedGridXZ.GetCellSize(); //Call 'GetWorldPosition' in 'grid' and assign value to 'placedObjectWorldPosition'
                Vector3 placedObjectWorldPosition = new Vector3(placedObjectWorldPositionOld.x, gridOriginXZ.y, placedObjectWorldPositionOld.z);

                buildableGridObject = BuildableGridObject.Create(placedObjectWorldPosition, placedObjectOrigin, dir, buildableGridObjectTypeSO, this); //Call function 'Create' in 'PlacedObject' that will instantiate an object and cache it in 'placedObject'
                
                if (!isCallFromLoad)
                {
                    if (buildableGridObjectTypeSO.enableBuildCondition)
                    {
                        CompleteBuildConditionBuildableGridObject(buildableGridObjectTypeSO);
                    }
                }

                foreach (Vector2Int gridPosition in gridPositionList) //Cycle througth 'gridPosition' in 'gridPositionList'
                {
                    passedGridXZ.GetGridObjectXZ(gridPosition.x, gridPosition.y).SetPlacedObject(buildableGridObject); //Call function 'GetGridObject' in 'grid' and call function 'SetPlacedObject' 
                    if (enableUnityEvents) OnGridCellChangedUnityEvent?.Invoke(); //Invoke the Unity event 'OnGridCellChangeUnityEvent'
                }

                buildableGridObject.GridSetupDone(this, true, currentActiveGridIndex, dir); //Call function 'GridSetupDone'
                OnObjectPlaced?.Invoke(buildableGridObject, EventArgs.Empty); //Invoke the event 'OnObjectPlaced'
                if (enableUnityEvents) OnObjectPlacedUnityEvent?.Invoke(); //Invoke the Unity event 'OnObjectPlacedUnityEvent'

                return true;
            }
            else //If 'canBuild' is false
            {
                buildableGridObject = null; //Set 'placedObject' to empty
                return false;
            }
        }
        
        public bool TryPlaceGridObjectXY(Vector2Int placedObjectOrigin, BuildableGridObjectTypeSO buildableGridObjectTypeSO, BuildableGridObjectTypeSO.Dir dir, bool isCallFromLoad, out BuildableGridObject buildableGridObject)
        {
            return TryPlaceGridObjectXY(gridXY, placedObjectOrigin, buildableGridObjectTypeSO, dir, isCallFromLoad, out buildableGridObject);
        }

        public bool TryPlaceGridObjectXY(GridXY<GridObjectXY> passedGridXY, Vector2Int placedObjectOrigin, BuildableGridObjectTypeSO buildableGridObjectTypeSO, BuildableGridObjectTypeSO.Dir dir, bool isCallFromLoad, out BuildableGridObject buildableGridObject) //This function handles object placement and return a bool
        {
            List<Vector2Int> gridPositionList = buildableGridObjectTypeSO.GetGridPositionList(placedObjectOrigin, dir, passedGridXY.GetCellSize()); //Grab 'GetGridPositionList' from 'buildableGridObjectTypeSO' and cache it to a new list 'gridPositionList'
            bool canBuild = true; //Set 'canBuild' to true

            foreach (Vector2Int gridPosition in gridPositionList) //Loop through created 'gridPositionList'
            {
                bool isValidPosition = passedGridXY.IsValidGridPosition(gridPosition); //Call 'IsValidGridPosition' function in 'grid' and assign return value in 'isValidPosition'

                if (!isValidPosition) //If not a valid position
                {
                    //Debug.Log("Not a valid build position");
                    canBuild = false; //Set 'canBuild' to false
                    break;
                }
                if (!passedGridXY.GetGridObjectXY(gridPosition.x, gridPosition.y).CanBuild()) //Call 'CanBuild' function and check if it returns false
                {
                    //Debug.Log("Not a valid build position");
                    canBuild = false; //Set 'canBuild' to false
                    break;
                }
                if (!isCallFromLoad)
                {
                    if (buildableGridObjectTypeSO.enableBuildCondition)
                    {
                        if (!GetBuildConditionBuildableGridObject(buildableGridObjectTypeSO))
                        {
                            //Debug.Log("Conditions not met");
                            canBuild = false; //Set 'canBuild' to false
                            break;
                        }
                    }

                    // Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    // if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f))
                    // {
                    //     if (raycastHit.collider.GetComponent<BuildableObjectAreaBlocker>())
                    //     {
                    //         //Debug.Log("Conditions not met");
                    //         canBuild = false; //Set 'canBuild' to false
                    //         break;
                    //     }
                    // }

                    if (buildableAreaBlockerHit)
                    {
                        //Debug.Log("Conditions not met");
                        canBuild = false; //Set 'canBuild' to false
                        break;
                    }
                }
            }

            if (canBuild) //If 'canBuild' is true
            {
                Vector2Int rotationOffset = buildableGridObjectTypeSO.GetRotationOffset(dir, passedGridXY.GetCellSize()); //Call 'GetRotationOffset' function in 'buildableGridObjectTypeSO' and assign value to 'rotationOffset'
                Vector3 mousePosition = GetMouseWorldPosition(); //Call function 'GetMouseWorldPosition' and cache it in 'mousePosition'
                Vector3 placedObjectWorldPositionOld = passedGridXY.GetWorldPosition(placedObjectOrigin.x, placedObjectOrigin.y) + new Vector3(rotationOffset.x, rotationOffset.y, transform.localPosition.z) * passedGridXY.GetCellSize(); //Call 'GetWorldPosition' in 'grid' and assign value to 'placedObjectWorldPosition'
                Vector3 placedObjectWorldPosition = new Vector3(placedObjectWorldPositionOld.x, placedObjectWorldPositionOld.y, gridOriginXY.z);

                buildableGridObject = BuildableGridObject.Create(placedObjectWorldPosition, placedObjectOrigin, dir, buildableGridObjectTypeSO, this); //Call function 'Create' in 'PlacedObject' that will instantiate an object and cache it in 'placedObject'
                buildableGridObject.transform.rotation = Quaternion.Euler(0, 0, -buildableGridObjectTypeSO.GetRotationAngle(dir));
                                
                if (!isCallFromLoad)
                {
                    if (buildableGridObjectTypeSO.enableBuildCondition)
                    {
                        CompleteBuildConditionBuildableGridObject(buildableGridObjectTypeSO);
                    }
                }

                foreach (Vector2Int gridPosition in gridPositionList) //Cycle througth 'gridPosition' in 'gridPositionList'
                {
                    passedGridXY.GetGridObjectXY(gridPosition.x, gridPosition.y).SetPlacedObject(buildableGridObject); //Call function 'GetGridObject' in 'grid' and call function 'SetPlacedObject' 
                    if (enableUnityEvents) OnGridCellChangedUnityEvent?.Invoke(); //Invoke the Unity event 'OnGridCellChangeUnityEvent'
                }

                buildableGridObject.GridSetupDone(this, true, currentActiveGridIndex, dir); //Call function 'GridSetupDone'
                OnObjectPlaced?.Invoke(buildableGridObject, EventArgs.Empty); //Invoke the event 'OnObjectPlaced'
                if (enableUnityEvents) OnObjectPlacedUnityEvent?.Invoke(); //Invoke the Unity event 'OnObjectPlacedUnityEvent'

                return true;
            }
            else //If 'canBuild' is false
            {
                buildableGridObject = null; //Set 'placedObject' to empty
                return false;
            }
        }

        public bool TryPlaceEdgeObjectXZ(Vector2Int placedObjectOrigin, BuildableEdgeObjectTypeSO buildableEdgeObjectTypeSO, BuildableEdgeObjectTypeSO.Dir dir, float edgeRotation, Vector3 mousePosition, bool isCallFromLoad, out BuildableEdgeObject buildableEdgeObject)
        {
            return TryPlaceEdgeObjectXZ(gridXZ, placedObjectOrigin, buildableEdgeObjectTypeSO, dir, edgeRotation, mousePosition, isCallFromLoad, out buildableEdgeObject);
        }

        public bool TryPlaceEdgeObjectXZ(GridXZ<GridObjectXZ> passedGridXZ, Vector2Int placedObjectOrigin, BuildableEdgeObjectTypeSO buildableEdgeObjectTypeSO, BuildableEdgeObjectTypeSO.Dir dir, float edgeRotation, Vector3 mousePosition, bool isCallFromLoad, out BuildableEdgeObject buildableEdgeObject) //This function handles object placement and return a bool
        {
            List<Vector2Int> gridPositionList = buildableEdgeObjectTypeSO.GetGridPositionList(placedObjectOrigin, dir, passedGridXZ.GetCellSize()); //Grab 'GetGridPositionList' from 'buildableGridObjectTypeSO' and cache it to a new list 'gridPositionList'
            bool canBuild = true; //Set 'canBuild' to true

            foreach (Vector2Int gridPosition in gridPositionList) //Loop through created 'gridPositionList'
            {
                bool isValidPosition = passedGridXZ.IsValidGridPosition(gridPosition); //Call 'IsValidGridPosition' function in 'grid' and assign return value in 'isValidPosition'

                if (!isValidPosition) //If not a valid position
                {
                    //Debug.Log("Not a valid build position");
                    canBuild = false; //Set 'canBuild' to false
                    break;
                }

                switch (dir)
                {
                    case BuildableEdgeObjectTypeSO.Dir.Down:
                        if (!passedGridXZ.GetGridObjectXZ(gridPosition.x, gridPosition.y).CanBuildEdgeObjectDown()) //Call 'CanBuild' function and check if it returns false
                        {
                            //Debug.Log("Not a valid build position");
                            canBuild = false; //Set 'canBuild' to false
                            break;
                        }
                    break;
                    case BuildableEdgeObjectTypeSO.Dir.Left:
                        if (!passedGridXZ.GetGridObjectXZ(gridPosition.x, gridPosition.y).CanBuildEdgeObjectLeft()) //Call 'CanBuild' function and check if it returns false
                        {
                            //Debug.Log("Not a valid build position");
                            canBuild = false; //Set 'canBuild' to false
                            break;
                        }
                    break;
                    case BuildableEdgeObjectTypeSO.Dir.Up:
                        if (!passedGridXZ.GetGridObjectXZ(gridPosition.x, gridPosition.y).CanBuildEdgeObjectUp()) //Call 'CanBuild' function and check if it returns false
                        {
                            //Debug.Log("Not a valid build position");
                            canBuild = false; //Set 'canBuild' to false
                            break;
                        }
                    break;
                    case BuildableEdgeObjectTypeSO.Dir.Right:
                        if (!passedGridXZ.GetGridObjectXZ(gridPosition.x, gridPosition.y).CanBuildEdgeObjectRight()) //Call 'CanBuild' function and check if it returns false
                        {
                            //Debug.Log("Not a valid build position");
                            canBuild = false; //Set 'canBuild' to false
                            break;
                        }
                    break;
                }

                if (!isCallFromLoad)
                {
                    if (buildableEdgeObjectTypeSO.enableBuildCondition)
                    {
                        if (!GetBuildConditionBuildableEdgeObject(buildableEdgeObjectTypeSO))
                        {
                            //Debug.Log("Conditions not met");
                            canBuild = false; //Set 'canBuild' to false
                            break;
                        }
                    }

                    // Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    // if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f))
                    // {
                    //     if (raycastHit.collider.GetComponent<BuildableObjectAreaBlocker>())
                    //     {
                    //         //Debug.Log("Conditions not met");
                    //         canBuild = false; //Set 'canBuild' to false
                    //         break;
                    //     }
                    // }

                    if (buildableAreaBlockerHit)
                    {
                        //Debug.Log("Conditions not met");
                        canBuild = false; //Set 'canBuild' to false
                        break;
                    }
                }
            }

            if (canBuild) //If 'canBuild' is true
            {
                Vector2Int rotationOffset = buildableEdgeObjectTypeSO.GetRotationOffset(CalcualteEdgeObjectDir(mousePosition), passedGridXZ.GetCellSize()); //Call 'GetRotationOffset' function in 'buildableEdgeObjectTypeSO' and assign value to 'rotationOffset'
                Vector3 placedObjectWorldPositionOld = passedGridXZ.GetWorldPosition(placedObjectOrigin.x, placedObjectOrigin.y) + new Vector3(rotationOffset.x, transform.localPosition.y, rotationOffset.y) * passedGridXZ.GetCellSize(); //Call 'GetWorldPosition' in 'grid' and assign value to 'placedObjectWorldPosition'
                Vector3 placedObjectWorldPosition = new Vector3(placedObjectWorldPositionOld.x, gridOriginXZ.y, placedObjectWorldPositionOld.z);

                buildableEdgeObject = BuildableEdgeObject.Create(placedObjectWorldPosition, placedObjectOrigin, CalcualteEdgeObjectDir(mousePosition), mousePosition, buildableEdgeObjectTypeSO, this, edgeRotation); //Call function 'Create' in 'PlacedObject' that will instantiate an object and cache it in 'placedObject'
                
                if (!isCallFromLoad)
                {
                    if (buildableEdgeObjectTypeSO.enableBuildCondition)
                    {
                        CompleteBuildConditionBuildableEdgeObject(buildableEdgeObjectTypeSO);
                    }
                }

                foreach (Vector2Int gridPosition in gridPositionList) //Cycle througth 'gridPosition' in 'gridPositionList'
                {
                    switch (CalcualteEdgeObjectDir(mousePosition))
                    {
                        case BuildableEdgeObjectTypeSO.Dir.Down:
                            passedGridXZ.GetGridObjectXZ(gridPosition.x, gridPosition.y).SetPlacedDownEdgeObject(buildableEdgeObject); //Call function 'GetGridObject' in 'grid' and call function 'SetPlacedObject' 
                        break;
                        case BuildableEdgeObjectTypeSO.Dir.Left:
                            passedGridXZ.GetGridObjectXZ(gridPosition.x, gridPosition.y).SetPlacedLeftEdgeObject(buildableEdgeObject); //Call function 'GetGridObject' in 'grid' and call function 'SetPlacedObject' 
                        break;
                        case BuildableEdgeObjectTypeSO.Dir.Up:
                            passedGridXZ.GetGridObjectXZ(gridPosition.x, gridPosition.y).SetPlacedUpEdgeObject(buildableEdgeObject); //Call function 'GetGridObject' in 'grid' and call function 'SetPlacedObject' 
                        break;
                        case BuildableEdgeObjectTypeSO.Dir.Right:
                            passedGridXZ.GetGridObjectXZ(gridPosition.x, gridPosition.y).SetPlacedRightEdgeObject(buildableEdgeObject); //Call function 'GetGridObject' in 'grid' and call function 'SetPlacedObject' 
                        break;
                    }
                    if (enableUnityEvents) OnGridCellChangedUnityEvent?.Invoke(); //Invoke the Unity event 'OnGridCellChangeUnityEvent'
                }

                buildableEdgeObject.GridSetupDone(this, true, currentActiveGridIndex, dir); //Call function 'GridSetupDone'
                OnObjectPlaced?.Invoke(buildableEdgeObject, EventArgs.Empty); //Invoke the event 'OnObjectPlaced'
                if (enableUnityEvents) OnObjectPlacedUnityEvent?.Invoke(); //Invoke the Unity event 'OnObjectPlacedUnityEvent'

                return true;
            }
            else //If 'canBuild' is false
            {
                buildableEdgeObject = null; //Set 'placedObject' to empty
                return false;
            }
        }

        public bool TryPlaceEdgeObjectXY(Vector2Int placedObjectOrigin, BuildableEdgeObjectTypeSO buildableEdgeObjectTypeSO, BuildableEdgeObjectTypeSO.Dir dir, float edgeRotation, Vector3 mousePosition, bool isCallFromLoad, out BuildableEdgeObject buildableEdgeObject)
        {
            return TryPlaceEdgeObjectXY(gridXY, placedObjectOrigin, buildableEdgeObjectTypeSO, dir, edgeRotation, mousePosition, isCallFromLoad, out buildableEdgeObject);
        }

        public bool TryPlaceEdgeObjectXY(GridXY<GridObjectXY> passedGridXY, Vector2Int placedObjectOrigin, BuildableEdgeObjectTypeSO buildableEdgeObjectTypeSO, BuildableEdgeObjectTypeSO.Dir dir, float edgeRotation, Vector3 mousePosition, bool isCallFromLoad, out BuildableEdgeObject buildableEdgeObject) //This function handles object placement and return a bool
        {
            List<Vector2Int> gridPositionList = buildableEdgeObjectTypeSO.GetGridPositionList(placedObjectOrigin, dir, passedGridXY.GetCellSize()); //Grab 'GetGridPositionList' from 'buildableGridObjectTypeSO' and cache it to a new list 'gridPositionList'
            bool canBuild = true; //Set 'canBuild' to true

            foreach (Vector2Int gridPosition in gridPositionList) //Loop through created 'gridPositionList'
            {
                bool isValidPosition = passedGridXY.IsValidGridPosition(gridPosition); //Call 'IsValidGridPosition' function in 'grid' and assign return value in 'isValidPosition'

                if (!isValidPosition) //If not a valid position
                {
                    //Debug.Log("Not a valid build position");
                    canBuild = false; //Set 'canBuild' to false
                    break;
                }
                if (!passedGridXY.GetGridObjectXY(gridPosition.x, gridPosition.y).CanBuild()) //Call 'CanBuild' function and check if it returns false
                {
                    //Debug.Log("Not a valid build position");
                    canBuild = false; //Set 'canBuild' to false
                    break;
                }
                if (!isCallFromLoad)
                {
                    if (buildableGridObjectTypeSO.enableBuildCondition)
                    {
                        if (!GetBuildConditionBuildableGridObject(buildableGridObjectTypeSO))
                        {
                            //Debug.Log("Conditions not met");
                            canBuild = false; //Set 'canBuild' to false
                            break;
                        }
                    }

                    // Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    // if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f))
                    // {
                    //     if (raycastHit.collider.GetComponent<BuildableObjectAreaBlocker>())
                    //     {
                    //         //Debug.Log("Conditions not met");
                    //         canBuild = false; //Set 'canBuild' to false
                    //         break;
                    //     }
                    // }

                    if (buildableAreaBlockerHit)
                    {
                        //Debug.Log("Conditions not met");
                        canBuild = false; //Set 'canBuild' to false
                        break;
                    }
                }
            }

            if (canBuild) //If 'canBuild' is true
            {
                Vector2Int rotationOffset = buildableEdgeObjectTypeSO.GetRotationOffset(dir, passedGridXY.GetCellSize()); //Call 'GetRotationOffset' function in 'buildableGridObjectTypeSO' and assign value to 'rotationOffset'
                Vector3 placedObjectWorldPositionOld = passedGridXY.GetWorldPosition(placedObjectOrigin.x, placedObjectOrigin.y) + new Vector3(rotationOffset.x, rotationOffset.y, transform.localPosition.z) * passedGridXY.GetCellSize(); //Call 'GetWorldPosition' in 'grid' and assign value to 'placedObjectWorldPosition'
                Vector3 placedObjectWorldPosition = new Vector3(placedObjectWorldPositionOld.x, placedObjectWorldPositionOld.y, gridOriginXY.z);

                buildableEdgeObject = BuildableEdgeObject.Create(placedObjectWorldPosition, placedObjectOrigin, dir, mousePosition, buildableEdgeObjectTypeSO, this, edgeRotation); //Call function 'Create' in 'PlacedObject' that will instantiate an object and cache it in 'placedObject'
                buildableEdgeObject.transform.rotation = Quaternion.Euler(0, 0, -buildableEdgeObjectTypeSO.GetRotationAngle(dir));
                                
                if (!isCallFromLoad)
                {
                    if (buildableEdgeObjectTypeSO.enableBuildCondition)
                    {
                        CompleteBuildConditionBuildableEdgeObject(buildableEdgeObjectTypeSO);
                    }
                }

                foreach (Vector2Int gridPosition in gridPositionList) //Cycle througth 'gridPosition' in 'gridPositionList'
                {
                    passedGridXY.GetGridObjectXY(gridPosition.x, gridPosition.y).SetPlacedDownEdgeObject(buildableEdgeObject); //Call function 'GetGridObject' in 'grid' and call function 'SetPlacedObject' 
                    if (enableUnityEvents) OnGridCellChangedUnityEvent?.Invoke(); //Invoke the Unity event 'OnGridCellChangeUnityEvent'
                }

                buildableEdgeObject.GridSetupDone(this, true, currentActiveGridIndex, dir); //Call function 'GridSetupDone'
                OnObjectPlaced?.Invoke(buildableEdgeObject, EventArgs.Empty); //Invoke the event 'OnObjectPlaced'
                if (enableUnityEvents) OnObjectPlacedUnityEvent?.Invoke(); //Invoke the Unity event 'OnObjectPlacedUnityEvent'

                return true;
            }
            else //If 'canBuild' is false
            {
                buildableEdgeObject = null; //Set 'placedObject' to empty
                return false;
            }
        }

        public bool TryPlaceFreeObjectXZ(BuildableFreeObjectTypeSO buildableFreeObjectTypeSO, Vector3 worldPosition, float rotation, bool isCallFromLoad,  out BuildableFreeObject buildableFreeObject) //This function handles object placement and return a bool
        {
            bool canBuild = true;
            if (!isCallFromLoad)
            {
                if (buildableFreeObjectTypeSO.enableBuildCondition)
                {
                    if (!GetBuildConditionBuildableFreeObject(buildableFreeObjectTypeSO))
                    {
                        //Debug.Log("Conditions not met");
                        canBuild = false;
                    }
                }
                
                // Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                // if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f))
                // {
                //     if (raycastHit.collider.GetComponent<BuildableObjectAreaBlocker>())
                //     {
                //         //Debug.Log("Conditions not met");
                //         canBuild = false; //Set 'canBuild' to false
                //     }
                // }

                if (buildableAreaBlockerHit)
                {
                    //Debug.Log("Conditions not met");
                    canBuild = false; //Set 'canBuild' to false
                }
            }

            if (canBuild)
            {
                Vector3 placedObjectWorldPosition = BuildableFreeObjectCollidingMousePosition();
                buildableFreeObject = BuildableFreeObject.Create(worldPosition, rotation, buildableFreeObjectTypeSO, this);
                builtBuildableFreeObjectList.Add(buildableFreeObject.transform);

                if (!isCallFromLoad)
                {
                    if (buildableFreeObjectTypeSO.enableBuildCondition)
                    {
                        CompleteBuildConditionBuildableFreeObject(buildableFreeObjectTypeSO);
                    }
                }

                buildableFreeObject.GridSetupDone(this, true, rotation);
                OnObjectPlaced?.Invoke(buildableFreeObject, EventArgs.Empty);
                if (enableUnityEvents) OnObjectPlacedUnityEvent?.Invoke();

                return true;
            }
            else
            {
                buildableFreeObject = null; 
                return false;
            }
        }

        public bool TryPlaceFreeObjectXY(BuildableFreeObjectTypeSO buildableFreeObjectTypeSO, Vector3 worldPosition, float rotation, bool isCallFromLoad, out BuildableFreeObject buildableFreeObject)
        {
            bool canBuild = true; 
            if (!isCallFromLoad)
            {
                if (buildableFreeObjectTypeSO.enableBuildCondition)
                {
                    if (!GetBuildConditionBuildableFreeObject(buildableFreeObjectTypeSO))
                    {
                        //Debug.Log("Conditions not met");
                        canBuild = false;
                    }
                }

                // Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                // if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f))
                // {
                //     if (raycastHit.collider.GetComponent<BuildableObjectAreaBlocker>())
                //     {
                //         //Debug.Log("Conditions not met");
                //         canBuild = false; //Set 'canBuild' to false
                //     }
                // }

                if (buildableAreaBlockerHit)
                {
                    //Debug.Log("Conditions not met");
                    canBuild = false; //Set 'canBuild' to false
                }
            }

            if (canBuild)
            {
                buildableFreeObject = BuildableFreeObject.Create(worldPosition, rotation, buildableFreeObjectTypeSO, this);
                builtBuildableFreeObjectList.Add(buildableFreeObject.transform);

                if (!isCallFromLoad)
                {
                    if (buildableFreeObjectTypeSO.enableBuildCondition)
                    {
                        CompleteBuildConditionBuildableFreeObject(buildableFreeObjectTypeSO);
                    }
                }

                buildableFreeObject.GridSetupDone(this, true, rotation);
                OnObjectPlaced?.Invoke(buildableFreeObject, EventArgs.Empty);
                if (enableUnityEvents) OnObjectPlacedUnityEvent?.Invoke();

                return true;
            }
            else
            {
                buildableFreeObject = null;
                return false;
            }
        }

        public BuildableEdgeObjectTypeSO.Dir CalcualteEdgeObjectDir(Vector3 mousePosition)
        {
            if (gridAxis == GridAxis.XZ)
            {
                gridXZ.GetXZ(mousePosition, out int x, out int z); //Call function 'GetXZ' in 'grid' and pass 'mousePosition' to get gridPosition 'x' and 'z'
                
                Vector3 worldPos = gridXZ.GetWorldPosition(x, z);
                Vector3 cellPosition = new Vector3 (worldPos.x + (cellSize/2), worldPos.y, worldPos.z + (cellSize/2));
                Vector3 cellPositionDown = new Vector3 (worldPos.x + (cellSize/2), worldPos.y, worldPos.z + cellSize);
                Vector3 cellPositionLeft = new Vector3 (worldPos.x, worldPos.y, worldPos.z + (cellSize/2));
                Vector3 cellPositionUp = new Vector3 (worldPos.x + (cellSize/2), worldPos.y, worldPos.z);
                Vector3 cellPositionRight = new Vector3 (worldPos.x + cellSize, worldPos.y, worldPos.z + (cellSize/2));

                BuildableEdgeObjectTypeSO.Dir returnCellDir = BuildableEdgeObjectTypeSO.Dir.Down;

                float cellDownDistance = Vector3.Distance(mousePosition, cellPositionDown);
                float cellLeftDistance = Vector3.Distance(mousePosition, cellPositionLeft);
                float cellUpDistance = Vector3.Distance(mousePosition, cellPositionUp);
                float cellRightDistance = Vector3.Distance(mousePosition, cellPositionRight);

                float closestDistance = Mathf.Min(cellDownDistance, Mathf.Min(cellLeftDistance, Mathf.Min(cellUpDistance, cellRightDistance)));
                
                if (cellDownDistance == closestDistance)
                {
                    //Debug.DrawLine(cellPositionDown, new Vector3(cellPositionDown.x, cellPositionDown.y + 30, cellPositionDown.z), Color.blue);
                    returnCellDir = BuildableEdgeObjectTypeSO.Dir.Down;;
                }
                else if (cellLeftDistance == closestDistance)
                {
                    //Debug.DrawLine(cellPositionLeft, new Vector3(cellPositionLeft.x, cellPositionLeft.y + 30, cellPositionLeft.z), Color.yellow);
                    returnCellDir = BuildableEdgeObjectTypeSO.Dir.Left;;
                }
                else if (cellUpDistance == closestDistance)
                {
                    //Debug.DrawLine(cellPositionUp, new Vector3(cellPositionUp.x, cellPositionUp.y + 30, cellPositionUp.z), Color.black);
                    returnCellDir = BuildableEdgeObjectTypeSO.Dir.Up;;
                }
                else if (cellRightDistance == closestDistance)
                {
                    //Debug.DrawLine(cellPositionRight, new Vector3(cellPositionRight.x, cellPositionRight.y + 30, cellPositionRight.z), Color.green);
                    returnCellDir = BuildableEdgeObjectTypeSO.Dir.Right;;
                }

                edgeDir = returnCellDir;
                return returnCellDir;
            }
            else
            {
                gridXY.GetXY(mousePosition, out int x, out int y); //Call function 'GetXZ' in 'grid' and pass 'mousePosition' to get gridPosition 'x' and 'z'
                if (buildableGridObjectTypeSO != null) //If 'buildableGridObjectTypeSO' is not empty
                {
                    Vector2Int rotationOffset = buildableGridObjectTypeSO.GetRotationOffset(dir, cellSize); //Call function 'GetRotationOffset' in 'buildableGridObjectTypeSO' and cache value in 'rotationOffset'
                    Vector3 placedObjectWorldPosition = gridXY.GetWorldPosition(x, y) + new Vector3(rotationOffset.x, rotationOffset.y, mousePosition.z / gridXY.GetCellSize()) * gridXY.GetCellSize(); //Call function 'GetWorldPosition' in 'grid' and cache value in 'placedObjectWorldPosition'
                    return BuildableEdgeObjectTypeSO.Dir.Down; //Return calcualted 'placedObjectWorldPosition'
                }
                else //If 'buildableGridObjectTypeSO' is empty
                {
                    return BuildableEdgeObjectTypeSO.Dir.Down; //Return back 'mousePosition'
                }
            }
        }

        private void DeselectObjectType() //This function deselect 'selectedIndex'
        {
            selectedIndex = 1; //Set 'selectedIndex' back to 0
            buildableGridObjectTypeSO = null; //Set 'placedObjectTypeSO' back to null
            buildableEdgeObjectTypeSO = null;
            buildableFreeObjectTypeSO = null;
            isBuildableDestroyActive = false; //Set 'isBuildableDestroyActive' back to false
            isBuildableBuildActive = false;
            if (GetGridMode() != GridMode.None) SetGridMode(GridMode.None);
            RefreshselectedIndexType(); //Call function 'RefreshselectedIndexType'
        }

        private void RefreshselectedIndexType()
        {
            OnSelectedBuildableChanged?.Invoke(this, EventArgs.Empty); //Invoke the event 'OnSelectedBuildableChanged'
        }

        public Vector2Int GetGridPositionXZ(Vector3 worldPosition) //This function will get a 'worldPosition' and return a 'Vector2Int gridPosition'
        {
            gridXZ.GetXZ(worldPosition, out int x, out int z); //Call function 'GetXZ' in 'grid' that will return a 'Vector2Int gridPosition'
            return new Vector2Int(x, z);
        }
        public Vector2Int GetGridPositionXY(Vector3 worldPosition) //This function will get a 'worldPosition' and return a 'Vector2Int gridPosition'
        {
            gridXY.GetXY(worldPosition, out int x, out int z); //Call function 'GetXZ' in 'grid' that will return a 'Vector2Int gridPosition'
            return new Vector2Int(x, z);
        }

        public Vector3 GetWorldPositionXZ(Vector2Int gridPosition) //This function will get a 'gridPosition' and return a 'worldPosition'
        {
            return gridXZ.GetWorldPosition(gridPosition.x, gridPosition.y);
        }
        public Vector3 GetWorldPositionXY(Vector2Int gridPosition) //This function will get a 'gridPosition' and return a 'worldPosition'
        {
            return gridXY.GetWorldPosition(gridPosition.x, gridPosition.y);
        }

        public GridObjectXZ GetGridObjectXZ(Vector2Int gridPosition) //This function will get a 'gridPosition' and return its 'GridObject'
        {
            return gridXZ.GetGridObjectXZ(gridPosition.x, gridPosition.y);
        }
        public GridObjectXY GetGridObjectXY(Vector2Int gridPosition) //This function will get a 'gridPosition' and return its 'GridObject'
        {
            return gridXY.GetGridObjectXY(gridPosition.x, gridPosition.y);
        }

        public GridObjectXZ GetGridObjectXZ(Vector3 worldPosition) //This function will get a 'worldPosition' and return its 'GridObject'
        {
            return gridXZ.GetGridObjectXZ(worldPosition);
        }
        public GridObjectXY GetGridObjectXY(Vector3 worldPosition) //This function will get a 'worldPosition' and return its 'GridObject'
        {
            return gridXY.GetGridObjectXY(worldPosition);
        }

        public bool IsValidGridPositionXZ(Vector2Int gridPosition) //This function take 'gridPosition' and check wheather it's inside the fixed grid and return a bool
        {
            return gridXZ.IsValidGridPosition(gridPosition); //Call function 'IsValidGridPosition' in 'grid'
        }
        public bool IsValidGridPositionXY(Vector2Int gridPosition) //This function take 'gridPosition' and check wheather it's inside the fixed grid and return a bool
        {
            return gridXY.IsValidGridPosition(gridPosition); //Call function 'IsValidGridPosition' in 'grid'
        }

        public Vector3 GetMouseWorldSnappedPosition() //This function will get the positions to snap the mouse on the grid
        {
            if (gridAxis == GridAxis.XZ)
            {
                Vector3 mousePosition = GetMouseWorldPosition(); //Call function 'GetMouseWorldPosition' and cache it in 'mousePosition'
                gridXZ.GetXZ(mousePosition, out int x, out int z); //Call function 'GetXZ' in 'grid' and pass 'mousePosition' to get gridPosition 'x' and 'z'
                if (buildableGridObjectTypeSO != null) //If 'buildableGridObjectTypeSO' is not empty
                {
                    Vector2Int rotationOffset = buildableGridObjectTypeSO.GetRotationOffset(dir, cellSize); //Call function 'GetRotationOffset' in 'buildableGridObjectTypeSO' and cache value in 'rotationOffset'
                    Vector3 placedObjectWorldPosition = gridXZ.GetWorldPosition(x, z) + new Vector3(rotationOffset.x, mousePosition.y / gridXZ.GetCellSize(), rotationOffset.y) * gridXZ.GetCellSize(); //Call function 'GetWorldPosition' in 'grid' and cache value in 'placedObjectWorldPosition'
                    return placedObjectWorldPosition; //Return calcualted 'placedObjectWorldPosition'
                }
                else //If 'buildableGridObjectTypeSO' is empty
                {
                    return mousePosition; //Return back 'mousePosition'
                }
            }
            else
            {
                Vector3 mousePosition = GetMouseWorldPosition(); //Call function 'GetMouseWorldPosition' and cache it in 'mousePosition'
                gridXY.GetXY(mousePosition, out int x, out int y); //Call function 'GetXZ' in 'grid' and pass 'mousePosition' to get gridPosition 'x' and 'z'
                if (buildableGridObjectTypeSO != null) //If 'buildableGridObjectTypeSO' is not empty
                {
                    Vector2Int rotationOffset = buildableGridObjectTypeSO.GetRotationOffset(dir, cellSize); //Call function 'GetRotationOffset' in 'buildableGridObjectTypeSO' and cache value in 'rotationOffset'
                    Vector3 placedObjectWorldPosition = gridXY.GetWorldPosition(x, y) + new Vector3(rotationOffset.x, rotationOffset.y, mousePosition.z / gridXY.GetCellSize()) * gridXY.GetCellSize(); //Call function 'GetWorldPosition' in 'grid' and cache value in 'placedObjectWorldPosition'
                    return placedObjectWorldPosition; //Return calcualted 'placedObjectWorldPosition'
                }
                else //If 'buildableGridObjectTypeSO' is empty
                {
                    return mousePosition; //Return back 'mousePosition'
                }
            }
        }

        public Vector3 GetMouseWorldSnappedPositionForEdgeObjects() //This function will get the positions to snap the mouse on the grid
        {
            if (gridAxis == GridAxis.XZ)
            {
                Vector3 mousePosition = GetMouseWorldPosition(); //Call function 'GetMouseWorldPosition' and cache it in 'mousePosition'
                gridXZ.GetXZ(mousePosition, out int x, out int z); //Call function 'GetXZ' in 'grid' and pass 'mousePosition' to get gridPosition 'x' and 'z'
                
                Vector3 worldPos = gridXZ.GetWorldPosition(x, z);
                Vector3 cellPosition = new Vector3 (worldPos.x + (cellSize/2), worldPos.y, worldPos.z + (cellSize/2));
                Vector3 cellPositionDown = new Vector3 (worldPos.x + (cellSize/2), worldPos.y, worldPos.z + cellSize);
                Vector3 cellPositionLeft = new Vector3 (worldPos.x, worldPos.y, worldPos.z + (cellSize/2));
                Vector3 cellPositionUp = new Vector3 (worldPos.x + (cellSize/2), worldPos.y, worldPos.z);
                Vector3 cellPositionRight = new Vector3 (worldPos.x + cellSize, worldPos.y, worldPos.z + (cellSize/2));

                Vector3 returnCellPosition = mousePosition;
                
                //Debug.DrawLine(cellPosition, new Vector3(cellPosition.x, cellPosition.y + 30, cellPosition.z), Color.red);

                float cellDownDistance = Vector3.Distance(mousePosition, cellPositionDown);
                float cellLeftDistance = Vector3.Distance(mousePosition, cellPositionLeft);
                float cellUpDistance = Vector3.Distance(mousePosition, cellPositionUp);
                float cellRightDistance = Vector3.Distance(mousePosition, cellPositionRight);

                float closestDistance = Mathf.Min(cellDownDistance, Mathf.Min(cellLeftDistance, Mathf.Min(cellUpDistance, cellRightDistance)));
                
                if (cellDownDistance == closestDistance)
                {
                    //Debug.DrawLine(cellPositionDown, new Vector3(cellPositionDown.x, cellPositionDown.y + 30, cellPositionDown.z), Color.blue);
                    returnCellPosition = cellPositionDown;
                }
                else if (cellLeftDistance == closestDistance)
                {
                    //Debug.DrawLine(cellPositionLeft, new Vector3(cellPositionLeft.x, cellPositionLeft.y + 30, cellPositionLeft.z), Color.yellow);
                    returnCellPosition = cellPositionLeft;
                }
                else if (cellUpDistance == closestDistance)
                {
                    //Debug.DrawLine(cellPositionUp, new Vector3(cellPositionUp.x, cellPositionUp.y + 30, cellPositionUp.z), Color.black);
                    returnCellPosition = cellPositionUp;
                }
                else if (cellRightDistance == closestDistance)
                {
                    //Debug.DrawLine(cellPositionRight, new Vector3(cellPositionRight.x, cellPositionRight.y + 30, cellPositionRight.z), Color.green);
                    returnCellPosition = cellPositionRight;
                }

                if (buildableEdgeObjectTypeSO != null) //If 'buildableGridObjectTypeSO' is not empty
                {
                    return returnCellPosition;
                }
                else //If 'buildableGridObjectTypeSO' is empty
                {
                    return mousePosition; //Return back 'mousePosition'
                }
            }
            else
            {
                Vector3 mousePosition = GetMouseWorldPosition(); //Call function 'GetMouseWorldPosition' and cache it in 'mousePosition'
                gridXY.GetXY(mousePosition, out int x, out int y); //Call function 'GetXZ' in 'grid' and pass 'mousePosition' to get gridPosition 'x' and 'z'
                if (buildableGridObjectTypeSO != null) //If 'buildableGridObjectTypeSO' is not empty
                {
                    Vector2Int rotationOffset = buildableGridObjectTypeSO.GetRotationOffset(dir, cellSize); //Call function 'GetRotationOffset' in 'buildableGridObjectTypeSO' and cache value in 'rotationOffset'
                    Vector3 placedObjectWorldPosition = gridXY.GetWorldPosition(x, y) + new Vector3(rotationOffset.x, rotationOffset.y, mousePosition.z / gridXY.GetCellSize()) * gridXY.GetCellSize(); //Call function 'GetWorldPosition' in 'grid' and cache value in 'placedObjectWorldPosition'
                    return placedObjectWorldPosition; //Return calcualted 'placedObjectWorldPosition'
                }
                else //If 'buildableGridObjectTypeSO' is empty
                {
                    return mousePosition; //Return back 'mousePosition'
                }
            }
        }

        public Vector3 GetMouseWorldSnappedPositionForMoving(BuildableGridObjectTypeSO buildableGridObjectTypeSO) //This function will get the positions to snap the mouse on the grid
        {
            if (gridAxis == GridAxis.XZ)
            {
                Vector3 mousePosition = GetMouseWorldPosition(); //Call function 'GetMouseWorldPosition' and cache it in 'mousePosition'
                gridXZ.GetXZ(mousePosition, out int x, out int z); //Call function 'GetXZ' in 'grid' and pass 'mousePosition' to get gridPosition 'x' and 'z'
                if (buildableGridObjectTypeSO != null) //If 'buildableGridObjectTypeSO' is not empty
                {
                    Vector2Int rotationOffset = buildableGridObjectTypeSO.GetRotationOffset(dir, cellSize); //Call function 'GetRotationOffset' in 'buildableGridObjectTypeSO' and cache value in 'rotationOffset'
                    Vector3 placedObjectWorldPosition = gridXZ.GetWorldPosition(x, z) + new Vector3(rotationOffset.x, mousePosition.y / gridXZ.GetCellSize(), rotationOffset.y) * gridXZ.GetCellSize(); //Call function 'GetWorldPosition' in 'grid' and cache value in 'placedObjectWorldPosition'
                    return placedObjectWorldPosition; //Return calcualted 'placedObjectWorldPosition'
                }
                else //If 'buildableGridObjectTypeSO' is empty
                {
                    return mousePosition; //Return back 'mousePosition'
                }
            }
            else
            {
                Vector3 mousePosition = GetMouseWorldPosition(); //Call function 'GetMouseWorldPosition' and cache it in 'mousePosition'
                gridXY.GetXY(mousePosition, out int x, out int y); //Call function 'GetXZ' in 'grid' and pass 'mousePosition' to get gridPosition 'x' and 'z'
                if (buildableGridObjectTypeSO != null) //If 'buildableGridObjectTypeSO' is not empty
                {
                    Vector2Int rotationOffset = buildableGridObjectTypeSO.GetRotationOffset(dir, cellSize); //Call function 'GetRotationOffset' in 'buildableGridObjectTypeSO' and cache value in 'rotationOffset'
                    Vector3 placedObjectWorldPosition = gridXY.GetWorldPosition(x, y) + new Vector3(rotationOffset.x, rotationOffset.y, mousePosition.z / gridXY.GetCellSize()) * gridXY.GetCellSize(); //Call function 'GetWorldPosition' in 'grid' and cache value in 'placedObjectWorldPosition'
                    return placedObjectWorldPosition; //Return calcualted 'placedObjectWorldPosition'
                }
                else //If 'buildableGridObjectTypeSO' is empty
                {
                    return mousePosition; //Return back 'mousePosition'
                }
            }
        }

        public Quaternion GetPlacedObjectRotation() //This function will return placed object's rotation
        {
            if (gridAxis == GridAxis.XZ)
            {
                if (buildableGridObjectTypeSO != null) //If 'buildableGridObjectTypeSO' is not empty
                {
                    return Quaternion.Euler(0, buildableGridObjectTypeSO.GetRotationAngle(dir), 0); //Call function 'GetRotationAngle' in 'buildableGridObjectTypeSO' and grab the euler angles and return
                }
                else //If 'buildableGridObjectTypeSO' is empty
                {
                    return Quaternion.identity; //Return quaternion.identity
                }
            }
            else
            {
                if (buildableGridObjectTypeSO != null) //If 'buildableGridObjectTypeSO' is not empty
                {
                    return Quaternion.Euler(0, 0, -buildableGridObjectTypeSO.GetRotationAngle(dir)); //Call function 'GetRotationAngle' in 'buildableGridObjectTypeSO' and grab the euler angles and return
                }
                else //If 'buildableGridObjectTypeSO' is empty
                {
                    return Quaternion.identity; //Return quaternion.identity
                }
            }
        }

        public Quaternion GetPlacedEdgeObjectRotation() //This function will return placed object's rotation
        {
            if (gridAxis == GridAxis.XZ)
            {
                if (buildableEdgeObjectTypeSO != null) //If 'buildableGridObjectTypeSO' is not empty
                {
                    return Quaternion.Euler(0, buildableEdgeObjectTypeSO.GetRotationAngle(CalcualteEdgeObjectDir(localMousePosition)), 0); //Call function 'GetRotationAngle' in 'buildableGridObjectTypeSO' and grab the euler angles and return
                }
                else //If 'buildableGridObjectTypeSO' is empty
                {
                    return Quaternion.identity; //Return quaternion.identity
                }
            }
            else
            {
                if (buildableEdgeObjectTypeSO != null) //If 'buildableGridObjectTypeSO' is not empty
                {
                    return Quaternion.Euler(0, 0, -buildableEdgeObjectTypeSO.GetRotationAngle(CalcualteEdgeObjectDir(localMousePosition))); //Call function 'GetRotationAngle' in 'buildableGridObjectTypeSO' and grab the euler angles and return
                }
                else //If 'buildableGridObjectTypeSO' is empty
                {
                    return Quaternion.identity; //Return quaternion.identity
                }
            }
        }

        public Quaternion GetPlacedObjectRotationForMoving(BuildableGridObjectTypeSO buildableGridObjectTypeSO) //This function will return placed object's rotation
        {
            if (gridAxis == GridAxis.XZ)
            {
                if (buildableGridObjectTypeSO != null) //If 'buildableGridObjectTypeSO' is not empty
                {
                    return Quaternion.Euler(0, buildableGridObjectTypeSO.GetRotationAngle(dir), 0); //Call function 'GetRotationAngle' in 'buildableGridObjectTypeSO' and grab the euler angles and return
                }
                else //If 'buildableGridObjectTypeSO' is empty
                {
                    return Quaternion.identity; //Return quaternion.identity
                }
            }
            else
            {
                if (buildableGridObjectTypeSO != null) //If 'buildableGridObjectTypeSO' is not empty
                {
                    return Quaternion.Euler(0, 0, -buildableGridObjectTypeSO.GetRotationAngle(dir)); //Call function 'GetRotationAngle' in 'buildableGridObjectTypeSO' and grab the euler angles and return
                }
                else //If 'buildableGridObjectTypeSO' is empty
                {
                    return Quaternion.identity; //Return quaternion.identity
                }
            }
        }

        public BuildableEdgeObjectTypeSO.Dir GetEdgeObjectDir()
        {
            return edgeDir;
        }

        public float GetEdgeObjectRotation()
        {
            return edgeRotation;
        }

        public BuildableGridObjectTypeSO GetBuildableGridObjectTypeSO() //This function will return 'buildableGridObjectTypeSO'
        {
            return buildableGridObjectTypeSO;
        }

        public BuildableEdgeObjectTypeSO GetBuildableEdgeObjectTypeSO() //This function will return 'buildableEdgeObjectTypeSO'
        {
            return buildableEdgeObjectTypeSO;
        }
        
        public BuildableFreeObjectTypeSO GetBuildableFreeObjectTypeSO() //This function will 'buildableGridObjectTypeSO'
        {
            return buildableFreeObjectTypeSO;
        }

        public Vector3 GetBuildableFreeObjectMouseWorldPosition()
        {
            return BuildableFreeObjectCollidingMousePosition();
        }

        public float GetBuildableFreeObjectRotation()
        {
            return buildableFreeObjectRotation;
        }

        public Vector3 GetLocalMousePosition()
        {
            return localMousePosition;
        }

        public void SetSelectedBuildableGridObjectType(BuildableGridObjectTypeSO buildableGridObjectTypeSO)
        {
            this.buildableGridObjectTypeSO = buildableGridObjectTypeSO;
            isBuildableDestroyActive = false;
            RefreshselectedIndexType();
        }

        public Vector3 GetWorldPositionForDebugXZ(int x, int z) //This function calculate and return cell world position (Only for debugging purposses)
        {
            Vector3 tempOriginXZ;
            if (lockDebugGridOnHeightChange)
            {
                tempOriginXZ = gridOriginXZList[0];
            }
            else
            {
                tempOriginXZ = gridOriginXZ;
            }
            return new Vector3(x, 0, z) * cellSize + tempOriginXZ;
        }
        public Vector3 GetWorldPositionForDebugXY(int x, int y) //This function calculate and return cell world position (Only for debugging purposses)
        {
            Vector3 tempOriginXY;
            if (lockDebugGridOnHeightChange)
            {
                tempOriginXY = gridOriginXYList[0];
            }
            else
            {
                tempOriginXY = gridOriginXY;
            }
            return new Vector3(x, y, 0) * cellSize + tempOriginXY;
        }

        public bool NotPlaceableVisualCallerBuildableGridObject()
        {
            Vector3 mousePosition = GetMouseWorldPosition();
            bool valid = false;
            if (gridAxis == GridAxis.XZ)
            {
                gridXZ.GetXZ(mousePosition, out int x, out int z);
                Vector2Int placedObjectOrigin = new Vector2Int(x, z); //create a new variable 'placedObjectOrigin' and pass vector2 int 'x' and 'z'
                List<Vector2Int> gridPositionList = buildableGridObjectTypeSO.GetGridPositionList(placedObjectOrigin, dir, cellSize); //Grab 'GetGridPositionList' from 'buildableGridObjectTypeSO' and cache it to a new list 'gridPositionList'
                valid = true;

                foreach (Vector2Int gridPosition in gridPositionList) //Loop through created 'gridPositionList'
                {
                    bool isValidPosition = gridXZ.IsValidGridPosition(gridPosition); //Call 'IsValidGridPosition' function in 'grid' and assign return value in 'isValidPosition'

                    if (!isValidPosition) //If not a valid position
                    {
                        valid = false;
                        break;
                    }
                    if (!gridXZ.GetGridObjectXZ(gridPosition.x, gridPosition.y).CanBuild()) //Call 'CanBuild' function and check if it returns false
                    {
                        valid = false;
                        break;
                    }
                    if (buildableGridObjectTypeSO.enableBuildCondition)
                    {
                        if (!GetBuildConditionBuildableGridObject(buildableGridObjectTypeSO))
                        {
                            valid = false;
                            break;
                        }
                    }
                }

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (useBuildableDistance)
                {
                    if (!distanceCheckObject) distanceCheckObject = Camera.main.transform;
                    if (Vector3.Distance(distanceCheckObject.position, mousePosition) > distanceMin && Vector3.Distance(distanceCheckObject.position, mousePosition) < distanceMax)
                    {
                    }
                    else
                    {
                        valid = false;
                    }
                }

                // if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f))
                // {
                //     if (raycastHit.collider.GetComponent<BuildableObjectAreaBlocker>())
                //     {
                //         valid = false;
                //     }
                // }

                if (buildableAreaBlockerHit)
                {
                    valid = false;
                }

                return valid;
            }
            else
            {
                gridXY.GetXY(mousePosition, out int x, out int y);
                Vector2Int placedObjectOrigin = new Vector2Int(x, y); //create a new variable 'placedObjectOrigin' and pass vector2 int 'x' and 'z'
                List<Vector2Int> gridPositionList = buildableGridObjectTypeSO.GetGridPositionList(placedObjectOrigin, dir, cellSize); //Grab 'GetGridPositionList' from 'buildableGridObjectTypeSO' and cache it to a new list 'gridPositionList'
                valid = true;
                    
                foreach (Vector2Int gridPosition in gridPositionList) //Loop through created 'gridPositionList'
                {
                    bool isValidPosition = gridXY.IsValidGridPosition(gridPosition); //Call 'IsValidGridPosition' function in 'grid' and assign return value in 'isValidPosition'

                    if (!isValidPosition) //If not a valid position
                    {
                        valid = false;
                        break;
                    }
                    if (!gridXY.GetGridObjectXY(gridPosition.x, gridPosition.y).CanBuild()) //Call 'CanBuild' function and check if it returns false
                    {
                        valid = false;
                        break;
                    }
                    if (buildableGridObjectTypeSO.enableBuildCondition)
                    {
                        if (!GetBuildConditionBuildableGridObject(buildableGridObjectTypeSO))
                        {
                            valid = false;
                            break;
                        }
                    }
                }

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (useBuildableDistance)
                {
                    if (!distanceCheckObject) distanceCheckObject = Camera.main.transform;
                    if (Vector3.Distance(distanceCheckObject.position, mousePosition) > distanceMin && Vector3.Distance(distanceCheckObject.position, mousePosition) < distanceMax)
                    {
                    }
                    else
                    {
                        valid = false;
                    }
                }

                // if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f))
                // {
                //     if (raycastHit.collider.GetComponent<BuildableObjectAreaBlocker>())
                //     {
                //         valid = false;
                //     }
                // }

                if (buildableAreaBlockerHit)
                {
                    valid = false;
                }
                
                return valid;
            }
        }

        public bool NotPlaceableVisualCallerBuildableEdgeObject()
        {
            Vector3 mousePosition = GetMouseWorldPosition();
            bool valid = false;
            if (gridAxis == GridAxis.XZ)
            {
                gridXZ.GetXZ(mousePosition, out int x, out int z);
                Vector2Int placedObjectOrigin = new Vector2Int(x, z); //create a new variable 'placedObjectOrigin' and pass vector2 int 'x' and 'z'
                List<Vector2Int> gridPositionList = buildableEdgeObjectTypeSO.GetGridPositionList(placedObjectOrigin, edgeDir, cellSize); //Grab 'GetGridPositionList' from 'buildableGridObjectTypeSO' and cache it to a new list 'gridPositionList'
                valid = true;

                foreach (Vector2Int gridPosition in gridPositionList) //Cycle througth 'gridPosition' in 'gridPositionList'
                {
                    bool isValidPosition = gridXZ.IsValidGridPosition(gridPosition); //Call 'IsValidGridPosition' function in 'grid' and assign return value in 'isValidPosition'

                    if (!isValidPosition) //If not a valid position
                    {
                        valid = false;
                        break;
                    }
                    switch (edgeDir)
                    {
                        case BuildableEdgeObjectTypeSO.Dir.Down:
                            if (!gridXZ.GetGridObjectXZ(gridPosition.x, gridPosition.y).CanBuildEdgeObjectDown()) //Call 'CanBuild' function and check if it returns false
                            {
                                valid = false;
                                break;
                            }
                        break;
                        case BuildableEdgeObjectTypeSO.Dir.Left:
                            if (!gridXZ.GetGridObjectXZ(gridPosition.x, gridPosition.y).CanBuildEdgeObjectLeft()) //Call 'CanBuild' function and check if it returns false
                            {
                                valid = false;
                                break;
                            }
                        break;
                        case BuildableEdgeObjectTypeSO.Dir.Up:
                            if (!gridXZ.GetGridObjectXZ(gridPosition.x, gridPosition.y).CanBuildEdgeObjectUp()) //Call 'CanBuild' function and check if it returns false
                            {
                                valid = false;
                                break;
                            }
                        break;
                        case BuildableEdgeObjectTypeSO.Dir.Right:
                            if (!gridXZ.GetGridObjectXZ(gridPosition.x, gridPosition.y).CanBuildEdgeObjectRight()) //Call 'CanBuild' function and check if it returns false
                            {
                                valid = false;
                                break;
                            }
                        break;
                    }
                    if (buildableEdgeObjectTypeSO.enableBuildCondition)
                    {
                        if (!GetBuildConditionBuildableGridObject(buildableGridObjectTypeSO))
                        {
                            valid = false;
                            break;
                        }
                    }
                }

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (useBuildableDistance)
                {
                    if (!distanceCheckObject) distanceCheckObject = Camera.main.transform;
                    if (Vector3.Distance(distanceCheckObject.position, mousePosition) > distanceMin && Vector3.Distance(distanceCheckObject.position, mousePosition) < distanceMax)
                    {
                    }
                    else
                    {
                        valid = false;
                    }
                }

                // if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f))
                // {
                //     if (raycastHit.collider.GetComponent<BuildableObjectAreaBlocker>())
                //     {
                //         valid = false;
                //     }
                // }

                if (buildableAreaBlockerHit)
                {
                    valid = false;
                }

                return valid;
            }
            else
            {
                gridXY.GetXY(mousePosition, out int x, out int y);
                Vector2Int placedObjectOrigin = new Vector2Int(x, y); //create a new variable 'placedObjectOrigin' and pass vector2 int 'x' and 'z'
                List<Vector2Int> gridPositionList = buildableGridObjectTypeSO.GetGridPositionList(placedObjectOrigin, dir, cellSize); //Grab 'GetGridPositionList' from 'buildableGridObjectTypeSO' and cache it to a new list 'gridPositionList'
                valid = true;
                    
                foreach (Vector2Int gridPosition in gridPositionList) //Loop through created 'gridPositionList'
                {
                    bool isValidPosition = gridXY.IsValidGridPosition(gridPosition); //Call 'IsValidGridPosition' function in 'grid' and assign return value in 'isValidPosition'

                    if (!isValidPosition) //If not a valid position
                    {
                        valid = false;
                        break;
                    }
                    if (!gridXY.GetGridObjectXY(gridPosition.x, gridPosition.y).CanBuild()) //Call 'CanBuild' function and check if it returns false
                    {
                        valid = false;
                        break;
                    }
                    if (buildableGridObjectTypeSO.enableBuildCondition)
                    {
                        if (!GetBuildConditionBuildableGridObject(buildableGridObjectTypeSO))
                        {
                            valid = false;
                            break;
                        }
                    }
                }

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (useBuildableDistance)
                {
                    if (!distanceCheckObject) distanceCheckObject = Camera.main.transform;
                    if (Vector3.Distance(distanceCheckObject.position, mousePosition) > distanceMin && Vector3.Distance(distanceCheckObject.position, mousePosition) < distanceMax)
                    {
                    }
                    else
                    {
                        valid = false;
                    }
                }

                // if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f))
                // {
                //     if (raycastHit.collider.GetComponent<BuildableObjectAreaBlocker>())
                //     {
                //         valid = false;
                //     }
                // }

                if (buildableAreaBlockerHit)
                {
                    valid = false;
                }
                
                return valid;
            }
        }

        public bool NotPlaceableVisualCallerBuildableFreeObject()
        {
            Vector3 mousePosition = BuildableFreeObjectCollidingMousePosition();
            bool valid = false;

            if (currentBuildableObjectType != BuildableObjectType.FreeObject)
            {
                return valid;
            }
            else
            {
                if (gridAxis == GridAxis.XZ)
                {
                    valid = true;
                    if (buildableFreeObjectTypeSO.enableBuildCondition)
                    {
                        if (!GetBuildConditionBuildableFreeObject(buildableFreeObjectTypeSO))
                        {
                            valid = false;
                        }
                    }

                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (useBuildableDistance)
                    {
                        if (!distanceCheckObject) distanceCheckObject = Camera.main.transform;
                        if (Vector3.Distance(distanceCheckObject.position, mousePosition) > distanceMin && Vector3.Distance(distanceCheckObject.position, mousePosition) < distanceMax)
                        {
                        }
                        else
                        {
                            valid = false;
                        }
                    }

                    // if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f))
                    // {
                    //     if (raycastHit.collider.GetComponent<BuildableObjectAreaBlocker>())
                    //     {
                    //         valid = false;
                    //     }
                    // }

                    if (buildableAreaBlockerHit)
                    {
                        valid = false;
                    }

                    return valid;
                }
                else
                {
                    valid = true;
                    if (buildableFreeObjectTypeSO.enableBuildCondition)
                    {
                        if (!GetBuildConditionBuildableFreeObject(buildableFreeObjectTypeSO))
                        {
                            valid = false;
                        }
                    }
                    
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (useBuildableDistance)
                    {
                        if (!distanceCheckObject) distanceCheckObject = Camera.main.transform;
                        if (Vector3.Distance(distanceCheckObject.position, mousePosition) > distanceMin && Vector3.Distance(distanceCheckObject.position, mousePosition) < distanceMax)
                        {
                        }
                        else
                        {
                            valid = false;
                        }
                    }

                    // if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f))
                    // {
                    //     if (raycastHit.collider.GetComponent<BuildableObjectAreaBlocker>())
                    //     {
                    //         valid = false;
                    //     }
                    // }
                    
                    if (buildableAreaBlockerHit)
                    {
                        valid = false;
                    }

                    return valid;
                }
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
        //                                                                                     BUILD CONDITION FUNCTIONS                                                                                            //
        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//

        public bool GetBuildConditionBuildableGridObject(BuildableGridObjectTypeSO buildableGridObjectTypeSO)
        {
            OnBuildConditionCheckCallerBuildableGridObject?.Invoke(buildableGridObjectTypeSO);
            if (MultiGridBuildConditionManager.BuidConditionResponseBuildableGridObject)
            {
                return true;
            }
            else
            {
                if (showConsoleText && objectPlacement) Debug.Log("<color=orange>Cannot Build!</color> Build conditions are not met!");
                return false;
            }
        }

        public void CompleteBuildConditionBuildableGridObject(BuildableGridObjectTypeSO buildableGridObjectTypeSO)
        {
            OnBuildConditionCompleteCallerBuildableGridObject?.Invoke(buildableGridObjectTypeSO);
        }

        public bool GetBuildConditionBuildableEdgeObject(BuildableEdgeObjectTypeSO buildableEdgeObjectTypeSO)
        {
            OnBuildConditionCheckCallerBuildableEdgeObject?.Invoke(buildableEdgeObjectTypeSO);
            if (MultiGridBuildConditionManager.BuidConditionResponseBuildableEdgeObject)
            {
                return true;
            }
            else
            {
                if (showConsoleText && objectPlacement) Debug.Log("<color=orange>Cannot Build!</color> Build conditions are not met!");
                return false;
            }
        }

        public void CompleteBuildConditionBuildableEdgeObject(BuildableEdgeObjectTypeSO buildableEdgeObjectTypeSO)
        {
            OnBuildConditionCompleteCallerBuildableEdgeObject?.Invoke(buildableEdgeObjectTypeSO);
        }

        public bool GetBuildConditionBuildableFreeObject(BuildableFreeObjectTypeSO buildableFreeObjectTypeSO)
        {
            OnBuildConditionCheckCallerBuildableFreeObject?.Invoke(buildableFreeObjectTypeSO);
            if (MultiGridBuildConditionManager.BuidConditionResponseBuildableFreeObject)
            {
                return true;
            }
            else
            {
                if (showConsoleText && objectPlacement) Debug.Log("<color=orange>Cannot Build!</color> Build conditions are not met!");
                return false;
            }
            
        }

        public void CompleteBuildConditionBuildableFreeObject(BuildableFreeObjectTypeSO buildableFreebjectTypeSO)
        {
            OnBuildConditionCompleteCallerBuildableFreeObject?.Invoke(buildableFreebjectTypeSO);
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
        //                                                                                        SAVE & LOAD FUNCTIONS                                                                                             //
        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
        
        private void GridSave()
        {
            if (gridAxis == GridAxis.XZ)
            {
                List<PlacedObjectSaveObjectArray> placedObjectSaveObjectArrayList = new List<PlacedObjectSaveObjectArray>();
                foreach (GridXZ<GridObjectXZ> gridXZ in gridXZList)
                {
                    List<BuildableGridObject.SaveObject> saveObjectList = new List<BuildableGridObject.SaveObject>();
                    List<BuildableGridObject> savedPlacedObjectList = new List<BuildableGridObject>();
                    
                    for (int x = 0; x < gridXZ.GetWidth(); x++)
                    {
                        for (int y = 0; y < gridXZ.GetLength(); y++)
                        {
                            BuildableGridObject buildableGridObject = gridXZ.GetGridObjectXZ(x, y).GetPlacedObject();
                            if (buildableGridObject != null && !savedPlacedObjectList.Contains(buildableGridObject))
                            {
                                savedPlacedObjectList.Add(buildableGridObject);
                                saveObjectList.Add(buildableGridObject.GetSaveObject());
                            }
                        }
                    }
                    
                    PlacedObjectSaveObjectArray placedObjectSaveObjectArray = new PlacedObjectSaveObjectArray { placedObjectSaveObjectArray = saveObjectList.ToArray() };
                    placedObjectSaveObjectArrayList.Add(placedObjectSaveObjectArray);
                }

                List<PlacedEdgeObjectSaveObjectArray> placedEdgeObjectSaveObjectArrayList = new List<PlacedEdgeObjectSaveObjectArray>();
                foreach (GridXZ<GridObjectXZ> gridXZ in gridXZList)
                {
                    List<BuildableEdgeObject.SaveObject> edgeSaveObjectList = new List<BuildableEdgeObject.SaveObject>();
                    List<BuildableEdgeObject> edgeSavedPlacedObjectList = new List<BuildableEdgeObject>();
                    
                    for (int x = 0; x < gridXZ.GetWidth(); x++)
                    {
                        for (int y = 0; y < gridXZ.GetLength(); y++)
                        {
                            if (gridXZ.GetGridObjectXZ(x, y).GetPlacedDownEdgeObject() != null)
                            {
                                BuildableEdgeObject buildableEdgeObject = gridXZ.GetGridObjectXZ(x, y).GetPlacedDownEdgeObject();
                                if (buildableEdgeObject != null && !edgeSavedPlacedObjectList.Contains(buildableEdgeObject))
                                {
                                    edgeSavedPlacedObjectList.Add(buildableEdgeObject);
                                    edgeSaveObjectList.Add(buildableEdgeObject.GetSaveObject());
                                }
                            }
                            if (gridXZ.GetGridObjectXZ(x, y).GetPlacedLeftEdgeObject() != null)
                            {
                                BuildableEdgeObject buildableEdgeObject = gridXZ.GetGridObjectXZ(x, y).GetPlacedLeftEdgeObject();
                                if (buildableEdgeObject != null && !edgeSavedPlacedObjectList.Contains(buildableEdgeObject))
                                {
                                    edgeSavedPlacedObjectList.Add(buildableEdgeObject);
                                    edgeSaveObjectList.Add(buildableEdgeObject.GetSaveObject());
                                }
                            }
                            if (gridXZ.GetGridObjectXZ(x, y).GetPlacedUpEdgeObject() != null)
                            {
                                BuildableEdgeObject buildableEdgeObject = gridXZ.GetGridObjectXZ(x, y).GetPlacedUpEdgeObject();
                                if (buildableEdgeObject != null && !edgeSavedPlacedObjectList.Contains(buildableEdgeObject))
                                {
                                    edgeSavedPlacedObjectList.Add(buildableEdgeObject);
                                    edgeSaveObjectList.Add(buildableEdgeObject.GetSaveObject());
                                }
                            }
                            if (gridXZ.GetGridObjectXZ(x, y).GetPlacedRightEdgeObject() != null)
                            {
                                BuildableEdgeObject buildableEdgeObject = gridXZ.GetGridObjectXZ(x, y).GetPlacedRightEdgeObject();
                                if (buildableEdgeObject != null && !edgeSavedPlacedObjectList.Contains(buildableEdgeObject))
                                {
                                    edgeSavedPlacedObjectList.Add(buildableEdgeObject);
                                    edgeSaveObjectList.Add(buildableEdgeObject.GetSaveObject());
                                }
                            }
                        }
                    }
                    
                    PlacedEdgeObjectSaveObjectArray placedEdgeObjectSaveObjectArray = new PlacedEdgeObjectSaveObjectArray { placedEdgeObjectSaveObjectArray = edgeSaveObjectList.ToArray() };
                    placedEdgeObjectSaveObjectArrayList.Add(placedEdgeObjectSaveObjectArray);
                }

                List<LooseSaveObject> looseSaveObjectList = new List<LooseSaveObject>();
                foreach (Transform looseObjectTransform in builtBuildableFreeObjectList)
                {
                    if (looseObjectTransform == null) continue;
                    looseSaveObjectList.Add(new LooseSaveObject
                    {
                        looseObjectSOName = looseObjectTransform.GetComponent<BuildableFreeObject>().GetBuildableFreeObjectTypeSO().name,
                        position = looseObjectTransform.position,
                        quaternion = looseObjectTransform.rotation.y,
                    });
                }

                SaveObject saveObject = new SaveObject
                {
                    placedObjectSaveObjectArrayArray = placedObjectSaveObjectArrayList.ToArray(),
                    placedEdgeObjectSaveObjectArrayArray = placedEdgeObjectSaveObjectArrayList.ToArray(),
                    looseSaveObjectArray = looseSaveObjectList.ToArray(),
                };

                string json = JsonUtility.ToJson(saveObject);
                PlayerPrefs.SetString(uniqueSaveName+"_XZ", json);
                GridSaveSystem.Save(uniqueSaveName+"_XZ", json, true);

                if (showConsoleText && saveAndLoad) Debug.Log("Grid XZ " + "<color=green>Grid Data Saved!</color>");
            }
            else
            {
                List<PlacedObjectSaveObjectArray> placedObjectSaveObjectArrayList = new List<PlacedObjectSaveObjectArray>();

                foreach (GridXY<GridObjectXY> gridXY in gridXYList)
                {
                    List<BuildableGridObject.SaveObject> saveObjectList = new List<BuildableGridObject.SaveObject>();
                    List<BuildableGridObject> savedPlacedObjectList = new List<BuildableGridObject>();
                    
                    for (int x = 0; x < gridXY.GetWidth(); x++)
                    {
                        for (int y = 0; y < gridXY.GetLength(); y++)
                        {
                            BuildableGridObject buildableGridObject = gridXY.GetGridObjectXY(x, y).GetPlacedObject();
                            if (buildableGridObject != null && !savedPlacedObjectList.Contains(buildableGridObject))
                            {
                                savedPlacedObjectList.Add(buildableGridObject);
                                saveObjectList.Add(buildableGridObject.GetSaveObject());
                            }
                        }
                    }

                    PlacedObjectSaveObjectArray placedObjectSaveObjectArray = new PlacedObjectSaveObjectArray { placedObjectSaveObjectArray = saveObjectList.ToArray() };
                    placedObjectSaveObjectArrayList.Add(placedObjectSaveObjectArray);
                }

                List<LooseSaveObject> looseSaveObjectList = new List<LooseSaveObject>();
                foreach (Transform looseObjectTransform in builtBuildableFreeObjectList)
                {
                    if (looseObjectTransform == null) continue;
                    looseSaveObjectList.Add(new LooseSaveObject
                    {
                        looseObjectSOName = looseObjectTransform.GetComponent<BuildableFreeObject>().GetBuildableFreeObjectTypeSO().name,
                        position = looseObjectTransform.position,
                        quaternion = looseObjectTransform.rotation.z,
                    });
                }

                SaveObject saveObject = new SaveObject
                {
                    placedObjectSaveObjectArrayArray = placedObjectSaveObjectArrayList.ToArray(),
                    looseSaveObjectArray = looseSaveObjectList.ToArray(),
                };

                string json = JsonUtility.ToJson(saveObject);
                PlayerPrefs.SetString(uniqueSaveName+"_XY", json);
                GridSaveSystem.Save(uniqueSaveName+"_XY", json, true);

                if (showConsoleText && saveAndLoad) Debug.Log("Grid XY " + "<color=green>Grid Data Saved!</color>");
            }
        }

        private void GridLoad()
        {
            if (gridAxis == GridAxis.XZ)
            {
                if (PlayerPrefs.HasKey(uniqueSaveName+"_XZ"))
                {
                    string json = PlayerPrefs.GetString(uniqueSaveName+"_XZ");
                    json = GridSaveSystem.Load(uniqueSaveName+"_XZ");
                    SaveObject saveObject = JsonUtility.FromJson<SaveObject>(json);

                    Vector3 tempGridOriginXZ = gridOriginXZ;

                    for (int i = 0; i < gridXZList.Count; i++)
                    {
                        GridXZ<GridObjectXZ> gridXZ = gridXZList[i];
                        gridOriginXZ = gridOriginXZList[i];
                        foreach (BuildableGridObject.SaveObject placedObjectSaveObject in saveObject.placedObjectSaveObjectArrayArray[i].placedObjectSaveObjectArray)
                        {
                            BuildableGridObjectTypeSO buildableGridObjectTypeSO = GetBuildableGridObjectTypeSOFromName(placedObjectSaveObject.buildableGridObjectTypeSOName);
                            TryPlaceGridObjectXZ(gridXZ, placedObjectSaveObject.origin, buildableGridObjectTypeSO, placedObjectSaveObject.dir, true, out BuildableGridObject buildableGridObject);
                        }
                    }

                    for (int i = 0; i < gridXZList.Count; i++)
                    {
                        GridXZ<GridObjectXZ> gridXZ = gridXZList[i];
                        gridOriginXZ = gridOriginXZList[i];
                        foreach (BuildableEdgeObject.SaveObject placedEdgeObjectSaveObject in saveObject.placedEdgeObjectSaveObjectArrayArray[i].placedEdgeObjectSaveObjectArray)
                        {
                            BuildableEdgeObjectTypeSO buildableEdgeObjectTypeSO = GetBuildableEdgeObjectTypeSOFromName(placedEdgeObjectSaveObject.buildableEdgeObjectTypeSOName);
                            TryPlaceEdgeObjectXZ(gridXZ, placedEdgeObjectSaveObject.origin, buildableEdgeObjectTypeSO, placedEdgeObjectSaveObject.dir, placedEdgeObjectSaveObject.edgeRotation, placedEdgeObjectSaveObject.mousePosition, true, out BuildableEdgeObject buildableEdgeObject);
                        }
                    }
                    
                    foreach (LooseSaveObject looseSaveObject in saveObject.looseSaveObjectArray)
                    {
                        TryPlaceFreeObjectXZ(
                            GetBuildableFreeObjectTypeSOFromName(looseSaveObject.looseObjectSOName),
                            looseSaveObject.position,
                            looseSaveObject.quaternion,
                            true,
                            out BuildableFreeObject obj
                        );
                    }

                    gridOriginXZ = tempGridOriginXZ;
                }
                if (showConsoleText && saveAndLoad) Debug.Log("Grid XZ " + "<color=green>Grid Data Loaded!</color>");
            }
            else
            {
                if (PlayerPrefs.HasKey(uniqueSaveName+"_XY"))
                {
                    string json = PlayerPrefs.GetString(uniqueSaveName+"_XY");
                    json = GridSaveSystem.Load(uniqueSaveName+"_XY");
                    SaveObject saveObject = JsonUtility.FromJson<SaveObject>(json);
                    
                    Vector3 tempGridOriginXY = gridOriginXY;

                    for (int i = 0; i < gridXYList.Count; i++)
                    {
                        GridXY<GridObjectXY> gridXY = gridXYList[i];
                        gridOriginXY = gridOriginXYList[i];
                        foreach (BuildableGridObject.SaveObject placedObjectSaveObject in saveObject.placedObjectSaveObjectArrayArray[i].placedObjectSaveObjectArray)
                        {
                            BuildableGridObjectTypeSO placedObjectTypeSO = GetBuildableGridObjectTypeSOFromName(placedObjectSaveObject.buildableGridObjectTypeSOName);
                            TryPlaceGridObjectXY(gridXY, placedObjectSaveObject.origin, placedObjectTypeSO, placedObjectSaveObject.dir, true, out BuildableGridObject buildableGridObject);
                        }
                    }
                    
                    foreach (LooseSaveObject looseSaveObject in saveObject.looseSaveObjectArray)
                    {
                        TryPlaceFreeObjectXY(
                            GetBuildableFreeObjectTypeSOFromName(looseSaveObject.looseObjectSOName),
                            looseSaveObject.position,
                            looseSaveObject.quaternion,
                            true,
                            out BuildableFreeObject obj
                        );
                    }
                    
                    gridOriginXY = tempGridOriginXY;
                }
                if (showConsoleText && saveAndLoad) Debug.Log("Grid XY " + "<color=green>Grid Data Loaded!</color>");
            }
        }

        public BuildableFreeObjectTypeSO GetBuildableFreeObjectTypeSOFromName(string buildableFreeObjectTypeSOName) //This take a string and check if it exist in the name of object, if it is then return 'buildableGridObjectTypeSO' object.
        {
            foreach (BuildableFreeObjectTypeSO buildableFreeObjectTypeSO in buildableFreeObjectTypeSOList)
            {
                if (buildableFreeObjectTypeSO.name == buildableFreeObjectTypeSOName)
                {
                    return buildableFreeObjectTypeSO;
                }
            }
            return null;
        }
        
        public BuildableEdgeObjectTypeSO GetBuildableEdgeObjectTypeSOFromName(string buildableEdgeObjectTypeSOName) //This take a string and check if it exist in the name of object, if it is then return 'buildableGridObjectTypeSO' object.
        {
            foreach (BuildableEdgeObjectTypeSO buildableEdgeObjectTypeSO in buildableEdgeObjectTypeSOList)
            {
                if (buildableEdgeObjectTypeSO.name == buildableEdgeObjectTypeSOName)
                {
                    return buildableEdgeObjectTypeSO;
                }
            }
            return null;
        }

        public BuildableGridObjectTypeSO GetBuildableGridObjectTypeSOFromName(string buildableGridObjectTypeSOName) //This take a string and check if it exist in the name of object, if it is then return 'buildableGridObjectTypeSO' object.
        {
            foreach (BuildableGridObjectTypeSO buildableGridObjectTypeSO in buildableGridObjectTypeSOList)
            {
                if (buildableGridObjectTypeSO.name == buildableGridObjectTypeSOName)
                {
                    return buildableGridObjectTypeSO;
                }
            }
            return null;
        }

        [Serializable]
        public class SaveObject
        {
            public PlacedObjectSaveObjectArray[] placedObjectSaveObjectArrayArray;
            public PlacedEdgeObjectSaveObjectArray[] placedEdgeObjectSaveObjectArrayArray;
            public LooseSaveObject[] looseSaveObjectArray;
        }

        [Serializable]
        public class PlacedObjectSaveObjectArray
        {
            public BuildableGridObject.SaveObject[] placedObjectSaveObjectArray;
        }

        [Serializable]
        public class PlacedEdgeObjectSaveObjectArray
        {
            public BuildableEdgeObject.SaveObject[] placedEdgeObjectSaveObjectArray;
        }

        [Serializable]
        public class LooseSaveObject
        {
            public string looseObjectSOName;
            public Vector3 position;
            public float quaternion;
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
        //                                                                                        PUBLIC SET FUNCTIONS                                                                                              //
        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
        
        /// <summary>
        /// DO NOT USE IN RUNTIME! Set grid system's GridWidth. Takes one parameter.
        /// </summary>
        /// <param name="gridWidth">Grid width int value</param>
        public void SetGridWidth(int gridWidth)
        {
            this.gridWidth = gridWidth;
        }
        
        /// <summary>
        /// DO NOT USE IN RUNTIME! Set grid system's GridLength. Takes one parameter.
        /// </summary>
        /// <param name="gridLength">Grid length int value</param>
        public void SetGridLength(int gridLength)
        {
            this.gridLength = gridLength;
        }
        
        /// <summary>
        /// DO NOT USE IN RUNTIME! Set grid system's CellSize. Takes one parameter.
        /// </summary>
        /// <param name="cellSize">Grid cellSize float value</param>
        public void SetGridCellSize(float cellSize)
        {
            this.cellSize = cellSize;
        }

        /// <summary>
        /// DO NOT USE IN RUNTIME! Set grid system's GridHeight. Takes one parameter.
        /// </summary>
        /// <param name="gridHeight">Grid Height float value</param>
        public void SetGridHeight(float gridHeight)
        {
            this.gridHeight = gridHeight;
        }

        /// <summary>
        /// Set grid system's GridMode. Takes one parameter.
        /// </summary>
        /// <param name="gridMode">GridMode enum value</param>
        public void SetGridMode(GridMode gridMode)
        {
            this.gridMode = gridMode;
            OnGridModeChange?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Set grid system's Vertical Grid. Takes one parameter.
        /// </summary>
        /// <param name="currentActiveGridIndex">int Vertical Grid Index</param>
        public void SetActiveVerticalGrid(int currentActiveGridIndex)
        {
            if (gridAxis == GridAxis.XZ)
            {
                gridOriginXZ = gridOriginXZList[currentActiveGridIndex];
            }
            else
            {
                gridOriginXY = gridOriginXYList[currentActiveGridIndex];
            }
        }

        /// <summary>
        /// Set grid system's Vertical Grid Auto Height Detect. Takes one parameter.
        /// </summary>
        /// <param name="setActive">bool enable or disable</param>
        public void SetAutoDetectHeight(bool setActive)
        {
            autoDetectHeight = setActive;
        }
        
        /// <summary>
        /// Set grid system's Vertical Grid Change Height with Input. Takes one parameter.
        /// </summary>
        /// <param name="setActive">bool enable or disable</param>
        public void SetChangeHeightWithInput(bool setActive)
        {
            changeHeightWithInput = setActive;
        }

        /// <summary>
        /// Add Buildable Grid Object Type SO asset to the Grid System. Takes one parameter.
        /// </summary>
        /// <param name="buildableGridObjectTypeSO">BuildableGridObjectTypeSO asset</param>
        /// <param name="checkIfAlreadyHas">Check before if asset already exist, If so is ignore</param>
        public void AddBuildableGridObjectTypeSO(BuildableGridObjectTypeSO buildableGridObjectTypeSO, bool checkIfAlreadyHas = false)
        {
            if (checkIfAlreadyHas)
            {
                if (!buildableGridObjectTypeSOList.Contains(buildableGridObjectTypeSO)) buildableGridObjectTypeSOList.Add(buildableGridObjectTypeSO);
            }
            else
            {
                buildableGridObjectTypeSOList.Remove(buildableGridObjectTypeSO);
            }  
        }

        /// <summary>
        /// Add Buildable Free Object Type SO asset to the Grid System. Takes one parameter.
        /// </summary>
        /// <param name="buildableFreeObjectTypeSO">BuildableFreeObjectTypeSO asset</param>
        /// <param name="checkIfAlreadyHas">Check before if asset already exist, If so is ignore</param>
        public void AddBuildableFreeObjectTypeSO(BuildableFreeObjectTypeSO buildableFreeObjectTypeSO, bool checkIfAlreadyHas = false)
        {
            if (checkIfAlreadyHas)
            {
                if (!buildableFreeObjectTypeSOList.Contains(buildableFreeObjectTypeSO)) buildableFreeObjectTypeSOList.Add(buildableFreeObjectTypeSO);
            }
            else
            {
                buildableFreeObjectTypeSOList.Remove(buildableFreeObjectTypeSO);
            }  
        }

        /// <summary>
        /// Remove Buildable Grid Object Type SO asset from the Grid System. Takes one parameter.
        /// </summary>
        /// <param name="buildableGridObjectTypeSO">BuildableGridObjectTypeSO asset</param>
        public void RemoveBuildableFreeObjectTypeSO(BuildableGridObjectTypeSO buildableGridObjectTypeSO)
        {
            if (!buildableGridObjectTypeSOList.Contains(buildableGridObjectTypeSO)) buildableGridObjectTypeSOList.Remove(buildableGridObjectTypeSO); 
        }

        /// <summary>
        /// Remove Buildable Free Object Type SO asset from the Grid System. Takes one parameter.
        /// </summary>
        /// <param name="buildableFreeObjectTypeSO">BuildableFreeObjectTypeSO asset</param>
        public void RemoveBuildableFreeObjectTypeSO(BuildableFreeObjectTypeSO buildableFreeObjectTypeSO)
        {
            if (!buildableFreeObjectTypeSOList.Contains(buildableFreeObjectTypeSO)) buildableFreeObjectTypeSOList.Remove(buildableFreeObjectTypeSO); 
        }

        /// <summary>
        /// Set grid system's UseBuildableDistance. Takes four parameters.
        /// </summary>
        /// <param name="enable">Enable/Disable UseBuildableDistance</param>
        /// <param name="objectTransform">Take object transform to distance check, default null</param>
        /// <param name="distanceMin">Minimum distance</param>
        /// <param name="distanceMax">Maximum distance</param>
        public void SetUseBuildableDistance(bool enable, Transform objectTransform = null, float distanceMin = 0f, float distanceMax = 0f)
        {
            this.useBuildableDistance = enable;
            this.distanceCheckObject = objectTransform;
            this.distanceMin = distanceMin;
            this.distanceMax = distanceMax;
        }

        /// <summary>
        /// Set Grid System's Unique Save Name. Takes one parameter.
        /// </summary>
        /// <param name="uniqueSaveName">string unique save name</param>
        public void SetUniqueSaveName(string uniqueSaveName)
        {
            this.uniqueSaveName = uniqueSaveName;
        }

        /// <summary>
        /// Set Grid System's Save Location. Takes one parameter.
        /// </summary>
        /// <param name="saveLocation">string save location</param>
        public void SetSaveLocation(string saveLocation)
        {
            this.saveLocation = saveLocation;
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
        //                                                                                        PUBLIC GET FUNCTIONS                                                                                              //
        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Returns grid system's GridAxis. Takes no parameter.
        /// </summary>
        public GridAxis GetGridAxis()
        {
            return gridAxis;
        }

        /// <summary>
        /// Returns grid system's GridWidth. Takes no parameter.
        /// </summary>
        public int GetGridWidth()
        {
            return gridWidth;
        }
        
        /// <summary>
        /// Returns grid system's GridLength. Takes no parameter.
        /// </summary>
        public int GetGridLength()
        {
            return gridLength;
        }

        /// <summary>
        /// Returns grid system's CellSize. Takes no parameter.
        /// </summary>
        public float GetGridCellSize()
        {
            return cellSize;
        }

        /// <summary>
        /// Returns grid system's GridHeight. Takes no parameter.
        /// </summary>
        public float GetGridHeight()
        {
            return gridHeight;
        }

        /// <summary>
        /// Returns current active Vertical Grid system's grid index. Takes no parameter.
        /// </summary>
        public int GetActiveVerticalGridIndex()
        {
            return currentActiveGridIndex;
        }

        /// <summary>
        /// Returns Bertical Grids Count. Takes no parameter.
        /// </summary>
        public int GetVerticalGridCount()
        {
            return verticalGridsCount;
        }

        /// <summary>
        /// Returns Vertical Grid Auto detect height. Takes no parameter.
        /// </summary>
        public bool GetAutoDetectHeight()
        {
            return autoDetectHeight;
        }

        /// <summary>
        /// Returns Vertical Grid Change Height with Input. Takes no parameter.
        /// </summary>
        public bool GetChangeHeightWithInput()
        {
            return changeHeightWithInput;
        }

        /// <summary>
        /// Returns current stored BuildableGridObjectTypeSOList. Takes no parameter.
        /// </summary>
        public List<BuildableGridObjectTypeSO> GetBuildableGridObjectTypeSOList()
        {
            return buildableGridObjectTypeSOList;
        }

        /// <summary>
        /// Returns current stored BuildableEdgeObjectTypeSOList. Takes no parameter.
        /// </summary>
        public List<BuildableEdgeObjectTypeSO> GetBuildableEdgeObjectTypeSOList()
        {
            return buildableEdgeObjectTypeSOList;
        }

        /// <summary>
        /// Returns current stored BuildableGridObjectTypeSOList. Takes no parameter.
        /// </summary>
        public List<BuildableFreeObjectTypeSO> GetBuildableFreeObjectTypeSOList()
        {
            return buildableFreeObjectTypeSOList;
        }

        /// <summary>
        /// Returns grid system's GridOrigin. Takes no parameter.
        /// </summary>
        public Vector3 GetGridOrigin()
        {
            if (gridAxis == GridAxis.XZ)
            {
                return gridOriginXZ;
            }
            else
            {
                return gridOriginXY;
            }
        }

        /// <summary>
        /// Returns grid system's GridMode. Takes no parameter.
        /// </summary>
        public GridMode GetGridMode()
        {
            return gridMode;
        }

        /// <summary>
        /// Returns grid system's Unique Save Name. Takes no parameter.
        /// </summary>
        public string GetUniqueSaveName()
        {
            return uniqueSaveName;
        }

        /// <summary>
        /// Returns grid system's Save Location. Takes no parameter.
        /// </summary>
        public string GetSaveLocation()
        {
            return saveLocation;
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
        //                                                                                        UNUSED FUNCTIONS (FUTURE)                                                                                         //
        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//

        private void OnObjectStartMoving(EasyGridBuilderPro ownSystem, GameObject movingObject)
        {
            if (ownSystem == this)
            {
                //if (enableUnityEvents) OnObjectStartMovingUnityEvent?.Invoke();
                if (gridAxis == GridAxis.XZ)
                {
                    Vector3 mousePosition = GetMouseWorldPosition(); //Call function 'GetMouseWorldPosition' and assign value to 'mousePosition'
                    BuildableGridObject buildableGridObject; //'placedObject' variable
                    gridXZ.GetXZ(mousePosition, out int x, out int z); //Call function 'GetXZ' and pass 'mousePosition' to grab grid position 'x' and 'z'

                    if (IsValidGridPositionXZ(new Vector2Int(x, z))) //Call 'IsValidGridPosition' function and check if inside grid
                    {
                        buildableGridObject = gridXZ.GetGridObjectXZ(mousePosition).GetPlacedObject(); //Call function 'GetGridObject' in 'grid' and cache it in 'placedObject'
                    }
                    else
                    {
                        buildableGridObject = null;
                    }

                    if (buildableGridObject != null)
                    {
                        List<Vector2Int> gridPositionList = buildableGridObject.GetGridPositionList(); //Create a new list and grab all used grid positions from 'GetGridPositionList' function in 'placedObject'
                        foreach (Vector2Int gridPosition in gridPositionList) //Cycle through the list
                        {
                            gridXZ.GetGridObjectXZ(gridPosition.x, gridPosition.y).ClearPlacedObject(); //Call 'GetGridObject' in 'grid' and grab grid cell values then call 'ClearPlacedObject' function
                            if (enableUnityEvents) OnGridCellChangedUnityEvent?.Invoke(); //Invoke 'OnGridCellChangeUnityEvent' event
                        }
                    }
                }
                else
                {
                    Vector3 mousePosition = GetMouseWorldPosition(); //Call function 'GetMouseWorldPosition' and assign value to 'mousePosition'
                    BuildableGridObject buildableGridObject; //'placedObject' variable
                    gridXY.GetXY(mousePosition, out int x, out int z); //Call function 'GetXZ' and pass 'mousePosition' to grab grid position 'x' and 'z'

                    if (IsValidGridPositionXY(new Vector2Int(x, z))) //Call 'IsValidGridPosition' function and check if inside grid
                    {
                        buildableGridObject = gridXY.GetGridObjectXY(mousePosition).GetPlacedObject(); //Call function 'GetGridObject' in 'grid' and cache it in 'placedObject'
                    }
                    else
                    {
                        buildableGridObject = null;
                    }

                    if (buildableGridObject != null)
                    {
                        List<Vector2Int> gridPositionList = buildableGridObject.GetGridPositionList(); //Create a new list and grab all used grid positions from 'GetGridPositionList' function in 'placedObject'
                        foreach (Vector2Int gridPosition in gridPositionList) //Cycle through the list
                        {
                            gridXY.GetGridObjectXY(gridPosition.x, gridPosition.y).ClearPlacedObject(); //Call 'GetGridObject' in 'grid' and grab grid cell values then call 'ClearPlacedObject' function
                            if (enableUnityEvents) OnGridCellChangedUnityEvent?.Invoke(); //Invoke 'OnGridCellChangeUnityEvent' event
                        }
                    }
                }
            }
        }
        
        private void OnObjectStoppedMoving(EasyGridBuilderPro ownSystem, GameObject movingObject)
        {
            if (ownSystem == this)
            {
                //if (enableUnityEvents) OnObjectStoppedMovingUnityEvent?.Invoke();
            }
        }

        private void RemoveGridObjects()
        {
            BuildableGridObject[] buildableGridObjects = FindObjectsOfType<BuildableGridObject>();
            if (gridAxis == GridAxis.XZ)
            {
                foreach (var buildableGridObject in buildableGridObjects)
                {
                    if (buildableGridObject != null)
                    {
                        if (buildableGridObject.GetOwnGridSystem() == this)
                        {
                            buildableGridObject.DestroySelf();
                            if (enableUnityEvents) OnObjectRemovedUnityEvent?.Invoke();
                            if (showConsoleText && objectDestruction) Debug.Log("Grid XZ " + "<color=Red>Object Destroyed :</color> " + buildableGridObject);

                            List<Vector2Int> gridPositionList = buildableGridObject.GetGridPositionList();
                            foreach (Vector2Int gridPosition in gridPositionList)
                            {
                                gridXZ.GetGridObjectXZ(gridPosition.x, gridPosition.y).ClearPlacedObject();
                                if (enableUnityEvents) OnGridCellChangedUnityEvent?.Invoke();
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (var buildableGridObject in buildableGridObjects)
                {
                    if (buildableGridObject != null)
                    {
                        buildableGridObject.DestroySelf();
                        if (enableUnityEvents) OnObjectRemovedUnityEvent?.Invoke();
                        if (showConsoleText && objectDestruction) Debug.Log("Grid XY " + "<color=Red>Object Destroyed :</color> " + buildableGridObject);

                        List<Vector2Int> gridPositionList = buildableGridObject.GetGridPositionList();
                        foreach (Vector2Int gridPosition in gridPositionList)
                        {
                            gridXY.GetGridObjectXY(gridPosition.x, gridPosition.y).ClearPlacedObject();
                            if (enableUnityEvents) OnGridCellChangedUnityEvent?.Invoke();
                        }
                    }
                }
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
        //                                                                                                GIZMOS                                                                                                    //
        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//

        public void OnDrawGizmos()
        {
            if (gridEditorMode == GridEditorMode.None) return;

            if (showEditorAndRuntimeDebugGrid)
            {
                if (gridAxis == GridAxis.XZ)
                {
                    Gizmos.color = editorGridLineColor;
                    int[,] debugGridArray = new int[gridWidth, gridLength];
                    for (int x = 0; x < debugGridArray.GetLength(0); x++) //Cycle through the grid 1st dimention
                    {
                        for (int z = 0; z < debugGridArray.GetLength(1); z++) //Cycle through the grid 2nd dimention
                        {
                            Gizmos.DrawLine(GetWorldPositionForDebugXZ(x, z), GetWorldPositionForDebugXZ(x, z + 1)); //Draw debug lines to display grid cells in 'z' axis
                            Gizmos.DrawLine(GetWorldPositionForDebugXZ(x, z), GetWorldPositionForDebugXZ(x + 1, z)); //Draw debug lines to display grid cells in 'x' axis
                        }
                    }
                    Gizmos.DrawLine(GetWorldPositionForDebugXZ(0, gridLength), GetWorldPositionForDebugXZ(gridWidth, gridLength)); //Draw debug lines to display edge of the grid cells in 'z' axis
                    Gizmos.DrawLine(GetWorldPositionForDebugXZ(gridWidth, 0), GetWorldPositionForDebugXZ(gridWidth, gridLength)); //Draw debug lines to display edge of the grid cells in 'x' axis
                }
                else
                {
                    Gizmos.color = editorGridLineColor;
                    int[,] debugGridArray = new int[gridWidth, gridLength];
                    for (int x = 0; x < debugGridArray.GetLength(0); x++) //Cycle through the grid 1st dimention
                    {
                        for (int y = 0; y < debugGridArray.GetLength(1); y++) //Cycle through the grid 2nd dimention
                        {
                            Gizmos.DrawLine(GetWorldPositionForDebugXY(x, y), GetWorldPositionForDebugXY(x, y + 1)); //Draw debug lines to display grid cells in 'z' axis
                            Gizmos.DrawLine(GetWorldPositionForDebugXY(x, y), GetWorldPositionForDebugXY(x + 1, y)); //Draw debug lines to display grid cells in 'x' axis
                        }
                    }
                    Gizmos.DrawLine(GetWorldPositionForDebugXY(0, gridLength), GetWorldPositionForDebugXY(gridWidth, gridLength)); //Draw debug lines to display edge of the grid cells in 'z' axis
                    Gizmos.DrawLine(GetWorldPositionForDebugXY(gridWidth, 0), GetWorldPositionForDebugXY(gridWidth, gridLength)); //Draw debug lines to display edge of the grid cells in 'x' axis
                }
            }
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
        //                                                                                                  EXTRAS                                                                                                  //
        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//

        #region Extras
        public bool IsPointerOverUI() 
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            foreach (RaycastResult r in results)
            {
                if (r.gameObject.GetComponent<RectTransform>() != null) return true;
            }
            return false;
        }

        protected Vector3 GetMouseWorldPosition()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, mouseColliderLayerMask))
            {
                return raycastHit.point;
            }
            else
            {
                return new Vector3(-99999, -99999, -99999);
            }
        }

        private Vector3 BuildableFreeObjectCollidingMousePosition()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, freeObjectCollidingLayerMask))
            {
                if (currentBuildableObjectType == BuildableObjectType.FreeObject && buildableFreeObjectTypeSO != null)
                {
                    if (buildableFreeObjectTypeSO.snapToObjectSnappers)
                    {
                        if (raycastHit.collider.GetComponent<BuildableFreeObjectSnapper>()) return raycastHit.collider.GetComponent<BuildableFreeObjectSnapper>().transform.position;
                        else return raycastHit.point;
                    }
                    else return raycastHit.point;
                }
                else return raycastHit.point;
            }
            else
            {
                return new Vector3(-99999, -99999, -99999);
            }
        }

        private Vector3 AutoDetectHeightMousePosition()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, autoDetectHeightLayerMask))
            {
                return raycastHit.point;
            }
            else
            {
                return GetGridOrigin();
            }
        }

        private Vector3 GetPlacedObjectMouseWorldPosition()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f))
            {
                if (raycastHit.collider.transform.root.GetComponent<BuildableGridObject>())
                {
                    return raycastHit.collider.transform.root.GetComponent<BuildableGridObject>().transform.position;
                }
                else
                {
                    return new Vector3(-99999, -99999, -99999);
                }
            }
            else
            {
                return new Vector3(-99999, -99999, -99999);
            }
        }

        private BuildableEdgeObject GetPlacedEdgeObjectMouseWorldPosition()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f))
            {
                if (raycastHit.collider.transform.root.GetComponent<BuildableEdgeObject>())
                {
                    return raycastHit.collider.transform.root.GetComponent<BuildableEdgeObject>();
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        #endregion
    }
}