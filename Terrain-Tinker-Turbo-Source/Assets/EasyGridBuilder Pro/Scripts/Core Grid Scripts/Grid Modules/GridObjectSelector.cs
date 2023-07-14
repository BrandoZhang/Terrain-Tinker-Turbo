using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using SoulGames.Utilities;

namespace SoulGames.EasyGridBuilderPro
{
    public class GridObjectSelector : MonoBehaviour
    {
        public static event OnObjectSelectDelegate OnObjectSelect;
        public delegate void OnObjectSelectDelegate(EasyGridBuilderPro ownSystem, GameObject selectedObject);
        public static event OnObjectDeselectDelegate OnObjectDeselect;
        public delegate void OnObjectDeselectDelegate(EasyGridBuilderPro ownSystem, GameObject selectedObject);

        [Tooltip("Read Only, Display currently selected object.")]
        [SerializeField][ReadOnly]public GameObject selectedObject; //Currently selected object
        [Tooltip("If enabled, When clicked on somewhere that is not selectable, the previous selected object will be deselected and the grid mode will reset.")]
        [SerializeField]private bool deselectOnFalseSelect = true;

        private LayerMask mouseColliderLayerMask; //Layermask for raycast hit(Ground layer)
        private EasyGridBuilderPro currentActiveSystem;
        private bool useSelectionModeActivationKey;
        private bool isBuildableSelectionActive;

        private void Start()
        {
            if (MultiGridManager.Instance.activeGridSystem == null) return;
            currentActiveSystem = MultiGridManager.Instance.activeGridSystem;
            mouseColliderLayerMask = MultiGridManager.Instance.mouseColliderLayerMask;
        }

        protected void Update()
        {
            if (MultiGridManager.Instance.activeGridSystem == null) return;
            currentActiveSystem = MultiGridManager.Instance.activeGridSystem;

            if (selectedObject)
            {
                if (currentActiveSystem.GetGridMode() != GridMode.None && currentActiveSystem.GetGridMode() != GridMode.Selected)
                {
                    OnObjectDeselect?.Invoke(selectedObject.GetComponent<BuildableGridObject>().GetOwnGridSystem(), selectedObject); //Event
                    selectedObject = null;
                }
            }
        }

        public void SetInputGridModeVariables(bool useBuildModeActivationKey, bool useDestructionModeActivationKey, bool useSelectionModeActivationKey)
        {
            this.useSelectionModeActivationKey = useSelectionModeActivationKey;
        }

        public void SetGridModeReset()
        {
            if (selectedObject)
            {
                OnObjectDeselect?.Invoke(selectedObject.GetComponent<BuildableGridObject>().GetOwnGridSystem(), selectedObject); //Event
                selectedObject = null;
            }
        }

        public void SetGridModeSelection()
        {
            if (useSelectionModeActivationKey)
            {
                foreach (EasyGridBuilderPro easyGridBuilderPro in MultiGridManager.Instance.easyGridBuilderProList)
                {
                    if (easyGridBuilderPro.GetGridMode() != GridMode.Selected)
                    {
                        isBuildableSelectionActive = true;
                        easyGridBuilderPro.SetGridMode(GridMode.Selected);
                    }
                    else
                    {
                        isBuildableSelectionActive = false;
                        easyGridBuilderPro.SetGridMode(GridMode.None);
                        SetGridModeReset();
                    }
                }
            }
        }

        public void TriggerBuildableSelection()
        {
            if (useSelectionModeActivationKey == false)
            {
                isBuildableSelectionActive = true;
            }

            if (isBuildableSelectionActive && !IsPointerOverUI())
            {
                if (currentActiveSystem.GetGridMode() == GridMode.None || currentActiveSystem.GetGridMode() == GridMode.Selected)
                {
                    if (currentActiveSystem.gridAxis == GridAxis.XZ)
                    {
                        HandleBuildingSelectionXZ();
                    }
                    else
                    {
                        HandleBuildingSelectionXY();
                    }
                }
                else
                {
                    DeselectSelectedObject();
                }
            }

            if (useSelectionModeActivationKey == false)
            {
                isBuildableSelectionActive = false;
            }
        }

        private void HandleBuildingSelectionXZ()
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
                    if (selectedObject != null)
                    {
                        if (selectedObject.GetComponent<BuildableGridObject>().GetOwnGridSystem() != currentActiveSystem) selectedObject.GetComponent<BuildableGridObject>().GetOwnGridSystem().SetGridMode(GridMode.None);
                    }
                    selectedObject = buildableGridObject.gameObject;
                    currentActiveSystem.SetGridMode(GridMode.Selected);
                    if (currentActiveSystem.showConsoleText && currentActiveSystem.objectSelected) Debug.Log("Grid XZ " + "<color=green>Object selected :</color> " + buildableGridObject);
                    OnObjectSelect?.Invoke(selectedObject.GetComponent<BuildableGridObject>().GetOwnGridSystem(), selectedObject); //Event
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

        private void HandleBuildingSelectionXY()
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
                    if (selectedObject != null)
                    {
                        if (selectedObject.GetComponent<BuildableGridObject>().GetOwnGridSystem() != currentActiveSystem) selectedObject.GetComponent<BuildableGridObject>().GetOwnGridSystem().SetGridMode(GridMode.None);
                    }
                    selectedObject = buildableGridObject.gameObject;
                    currentActiveSystem.SetGridMode(GridMode.Selected);
                    if (currentActiveSystem.showConsoleText && currentActiveSystem.objectSelected) Debug.Log("Grid XY " + "<color=green>Object selected :</color> " + buildableGridObject);
                    OnObjectSelect?.Invoke(selectedObject.GetComponent<BuildableGridObject>().GetOwnGridSystem(), selectedObject); //Event
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
            if (deselectOnFalseSelect)
            {
                if (selectedObject)
                {
                    OnObjectDeselect?.Invoke(selectedObject.GetComponent<BuildableGridObject>().GetOwnGridSystem(), selectedObject); //Event
                    selectedObject.GetComponent<BuildableGridObject>().GetOwnGridSystem().SetGridMode(GridMode.None);
                    selectedObject = null;
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

        private Vector3 GetPlacedObjectMouseWorldPosition()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f))
            {
                if (raycastHit.collider.transform.root.GetComponent<BuildableGridObject>())
                {
                    BuildableGridObject buildableGridObject = raycastHit.collider.transform.root.GetComponent<BuildableGridObject>();
                    return buildableGridObject.transform.position;
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
