using UnityEngine.InputSystem;
using UnityEngine;

namespace SoulGames.EasyGridBuilderPro
{
    [CreateAssetMenu(menuName = "SoulGames/Easy Grid Builder Pro/EasyGridBuilderProInputsSO", order = 100)] //Scriptable object asset path
    public class EasyGridBuilderProInputsSO : ScriptableObject //This is a scriptable object class
    {   
        [Space]
        [Tooltip("Input Key used to Reset current grid mode.")]
        public InputAction gridModeResetKey;

        [Space]
        [Tooltip("Input Key used to switch between vertical grids.")]
        public InputAction gridHeightChangeKey;

        [Space]
        [Tooltip("If this is enabled, to activate Build mode you will need a provided key.")]
        public bool useBuildModeActivationKey = false;
        [Tooltip("Input Key used to enable Build Mode.")]
        public InputAction buildModeActivationKey;
        [Tooltip("Input Key used to place objects on grid.")]
        public InputAction buildablePlacementKey;
        [Tooltip("Input Key used to scroll through available buildable objects.")]
        public InputAction buildableListScrollKey;
        [Tooltip("Input Key used to rotate buildable object ghost to left.")]
        public InputAction ghostRotateLeftKey;
        [Tooltip("Input Key used to rotate buildable object ghost to right.")]
        public InputAction ghostRotateRightKey;

        [Space]
        [Tooltip("If this is enabled, to activate Destruction mode you will need a provided key.")]
        public bool useDestructionModeActivationKey = true;
        [Tooltip("Input Key used to enable Destuct Mode.")]
        public InputAction destructionModeActivationKey;
        [Tooltip("Input Key used to destroy objects on grid.")]
        public InputAction buildableDestroyKey;

        [Space]
        [Tooltip("If this is enabled, to activate Selection mode you will need a provided key.")]
        public bool useSelectionModeActivationKey = false;
        [Tooltip("Input Key used to enable Selection Mode.")]
        public InputAction selectionModeActivationKey;
        [Tooltip("Input Key used to select objects on grid.")]
        public InputAction buildableSelectionKey;

        //[Space]
        //[Tooltip("Food amount should be in inventory to build.")]
        //public bool useMoveModeActivationKey = true;
        //[Tooltip("Food amount should be in inventory to build.")]
        //public InputAction moveModeActivationKey;
        //[Tooltip("Food amount should be in inventory to build.")]
        //public InputAction buildableMoveKey;

        [Space]
        [Tooltip("Input Key used to save the grid.")]
        public InputAction gridSaveKey;
        [Tooltip("Input Key used to load the grid.")]
        public InputAction gridLoadKey;
    }
}