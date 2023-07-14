using System.Collections.Generic;
using SoulGames.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace SoulGames.EasyGridBuilderPro
{
    public class BuildableGridObject : MonoBehaviour
    {
        private Vector2Int calculatedWidthAndlength;
        private bool showGridBelowObject;
        private Canvas objectGridCanvas;
        private Sprite gridImageSprite;
        private Color gridImagePlaceableColor;
        private Color gridImageNotPlaceableColor;

        private GameObject spawnedCanvas;
        private Image gridImage;
        private float cellSize;
        private bool isObjectBuilt = false;
        private bool enableCanvas = false;

        private EasyGridBuilderPro ownGridSystem = null;
        private int ownGridSystemActiveGridIndex;
        private EasyGridBuilderPro activeGridSystem;

        [Tooltip("Provide this Buildable Grid Object's 'Buildable Grid Object Type SO'")]
        [SerializeField]private BuildableGridObjectTypeSO buildableGridObjectTypeSO;
        [Tooltip("Rotate object automatically in XY Grid Axis. (Which means this object is originally prepared for the Grid Axis XZ)")]
        [SerializeField]public bool rotateObjectForXY;
        [Tooltip("Rotate object automatically in XZ Grid Axis. (Which means this object is originally prepared for the Grid Axis XY)")]
        [SerializeField]public bool rotateObjectForXZ;
        private Vector2Int origin;
        private BuildableGridObjectTypeSO.Dir dir;
        private bool canvasHandleCalled = false;

        public static event OnBuildableGridObjectBuiltDelegate OnBuildableGridObjectBuilt;
        public delegate void OnBuildableGridObjectBuiltDelegate(BuildableGridObject buildableGridObject);

        [Rename("Rotate Scale & Pivot For XY")]
        [Tooltip("Set pivot on XY axis instead of XZ axis. (Use this if the object is originally prepared for the Grid Axis XY)")]
        [SerializeField]private bool rotateForXY;
        [Tooltip("Scale of the Object. (This is used to calculate grid object size and collision)")]
        [SerializeField]private Vector3 objectScale;
        [Tooltip("Offset of the Object Scale")]
        [SerializeField]private Vector3 objectCenter;
        [Tooltip("Custom Pivot position of this object")]
        [SerializeField]private Vector3 objectCustomPivot;

        private bool hasCollider = false;

        private void Start()
        {
            if (MultiGridManager.Instance.activeGridSystem == null) return;
            activeGridSystem = MultiGridManager.Instance.activeGridSystem;

            GridObjectSelector.OnObjectSelect += OnObjectSelect;
            GridObjectSelector.OnObjectDeselect += OnObjectDeselect;
            GridObjectMover.OnObjectStartMoving += OnObjectStartMoving;
            GridObjectMover.OnObjectStoppedMoving += OnObjectStoppedMoving;

            //Getting Variable Values (Usefull for Ghost Object phase)
            cellSize = activeGridSystem.GetGridCellSize();
            calculatedWidthAndlength = buildableGridObjectTypeSO.CalculatePlacedObjectSize(cellSize);
            showGridBelowObject = buildableGridObjectTypeSO.showGridBelowObject;
            objectGridCanvas = buildableGridObjectTypeSO.objectGridCanvas;
            gridImageSprite = buildableGridObjectTypeSO.gridImageSprite;
            gridImagePlaceableColor = buildableGridObjectTypeSO.gridImagePlaceableColor;
            gridImageNotPlaceableColor = buildableGridObjectTypeSO.gridImageNotPlaceableColor;

            if (showGridBelowObject)
            {
                if (!canvasHandleCalled)
                {
                    spawnedCanvas = Instantiate(objectGridCanvas.gameObject, Vector3.zero, Quaternion.identity);
                    spawnedCanvas.transform.SetParent(this.transform, false);
                }
                HandleVisualCanvasGrid(activeGridSystem);
            }
        }

        private void OnDestroy()
        {
            GridObjectSelector.OnObjectSelect -= OnObjectSelect;
            GridObjectSelector.OnObjectDeselect -= OnObjectDeselect;
            GridObjectMover.OnObjectStartMoving -= OnObjectStartMoving;
            GridObjectMover.OnObjectStoppedMoving -= OnObjectStoppedMoving;
        }

        private void OnObjectSelect(EasyGridBuilderPro ownSystem, GameObject selectedObject)
        {
            if (selectedObject.GetComponent<BuildableGridObject>() == this)
            {
                enableCanvas = true;
            }
            else
            {
                enableCanvas = false;
            }
        }

        private void OnObjectDeselect(EasyGridBuilderPro ownSystem, GameObject selectedObject)
        {
            enableCanvas = false;
        }
    
        private void OnObjectStartMoving(EasyGridBuilderPro ownSystem, GameObject movingObject)
        {
            if (movingObject.GetComponent<BuildableGridObject>() == this)
            {
                enableCanvas = true;
            }
            else
            {
                enableCanvas = false;
            }
        }

        private void OnObjectStoppedMoving(EasyGridBuilderPro ownSystem, GameObject movingObject)
        {
            enableCanvas = false;
        }

        private void Update()
        {
            if (!isObjectBuilt && showGridBelowObject)
            {
                HandleVisualCanvasGridColor();
            }
            else if (isObjectBuilt)
            {
                HandleVisualCanvasGridMode();
            }
        }

        public static BuildableGridObject Create(Vector3 worldPosition, Vector2Int origin, BuildableGridObjectTypeSO.Dir dir, BuildableGridObjectTypeSO buildableGridObjectTypeSO, EasyGridBuilderPro system)
        {
            EasyGridBuilderPro activeGridSystem = system;
            Transform placedObjectTransform = Instantiate(buildableGridObjectTypeSO.objectPrefab[Random.Range(0, buildableGridObjectTypeSO.objectPrefab.Length)], Vector3.zero, Quaternion.identity);

            placedObjectTransform.name = placedObjectTransform.name.Replace("(Clone)","").Trim();
            placedObjectTransform.rotation = Quaternion.Euler(0, buildableGridObjectTypeSO.GetRotationAngle(dir), 0);
            placedObjectTransform.localPosition = worldPosition;
            if (buildableGridObjectTypeSO.setBuiltObjectLayer) SetLayerRecursive(placedObjectTransform.gameObject, LayerNumber(buildableGridObjectTypeSO.builtObjectLayer));

            float cellSize = activeGridSystem.GetGridCellSize();
            Vector2Int offset = buildableGridObjectTypeSO.CalculatePlacedObjectSize(cellSize);
            BuildableGridObject buildableGridObject = placedObjectTransform.GetComponent<BuildableGridObject>();

            if (activeGridSystem.gridAxis == GridAxis.XZ)
            {
                if (buildableGridObject.IsRotateObjectForXZ())
                {
                    switch (buildableGridObjectTypeSO.GetRotationAngle(dir))
                    {
                        case 0:
                            placedObjectTransform.localPosition = new Vector3 (placedObjectTransform.localPosition.x + (offset.x * cellSize / 2), worldPosition.y, placedObjectTransform.localPosition.z + (offset.y * cellSize / 2));
                            placedObjectTransform.localPosition = new Vector3 (placedObjectTransform.localPosition.x - buildableGridObject.GetObjectPivotOffset().x, placedObjectTransform.localPosition.y - buildableGridObject.GetObjectPivotOffset().z, placedObjectTransform.localPosition.z - buildableGridObject.GetObjectPivotOffset().y);
                        break;
                        case 90:
                            placedObjectTransform.localPosition = new Vector3 (placedObjectTransform.localPosition.x + (offset.y * cellSize / 2), worldPosition.y, placedObjectTransform.localPosition.z - (offset.x * cellSize / 2));
                            placedObjectTransform.localPosition = new Vector3 (placedObjectTransform.localPosition.x - buildableGridObject.GetObjectPivotOffset().y, placedObjectTransform.localPosition.y - buildableGridObject.GetObjectPivotOffset().z, placedObjectTransform.localPosition.z + buildableGridObject.GetObjectPivotOffset().x);
                        break;
                        case 180:
                            placedObjectTransform.localPosition = new Vector3 (placedObjectTransform.localPosition.x - (offset.x * cellSize / 2), worldPosition.y, placedObjectTransform.localPosition.z - (offset.y * cellSize / 2));
                            placedObjectTransform.localPosition = new Vector3 (placedObjectTransform.localPosition.x + buildableGridObject.GetObjectPivotOffset().x, placedObjectTransform.localPosition.y - buildableGridObject.GetObjectPivotOffset().z, placedObjectTransform.localPosition.z + buildableGridObject.GetObjectPivotOffset().y);
                        break;
                        case 270:
                            placedObjectTransform.localPosition = new Vector3 (placedObjectTransform.localPosition.x - (offset.y * cellSize / 2), worldPosition.y, placedObjectTransform.localPosition.z + (offset.x * cellSize / 2));
                            placedObjectTransform.localPosition = new Vector3 (placedObjectTransform.localPosition.x + buildableGridObject.GetObjectPivotOffset().y, placedObjectTransform.localPosition.y - buildableGridObject.GetObjectPivotOffset().z, placedObjectTransform.localPosition.z - buildableGridObject.GetObjectPivotOffset().x);
                        break;
                    }
                }
                else
                {
                    switch (buildableGridObjectTypeSO.GetRotationAngle(dir))
                    {
                        case 0:
                            placedObjectTransform.localPosition = new Vector3 (placedObjectTransform.localPosition.x + (offset.x * cellSize / 2), worldPosition.y, placedObjectTransform.localPosition.z + (offset.y * cellSize / 2));
                            placedObjectTransform.localPosition = new Vector3 (placedObjectTransform.localPosition.x - buildableGridObject.GetObjectPivotOffset().x, placedObjectTransform.localPosition.y - buildableGridObject.GetObjectPivotOffset().y, placedObjectTransform.localPosition.z - buildableGridObject.GetObjectPivotOffset().z);
                        break;
                        case 90:
                            placedObjectTransform.localPosition = new Vector3 (placedObjectTransform.localPosition.x + (offset.y * cellSize / 2), worldPosition.y, placedObjectTransform.localPosition.z - (offset.x * cellSize / 2));
                            placedObjectTransform.localPosition = new Vector3 (placedObjectTransform.localPosition.x - buildableGridObject.GetObjectPivotOffset().z, placedObjectTransform.localPosition.y - buildableGridObject.GetObjectPivotOffset().y, placedObjectTransform.localPosition.z + buildableGridObject.GetObjectPivotOffset().x);
                        break;
                        case 180:
                            placedObjectTransform.localPosition = new Vector3 (placedObjectTransform.localPosition.x - (offset.x * cellSize / 2), worldPosition.y, placedObjectTransform.localPosition.z - (offset.y * cellSize / 2));
                            placedObjectTransform.localPosition = new Vector3 (placedObjectTransform.localPosition.x + buildableGridObject.GetObjectPivotOffset().x, placedObjectTransform.localPosition.y - buildableGridObject.GetObjectPivotOffset().y, placedObjectTransform.localPosition.z + buildableGridObject.GetObjectPivotOffset().z);
                        break;
                        case 270:
                            placedObjectTransform.localPosition = new Vector3 (placedObjectTransform.localPosition.x - (offset.y * cellSize / 2), worldPosition.y, placedObjectTransform.localPosition.z + (offset.x * cellSize / 2));
                            placedObjectTransform.localPosition = new Vector3 (placedObjectTransform.localPosition.x + buildableGridObject.GetObjectPivotOffset().z, placedObjectTransform.localPosition.y - buildableGridObject.GetObjectPivotOffset().y, placedObjectTransform.localPosition.z - buildableGridObject.GetObjectPivotOffset().x);
                        break;
                    }
                }
            }
            else
            {
                if (buildableGridObject.IsRotateObjectForXY())
                {
                    switch (buildableGridObjectTypeSO.GetRotationAngle(dir))
                    {
                        case 0:
                            placedObjectTransform.localPosition = new Vector3 (placedObjectTransform.localPosition.x + (offset.x * cellSize / 2), placedObjectTransform.localPosition.y + (offset.y * cellSize / 2), placedObjectTransform.localPosition.z);
                            placedObjectTransform.localPosition = new Vector3 (placedObjectTransform.localPosition.x - buildableGridObject.GetObjectPivotOffset().x, placedObjectTransform.localPosition.y - buildableGridObject.GetObjectPivotOffset().z, placedObjectTransform.localPosition.z + buildableGridObject.GetObjectPivotOffset().y);
                        break;
                        case 90:
                            placedObjectTransform.localPosition = new Vector3 (placedObjectTransform.localPosition.x + (offset.y * cellSize / 2), placedObjectTransform.localPosition.y - (offset.x * cellSize / 2), placedObjectTransform.localPosition.z);
                            placedObjectTransform.localPosition = new Vector3 (placedObjectTransform.localPosition.x - buildableGridObject.GetObjectPivotOffset().z, placedObjectTransform.localPosition.y + buildableGridObject.GetObjectPivotOffset().x, placedObjectTransform.localPosition.z + buildableGridObject.GetObjectPivotOffset().y);
                        break;
                        case 180:
                            placedObjectTransform.localPosition = new Vector3 (placedObjectTransform.localPosition.x - (offset.x * cellSize / 2), placedObjectTransform.localPosition.y - (offset.y * cellSize / 2), placedObjectTransform.localPosition.z);
                            placedObjectTransform.localPosition = new Vector3 (placedObjectTransform.localPosition.x + buildableGridObject.GetObjectPivotOffset().x, placedObjectTransform.localPosition.y + buildableGridObject.GetObjectPivotOffset().z, placedObjectTransform.localPosition.z + buildableGridObject.GetObjectPivotOffset().y);
                        break;
                        case 270:
                            placedObjectTransform.localPosition = new Vector3 (placedObjectTransform.localPosition.x - (offset.y * cellSize / 2), placedObjectTransform.localPosition.y + (offset.x * cellSize / 2), placedObjectTransform.localPosition.z);
                            placedObjectTransform.localPosition = new Vector3 (placedObjectTransform.localPosition.x + buildableGridObject.GetObjectPivotOffset().z, placedObjectTransform.localPosition.y - buildableGridObject.GetObjectPivotOffset().x, placedObjectTransform.localPosition.z + buildableGridObject.GetObjectPivotOffset().y);
                        break;
                    }
                }
                else
                {
                    switch (buildableGridObjectTypeSO.GetRotationAngle(dir))
                    {
                        case 0:
                            placedObjectTransform.localPosition = new Vector3 (placedObjectTransform.localPosition.x + (offset.x * cellSize / 2), placedObjectTransform.localPosition.y + (offset.y * cellSize / 2), placedObjectTransform.localPosition.z);
                            placedObjectTransform.localPosition = new Vector3 (placedObjectTransform.localPosition.x - buildableGridObject.GetObjectPivotOffset().x, placedObjectTransform.localPosition.y - buildableGridObject.GetObjectPivotOffset().y, placedObjectTransform.localPosition.z + buildableGridObject.GetObjectPivotOffset().z);
                        break;
                        case 90:
                            placedObjectTransform.localPosition = new Vector3 (placedObjectTransform.localPosition.x + (offset.y * cellSize / 2), placedObjectTransform.localPosition.y - (offset.x * cellSize / 2), placedObjectTransform.localPosition.z);
                            placedObjectTransform.localPosition = new Vector3 (placedObjectTransform.localPosition.x - buildableGridObject.GetObjectPivotOffset().y, placedObjectTransform.localPosition.y + buildableGridObject.GetObjectPivotOffset().x, placedObjectTransform.localPosition.z + buildableGridObject.GetObjectPivotOffset().z);
                        break;
                        case 180:
                            placedObjectTransform.localPosition = new Vector3 (placedObjectTransform.localPosition.x - (offset.x * cellSize / 2), placedObjectTransform.localPosition.y - (offset.y * cellSize / 2), placedObjectTransform.localPosition.z);
                            placedObjectTransform.localPosition = new Vector3 (placedObjectTransform.localPosition.x + buildableGridObject.GetObjectPivotOffset().x, placedObjectTransform.localPosition.y + buildableGridObject.GetObjectPivotOffset().y, placedObjectTransform.localPosition.z + buildableGridObject.GetObjectPivotOffset().z);
                        break;
                        case 270:
                            placedObjectTransform.localPosition = new Vector3 (placedObjectTransform.localPosition.x - (offset.y * cellSize / 2), placedObjectTransform.localPosition.y + (offset.x * cellSize / 2), placedObjectTransform.localPosition.z);
                            placedObjectTransform.localPosition = new Vector3 (placedObjectTransform.localPosition.x + buildableGridObject.GetObjectPivotOffset().y, placedObjectTransform.localPosition.y - buildableGridObject.GetObjectPivotOffset().x, placedObjectTransform.localPosition.z + buildableGridObject.GetObjectPivotOffset().z);
                        break;
                    }
                }
            }

            //placedObject.buildableGridObjectTypeSO = buildableGridObjectTypeSO;
            buildableGridObject.origin = origin;
            buildableGridObject.dir = dir;
            buildableGridObject.cellSize = cellSize;
            buildableGridObject.calculatedWidthAndlength = offset;

            if (!buildableGridObject.canvasHandleCalled)
            {
                buildableGridObject.spawnedCanvas = Instantiate(buildableGridObject.buildableGridObjectTypeSO.objectGridCanvas.gameObject, Vector3.zero, Quaternion.identity);
                buildableGridObject.spawnedCanvas.transform.SetParent(buildableGridObject.transform, false);
            }
            buildableGridObject.HandleVisualCanvasGrid(system);

            buildableGridObject.Setup();
            return buildableGridObject;
        }

        protected void Setup()
        {
            OnBuildableGridObjectBuilt?.Invoke(this);
            //Debug.Log("PlacedObject.Setup() " + transform);
        }

        public void GridSetupDone(EasyGridBuilderPro gridSystem, bool isObjectBuilt, int activeGridIndex, BuildableGridObjectTypeSO.Dir dir)
        {
            //Debug.Log("PlacedObject.GridSetupDone() " + transform);
            this.isObjectBuilt = isObjectBuilt;
            ownGridSystem = gridSystem;
            ownGridSystemActiveGridIndex = activeGridIndex;

            if (gridSystem.gridAxis == GridAxis.XY)
            {
                if (IsRotateObjectForXY())
                {
                    switch (dir)
                    {
                        case BuildableGridObjectTypeSO.Dir.Down:
                            transform.rotation = Quaternion.Euler(-90, buildableGridObjectTypeSO.GetRotationAngle(dir), 0);
                        break;
                        case BuildableGridObjectTypeSO.Dir.Left:
                            transform.rotation = Quaternion.Euler(0, buildableGridObjectTypeSO.GetRotationAngle(dir), -90);
                        break;
                        case BuildableGridObjectTypeSO.Dir.Up:
                            transform.rotation = Quaternion.Euler(90, buildableGridObjectTypeSO.GetRotationAngle(dir), 0);
                        break;
                        case BuildableGridObjectTypeSO.Dir.Right:
                            transform.rotation = Quaternion.Euler(0, buildableGridObjectTypeSO.GetRotationAngle(dir), 90);
                        break;
                    }
                }
            }
            else
            {
                if (IsRotateObjectForXZ())
                {
                    switch (dir)
                    {
                        case BuildableGridObjectTypeSO.Dir.Down:
                            transform.rotation = Quaternion.Euler(90, buildableGridObjectTypeSO.GetRotationAngle(dir), 0);
                        break;
                        case BuildableGridObjectTypeSO.Dir.Left:
                            transform.rotation = Quaternion.Euler(90, buildableGridObjectTypeSO.GetRotationAngle(dir), 0);
                        break;
                        case BuildableGridObjectTypeSO.Dir.Up:
                            transform.rotation = Quaternion.Euler(90, buildableGridObjectTypeSO.GetRotationAngle(dir), 0);
                        break;
                        case BuildableGridObjectTypeSO.Dir.Right:
                            transform.rotation = Quaternion.Euler(90, buildableGridObjectTypeSO.GetRotationAngle(dir), 0);
                        break;
                    }
                }
            }
        }

        public bool GetIsObjectBuilt()
        {
            return isObjectBuilt;
        }

        protected void TriggerGridObjectChanged()
        {
            if (activeGridSystem.gridAxis == GridAxis.XZ)
            {
                foreach (Vector2Int gridPosition in GetGridPositionList())
                {
                    activeGridSystem.GetGridObjectXZ(gridPosition).TriggerGridObjectChanged();
                }
            }
            else
            {
                foreach (Vector2Int gridPosition in GetGridPositionList())
                {
                    activeGridSystem.GetGridObjectXY(gridPosition).TriggerGridObjectChanged();
                }
            }
        }

        private void HandleVisualCanvasGrid(EasyGridBuilderPro activeGridSystem)
        {
            if (activeGridSystem.gridAxis == GridAxis.XZ)
            {
                if (spawnedCanvas)
                {
                    Transform gridTexture = spawnedCanvas.transform.GetChild(0);
                    gridImage = gridTexture.GetComponent<Image>();

                    if (!canvasHandleCalled)
                    {
                        Vector2 widthAndHeight = new Vector2(buildableGridObjectTypeSO.CalculatePlacedObjectSize(activeGridSystem.GetGridCellSize()).x * activeGridSystem.GetGridCellSize(), buildableGridObjectTypeSO.CalculatePlacedObjectSize(activeGridSystem.GetGridCellSize()).y * activeGridSystem.GetGridCellSize());

                        if (IsRotateObjectForXZ())
                        {
                            spawnedCanvas.transform.localEulerAngles = new Vector3(0, 0, 0);
                            spawnedCanvas.transform.localPosition = new Vector3(widthAndHeight.x/-2, widthAndHeight.y/-2, 0);
                            spawnedCanvas.transform.localPosition = new Vector3(spawnedCanvas.transform.localPosition.x + GetObjectPivotOffset().x, spawnedCanvas.transform.localPosition.y + GetObjectPivotOffset().y, spawnedCanvas.transform.localPosition.z);
                        }
                        else
                        {
                            spawnedCanvas.transform.localEulerAngles = new Vector3(90, 0, 0);
                            spawnedCanvas.transform.localPosition = new Vector3(widthAndHeight.x/-2, 0, widthAndHeight.y/-2);
                            spawnedCanvas.transform.localPosition = new Vector3(spawnedCanvas.transform.localPosition.x + GetObjectPivotOffset().x, spawnedCanvas.transform.localPosition.y, spawnedCanvas.transform.localPosition.z + GetObjectPivotOffset().z);
                        }

                        spawnedCanvas.GetComponent<RectTransform>().sizeDelta = widthAndHeight;
                        gridTexture.GetComponent<RectTransform>().sizeDelta = widthAndHeight;
                    }
                    
                    gridImage.sprite = gridImageSprite;
                    gridImage.type = Image.Type.Tiled;
                    if (!canvasHandleCalled)
                    {
                        gridImage.pixelsPerUnitMultiplier = 100 / activeGridSystem.GetGridCellSize();
                        canvasHandleCalled = true;
                    }

                    gridImage.color = gridImagePlaceableColor;
                }
            }
            else
            {
                 if (spawnedCanvas)
                {
                    Transform gridTexture = spawnedCanvas.transform.GetChild(0);
                    gridImage = gridTexture.GetComponent<Image>();

                    if (!canvasHandleCalled)
                    {
                        Vector2 widthAndHeight = new Vector2(buildableGridObjectTypeSO.CalculatePlacedObjectSize(activeGridSystem.GetGridCellSize()).x * activeGridSystem.GetGridCellSize(), buildableGridObjectTypeSO.CalculatePlacedObjectSize(activeGridSystem.GetGridCellSize()).y * activeGridSystem.GetGridCellSize());

                        if (IsRotateObjectForXY())
                        {
                            spawnedCanvas.transform.localEulerAngles = new Vector3(-90, 0, 0);
                            spawnedCanvas.transform.localPosition = new Vector3(widthAndHeight.x/-2, 0, widthAndHeight.y/2);
                            spawnedCanvas.transform.localPosition = new Vector3(spawnedCanvas.transform.localPosition.x  + GetObjectPivotOffset().x, spawnedCanvas.transform.localPosition.y, spawnedCanvas.transform.localPosition.z + GetObjectPivotOffset().z);
                        }
                        else
                        {
                            spawnedCanvas.transform.localEulerAngles = new Vector3(0, 0, 0);
                            spawnedCanvas.transform.localPosition = new Vector3(widthAndHeight.x/-2, widthAndHeight.y/-2, 0);
                            spawnedCanvas.transform.localPosition = new Vector3(spawnedCanvas.transform.localPosition.x + GetObjectPivotOffset().x, spawnedCanvas.transform.localPosition.y + GetObjectPivotOffset().y, spawnedCanvas.transform.localPosition.z);
                        }

                        spawnedCanvas.GetComponent<RectTransform>().sizeDelta = widthAndHeight;
                        gridTexture.GetComponent<RectTransform>().sizeDelta = widthAndHeight;
                    }
                    
                    gridImage.sprite = gridImageSprite;
                    gridImage.type = Image.Type.Tiled;
                    if (!canvasHandleCalled)
                    {
                        gridImage.pixelsPerUnitMultiplier = 100 / activeGridSystem.GetGridCellSize();
                        canvasHandleCalled = true;
                    }

                    gridImage.color = gridImagePlaceableColor;
                }
            }
        }

        private void HandleVisualCanvasGridColor()
        {
            if (spawnedCanvas)
            {
                if (!activeGridSystem.NotPlaceableVisualCallerBuildableGridObject())
                {
                    gridImage.color = gridImageNotPlaceableColor;
                }
                else
                {
                    gridImage.color = gridImagePlaceableColor;
                }
            }
        }

        private void HandleVisualCanvasGridMode()
        {
            if (enableCanvas && !spawnedCanvas.activeSelf)
            {
                spawnedCanvas.SetActive(true);
            }
            else if (!enableCanvas && spawnedCanvas.activeSelf)
            {
                spawnedCanvas.SetActive(false);
            }
        }

        public EasyGridBuilderPro GetOwnGridSystem()
        {
            return ownGridSystem;
        }

        public int GetOwnGridSystemActiveGridIndex()
        {
            return ownGridSystemActiveGridIndex;
        }

        public Vector2Int GetGridOrigin()
        {
            return origin;
        }

        public List<Vector2Int> GetGridPositionList()
        {
            return buildableGridObjectTypeSO.GetGridPositionList(origin, dir, activeGridSystem.GetGridCellSize());
        }

        public virtual void DestroySelf()
        {
            Destroy(gameObject);
        }

        public override string ToString()
        {
            return buildableGridObjectTypeSO.objectName;
        }

        public BuildableGridObjectTypeSO GetBuildableGridObjectTypeSO()
        {
            return buildableGridObjectTypeSO;
        }

        public Vector3 GetRawObjectScale()
        {
            return objectScale;
        }

        public Vector3 GetRawObjectCenter()
        {
            return objectCenter;
        }

        public bool IsRotateObjectForXY()
        {
            return rotateObjectForXY;
        }

        public bool IsRotateObjectForXZ()
        {
            return rotateObjectForXZ;
        }

        public Vector3 GetObjectPivotOffset()
        {
            return objectCustomPivot;
        }

        public Vector2 GetObjectScale()
        {
            if (ownGridSystem != null)
            {
                if (ownGridSystem.gridAxis == GridAxis.XZ)
                {
                    if (rotateObjectForXZ)
                    {
                        return new Vector2(objectScale.x, objectScale.y);
                    }
                    else
                    {
                        return new Vector2(objectScale.x, objectScale.z);
                    }
                }
                else
                {
                    if (rotateObjectForXY)
                    {
                        return new Vector2(objectScale.x, objectScale.z);
                    }
                    else
                    {
                        return new Vector2(objectScale.x, objectScale.y);
                    }
                }
            }
            else
            {
                if (MultiGridManager.Instance.activeGridSystem.gridAxis == GridAxis.XZ)
                {
                    if (rotateObjectForXZ)
                    {
                        return new Vector2(objectScale.x, objectScale.y);
                    }
                    else
                    {
                        return new Vector2(objectScale.x, objectScale.z);
                    }
                }
                else
                {
                    if (rotateObjectForXY)
                    {
                        return new Vector2(objectScale.x, objectScale.z);
                    }
                    else
                    {
                        return new Vector2(objectScale.x, objectScale.y);
                    }
                }
            }
        }

        public void AutoCalculatePivotAndSize()
        {
            if (!gameObject.GetComponent<BoxCollider>())
            {
                gameObject.AddComponent<BoxCollider>();
                hasCollider = false;
            }
            else
            {
                hasCollider = true;
            }

            bool hasBounds = false;
            Bounds bounds = new Bounds(Vector3.zero, Vector3.zero);

            if (gameObject.transform.childCount != 0)
            {
                for (int i = 0; i < gameObject.transform.childCount; ++i)
                {
                    Renderer childRenderer = gameObject.transform.GetChild(i).GetComponent<Renderer>();
                    if (childRenderer != null)
                    {
                        if (hasBounds)
                        {
                            bounds.Encapsulate(childRenderer.bounds);
                        }
                        else
                        {
                            bounds = childRenderer.bounds;
                            hasBounds = true;
                        }
                    }
                }

                BoxCollider collider = gameObject.GetComponent<BoxCollider>();
                collider.center = bounds.center - gameObject.transform.position;
                collider.size = bounds.size;
            }

            objectScale = GetComponent<BoxCollider>().bounds.size;
            objectCenter = GetComponent<BoxCollider>().bounds.center;

            if (rotateForXY)
            {
                objectCustomPivot = new Vector3(objectCenter.x, objectCenter.y, (objectCenter.z + objectScale.z / 2));
            }
            else
            {
                objectCustomPivot = new Vector3(objectCenter.x, (objectCenter.y - objectScale.y / 2), objectCenter.z);
            }

            if (!hasCollider)
            {
                BoxCollider collider = gameObject.GetComponent<BoxCollider>();
                DestroyImmediate(collider, true);
            }
            #if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(gameObject);
            #endif
        }

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying)
            {
                //Gizmos.matrix = this.transform.localToWorldMatrix;
                Color color = Color.cyan;
                Gizmos.color = color;
                Gizmos.DrawWireCube(objectCenter, objectScale);
                
                Vector3 startPos = objectCustomPivot;
                Vector3 endPos;

                if (rotateForXY)
                {
                    endPos = new Vector3(objectCustomPivot.x, objectCustomPivot.y, objectCustomPivot.z - (objectScale.z));
                }
                else
                {
                    endPos = new Vector3(objectCustomPivot.x, objectCustomPivot.y + (objectScale.y), objectCustomPivot.z);
                }

                Gizmos.color = Color.red;
                Gizmos.DrawSphere(startPos, .2f);
                Gizmos.DrawLine(startPos, endPos);
            }
        }

        private static int LayerNumber(LayerMask builtObjectLayer)
        {
            int layerNumber = 0;
            int layer = builtObjectLayer.value;
            while(layer > 0)
            {
                layer = layer >> 1;
                layerNumber++;
            }
            if (layerNumber > 1) return layerNumber - 1;
            else return 0;
        }

        private static void SetLayerRecursive(GameObject targetGameObject, int layer)
        {
            targetGameObject.layer = layer;
            foreach (Transform child in targetGameObject.transform)
            {
                SetLayerRecursive(child.gameObject, layer);
            }
        }

        public SaveObject GetSaveObject()
        {
            return new SaveObject
            {
                buildableGridObjectTypeSOName = buildableGridObjectTypeSO.name,
                origin = origin,
                dir = dir,
            };
        }

        [System.Serializable]
        public class SaveObject
        {
            public string buildableGridObjectTypeSOName;
            public Vector2Int origin;
            public BuildableGridObjectTypeSO.Dir dir;
        }
    }
}