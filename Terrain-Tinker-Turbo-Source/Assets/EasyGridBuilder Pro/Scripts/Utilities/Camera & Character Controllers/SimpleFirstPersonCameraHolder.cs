using UnityEngine;

namespace SoulGames.Utilities
{
    public class SimpleFirstPersonCameraHolder : MonoBehaviour
    {
        [Tooltip("Camera Position transform empty game object")]
        [SerializeField]private Transform cameraPosition;

        void LateUpdate()
        {
            transform.position = cameraPosition.position;
        }
    }
}