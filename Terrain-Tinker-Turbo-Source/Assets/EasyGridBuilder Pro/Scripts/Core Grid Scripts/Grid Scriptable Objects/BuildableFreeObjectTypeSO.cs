using UnityEngine;
using SoulGames.Utilities;

namespace SoulGames.EasyGridBuilderPro
{
    [CreateAssetMenu(menuName = "SoulGames/Easy Grid Builder Pro/BuildableFreeObjectTypeSO", order = 10)]
    public class BuildableFreeObjectTypeSO : ScriptableObject
    {
        [Space]
        [Tooltip("Name of the buildable object.")] [Rename("Unique Object Name")]
        public string objectName;
        [Tooltip("Provide a Buildable Object Type Category SO asset. \n(This is used in UI to categorize the icons.)")]
        public BuildableObjectTypeCategorySO buildableCategorySO;
        [Tooltip("Leave these empty for now. These will be used in future updates.)")] [TextArea(4,5)]
        public string objectDescription;
        [Tooltip("Leave these empty for now. These will be used in future updates.")] [TextArea(2,2)]
        public string objectToolTipDescription;
        [Tooltip("Sprite image of the object's icon. \n(This is used in UI to display the icons.)")]
        public Sprite objectIcon;
        [Space]
        [Tooltip("Buildable object prefab. This is the prefab that will be spawned. \n(If use multiple objects, a random one will be choose to spawn.)")]
        public Transform[] objectPrefab;
        [Tooltip("Buildable object visual. This is used to display a temparary ghost visual before spawn the object. If this is empty, Prefab's first object will be used instead")]
        public Transform ghostPrefab;
        [Tooltip("A custome material to be used in temparary ghost object, when object is placeable. \n(If nested children objects exist, check upto 4 nested levels). If this is empty, 'Visual' objects defualt materials will be used.")]
        public Material placeableGhostMaterial;
        [Tooltip("A custome material to be used in temparary ghost object, when object is no placeable. \n(If nested children objects exist, check upto 4 nested levels). If this is empty, 'placeableVisualMaterial' materials will be used.")]
        public Material notPlaceableGhostMaterial;
        [Tooltip("If enabled you can set the layer of the spawnning object.)")]
        public bool setBuiltObjectLayer = false;
        [Tooltip("This layer will be set to the spawnned buildable objetct.")]
        public LayerMask builtObjectLayer;
        [Space]
        [Tooltip("Enable hold and place objects on grid. \n(Can be used to hold click and paint objects on grid instead of click each time to spawn objects.) \n(Doesn't work with 'placeAndDeselect'. Only use one at a time.)")] [Rename("Hold Key To Place")]
        public bool holdToPlace;
        [Tooltip("Enable deselect object automatically after placing it. \n(Doesn't work with 'holdToPlace'. Only use one at a time.)")] [Rename("Place & Auto Deselect")]
        public bool placeAndDeselect;
        [Tooltip("Enable snapping to provided Object Snapper positions.)")]
        public bool snapToObjectSnappers;
        //[Tooltip("Enable deselect object automatically after placing it. \n(Doesn't work with 'holdToPlace'. Only use one at a time.)")] [Rename("Object Movable After Build")]
        //public bool objectMovable;
        [Space]
        [Tooltip("Enable using build conditions for the object type. \n(Can add build conditions via script. Check documentation.)")]
        public bool enableBuildCondition;
        [Tooltip("If 'Build Condition' is enabled, provide a Build Conditon SO asset. \n('Build Condition' must be enabled to use this.)")]
        public BuildConditionSO buildConditionSO;
    }
}
