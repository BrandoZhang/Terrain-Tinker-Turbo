using SoulGames.Utilities;
using UnityEngine;

namespace SoulGames.EasyGridBuilderPro
{
    public class BuildableFreeObject : MonoBehaviour
    {
        private EasyGridBuilderPro ownGridSystem;
        private EasyGridBuilderPro activeGridSystem;
        private bool isObjectBuilt = false;

        [Tooltip("Provide this Buildable Free Object's 'Buildable Free Object Type SO'")]
        [SerializeField]private BuildableFreeObjectTypeSO buildableFreeObjectTypeSO;
        [Tooltip("Rotate object automatically in XY Grid Axis. (Which means this object is originally prepared for the Grid Axis XZ)")]
        [SerializeField]public bool rotateObjectForXY;
        [Tooltip("Rotate object automatically in XZ Grid Axis. (Which means this object is originally prepared for the Grid Axis XY)")]
        [SerializeField]public bool rotateObjectForXZ;

        public static event OnBuildableFreeObjectBuiltDelegate OnBuildableFreeObjectBuilt;
        public delegate void OnBuildableFreeObjectBuiltDelegate(BuildableFreeObject buildableFreeObject);

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
        }

        private void OnObjectDeselect(EasyGridBuilderPro ownSystem, GameObject selectedObject)
        {
        }
    
        private void OnObjectStartMoving(EasyGridBuilderPro ownSystem, GameObject movingObject)
        {
        }

        private void OnObjectStoppedMoving(EasyGridBuilderPro ownSystem, GameObject movingObject)
        {
        }

        public static BuildableFreeObject Create(Vector3 worldPosition, float rotation, BuildableFreeObjectTypeSO buildableFreeObjectTypeSO, EasyGridBuilderPro system)
        {
            EasyGridBuilderPro activeGridSystem = system;
            Transform placedObjectTransform = Instantiate(buildableFreeObjectTypeSO.objectPrefab[Random.Range(0, buildableFreeObjectTypeSO.objectPrefab.Length)], Vector3.zero, Quaternion.identity);
            placedObjectTransform.name = placedObjectTransform.name.Replace("(Clone)","").Trim();
            placedObjectTransform.rotation = Quaternion.Euler(0, rotation, 0);
            placedObjectTransform.localPosition = worldPosition;

            BuildableFreeObject buildableFreeObject = placedObjectTransform.GetComponent<BuildableFreeObject>();

            if (activeGridSystem.gridAxis == GridAxis.XZ)
            {
                if (buildableFreeObject.IsRotateObjectForXZ())
                {
                    Transform tempTransform = new GameObject("tempTransform").transform;
                    tempTransform.localPosition = worldPosition;
                    tempTransform.rotation = Quaternion.Euler(90, 0, 0);

                    placedObjectTransform.parent = tempTransform;
                    placedObjectTransform.localEulerAngles = new Vector3(0, 0, -rotation);

                    placedObjectTransform.parent = null;
                    Destroy(tempTransform.gameObject);
                }
                else
                {
                    placedObjectTransform.rotation = Quaternion.Euler(0, rotation, 0);
                }
                
            }
            else
            {
                if (buildableFreeObject.IsRotateObjectForXY())
                {
                    Transform tempTransform = new GameObject("tempTransform").transform;
                    tempTransform.localPosition = worldPosition;
                    tempTransform.rotation = Quaternion.Euler(-90, 0, 0);

                    placedObjectTransform.parent = tempTransform;
                    placedObjectTransform.localEulerAngles = new Vector3(0, -rotation, 0);

                    placedObjectTransform.parent = null;
                    Destroy(tempTransform.gameObject);
                }
                else
                {
                    placedObjectTransform.rotation = Quaternion.Euler(0, 0, rotation);
                }
            }

            if (buildableFreeObjectTypeSO.setBuiltObjectLayer) SetLayerRecursive(placedObjectTransform.gameObject, LayerNumber(buildableFreeObjectTypeSO.builtObjectLayer));

            buildableFreeObject.Setup();
            return buildableFreeObject;
        }

        protected void Setup()
        {
            OnBuildableFreeObjectBuilt?.Invoke(this);
        }

        public void GridSetupDone(EasyGridBuilderPro gridSystem, bool isObjectBuilt, float rotation)
        {
            this.isObjectBuilt = isObjectBuilt;
            ownGridSystem = gridSystem;
        }

        public bool GetIsObjectBuilt()
        {
            return isObjectBuilt;
        }

        public virtual void DestroySelf()
        {
            Destroy(gameObject);
        }

        public override string ToString()
        {
            return buildableFreeObjectTypeSO.objectName;
        }

        public EasyGridBuilderPro GetOwnGridSystem()
        {
            return ownGridSystem;
        }

        public BuildableFreeObjectTypeSO GetBuildableFreeObjectTypeSO()
        {
            return buildableFreeObjectTypeSO;
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
    }
}