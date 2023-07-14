using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using SoulGames.Utilities;

//THIS IS AN UNFINISHED SCRIPT
namespace SoulGames.EasyGridBuilderPro
{
    public class FreeObjectMover : MonoBehaviour
    {
        public static event OnBuildableFreeObjectStartMovingDelegate OnBuildableFreeObjectStartMoving;
        public delegate void OnBuildableFreeObjectStartMovingDelegate(EasyGridBuilderPro ownSystem, GameObject movingObject);
        public static event OnBuildableFreeObjectStoppedMovingDelegate OnBuildableFreeObjectStoppedMoving;
        public delegate void OnBuildableFreeObjectStoppedMovingDelegate(EasyGridBuilderPro ownSystem, GameObject movingObject);

        [SerializeField][ReadOnly]public GameObject movingObject; //Currently moving object
        [SerializeField]private bool resetOnFalsePlace = false;

        private EasyGridBuilderPro currentActiveSystem;
        private LayerMask mouseColliderLayerMask; //Layermask for raycast hit(Ground layer)
        private bool useMoveModeActivationKey;
        private bool isBuildableMoveActive;
        //private bool movePressed = false;
        private Transform parentObject;
        private Vector3 movingObjectPreviousPosition;
        private Vector3 movingObjectPreviousRotation;

        private void Start()
        {
            if (MultiGridManager.Instance.activeGridSystem == null) return;
            currentActiveSystem = MultiGridManager.Instance.activeGridSystem;
            mouseColliderLayerMask = MultiGridManager.Instance.mouseColliderLayerMask;
        }

        private void OnDestroy()
        {
        }

        private void Update()
        {
            if (MultiGridManager.Instance.activeGridSystem == null) return;
            currentActiveSystem = MultiGridManager.Instance.activeGridSystem;

            if (currentActiveSystem.GetGridMode() != GridMode.None && currentActiveSystem.GetGridMode() != GridMode.Moving)
            {
                SetGridModeReset();
            }
        }

        private void LateUpdate()
        {
            if (MultiGridManager.Instance.activeGridSystem == null) return;
            if (movingObject != null)
            {
                if (currentActiveSystem.gridAxis == GridAxis.XZ)
                {
                    Vector3 targetPosition = currentActiveSystem.GetBuildableFreeObjectMouseWorldPosition();
                    float targetRotation = currentActiveSystem.GetBuildableFreeObjectRotation();
                    transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 15f);
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, targetRotation, 0), Time.deltaTime * 25f);
                }
                else
                {
                    Vector3 targetPosition = currentActiveSystem.GetBuildableFreeObjectMouseWorldPosition();
                    float targetRotation = currentActiveSystem.GetBuildableFreeObjectRotation();
                    transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 15f);
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, targetRotation, 0), Time.deltaTime * 25f);
                }
            }
        }

        public void SetInputGridModeVariables(bool useBuildModeActivationKey, bool useDestructionModeActivationKey, bool useSelectionModeActivationKey)
        {
            //this.useMoveModeActivationKey = useMoveModeActivationKey;
        }

        public void SetGridModeReset()
        {
            if (movingObject)
            {
                OnBuildableFreeObjectStoppedMoving?.Invoke(movingObject.GetComponent<BuildableFreeObject>().GetOwnGridSystem(), movingObject); //Event
                movingObject.transform.SetParent(null);
                movingObject.transform.position = movingObjectPreviousPosition;
                movingObject.transform.eulerAngles = movingObjectPreviousRotation;
                movingObject = null;
            }
        }

        public void SetGridModeMoving()
        {
            if (useMoveModeActivationKey)
            {
                isBuildableMoveActive = true;
                foreach (EasyGridBuilderPro easyGridBuilderPro in MultiGridManager.Instance.easyGridBuilderProList)
                {
                    if (easyGridBuilderPro.GetGridMode() != GridMode.Moving) easyGridBuilderPro.SetGridMode(GridMode.Moving);
                }
            }
        }

        public void TriggerBuildableMove()
        {
            if (useMoveModeActivationKey == false)
            {
                isBuildableMoveActive = true;
            }

            if (isBuildableMoveActive && !IsPointerOverUI())
            {
                if (currentActiveSystem.GetGridMode() == GridMode.None || currentActiveSystem.GetGridMode() == GridMode.Moving)
                {
                    if (currentActiveSystem.gridAxis == GridAxis.XZ)
                    {
                        HandleBuildingMovingXZ();
                    }
                    else
                    {
                        HandleBuildingMovingXY();
                    }
                }
                else
                {
                    DeselectSelectedObject();
                }
            }

            if (useMoveModeActivationKey == false)
            {
                isBuildableMoveActive = false;
            }
        }
        
        private void HandleBuildingMovingXZ()
        {
            if (currentActiveSystem.GetGridObjectXZ(GetPlacedObjectCalculatedMouseWorldPositionXZ()) != null || currentActiveSystem.GetGridObjectXZ(GetMouseWorldPosition()) != null)
            {
                BuildableGridObject buildableGridObject;
                if (currentActiveSystem.GetGridObjectXZ(GetPlacedObjectCalculatedMouseWorldPositionXZ()) != null) 
                {
                    buildableGridObject = currentActiveSystem.GetGridObjectXZ(GetPlacedObjectCalculatedMouseWorldPositionXZ()).GetPlacedObject();
                }
                else
                {
                    buildableGridObject = currentActiveSystem.GetGridObjectXZ(GetMouseWorldPosition()).GetPlacedObject();
                }
                
                if (buildableGridObject != null)
                {
                    if (movingObject != null)
                    {
                        if (movingObject.GetComponent<BuildableGridObject>().GetOwnGridSystem() != currentActiveSystem) movingObject.GetComponent<BuildableGridObject>().GetOwnGridSystem().SetGridMode(GridMode.None);
                    }
                    movingObject = buildableGridObject.gameObject;
                    currentActiveSystem.SetGridMode(GridMode.Moving);
                    //if (currentActiveSystem.showConsoleText && currentActiveSystem.objectMoving) Debug.Log("Grid XZ " + "<color=green>Object Moving :</color> " + buildableGridObject);
                    OnBuildableFreeObjectStartMoving?.Invoke(movingObject.GetComponent<BuildableGridObject>().GetOwnGridSystem(), movingObject); //Event
                    HookMovingObject();
                }
                else
                {
                    DeselectSelectedObject();
                }
            }
            else
            {
                DeselectSelectedObject();
            }
        }

        private void HookMovingObject() //This function is used to refresh the visual object
        {
            if (movingObject != null) //If 'buildableGridObjectTypeSO' is not empty(If currently a 'buildableGridObjectType' is selected)
            {
                movingObjectPreviousPosition = movingObject.transform.position;
                movingObjectPreviousRotation = movingObject.transform.eulerAngles;
                movingObject.transform.parent = transform;

                movingObject.transform.localPosition = Vector3.zero; //Change position of 'visual' to 0, 0, 0
                movingObject.transform.localEulerAngles = Vector3.zero; //Change rotation of 'visual' to 0, 0, 0

                parentObject = new GameObject(movingObject.GetComponent<BuildableGridObject>().GetBuildableGridObjectTypeSO().name).transform;
                parentObject.parent = transform;
                parentObject.localPosition = Vector3.zero;
                parentObject.localEulerAngles = Vector3.zero;
                parentObject.localScale = new Vector3(parentObject.localScale.x + 0.01f, parentObject.localScale.y + 0.01f, parentObject.localScale.z + 0.01f);

                movingObject.transform.parent = parentObject;
                Vector2Int offset = movingObject.GetComponent<BuildableGridObject>().GetBuildableGridObjectTypeSO().CalculatePlacedObjectSize(currentActiveSystem.GetGridCellSize());
                if (currentActiveSystem.gridAxis == GridAxis.XZ)
                {
                    parentObject.localPosition = new Vector3(offset.x * currentActiveSystem.GetGridCellSize() / 2, movingObject.transform.localPosition.y, offset.y * currentActiveSystem.GetGridCellSize() / 2);
                }
                else
                {
                    parentObject.localPosition = new Vector3(offset.x * currentActiveSystem.GetGridCellSize() / 2, offset.y * currentActiveSystem.GetGridCellSize() / 2, movingObject.transform.localPosition.z);
                }
            }
        }

        private void HandleBuildingMovingXY()
        {
            if (currentActiveSystem.GetGridObjectXY(GetMouseWorldPosition()) != null)
            {
                BuildableGridObject buildableGridObject;
                // if (currentActiveSystem.GetGridObjectXY(GetPlacedObjectCalculatedMouseWorldPositionXY()) != null) 
                // {
                //     placedObject = currentActiveSystem.GetGridObjectXY(GetPlacedObjectCalculatedMouseWorldPositionXY()).GetPlacedObject();
                // }
                // else
                // {
                //     placedObject = currentActiveSystem.GetGridObjectXY(GetMouseWorldPosition()).GetPlacedObject();
                // }
                buildableGridObject = currentActiveSystem.GetGridObjectXY(GetMouseWorldPosition()).GetPlacedObject();
                if (buildableGridObject != null)
                {
                    if (movingObject != null)
                    {
                        if (movingObject.GetComponent<BuildableGridObject>().GetOwnGridSystem() != currentActiveSystem) movingObject.GetComponent<BuildableGridObject>().GetOwnGridSystem().SetGridMode(GridMode.None);
                    }
                    movingObject = buildableGridObject.gameObject;
                    currentActiveSystem.SetGridMode(GridMode.Selected);
                    if (currentActiveSystem.showConsoleText && currentActiveSystem.objectSelected) Debug.Log("Grid XY " + "<color=green>Object selected :</color> " + buildableGridObject);
                    OnBuildableFreeObjectStartMoving?.Invoke(movingObject.GetComponent<BuildableGridObject>().GetOwnGridSystem(), movingObject); //Event
                }
                else
                {
                    DeselectSelectedObject();
                }
            }
            else
            {
                DeselectSelectedObject();
            }
        }

        private void DeselectSelectedObject()
        {
            if (resetOnFalsePlace)
            {
                if (movingObject)
                {
                    Debug.Log("Here");
                    OnBuildableFreeObjectStoppedMoving?.Invoke(movingObject.GetComponent<BuildableGridObject>().GetOwnGridSystem(), movingObject); //Event
                    movingObject.GetComponent<BuildableGridObject>().GetOwnGridSystem().SetGridMode(GridMode.None);
                    movingObject.transform.position = movingObjectPreviousPosition;
                    movingObject.transform.eulerAngles = movingObjectPreviousRotation;
                    movingObject.transform.SetParent(null);
                    movingObject = null;
                }
            }
        }

        private Vector3 GetPlacedObjectCalculatedMouseWorldPositionXZ()
        {
            Vector3 oldPos = GetPlacedObjectMouseWorldPosition();
            Vector3 newPos = new Vector3 (oldPos.x, oldPos.y + 100, oldPos.z);

            if (Physics.Raycast(newPos, Vector3.down, out RaycastHit raycastHit, 999f, mouseColliderLayerMask))
            {
                return raycastHit.point;
            }
            else
            {
                return new Vector3(-99999, -99999, -99999);
            }
        }

        // private Vector3 GetPlacedObjectCalculatedMouseWorldPositionXY()
        // {
        //     Vector3 oldPos = GetPlacedObjectMouseWorldPosition();
        //     Vector3 newPos = new Vector3 (oldPos.x, oldPos.y, oldPos.z  + 100);

        //     if (Physics.Raycast(newPos, Vector3.back, out RaycastHit raycastHit, 999f, mouseColliderLayerMask))
        //     {
        //         return raycastHit.point;
        //     }
        //     else
        //     {
        //         return new Vector3(-99999, -99999, -99999);
        //     }
        // }

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

        private Vector3 GetMouseWorldPosition()
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
        #endregion
    }
}
