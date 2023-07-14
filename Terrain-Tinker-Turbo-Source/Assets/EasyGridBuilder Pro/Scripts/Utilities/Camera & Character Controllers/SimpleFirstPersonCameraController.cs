using UnityEngine;

namespace SoulGames.Utilities
{
    public class SimpleFirstPersonCameraController : MonoBehaviour
    {
        [Tooltip("Camera 'X' axis look sensitivity")]
        [SerializeField]private float sensX;
        [Tooltip("Camera 'Y' axis look sensitivity")]
        [SerializeField]private float sensY;
        [Tooltip("Player Position transform empty game object")]
        [SerializeField]private Transform playerOrientation;
        [Tooltip("Set cursor mode to Locked. When cursor is in locked mode you can not interact with UI using cursor.")]
        [SerializeField]private bool lockCursor = false;

        private float xRotation;
        private float yRotation;

        private void OnEnable()
        {
            if (lockCursor) Cursor.lockState = CursorLockMode.Locked;
        }
        private void OnDisable()
        {
            if (lockCursor) Cursor.lockState = CursorLockMode.None;
        }

        private void Update()
        {
            float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
            float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

            yRotation += mouseX;
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            playerOrientation.rotation = Quaternion.Euler(0, yRotation, 0);
        }
    }
}