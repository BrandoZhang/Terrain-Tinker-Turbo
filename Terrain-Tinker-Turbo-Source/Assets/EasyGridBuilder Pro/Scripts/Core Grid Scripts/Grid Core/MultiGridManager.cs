using System.Collections.Generic;
using UnityEngine;

namespace SoulGames.EasyGridBuilderPro
{
    public class MultiGridManager : MonoBehaviour
    {
        public static MultiGridManager Instance { get; private set; } //This instance

        public event OnActiveGridChangedDelegate OnActiveGridChanged;
        public delegate void OnActiveGridChangedDelegate(EasyGridBuilderPro activeGridSystem);
        
        [Tooltip("Simply select the 'Grid Surface' Layer Mask")]
        public LayerMask mouseColliderLayerMask; //Layermask for raycast hit(Ground layer)

        [HideInInspector]public List<EasyGridBuilderPro> easyGridBuilderProList = new List<EasyGridBuilderPro>();
        [HideInInspector]public EasyGridBuilderPro activeGridSystem;
        [HideInInspector]public bool onGrid;

        private void Awake()
        {
            Instance = this;
            
            foreach (EasyGridBuilderPro grid in FindObjectsOfType<EasyGridBuilderPro>())
            {
                easyGridBuilderProList.Add(grid);
            }
            
            if (easyGridBuilderProList.Count <= 0)
            {
                Debug.Log("<color=Red>Grid objects not found - Multi Grid Manager</color>");
            }
            else
            {
                activeGridSystem = easyGridBuilderProList[0];
                OnActiveGridChanged?.Invoke(activeGridSystem);
            }
        }

        private void Update()
        {
            if (easyGridBuilderProList.Count <= 0)
            {
                Debug.Log("<color=Red>Grid objects not found - Multi Grid Manager</color>");
                return;
            }
            if (activeGridSystem != GetUsingGrid())
            {
                activeGridSystem = GetUsingGrid();
                OnActiveGridChanged?.Invoke(activeGridSystem);
            }
        }

        private EasyGridBuilderPro GetUsingGrid()
        {
            Collider collider = GetMouseWorldPositionCollider3D();
            if (collider)
            {
                if (collider.gameObject.GetComponent<EasyGridBuilderPro>())
                {
                    onGrid = true;
                    foreach (EasyGridBuilderPro grid in easyGridBuilderProList)
                    {
                        if (collider.gameObject.GetComponent<EasyGridBuilderPro>() == grid)
                        {
                            return grid;
                        }
                    }
                    if (activeGridSystem) return activeGridSystem;
                    else return easyGridBuilderProList[0];
                }
                else
                {
                    //Debug.Log("Colliding but not Grid");
                    onGrid = false;
                    if (activeGridSystem) return activeGridSystem;
                    else return easyGridBuilderProList[0];
                }
            }
            else
            {
                //Debug.Log("Not Colliding");
                onGrid = false;
                if (activeGridSystem) return activeGridSystem;
                else return easyGridBuilderProList[0];
            }
        }

        /// <summary>
        /// Add a spawnned or newely created grid system. Takes one parameter.
        /// </summary>
        /// <param name="gridSystem">EasyGridBuilderPro Grid System</param>
        // public void AddSpawnnedGridSystem(EasyGridBuilderPro gridSystem)
        // {
        //     easyGridBuilderProList.Add(gridSystem);
        // }

        #region Extras
        private Collider GetMouseWorldPositionCollider3D()
        {
            foreach (EasyGridBuilderPro easyGridBuilderLite in easyGridBuilderProList)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, easyGridBuilderLite.mouseColliderLayerMask))
                {
                    return raycastHit.collider;
                }
                // else if (Physics.Raycast(Camera.main.transform.position, Vector3.down, out RaycastHit raycastHit2, 999f, easyGridBuilderLite.mouseColliderLayerMask))
                // {
                //     return raycastHit2.collider;
                // }
                else
                {
                    return null;
                }
            }
            return null;
        }
        #endregion
    }
}
