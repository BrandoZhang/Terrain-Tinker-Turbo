using UnityEngine;

namespace SoulGames.EasyGridBuilderPro
{
    public class BuildableFreeObjectSnapper : MonoBehaviour
    {
        [Tooltip("When mouse pointer enter inside of this bounding box area Buildable Free Object will snap to this object's origin")]
        [SerializeField]private Vector3 snappingTriggerSize = new Vector3(1, 1, 1);

        void Start()
        {
            BoxCollider collider;
            collider = gameObject.AddComponent<BoxCollider>();
            collider.isTrigger = enabled;
            collider.size = snappingTriggerSize;
        }
        
        private void OnDrawGizmos()
        {
            Color color = Color.cyan;
            color.a = .25f;
            Gizmos.color = color;
            Gizmos.matrix = this.transform.localToWorldMatrix;
            Gizmos.DrawCube(Vector3.zero, snappingTriggerSize);
            Gizmos.DrawWireCube(Vector3.zero, snappingTriggerSize);
        }
    }
}