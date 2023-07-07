using UnityEngine;
using SoulGames.Utilities;
using System;

namespace SoulGames.EasyGridBuilderPro
{
    public class EdgeObjectGhost : MonoBehaviour //This class handles displaying selected 'PlacedObjectType'
    {
        [Tooltip("Spawned Ghost object's layer. \nIMPORTANT: Set this to 'Ignore Raycast' layer.")]
        [SerializeField]private LayerMask ghostObjectLayer; //Layer that should be used
        
        private Transform visual = null; //Visual reference
        private Transform parentObject;
        private EasyGridBuilderPro currentActiveSystem;
        private Material selectedMat = null;
        private bool updateFix = false;

        private BoxCollider tempCollider;
        private Rigidbody tempRB;
        private ColliderBridgeGridObject colliderBridge;
        private Vector3 tempColliderScale;
        private Vector3 tempColliderCenter;
        private BuildableEdgeObjectTypeSO.Dir edgeDir = BuildableEdgeObjectTypeSO.Dir.Down;
        private BuildableEdgeObjectTypeSO.Dir previousEdgeDir = BuildableEdgeObjectTypeSO.Dir.Down;
        private float edgeRotation = 0f;

        public static event OnBuildableObjectAreaBlockerEnterDelegate OnBuildableObjectAreaBlockerEnter;
        public delegate void OnBuildableObjectAreaBlockerEnterDelegate();
        public static event OnBuildableObjectAreaBlockerExitDelegate OnBuildableObjectAreaBlockerExit;
        public delegate void OnBuildableObjectAreaBlockerExitDelegate();

        private void Start()
        {
            if (MultiGridManager.Instance.activeGridSystem == null) return;
            currentActiveSystem = MultiGridManager.Instance.activeGridSystem;

            CleanObject();
            RefreshVisual(false);
            
            MultiGridManager.Instance.OnActiveGridChanged += OnGridSystemChanged;
            foreach (EasyGridBuilderPro grid in MultiGridManager.Instance.easyGridBuilderProList)
            {
                grid.OnSelectedBuildableChanged += OnSelectedChanged;
                grid.OnBuildableEdgeObjectFlip += OnBuildableEdgeObjectFlip;
            }
        }

        private void OnDestroy()
        {
            MultiGridManager.Instance.OnActiveGridChanged -= OnGridSystemChanged;  
            foreach (EasyGridBuilderPro grid in MultiGridManager.Instance.easyGridBuilderProList)
            {
                grid.OnSelectedBuildableChanged -= OnSelectedChanged;
                grid.OnBuildableEdgeObjectFlip -= OnBuildableEdgeObjectFlip;
            }
        }

        private void OnSelectedChanged(object sender, System.EventArgs e)
        {
            //Debug.Log("OnSelectedChanged");
            CleanObject();
            RefreshVisual(false);
        }

        private void OnGridSystemChanged(EasyGridBuilderPro currentActiveSystem)
        {
            //Debug.Log("OnGirdSystemChanged");
            this.currentActiveSystem = currentActiveSystem;
            BuildableEdgeObjectTypeSO buildableEdgeObjectTypeSO = currentActiveSystem.GetBuildableEdgeObjectTypeSO();
            CleanObject();
            RefreshVisual(false);
        }

        
        private void OnBuildableEdgeObjectFlip(float edgeRotation)
        {
            this.edgeRotation = edgeRotation;
            CleanObject();
            RefreshVisual(true);
        }

        private void Update()
        {
            if (MultiGridManager.Instance.activeGridSystem == null) return;

            currentActiveSystem = MultiGridManager.Instance.activeGridSystem;
            BuildableEdgeObjectTypeSO buildableEdgeObjectTypeSO = currentActiveSystem.GetBuildableEdgeObjectTypeSO(); //Call 'GetBuildableGridObjectTypeSO' function in 'GridBuildingSystem3D' and assign returned value to 'buildableGridObjectTypeSO'

            //edgeRotation = currentActiveSystem.GetEdgeObjectRotation();

            if (visual != null)
            {
                edgeDir = currentActiveSystem.CalcualteEdgeObjectDir(currentActiveSystem.GetLocalMousePosition());

                if (currentActiveSystem.GetGridMode() != GridMode.Build)
                {
                    Destroy(parentObject.gameObject);
                    parentObject = null;
                    visual = null;
                }
            }

            if (!MultiGridManager.Instance.onGrid)
            {
                if (visual != null)
                {
                    Destroy(parentObject.gameObject);
                    parentObject = null;
                    visual = null;
                    updateFix = true;
                }
            }
            else
            {
                if (updateFix)
                {
                    CleanObject();
                    RefreshVisual(false);
                    updateFix = false;
                }
            }

            if (edgeDir != previousEdgeDir)
            {
                CleanObject();
                RefreshVisual(false);
                previousEdgeDir = edgeDir;
            }
        }

        private void LateUpdate()
        {
            if (MultiGridManager.Instance.activeGridSystem == null) return;

            if (currentActiveSystem.gridAxis == GridAxis.XZ)
            {
                Vector3 targetPosition = currentActiveSystem.GetMouseWorldSnappedPositionForEdgeObjects(); //Call 'GetMouseWorldSnappedPosition' in 'GridBuildingSystem3D' and assign the returned position in 'targetPosition'
                transform.position = Vector3.Lerp(transform.position, new Vector3(targetPosition.x, currentActiveSystem.GetGridOrigin().y, targetPosition.z), Time.deltaTime * 25f); //Smoothly lerp through the current position and 'targetPosition'
                transform.rotation = Quaternion.Lerp(transform.rotation, currentActiveSystem.GetPlacedEdgeObjectRotation(), Time.deltaTime * 25f); //Smoothly lerp through the current rotation and target rotation grabbed from 'GetPlacedObjectRotation' fuction
            }
            else
            {
                Vector3 targetPosition = currentActiveSystem.GetMouseWorldSnappedPositionForEdgeObjects(); //Call 'GetMouseWorldSnappedPosition' in 'GridBuildingSystem3D' and assign the returned position in 'targetPosition'
                transform.position = Vector3.Lerp(transform.position, new Vector3(targetPosition.x, targetPosition.y, currentActiveSystem.GetGridOrigin().z), Time.deltaTime * 25f); //Smoothly lerp through the current position and 'targetPosition'
                transform.rotation = Quaternion.Lerp(transform.rotation, currentActiveSystem.GetPlacedEdgeObjectRotation(), Time.deltaTime * 25f); //Smoothly lerp through the current rotation and target rotation grabbed from 'GetPlacedObjectRotation' fuction
            }

            BuildableEdgeObjectTypeSO buildableEdgeObjectTypeSO = currentActiveSystem.GetBuildableEdgeObjectTypeSO(); //Call 'GetBuildableGridObjectTypeSO' function in 'GridBuildingSystem3D' and assign returned value to 'buildableGridObjectTypeSO'
            if (visual != null) HandleVisualColor(buildableEdgeObjectTypeSO);
        }

        private void RefreshVisual(bool callFromFlip) //This function is used to refresh the visual object
        {
            if (visual != null) //If 'visual' is not empty
            {
                Destroy(parentObject.gameObject); //Destroy 'visual' game object
                parentObject = null;
                visual = null; //And assign 'visual' to null
            }

            BuildableEdgeObjectTypeSO buildableEdgeObjectTypeSO = currentActiveSystem.GetBuildableEdgeObjectTypeSO(); //Call 'GetBuildableGridObjectTypeSO' function in 'GridBuildingSystem3D' and assign returned value to 'buildableGridObjectTypeSO'

            if (buildableEdgeObjectTypeSO != null) //If 'buildableGridObjectTypeSO' is not empty(If currently a 'buildableGridObjectType' is selected)
            {
                if (buildableEdgeObjectTypeSO.ghostPrefab == null)
                {
                    buildableEdgeObjectTypeSO.ghostPrefab = buildableEdgeObjectTypeSO.objectPrefab[0]; //If 'buildableGridObjectTypeSO' visual is empty then use the 'prefab' as the 'visual'
                }

                visual = Instantiate(buildableEdgeObjectTypeSO.ghostPrefab, Vector3.zero, Quaternion.identity); //Instantiate the visual game object and cache it in 'visual'

                HandleVisualColor(buildableEdgeObjectTypeSO);

                visual.parent = transform; //Attach 'visual' game object under this game objects transform as a child
                visual.localPosition = Vector3.zero; //Change position of 'visual' to 0, 0, 0
                visual.localEulerAngles = Vector3.zero; //Change rotation of 'visual' to 0, 0, 0

                parentObject = new GameObject(buildableEdgeObjectTypeSO.name).transform;
                parentObject.parent = transform;
                parentObject.localPosition = Vector3.zero;
                parentObject.localEulerAngles = Vector3.zero;
                parentObject.localScale = new Vector3(parentObject.localScale.x + 0.01f, parentObject.localScale.y + 0.01f, parentObject.localScale.z + 0.01f);
                BuildableEdgeObject buildableEdgeObject = visual.GetComponent<BuildableEdgeObject>();

                if (currentActiveSystem.gridAxis == GridAxis.XY)
                {
                    if (buildableEdgeObject.IsRotateObjectForXY())
                    {
                        visual.localEulerAngles = new Vector3(-90, 0, 0);
                    }
                    else
                    {

                    }
                }
                else
                {
                    if (buildableEdgeObject.IsRotateObjectForXZ())
                    {
                        visual.localEulerAngles = new Vector3(90, 0, 0);
                        //if (!callFromFlip) 
                    }
                    else
                    {
                        //if (!callFromFlip) visual.localEulerAngles = new Vector3(visual.localEulerAngles.x, visual.localEulerAngles.y -180, visual.localEulerAngles.z);
                        visual.localEulerAngles = new Vector3(visual.localEulerAngles.x, visual.localEulerAngles.y - edgeRotation, visual.localEulerAngles.z);
                    }
                }

                visual.parent = parentObject;
                Vector2Int offset = buildableEdgeObjectTypeSO.CalculatePlacedObjectSize(currentActiveSystem.GetGridCellSize());
                if (currentActiveSystem.gridAxis == GridAxis.XZ)
                {
                    parentObject.localPosition = new Vector3(offset.x * currentActiveSystem.GetGridCellSize() / 2, visual.localPosition.y, visual.localPosition.z);
                    if (buildableEdgeObject.IsRotateObjectForXZ())
                    {
                        //parentObject.localPosition = new Vector3(parentObject.localPosition.x - buildableEdgeObject.GetObjectPivotOffset().x, parentObject.localPosition.y - buildableEdgeObject.GetObjectPivotOffset().z, parentObject.localPosition.z - buildableEdgeObject.GetObjectPivotOffset().y);
                    }
                    else
                    {
                        if (edgeRotation == 0)
                        {
                            parentObject.localPosition = new Vector3(parentObject.localPosition.x - buildableEdgeObject.GetObjectPivotOffset().x - currentActiveSystem.GetGridCellSize() / 2, parentObject.localPosition.y - buildableEdgeObject.GetObjectPivotOffset().y, parentObject.localPosition.z - buildableEdgeObject.GetObjectPivotOffset().z);
                        }
                        else if (edgeRotation == 180)
                        {
                            parentObject.localPosition = new Vector3(parentObject.localPosition.x + buildableEdgeObject.GetObjectPivotOffset().x - currentActiveSystem.GetGridCellSize() / 2, parentObject.localPosition.y - buildableEdgeObject.GetObjectPivotOffset().y, parentObject.localPosition.z + buildableEdgeObject.GetObjectPivotOffset().z);
                        }
                    }
                }
                else
                {
                    //parentObject.localPosition = new Vector3(offset.x * currentActiveSystem.GetGridCellSize() / 2, offset.y * currentActiveSystem.GetGridCellSize() / 2, visual.localPosition.z);
                    if (buildableEdgeObject.IsRotateObjectForXY())
                    {
                        //parentObject.localPosition = new Vector3(parentObject.localPosition.x - buildableEdgeObject.GetObjectPivotOffset().x, parentObject.localPosition.y - buildableEdgeObject.GetObjectPivotOffset().z, parentObject.localPosition.z + buildableEdgeObject.GetObjectPivotOffset().y);
                    }
                    else
                    {
                        //parentObject.localPosition = new Vector3(parentObject.localPosition.x - buildableEdgeObject.GetObjectPivotOffset().x, parentObject.localPosition.y - buildableEdgeObject.GetObjectPivotOffset().y, parentObject.localPosition.z + buildableEdgeObject.GetObjectPivotOffset().z);
                    }
                }
                
                tempColliderScale = buildableEdgeObject.GetRawObjectScale();
                tempColliderCenter = buildableEdgeObject.GetRawObjectCenter();
                tempCollider = parentObject.gameObject.AddComponent<BoxCollider>();

                if (currentActiveSystem.gridAxis == GridAxis.XZ)
                {
                    if (buildableEdgeObject.IsRotateObjectForXZ())
                    {
                        tempCollider.isTrigger = true;
                        tempCollider.size = new Vector3(tempColliderScale.x, tempColliderScale.z, tempColliderScale.y);
                        tempCollider.center = new Vector3(tempColliderCenter.x, -tempColliderCenter.z, tempColliderCenter.y);
                    }
                    else
                    {
                        tempCollider.isTrigger = true;
                        tempCollider.size = tempColliderScale;
                        tempCollider.center = tempColliderCenter;
                    }
                }
                else
                {
                    if (buildableEdgeObject.IsRotateObjectForXY())
                    {
                        tempCollider.isTrigger = true;
                        tempCollider.size = new Vector3(tempColliderScale.x, tempColliderScale.z, tempColliderScale.y);
                        tempCollider.center = new Vector3(tempColliderCenter.x, tempColliderCenter.z, -tempColliderCenter.y);
                    }
                    else
                    {
                        tempCollider.isTrigger = true;
                        tempCollider.size = tempColliderScale;
                        tempCollider.center = tempColliderCenter;
                    }
                }

                colliderBridge = parentObject.gameObject.AddComponent<ColliderBridgeGridObject>();

                tempRB = parentObject.gameObject.AddComponent<Rigidbody>();
                tempRB.isKinematic = true;

                SetLayerRecursive(parentObject.gameObject, LayerNumber()); //Call function 'SetLayerRecursive'
            }
        }

        private void CleanObject()
        {
            if (tempCollider)
            {
                OnBuildableObjectAreaBlockerExit?.Invoke();
                DestroyImmediate(tempCollider);
                DestroyImmediate(colliderBridge);
                DestroyImmediate(tempRB);
                tempCollider = null;
            } 
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<BuildableObjectAreaBlocker>())
            {
                OnBuildableObjectAreaBlockerEnter?.Invoke();
            }
        }

        public void OnTriggerExit(Collider other)
        {
            if (other.gameObject.GetComponent<BuildableObjectAreaBlocker>())
            {
                OnBuildableObjectAreaBlockerExit?.Invoke();
            }
        }

        public void OnTriggerStay(Collider other)
        {
            if (other.gameObject.GetComponent<BuildableObjectAreaBlocker>())
            {
                OnBuildableObjectAreaBlockerEnter?.Invoke();
            }
        }

        private void HandleVisualColor(BuildableEdgeObjectTypeSO buildableEdgeObjectTypeSO)
        {
            if (currentActiveSystem.NotPlaceableVisualCallerBuildableEdgeObject() == false)
            {
                if (buildableEdgeObjectTypeSO.notPlaceableGhostMaterial != null) //If in 'buildableGridObjectTypeSO' a 'customeVisualMaterial' is provided
                {
                    selectedMat = buildableEdgeObjectTypeSO.notPlaceableGhostMaterial;
                }
                else if (buildableEdgeObjectTypeSO.placeableGhostMaterial != null) //If in 'buildableGridObjectTypeSO' a 'customeVisualMaterial' is provided
                {
                    selectedMat = buildableEdgeObjectTypeSO.placeableGhostMaterial;
                }
                else
                {
                    selectedMat = null;
                }
            }
            else
            {
                if (buildableEdgeObjectTypeSO.placeableGhostMaterial != null) //If in 'buildableGridObjectTypeSO' a 'customeVisualMaterial' is provided
                {
                    selectedMat = buildableEdgeObjectTypeSO.placeableGhostMaterial;
                }
                else
                {
                    selectedMat = null;
                }
            }

            if (selectedMat != null)
            {
                GameObject newVisual1 = null;
                GameObject newVisual2 = null;
                GameObject newVisual3 = null;
                GameObject newVisual4 = null;

                if (visual.GetComponent<MeshRenderer>()) //If the child object has a material
                {
                    visual.GetComponent<MeshRenderer>().material = selectedMat; //Replace current material in the child object with provided 'customeVisualMaterial'
                }

                for (int firstChild = 0; firstChild < visual.childCount; firstChild++) //Loop through all the child objects in 'visual'
                {
                    newVisual1 = visual.GetChild(firstChild).gameObject;
                    if (newVisual1.GetComponent<MeshRenderer>()) //If the child object has a material
                    {
                        newVisual1.GetComponent<MeshRenderer>().material = selectedMat; //Replace current material in the child object with provided 'customeVisualMaterial'
                    }

                    for (int secondChild = 0; secondChild < newVisual1.transform.childCount; secondChild++) //Loop through all the child objects in 'newVisual'
                    {
                        newVisual2 = newVisual1.transform.GetChild(secondChild).gameObject;
                        if (newVisual2.GetComponent<MeshRenderer>()) //If the child object has a material
                        {
                            newVisual2.GetComponent<MeshRenderer>().material = selectedMat; //Replace current material in the child object with provided 'customeVisualMaterial'
                        }

                        for (int thridChild = 0; thridChild < newVisual2.transform.childCount; thridChild++) //Loop through all the child objects in 'newVisual'
                        {
                            newVisual3 = newVisual2.transform.GetChild(thridChild).gameObject;
                            if (newVisual3.GetComponent<MeshRenderer>()) //If the child object has a material
                            {
                                newVisual3.GetComponent<MeshRenderer>().material = selectedMat; //Replace current material in the child object with provided 'customeVisualMaterial'
                            }

                            for (int fourthChild = 0; fourthChild < newVisual3.transform.childCount; fourthChild++) //Loop through all the child objects in 'newVisual'
                            {
                                newVisual4 = newVisual3.transform.GetChild(fourthChild).gameObject;
                                if (newVisual4.GetComponent<MeshRenderer>()) //If the child object has a material
                                {
                                    newVisual4.GetComponent<MeshRenderer>().material = selectedMat; //Replace current material in the child object with provided 'customeVisualMaterial'
                                }
                            }
                        }
                    }
                }
            }
        }

        private int LayerNumber() //This function grab 'ghostObjectLayer' and return layer int vlaue
        {
            int layerNumber = 0;
            int layer = ghostObjectLayer.value;
            while(layer > 0)
            {
                layer = layer >> 1;
                layerNumber++;
            }
            if (layerNumber > 1) return layerNumber - 1;
            else return 0;
        }

        private void SetLayerRecursive(GameObject targetGameObject, int layer) //Change 'visual' object's and it's children's layer
        {
            targetGameObject.layer = layer; //Set passed object's layer
            foreach (Transform child in targetGameObject.transform) //Loop through all the child objects
            {
                SetLayerRecursive(child.gameObject, layer); //Call the function again but pass the child object
            }
        }
    }
}

