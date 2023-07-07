using UnityEngine;

namespace SoulGames.Utilities
{
    public class SwitchControllers : MonoBehaviour
    {
        [Space]
        [Tooltip("Starting enabled game object list")]
        [SerializeField]private GameObject[] startingActiveObjects;
        [Tooltip("Starting enabled camera parent")]
        [SerializeField]private Transform mainCamStartingActiveParent;
        [Space]
        [Tooltip("Switching enabled game object list. Disabled at the start")]
        [SerializeField]private GameObject[] switchingObjects;
        [Tooltip("Switching enabled camera parent. Disabled at the start")]
        [SerializeField]private Transform mainCamSwitchingParent;
        [Space]
        [Tooltip("Input key to switch between objects")]
        [SerializeField]private KeyCode switchToggleKey = KeyCode.Backspace;

        private Transform mainCam;
        private bool toggled = false;
        private SimpleFirstPersonCameraController cameraController;

        private void Start()
        {
            mainCam = Camera.main.transform;
        }

        void Update()
        {
            if (Input.GetKeyDown(switchToggleKey))
            {
                if (toggled)
                {
                    toggled = false;
                    if (mainCamStartingActiveParent && mainCamSwitchingParent) mainCam.parent = this.transform;

                    foreach (var item in startingActiveObjects)
                    {
                        item.SetActive(true);
                    }
                    foreach (var item in switchingObjects)
                    {
                        item.SetActive(false);
                    }

                    if (mainCamStartingActiveParent && mainCamSwitchingParent)
                    {
                        Invoke("ExecuteAfterTimeNotToggled", 1);
                    } 
                }
                else
                {
                    toggled = true;
                    if (mainCamStartingActiveParent && mainCamSwitchingParent) mainCam.parent = this.transform;

                    foreach (var item in switchingObjects)
                    {
                        item.SetActive(true);
                    }
                    foreach (var item in startingActiveObjects)
                    {
                        item.SetActive(false);
                    }

                    if (mainCamStartingActiveParent && mainCamSwitchingParent) 
                    {
                        Invoke("ExecuteAfterTimeToggled", 1);
                    }
                }
            }
        }

        private void ExecuteAfterTimeNotToggled()
        {
            mainCam.parent = this.mainCamStartingActiveParent;
        }

        private void ExecuteAfterTimeToggled()
        {
            mainCam.parent = this.mainCamSwitchingParent;
        }
    }
}
