using UnityEngine;

namespace SoulGames.EasyGridBuilderPro
{
    public class BuildableObjectAreaBlocker : MonoBehaviour
    {
        [Tooltip("Size of the area that used to block buildable objects")]
        [SerializeField]private Vector3 blockingAreaTriggerSize = new Vector3(1, 1, 1);

        void Start()
        {
            BoxCollider collider;
            collider = gameObject.AddComponent<BoxCollider>();
            collider.isTrigger = enabled;
            collider.size = blockingAreaTriggerSize;
        }
        
        private void OnDrawGizmos()
        {
            Color color = Color.red;
            color.a = .25f;
            Gizmos.color = color;
            Gizmos.matrix = this.transform.localToWorldMatrix;
            Gizmos.DrawCube(Vector3.zero, blockingAreaTriggerSize);
            Gizmos.DrawWireCube(Vector3.zero, blockingAreaTriggerSize);
        }
    }
}