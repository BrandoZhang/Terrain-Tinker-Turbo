using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

namespace SoulGames.EasyGridBuilderPro
{
    [RequireComponent(typeof(MultiGridManager))]
    public class MultiGridInputManager : MonoBehaviour
    {
        public static MultiGridInputManager Instance { get; private set; }                 //Create a static instance of this script

        [Tooltip("Add currently using Easy Grid Builder Pro Input SO asset")]
        [SerializeField]private EasyGridBuilderProInputsSO easyGridBuilderProInputsSO;
        [Tooltip("Add Grid Object Selector script")]
        [SerializeField]private GridObjectSelector gridObjectSelector;
        private GridObjectMover gridObjectMover;
        private FreeObjectMover freeObjectMover;
        private List<EasyGridBuilderPro> easyGridBuilderProList;

        private void OnEnable()
        {
            easyGridBuilderProInputsSO.gridModeResetKey.Enable();
            easyGridBuilderProInputsSO.gridHeightChangeKey.Enable();
            easyGridBuilderProInputsSO.buildModeActivationKey.Enable();
            easyGridBuilderProInputsSO.buildablePlacementKey.Enable();
            easyGridBuilderProInputsSO.buildableListScrollKey.Enable();
            easyGridBuilderProInputsSO.ghostRotateLeftKey.Enable();
            easyGridBuilderProInputsSO.ghostRotateRightKey.Enable();
            easyGridBuilderProInputsSO.destructionModeActivationKey.Enable();
            easyGridBuilderProInputsSO.buildableDestroyKey.Enable();
            easyGridBuilderProInputsSO.selectionModeActivationKey.Enable();
            easyGridBuilderProInputsSO.buildableSelectionKey.Enable();
            //easyGridBuilderProInputsSO.moveModeActivationKey.Enable();
            //easyGridBuilderProInputsSO.buildableMoveKey.Enable();
            easyGridBuilderProInputsSO.gridSaveKey.Enable();
            easyGridBuilderProInputsSO.gridLoadKey.Enable();
        }

        private void OnDisable()
        {
            easyGridBuilderProInputsSO.gridModeResetKey.performed -= context => GridModeResetKey(context);
            easyGridBuilderProInputsSO.gridHeightChangeKey.performed -= context => GridHeightChangeKey(context);
            easyGridBuilderProInputsSO.buildModeActivationKey.performed -= context => BuildModeActivationKey(context);
            easyGridBuilderProInputsSO.buildablePlacementKey.performed -= context => BuildablePlacementKey(context);
            easyGridBuilderProInputsSO.buildablePlacementKey.canceled -= context => BuildablePlacementKeyCancelled(context);
            easyGridBuilderProInputsSO.buildableListScrollKey.performed -= context => BuildableListScrollKey(context);
            easyGridBuilderProInputsSO.ghostRotateLeftKey.performed -= context => GhostRotateLeftKey(context);
            easyGridBuilderProInputsSO.ghostRotateLeftKey.canceled -= context => GhostRotateLeftKeyCancelled(context);
            easyGridBuilderProInputsSO.ghostRotateRightKey.performed -= context => GhostRotateRightKey(context);
            easyGridBuilderProInputsSO.ghostRotateRightKey.canceled -= context => GhostRotateRightKeyCancelled(context);
            easyGridBuilderProInputsSO.destructionModeActivationKey.performed -= context => DestructionModeActivationKey(context);
            easyGridBuilderProInputsSO.buildableDestroyKey.performed -= context => BuildableDestroyKey(context);
            easyGridBuilderProInputsSO.selectionModeActivationKey.performed -= context => SelectionModeActivationKey(context);
            easyGridBuilderProInputsSO.buildableSelectionKey.performed -= context => BuildableSelectionKey(context);
            //easyGridBuilderProInputsSO.moveModeActivationKey.performed -= context => MoveModeActivationKey(context);
            //easyGridBuilderProInputsSO.buildableMoveKey.performed -= context => BuildableMoveKey(context);
            easyGridBuilderProInputsSO.gridSaveKey.performed -= context => GridSaveKey(context);
            easyGridBuilderProInputsSO.gridLoadKey.performed -= context => GridLoadKey(context);

            easyGridBuilderProInputsSO.gridModeResetKey.Disable();
            easyGridBuilderProInputsSO.gridHeightChangeKey.Disable();
            easyGridBuilderProInputsSO.buildModeActivationKey.Disable();
            easyGridBuilderProInputsSO.buildablePlacementKey.Disable();
            easyGridBuilderProInputsSO.buildableListScrollKey.Disable();
            easyGridBuilderProInputsSO.ghostRotateLeftKey.Disable();
            easyGridBuilderProInputsSO.ghostRotateRightKey.Disable();
            easyGridBuilderProInputsSO.destructionModeActivationKey.Disable();
            easyGridBuilderProInputsSO.buildableDestroyKey.Disable();
            easyGridBuilderProInputsSO.selectionModeActivationKey.Disable();
            easyGridBuilderProInputsSO.buildableSelectionKey.Disable();
            //easyGridBuilderProInputsSO.moveModeActivationKey.Disable();
            //easyGridBuilderProInputsSO.buildableMoveKey.Disable();
            easyGridBuilderProInputsSO.gridSaveKey.Disable();
            easyGridBuilderProInputsSO.gridLoadKey.Disable();
        }

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            easyGridBuilderProList = MultiGridManager.Instance.easyGridBuilderProList;
            foreach (EasyGridBuilderPro easyGridBuilderPro in easyGridBuilderProList)
            {
                easyGridBuilderPro.SetInputGridModeVariables(easyGridBuilderProInputsSO.useBuildModeActivationKey, easyGridBuilderProInputsSO.useDestructionModeActivationKey, easyGridBuilderProInputsSO.useSelectionModeActivationKey);
            }
            if (gridObjectSelector) gridObjectSelector.SetInputGridModeVariables(easyGridBuilderProInputsSO.useBuildModeActivationKey, easyGridBuilderProInputsSO.useDestructionModeActivationKey, easyGridBuilderProInputsSO.useSelectionModeActivationKey);
            if (gridObjectMover) gridObjectMover.SetInputGridModeVariables(easyGridBuilderProInputsSO.useBuildModeActivationKey, easyGridBuilderProInputsSO.useDestructionModeActivationKey, easyGridBuilderProInputsSO.useSelectionModeActivationKey);
            if (freeObjectMover) freeObjectMover.SetInputGridModeVariables(easyGridBuilderProInputsSO.useBuildModeActivationKey, easyGridBuilderProInputsSO.useDestructionModeActivationKey, easyGridBuilderProInputsSO.useSelectionModeActivationKey);

            easyGridBuilderProInputsSO.gridModeResetKey.performed += context => GridModeResetKey(context);
            easyGridBuilderProInputsSO.gridHeightChangeKey.performed += context => GridHeightChangeKey(context);
            easyGridBuilderProInputsSO.buildModeActivationKey.performed += context => BuildModeActivationKey(context);
            easyGridBuilderProInputsSO.buildablePlacementKey.performed += context => BuildablePlacementKey(context);
            easyGridBuilderProInputsSO.buildablePlacementKey.canceled += context => BuildablePlacementKeyCancelled(context);
            easyGridBuilderProInputsSO.buildableListScrollKey.performed += context => BuildableListScrollKey(context);
            easyGridBuilderProInputsSO.ghostRotateLeftKey.performed += context => GhostRotateLeftKey(context);
            easyGridBuilderProInputsSO.ghostRotateLeftKey.canceled += context => GhostRotateLeftKeyCancelled(context);
            easyGridBuilderProInputsSO.ghostRotateRightKey.performed += context => GhostRotateRightKey(context);
            easyGridBuilderProInputsSO.ghostRotateRightKey.canceled += context => GhostRotateRightKeyCancelled(context);
            easyGridBuilderProInputsSO.destructionModeActivationKey.performed += context => DestructionModeActivationKey(context);
            easyGridBuilderProInputsSO.buildableDestroyKey.performed += context => BuildableDestroyKey(context);
            easyGridBuilderProInputsSO.selectionModeActivationKey.performed += context => SelectionModeActivationKey(context);
            easyGridBuilderProInputsSO.buildableSelectionKey.performed += context => BuildableSelectionKey(context);
            //easyGridBuilderProInputsSO.moveModeActivationKey.performed += context => MoveModeActivationKey(context);
            //easyGridBuilderProInputsSO.buildableMoveKey.performed += context => BuildableMoveKey(context);
            easyGridBuilderProInputsSO.gridSaveKey.performed += context => GridSaveKey(context);
            easyGridBuilderProInputsSO.gridLoadKey.performed += context => GridLoadKey(context);
        }

        public EasyGridBuilderProInputsSO GetEasyGridBuilderProInputsSO()
        {
            return easyGridBuilderProInputsSO;
        }

        private void GridHeightChangeKey(InputAction.CallbackContext context)
        {
            foreach (EasyGridBuilderPro easyGridBuilderPro in easyGridBuilderProList)
            {
                easyGridBuilderPro.TriggerGridHeightChangeManually();
            }
        }

        private void GridModeResetKey(InputAction.CallbackContext context)
        {
            foreach (EasyGridBuilderPro easyGridBuilderPro in easyGridBuilderProList)
            {
                easyGridBuilderPro.SetGridModeReset();
            }
            if (gridObjectSelector) gridObjectSelector.SetGridModeReset();
            if (gridObjectMover) gridObjectMover.SetGridModeReset();
            if (freeObjectMover) freeObjectMover.SetGridModeReset();
        }

        private void BuildModeActivationKey(InputAction.CallbackContext context)
        {
            if (!easyGridBuilderProInputsSO.useBuildModeActivationKey) return;
            foreach (EasyGridBuilderPro easyGridBuilderPro in easyGridBuilderProList)
            {
                easyGridBuilderPro.SetGridModeBuilding();
            }
        }

        private void BuildablePlacementKey(InputAction.CallbackContext context)
        {
            foreach (EasyGridBuilderPro easyGridBuilderPro in easyGridBuilderProList)
            {
                easyGridBuilderPro.TriggerBuildablePlacement();
            }
        }

        private void BuildablePlacementKeyCancelled(InputAction.CallbackContext context)
        {
            foreach (EasyGridBuilderPro easyGridBuilderPro in easyGridBuilderProList)
            {
                easyGridBuilderPro.TriggerBuildablePlacementCancelled();
            }
        }

        private void BuildableListScrollKey(InputAction.CallbackContext context)
        {
            foreach (EasyGridBuilderPro easyGridBuilderPro in easyGridBuilderProList)
            {
                easyGridBuilderPro.TriggerBuildableListScroll(easyGridBuilderProInputsSO.buildableListScrollKey.ReadValue<Vector2>());
            }
        }

        private void GhostRotateLeftKey(InputAction.CallbackContext context)
        {
            foreach (EasyGridBuilderPro easyGridBuilderPro in easyGridBuilderProList)
            {
                easyGridBuilderPro.TriggerGhostRotateLeft();
            }
        }

        private void GhostRotateLeftKeyCancelled(InputAction.CallbackContext context)
        {
            foreach (EasyGridBuilderPro easyGridBuilderPro in easyGridBuilderProList)
            {
                easyGridBuilderPro.TriggerGhostRotateLeftCancelled();
            }
        }

        private void GhostRotateRightKey(InputAction.CallbackContext context)
        {
            foreach (EasyGridBuilderPro easyGridBuilderPro in easyGridBuilderProList)
            {
                easyGridBuilderPro.TriggerGhostRotateRight();
            }
        }

        private void GhostRotateRightKeyCancelled(InputAction.CallbackContext context)
        {
            foreach (EasyGridBuilderPro easyGridBuilderPro in easyGridBuilderProList)
            {
                easyGridBuilderPro.TriggerGhostRotateRightCancelled();
            }
        }

        private void DestructionModeActivationKey(InputAction.CallbackContext context)
        {
            if (!easyGridBuilderProInputsSO.useDestructionModeActivationKey) return;
            foreach (EasyGridBuilderPro easyGridBuilderPro in easyGridBuilderProList)
            {
                easyGridBuilderPro.SetGridModeDestruction();
            }
        }

        private void BuildableDestroyKey(InputAction.CallbackContext context)
        {
            foreach (EasyGridBuilderPro easyGridBuilderPro in easyGridBuilderProList)
            {
                easyGridBuilderPro.TriggerBuildableDestroy();
            }
        }

        private void SelectionModeActivationKey(InputAction.CallbackContext context)
        {
            if (!easyGridBuilderProInputsSO.useSelectionModeActivationKey) return;
            gridObjectSelector.SetGridModeSelection();
        }

        private void BuildableSelectionKey(InputAction.CallbackContext context)
        {
            gridObjectSelector.TriggerBuildableSelection();
        }

        private void MoveModeActivationKey(InputAction.CallbackContext context)
        {
            //if (!easyGridBuilderProInputsSO.useMoveModeActivationKey) return;
            //if (gridObjectMover) gridObjectMover.SetGridModeMoving();
            //if (freeObjectMover) freeObjectMover.SetGridModeMoving();
        }

        private void BuildableMoveKey(InputAction.CallbackContext context)
        {
            if (gridObjectMover) gridObjectMover.TriggerBuildableMove();
            if (freeObjectMover) freeObjectMover.SetGridModeMoving();
        }

        private void GridSaveKey(InputAction.CallbackContext context)
        {
            foreach (EasyGridBuilderPro easyGridBuilderPro in easyGridBuilderProList)
            {
                easyGridBuilderPro.TriggerGridSave();
            }
        }

        private void GridLoadKey(InputAction.CallbackContext context)
        {
            foreach (EasyGridBuilderPro easyGridBuilderPro in easyGridBuilderProList)
            {
                easyGridBuilderPro.TriggerGridLoad();
            }
        }
    }
}
