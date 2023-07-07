using UnityEngine;

namespace SoulGames.Utilities
{
    public class WASDCameraController : MonoBehaviour
    {
        [Space]
        [Tooltip("Camera transform")]
        [SerializeField]private Transform cameraTransform;
        [Space]
        [Tooltip("Camera movement normal speed")]
        [SerializeField]private float normalSpeed = 1.5f;
        [Tooltip("Camera movement fast speed")]
        [SerializeField]private float fastSpeed = 3f;
        [Tooltip("Camera movement smooth time")]
        [SerializeField]private float movementLerpTime = 10f;
        [Space]
        [Tooltip("Camera rotation amount")]
        [SerializeField]private float rotationAmount = 2f;
        [Tooltip("Camera rotation smooth time")]
        [SerializeField]private float rotationLerpTime = 10f;
        [Space]
        [Tooltip("Camera zoom amount")]
        [SerializeField]private Vector3 zoomAmount = new Vector3(0f, -0.5f, 0.5f);
        [Tooltip("Camera minimum zoomable distance")]
        [SerializeField]private float minZoom = 10f;
        [Tooltip("Camera maximum zoomable distance")]
        [SerializeField]private float maxZoom = 100f;
        [Tooltip("Camera zoom smooth time")]
        [SerializeField]private float zoomLerpTime = 5f;
        [Space]
        [Tooltip("When enabled camera set its height automatically using raycast to the ground")]
        [SerializeField]public bool setHeigthByRaycast = false;
        [Tooltip("Raycast Layer Mask to 'Set Height By Raycast'")]
        [SerializeField]private LayerMask raycastLayerMask;
        [Space]
        [SerializeField]private KeyCode upKey = KeyCode.W;
        [SerializeField]private KeyCode downKey = KeyCode.S;
        [SerializeField]private KeyCode leftKey = KeyCode.A;
        [SerializeField]private KeyCode rightKey = KeyCode.D;
        [SerializeField]private KeyCode speedUpKey = KeyCode.LeftShift;
        [SerializeField]private KeyCode rotateLeftKey = KeyCode.Q;
        [SerializeField]private KeyCode rotateRightKey = KeyCode.E;
        [SerializeField]private KeyCode zoomInKey = KeyCode.Z;
        [SerializeField]private KeyCode zoomOutKey = KeyCode.X;

        private float movementSpeed;
        private Vector3 newPosition;
        private Quaternion newRotation;
        private Vector3 newZoom;
        private Vector3 raycastHeight = Vector3.zero;

        private void Start()
        {
            newPosition = transform.position;
            newRotation = transform.rotation;
            newZoom = cameraTransform.localPosition;
        }

        private void FixedUpdate()
        {
            HandleInput();
            if (setHeigthByRaycast)
            {
                raycastHeight = GetRaycastHeight();
                raycastHeight = new Vector3(0, raycastHeight.y, raycastHeight.y);
            }
        }

        private void HandleInput()
        {
            if (Input.GetKey(speedUpKey))
            {
                movementSpeed = fastSpeed;
            }
            else
            {
                movementSpeed = normalSpeed;
            }

            if (Input.GetKey(rotateLeftKey))
            {
                newRotation *= Quaternion.Euler(Vector3.up * rotationAmount);
            }
            if (Input.GetKey(rotateRightKey))
            {
                newRotation *= Quaternion.Euler(Vector3.up * -rotationAmount);
            }

            if (Input.GetKey(upKey) || Input.GetKey(KeyCode.UpArrow))
            {
                newPosition += (transform.forward * movementSpeed);
            }
            if (Input.GetKey(downKey) || Input.GetKey(KeyCode.DownArrow))
            {
                newPosition += (transform.forward * -movementSpeed);
            }
            if (Input.GetKey(rightKey) || Input.GetKey(KeyCode.RightArrow))
            {
                newPosition += (transform.right * movementSpeed);
            }
            if (Input.GetKey(leftKey) || Input.GetKey(KeyCode.LeftArrow))
            {
                newPosition += (transform.right * -movementSpeed);
            }

            if (Input.GetKey(zoomInKey))
            {
                newZoom += zoomAmount;
            }
            if (Input.GetKey(zoomOutKey))
            {
                newZoom -= zoomAmount;
            }

            newZoom.y = Mathf.Clamp(newZoom.y, minZoom, maxZoom);
            newZoom.z = Mathf.Clamp(newZoom.z, -maxZoom, -minZoom);

            //cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, raycastHeight, Time.deltaTime * movementSpeed);
            cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom + raycastHeight, Time.deltaTime * zoomLerpTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * rotationLerpTime);
            transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementLerpTime);
        }

        private Vector3 GetRaycastHeight()
        {
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit raycastHit, 999f, raycastLayerMask))
            {
                return raycastHit.point;
            }
            else
            {
                return Vector3.zero;
            }
        }
    }
}